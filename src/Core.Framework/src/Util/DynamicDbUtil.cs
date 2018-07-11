using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DynamicDbUtil
{
    public static class Util
    {
        #region base methods

        public static dynamic Get(this DbConnection connection, string commandText, params object[] args)
        {
            dynamic result = null;

            using (DbCommand dbCommand = CreateDbCommand(connection, null, CommandType.Text, commandText, args))
            {
                DbDataReader reader = dbCommand.ExecuteReader();
                if (reader.HasRows && reader.Read())
                {
                    result = MapToExpandoObject(reader);
                }
                reader.Close();
            }
            return result;
        }

        public static List<dynamic> List(this DbConnection connection, string commandText, params object[] args)
        {
            List<dynamic> result = new List<dynamic>();

            using (DbCommand dbCommand = CreateDbCommand(connection, null, CommandType.Text, commandText, args))
            {
                DbDataReader reader = dbCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(MapToExpandoObject(reader));
                    }
                }
                reader.Close();
            }
            return result;
        }
        public static List<dynamic> List(this DbConnection connection, string commandText, int pageNumber, int rowsPage, params object[] args)
        {
            if (!commandText.ToUpper(System.Globalization.CultureInfo.CurrentCulture).Contains("ORDER BY")) throw new Exception("commandText must contains ORDER BY expression!");

            commandText = string.Format(@"             
                                        {0}
                                        OFFSET (({1} - 1) * {2} ROWS
                                        FETCH NEXT {2} ROWS ONLY                                         
                                        ", commandText, pageNumber, rowsPage);

            return List(connection, commandText, null);
        }
        public static int Execute(this DbConnection connection, string commandText, params object[] args)
        {
            using (DbCommand dbCommand = CreateDbCommand(connection, null, CommandType.Text, commandText, args))
            {
                return dbCommand.ExecuteNonQuery();
            }
        }
        public static int Execute(this DbConnection connection, DbTransaction transaction, string commandText, params object[] args)
        {
            using (DbCommand dbCommand = CreateDbCommand(connection, transaction, CommandType.Text, commandText, args))
            {
                return dbCommand.ExecuteNonQuery();
            }
        }
        public static int Execute(this DbConnection connection, DbTransaction transaction, string commandText, CommandType commandType, params object[] args)
        {
            using (DbCommand dbCommand = CreateDbCommand(connection, transaction, commandType, commandText, args))
            {
                return dbCommand.ExecuteNonQuery();
            }
        }

        #endregion

        #region extended methods
        public static T Get<T>(this DbConnection connection, string commandText)
        {
            return ExpandoObjectMapperUtil.Map<T>(Get(connection, commandText));
        }
        public static T Get<T>(this DbConnection connection, string commandText, params object[] args)
        {
            return ExpandoObjectMapperUtil.Map<T>(Get(connection, commandText, args));
        }
        public static List<T> List<T>(this DbConnection connection, string commandText, params object[] args)
        {
            return ExpandoObjectMapperUtil.ToMap<T>(List(connection, commandText, args));
        }
        public static List<T> List<T>(this DbConnection connection, string commandText, int pageIndex, int PageCount, string sortExpression, params object[] args)
        {
            return ExpandoObjectMapperUtil.ToMap<T>(List(connection, commandText, pageIndex, PageCount, args));
        }

        #endregion

        #region private methods
        private static dynamic MapToExpandoObject(DbDataReader reader)
        {
            dynamic result = new ExpandoObject();

            //if (reader.HasRows) result = new ExpandoObject();

            //if (reader.Read())
            //{
            if (reader.FieldCount == 1)
            {
                result = (!(reader[0] is DBNull) ? reader[0] : null);
            }
            else
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string[] fields = reader.GetName(i).Split('.');

                    if (fields.Length == 1)
                    {
                        (result as IDictionary<string, object>)[fields[0]] = (!(reader[i] is DBNull) ? reader[i] : null);
                    }
                    else
                    {
                        if ((!(reader[i] is DBNull) ? reader[i] : null) != null)
                        {
                            (result as IDictionary<string, object>)[fields[0]] = MapToExpandoObject(fields.Where(f => f != fields[0]).ToArray(), (!(reader[i] is DBNull) ? reader[i] : null),
                                (result as IDictionary<string, object>).Keys.Contains(fields[0]) ? (result as IDictionary<string, object>)[fields[0]] : new ExpandoObject());
                        }
                    }
                }
            }
            //}

            return result;
        }
        private static dynamic MapToExpandoObject(string[] fields, object value, dynamic o)
        {
            dynamic result = o;

            if (fields.Length == 0)
                return value;

            IDictionary<string, object> fieldDict = result as IDictionary<string, object>;

            string currentField = fields[0];

            string[] nextFields = fields.Where(f => f != currentField).ToArray();

            dynamic obj = fieldDict.Keys.Contains(currentField) ? fieldDict[currentField] : new ExpandoObject();

            fieldDict[currentField] = MapToExpandoObject(nextFields, value, obj);

            return fieldDict;
        }
        private static DbCommand CreateDbCommand(DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, object[] args)
        {
            FormatParameters(ref commandText, args);

            DbCommand dbCommand = connection.CreateCommand();
            dbCommand.CommandText = commandText;
            dbCommand.Connection = connection;
            if (transaction != null) dbCommand.Transaction = transaction;

            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (!(args[i] is String) && !(args[i] is IEnumerable<byte>) && args[i] is IEnumerable)
                    {
                        int index = 0;
                        foreach (var arg in (args[i] as IEnumerable))
                        {
                            var parameter = dbCommand.CreateParameter();
                            parameter.ParameterName = string.Format("@p{0}inp{1}", i, index);
                            parameter.Value = arg ?? DBNull.Value;
                            dbCommand.Parameters.Add(parameter);
                            index++;
                        }
                    }
                    else
                    {
                        var parameter = dbCommand.CreateParameter();
                        parameter.ParameterName = string.Format("@p{0}", i);
                        parameter.Value = args[i] ?? DBNull.Value;
                        parameter.IsNullable = args[i] == null;
                        dbCommand.Parameters.Add(parameter);
                    }
                }
            }

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            return dbCommand;
        }
        private static void FormatParameters(ref string commandText, object[] args)
        {
            if (args != null)
            {
                string[] parameterNames = new string[args.Length];

                for (int i = 0; i < args.Length; i++)
                {
                    if (!(args[i] is String) && !(args[i] is IEnumerable<byte>) && args[i] is IEnumerable)
                    {
                        int index = 0;
                        string parameterNameValues = string.Empty;
                        StringBuilder sbParameterNameValues = new StringBuilder();

                        foreach (var arg in (args[i] as IEnumerable))
                        {
                            sbParameterNameValues.Append(string.Format("@p{0}inp{1},", i, index));
                            index++;
                        }
                        parameterNameValues = sbParameterNameValues.ToString();
                        parameterNames[i] = parameterNameValues.Substring(0, parameterNameValues.Length - 1);
                    }
                    else
                    {
                        parameterNames[i] = string.Format("@p{0}", i);
                    }
                }

                commandText = string.Format(commandText, parameterNames);
            }
        }


        #endregion

    }
    public static class ExpandoObjectMapperUtil
    {
        public static T Map<T>(dynamic obj)
        {
            T result = default(T);
            IDictionary<string, dynamic> objectProperties = obj as IDictionary<string, dynamic>;
            if (default(T) is ValueType || (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                result = (T)obj;
            }
            else
            {
                Type t = typeof(T);
                T instance = (T)t.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                foreach (var item in objectProperties)
                {
                    DynamicMap(item, instance, t);
                }
                result = instance;
            }
            return result;
        }
        public static List<T> Map<T>(List<dynamic> list)
        {
            List<T> result = new List<T>();

            if (default(T) is ValueType || typeof(T) == typeof(String) || (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                foreach (var item in list)
                {
                    result.Add(item);
                }
            }
            else
            {
                Type t = typeof(T);

                foreach (var item in list)
                {
                    T instance = (T)t.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                    IDictionary<string, dynamic> objectProperties = item as IDictionary<string, dynamic>;
                    foreach (var prop in objectProperties)
                    {
                        DynamicMap(prop, instance, t);
                    }
                    result.Add(instance);
                }
            }
            return result;
        }
        private static void DynamicMap(KeyValuePair<string, object> prop, dynamic instance, Type t)
        {
            PropertyInfo fi = t.GetProperty(prop.Key);
            if (fi != null)
            {
                if (fi.PropertyType.UnderlyingSystemType.Namespace == "System" || prop.Value == null)
                {
                    fi.SetValue(instance, prop.Value);
                }
                else
                {
                    object ins = Activator.CreateInstance(fi.PropertyType);
                    fi.SetValue(instance, ins);
                    foreach (var p in (prop.Value as IDictionary<string, dynamic>))
                    {
                        DynamicMap(p, ins, ins.GetType());
                    }
                }
            }

        }
        public static List<T> ToMap<T>(this List<dynamic> list)
        {
            return Map<T>(list);
        }
    }
}
