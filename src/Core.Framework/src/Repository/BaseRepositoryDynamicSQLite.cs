// /*
//  * @Author: Atilla Tanrikulu 
//  * @Date: 2018-04-16 10:10:45 
//  * @Last Modified by:   Atilla Tanrikulu 
//  * @Last Modified time: 2018-04-16 10:10:45 
//  */
// using System;
// using System.Collections.Concurrent;
// using System.Collections.Generic;
// using System.Data.Common;
// using System.Linq;
// using System.Reflection;
// using System.Collections;
// using Core.Framework.Repository.Entity;
// using Core.Framework.Repository.DTO;
// using System.Text;
// using DynamicDbUtil;

// namespace Core.Framework.Repository
// {
//     public class BaseRepositoryDynamicSQLite<TEntity> where TEntity : class
//     {
//         protected DbConnection Connection;
//         protected int userId;
//         protected string TableName;
//         public BaseRepositoryDynamicSQLite(DbConnection connection) => init(connection, 0);
//         public BaseRepositoryDynamicSQLite(DbConnection connection, int userId) => init(connection, userId);
//         public BaseRepositoryDynamicSQLite(DbConnection connection, string user_Id)
//         {
//             int.TryParse(user_Id, out userId);
//             init(connection, userId);
//         }

//         private void init(DbConnection connection, int userId)
//         {
//             this.userId = userId;
//             Connection = connection;
//             var TableName = GetTableName(typeof(TEntity));
//         }
//         #region CRUD operations
//         public virtual long Insert(TEntity obj)
//         {
//             BaseEntity baseEntity = setDefaults(obj, SqlType.Insert);
//             long result = Connection.Execute(InsertSqlCache(typeof(TEntity), false));

//             if (result == 1)
//             {
//                 result = Connection.Get<long>("SELECT last_insert_rowid()");
//             }

//             return result;
//         }

//         public virtual TEntity Get(int id)
//         {
//             return Connection.Get<TEntity>($"SELECT * FROM {TableName} WHERE ISACTIVE=1 AND ID = @ID ", id);
//         }

//         public virtual int Update(TEntity obj)
//         {
//             BaseEntity baseEntity = setDefaults(obj, SqlType.Update);
//             return Connection.Execute(UpdateSqlCache(typeof(TEntity)), obj);
//         }

//         public virtual int Delete(int id)
//         {
//             return Connection.Execute($@"UPDATE {TableName}
//                                         SET ISACTIVE=0, UPDATEDATE=datetime('now'), UPDATEDBY=@UpdatedBy
//                                         WHERE Id=@Id", id, userId, DateTime.Now);
// }

//         public virtual IEnumerable<TEntity> List(BaseDto baseDto, ref PagingDto pagingDto)
//         {
//             if (baseDto == null) return null;
//             //string sqlSelect;
//             //string sqlWhere;

//             var tableName = TableName;

//             if (pagingDto?.pageNumber == 1)
//             {
//                 pagingDto.count = Count(baseDto);
//             }

//             return Connection.List<TEntity>($"SELECT * FROM {tableName} T1 WHERE T1.ISACTIVE=1");// {OrderBy(pagingDto, null)} {PagingSql(pagingDto)}");
//         }

//         #endregion CRUD operations

//         #region Extensions

//         // public virtual int InsertAll(List<TEntity> list)
//         // {
//         //     string sql = InsertSqlCache(typeof(TEntity), false);

//         //     List<DynamicParameters> paramList = new List<DynamicParameters>();

//         //     for (int i = 0; i < list.Count; i++)
//         //     {
//         //         BaseEntity baseEntity = list.ElementAt(i) as BaseEntity;
//         //         baseEntity = setDefaults(baseEntity, SqlType.Insert);
//         //         var dynParams = GenerateDynamicParameters(baseEntity, SqlType.Insert);
//         //         paramList.Add(dynParams);
//         //         LogSql(sql, dynParams);
//         //     }
//         //     return Connection.Execute(sql, paramList);
//         // }

//         public virtual int UpdateAll(List<TEntity> list)
//         {
//             for (int i = 0; i < list.Count; i++)
//             {
//                 Update(list[i]);
//             }
//             return 1;
//         }

//         public virtual TEntity SaveOrUpdate(TEntity obj)
//         {
//             BaseEntity baseEntity = obj as BaseEntity;

//             if (baseEntity == null) throw new Exception("Ancak BaseEntity turundeki objeler insert yapilabilir.");

//             if (baseEntity.Id == 0)
//             {
//                 long pk = Insert(obj);
//                 baseEntity.Id = pk;
//                 IDictionary<string, object> result = baseEntity as IDictionary<string, object>;

//                 return result as TEntity;
//             }
//             else
//             {
//                 Update(obj);
//                 IDictionary<string, object> result = obj as IDictionary<string, object>;

//                 return result as TEntity;
//             }
//         }

//         public virtual IEnumerable<TEntity> Get(int[] id)
//         {
//             return Connection.List<TEntity>($@"SELECT * FROM {TableName} WHERE ISACTIVE=1 AND ID IN @ID ", new { Id = id });
//         }

//         public virtual int Delete(int[] id)
//         {
//             return Connection.Execute($@"UPDATE {TableName} SET ISACTIVE=0, UPDATEDATE=SYSDATE, UPDATEDBY=@UpdatedBy WHERE Id IN @ID", new { Id = id, UpdatedBy = userId });
//         }

//         public virtual int DeleteHard(int id)
//         {
//             return Connection.Execute($@"DELETE FROM {TableName} WHERE Id=@ID", new { Id = id });
//         }

//         public virtual int DeleteHard(int[] id)
//         {
//             return Connection.Execute($@"DELETE FROM {TableName} WHERE Id IN =@ID", new { Id = id });
//         }

//         public virtual IEnumerable<TEntity> ListAll()
//         {
//             return Connection.List<TEntity>("SELECT * FROM {TableName} WHERE ISACTIVE=1");
//         }

//         public virtual int Count()
//         {
//             return Connection.Get<int>($"SELECT count(Id) FROM {TableName} WHERE ISACTIVE=1 ");
//         }

//         public virtual int Count(BaseDto baseDto)
//         {
//             if (baseDto == null) return 0;
//             //string sqlSelect;
//             //string sqlWhere;
//             //DynamicParameters paramList = GenerateDynamicSearchParameters(baseDto, true, null, out sqlSelect, out sqlWhere);
//             return Connection.Get<int>($"SELECT count(Id) FROM {TableName} WHERE ISACTIVE=1 ");
//         }

//         #endregion Extensions

//         #region Helpers
//         protected BaseEntity setDefaults(Object obj, SqlType stlType)
//         {
//             Type type = typeof(TEntity);
//             BaseEntity baseEntity = obj as BaseEntity;

//             if (baseEntity == null) throw new Exception("Ancak BaseEntity turundeki objeler insert yapilabilir.");

//             switch (stlType)
//             {
//                 case SqlType.Insert:
//                     baseEntity.IsActive = true;
//                     baseEntity.CreateDate = DateTime.Now;
//                     baseEntity.CreatedBy = userId;
//                     baseEntity.UpdateDate = null;
//                     baseEntity.UpdatedBy = null;
//                     break;

//                 case SqlType.Update:
//                     baseEntity.UpdateDate = DateTime.Now;
//                     baseEntity.UpdatedBy = userId;
//                     break;

//                 default:
//                     break;
//             }

//             return baseEntity;
//         }
//         // private DynamicParameters GenerateDynamicParameters(object obj, SqlType sqlType)
//         // {
//         //     Type type = typeof(TEntity);
//         //     var tableName = GetTableName(type);

//         //     if (sqlType.Equals(SqlType.Select)) type = obj.GetType();
//         //     var paramList = new DynamicParameters();
//         //     IEnumerable<PropertyInfo> properties;

//         //     if (sqlType.Equals(SqlType.Insert))
//         //         properties = TypePropertiesCache(type).Where(p => !p.Name.ToUpperInvariant().Equals("ID")).ToList();
//         //     else if (sqlType.Equals(SqlType.Select))
//         //         properties = TypePropertiesCache(type).Where(p => !p.Name.ToUpperInvariant().Equals("CREATEDBY") &&
//         //                                            !p.Name.ToUpperInvariant().Equals("CREATEDATE") &&
//         //                                            !p.Name.ToUpperInvariant().Equals("UPDATEDATE") &&
//         //                                            !p.Name.ToUpperInvariant().Equals("UPDATEDBY")).ToList();
//         //     else
//         //         properties = TypePropertiesCache(type);

//         //     for (var i = 0; i < properties.Count(); i++)
//         //     {
//         //         var property = properties.ElementAt(i);
//         //         var propertyValue = property.GetValue(obj);

//         //         if (property.PropertyType.BaseType == typeof(BaseEntity)) continue;

//         //         if ((sqlType.Equals(SqlType.Insert) || sqlType.Equals(SqlType.Update)) && (propertyValue is ICollection || propertyValue is IEnumerable<object>)) continue;

//         //         if (propertyValue == null)
//         //         {
//         //             paramList.Add(name: property.Name.ToUpperInvariant(), value: null, dbType: sqliteDbTypeMap[property.PropertyType], direction: ParameterDirection.Input);
//         //             continue;
//         //         }

//         //         if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(Boolean) || property.PropertyType == typeof(bool?) || property.PropertyType == typeof(Boolean?))
//         //         {
//         //             paramList.Add(name: property.Name.ToUpperInvariant(), value: (bool)propertyValue ? 1 : 0, dbType: DbType.Decimal, direction: ParameterDirection.Input);
//         //             continue;
//         //         }

//         //         paramList.Add(name: property.Name.ToUpperInvariant(), value: propertyValue, dbType: sqliteDbTypeMap[property.PropertyType], direction: ParameterDirection.Input);
//         //     }
//         //     return paramList;
//         // }

//         // private DynamicParameters GenerateDynamicSearchParameters(BaseDto baseDto, bool isCount, string alias, out string sqlSelect, out string sqlWhere)
//         // {
//         //     Type type = baseDto.GetType();
//         //     var tableName = TableName;
//         //     var sbColumnList = new StringBuilder(null);

//         //     var paramList = new DynamicParameters();
//         //     List<PropertyInfo> properties = TypePropertiesCache(type).Where(p => !p.Name.ToUpperInvariant().Equals("CREATEDBY") &&
//         //                                           !p.Name.ToUpperInvariant().Equals("CREATEDATE") &&
//         //                                           !p.Name.ToUpperInvariant().Equals("UPDATEDATE") &&
//         //                                           !p.Name.ToUpperInvariant().Equals("UPDATEDBY") &&
//         //                                           !p.Name.ToUpperInvariant().Equals("ISACTIVE")).ToList();

//         //     if (string.IsNullOrEmpty(alias)) alias = "T1";

//         //     for (var i = 0; i < properties.Count(); i++)
//         //     {
//         //         var property = properties.ElementAt(i);
//         //         var propertyValue = property.GetValue(baseDto);

//         //         if (propertyValue == null) continue;

//         //         if (property.PropertyType.BaseType == typeof(BaseEntity)) continue;

//         //         if (propertyValue is ICollection || propertyValue is IEnumerable<object>) continue;

//         //         if (property.PropertyType.BaseType == typeof(BaseDto)) continue;

//         //         if (IsNumeric(property.PropertyType) && propertyValue.ToString().Equals("0")) continue;

//         //         if (sbColumnList.Length > 0) sbColumnList.Append(" AND ");

//         //         if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(Boolean) || property.PropertyType == typeof(bool?) || property.PropertyType == typeof(Boolean?))
//         //         {
//         //             paramList.Add(name: alias + "_" + property.Name.ToUpperInvariant(), value: (bool)propertyValue ? 1 : 0, dbType: DbType.Decimal, direction: ParameterDirection.Input);
//         //             sbColumnList.AppendFormat("{0}.{1}=@{0}_{1}", alias, property.Name.ToUpperInvariant());
//         //             continue;
//         //         }

//         //         if (property.PropertyType == typeof(string) || property.PropertyType == typeof(String))
//         //             sbColumnList.AppendFormat("{0}.{1} LIKE @{0}_{1} || '%'", alias, property.Name.ToUpperInvariant());
//         //         else
//         //             sbColumnList.AppendFormat("{0}.{1}=@{0}_{1}", alias, property.Name.ToUpperInvariant());

//         //         paramList.Add(name: alias + "_" + property.Name.ToUpperInvariant(), value: propertyValue, dbType: sqliteDbTypeMap[property.PropertyType], direction: ParameterDirection.Input);
//         //     }

//         //     sqlWhere = sbColumnList.Length > 0 ? $" WHERE {alias}.ISACTIVE=1 AND {sbColumnList} " : $" WHERE {alias}.ISACTIVE=1";

//         //     if (isCount)
//         //         sqlSelect = $@"SELECT COUNT({alias}.ID) FROM {tableName} {alias}";
//         //     else
//         //         sqlSelect = $@"SELECT * FROM {tableName} {alias}";

//         //     return paramList;
//         // }

//         private string InsertSqlCache(Type type, bool isReturnsPk)
//         {
//             object insertSql;
//             if (Cache($"InsertSqls-{isReturnsPk}").TryGetValue(type.Name, out insertSql)) return insertSql.ToString();

//             var tableName = GetTableName(type);
//             var sbColumnList = new StringBuilder(null);
//             var sbParameterList = new StringBuilder(null);
//             var allProperties = TypePropertiesCache(type).Where(p => !p.Name.ToUpperInvariant().Equals("ID")).ToList();

//             for (var i = 0; i < allProperties.Count; i++)
//             {
//                 var property = allProperties.ElementAt(i);
//                 if (property.PropertyType.BaseType == typeof(BaseEntity)) continue;
//                 if (IsEnumerable(property)) continue;
//                 sbColumnList.AppendFormat("{0}", property.Name.ToUpperInvariant());
//                 sbParameterList.AppendFormat("@{0}", property.Name.ToUpperInvariant());

//                 if (i < allProperties.Count - 1)
//                 {
//                     sbColumnList.Append(", ");
//                     sbParameterList.Append(", ");
//                 }
//             }

//             string insertSqlNew = "";

//             if (isReturnsPk)
//             {
//                 insertSqlNew = $@"INSERT INTO {tableName.ToUpperInvariant()} ({sbColumnList}) 
//                                   VALUES ({sbParameterList})";
//             }
//             else
//             {
//                 insertSqlNew = $@"INSERT INTO {tableName.ToUpperInvariant()} ({sbColumnList}) VALUES ({sbParameterList})";
//             }

//             Cache($"InsertSqls-{isReturnsPk}")[type.Name] = insertSqlNew;

//             return insertSqlNew;
//         }

//         private string UpdateSqlCache(Type type)
//         {
//             object updateSqls;
//             if (Cache("UpdateSqls").TryGetValue(type.Name, out updateSqls)) return updateSqls.ToString();

//             var tableName = GetTableName(type);
//             var sbColumnList = new StringBuilder(null);
//             var allProperties = TypePropertiesCache(type).Where(p => !p.Name.ToUpperInvariant().Equals("ID")).ToList();

//             for (var i = 0; i < allProperties.Count; i++)
//             {
//                 var property = allProperties.ElementAt(i);
//                 if (property.PropertyType.BaseType == typeof(BaseEntity)) continue;
//                 if (IsEnumerable(property)) continue;
//                 sbColumnList.AppendFormat("{0}= @{0}", property.Name.ToUpperInvariant());

//                 if (i < allProperties.Count - 1)
//                 {
//                     sbColumnList.Append(", ");
//                 }
//             }

//             string updateSqlsNew = $@"UPDATE {tableName.ToUpperInvariant()}
//                                       SET {sbColumnList}
//                                       WHERE Id= @ID";

//             Cache("UpdateSqls")[type.Name] = updateSqlsNew;

//             return updateSqlsNew;
//         }

//         // private static void LogSql(string sql, DynamicParameters paramList)
//         // {
//         //     try
//         //     {
//         //         string sqlStr = sql;
//         //         foreach (string pName in paramList.ParameterNames)
//         //         {
//         //             var value = paramList.Get<dynamic>(pName);
//         //             if (value != null)
//         //             {
//         //                 if (value is string || value is String || value is DateTime)
//         //                 {
//         //                     sqlStr = sqlStr.Replace($"@{pName}", $"'{value}'");
//         //                 }
//         //                 else
//         //                 {
//         //                     sqlStr = sqlStr.Replace($"@{pName}", $"{Convert.ToString(value)}");
//         //                 }
//         //             }
//         //             else
//         //             {
//         //                 sqlStr = sqlStr.Replace($"@{pName}", "null");
//         //             }
//         //         }

//         //         Console.WriteLine(sqlStr);
//         //     }
//         //     catch (Exception e)
//         //     {
//         //         Console.WriteLine("SQL loglama isleminde hata olustu!", e);
//         //     }
//         // }

//         // private static List<TEntity> SetNotDirty(IEnumerable<dynamic> result)
//         // {
//         //     var list = new List<TEntity>();
//         //     var type = typeof(TEntity);

//         //     foreach (IDictionary<string, object> res in result)
//         //     {
//         //         list.Add(res as TEntity);
//         //     }
//         //     return list;
//         // }

//         // public DynamicParameters GenerateSearchDynamicParameters(BaseDto baseDto, out string sqlWhere)
//         // {
//         //     return GenerateSearchDynamicParameters(baseDto, null, out sqlWhere);
//         // }

//         // public DynamicParameters GenerateSearchDynamicParameters(BaseDto baseDto, string alias, out string sqlWhere)
//         // {
//         //     DynamicParameters result = null;
//         //     string select = "";
//         //     result = GenerateDynamicSearchParameters(baseDto, false, alias, out select, out sqlWhere);
//         //     sqlWhere = sqlWhere.Substring(6);
//         //     return result;
//         // }
//         // public DynamicParameters GenerateSearchDynamicParameters(Dictionary<string, BaseDto> baseDtoList, out string sqlWhere)
//         // {
//         //     DynamicParameters result = new DynamicParameters();
//         //     StringBuilder sbSqlWhere = new StringBuilder();

//         //     foreach (string alias in baseDtoList.Keys)
//         //     {
//         //         string where = "";
//         //         BaseDto baseDto = baseDtoList[alias];
//         //         if (baseDto != null) result.AddDynamicParams(GenerateSearchDynamicParameters(baseDto, alias, out where));
//         //         if (sbSqlWhere.Length > 1 && where.Length > 0) sbSqlWhere.Append(" AND ");
//         //         sbSqlWhere.Append(where);
//         //     }

//         //     sqlWhere = sbSqlWhere.ToString();

//         //     return result;
//         // }
//         // public string PagingSql(PagingDto pagingDto)
//         // {
//         //     int start = (pagingDto.pageNumber - 1) * pagingDto.pageSize;
//         //     return $" LIMIT {start}, {pagingDto.pageSize}";
//         // }
//         // public string OrderBy(PagingDto pagingDto)
//         // {
//         //     return OrderBy(pagingDto, null);
//         // }
//         // public string OrderBy(PagingDto pagingDto, string alias)
//         // {
//         //     if (!string.IsNullOrEmpty(alias))
//         //     {
//         //         alias = alias + ".";
//         //     }
//         //     else
//         //         alias = "";

//         //     return (string.IsNullOrEmpty(pagingDto.orderBy) ? "" : $" ORDER BY {alias}{pagingDto.orderBy} {pagingDto.order}");
//         // }
//         private static bool IsEnumerable(PropertyInfo property)
//         {
//             var type = property.PropertyType;
//             return typeof(string) != type && typeof(IEnumerable).IsAssignableFrom(type);
//         }
//         private static string GetTableName(Type type)
//         {
//             IDictionary<string, object> tableNameCache = Cache("TypeNameTableName");

//             object name;
//             if (tableNameCache.TryGetValue(type.Name, out name)) return name.ToString();

//             var tableAttr = type.GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic;

//             if (tableAttr != null)
//                 name = tableAttr.Name;
//             else
//             {
//                 name = type.Name;
//                 if (IsInterface(type) && name.ToString().StartsWith("I"))
//                     name = name.ToString().Substring(1);
//             }

//             tableNameCache[type.Name] = name;
//             return name.ToString();
//         }
//         private static bool IsInterface(Type type)
//         {
//             return type.GetTypeInfo().IsInterface;

//         }
//         private static readonly ConcurrentDictionary<string, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<string, IEnumerable<PropertyInfo>>();

//         private static IEnumerable<PropertyInfo> TypePropertiesCache(Type type)
//         {
//             IEnumerable<PropertyInfo> pis;
//             if (TypeProperties.TryGetValue(type.FullName, out pis)) return pis.ToList();

//             var properties = type.GetProperties().ToArray();
//             TypeProperties[type.FullName] = properties;
//             return properties.ToList();
//         }
//         private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object>> CacheTable = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();
//         private static ConcurrentDictionary<string, object> Cache(string key)
//         {
//             ConcurrentDictionary<string, object> internalCache;
//             if (CacheTable.TryGetValue(key, out internalCache)) return internalCache;

//             ConcurrentDictionary<string, object> newCache = new ConcurrentDictionary<string, object>();

//             CacheTable[key] = newCache;

//             return newCache;
//         }
//         // private static bool IsNumeric(Type type)
//         // {
//         //     if (type == typeof(Int16) ||
//         //         type == typeof(Int32) ||
//         //         type == typeof(Int64) ||
//         //         type == typeof(Decimal) ||
//         //         type == typeof(Double) ||
//         //         type == typeof(Byte) ||
//         //         type == typeof(SByte) ||
//         //         type == typeof(UInt16) ||
//         //         type == typeof(UInt32) ||
//         //         type == typeof(UInt64) ||
//         //         type == typeof(Single) ||
//         //         type == typeof(float)
//         //         )
//         //     {
//         //         return true;
//         //     }
//         //     return false;
//         // }

//         #endregion Helpers

//     }




// }