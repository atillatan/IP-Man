// /*
//  * @Author: Atilla Tanrikulu 
//  * @Date: 2018-04-16 10:10:28 
//  * @Last Modified by:   Atilla Tanrikulu 
//  * @Last Modified time: 2018-04-16 10:10:28 
//  */

// using Core.Framework.Repository.DTO;
// using Core.Framework.Repository.Entity;
// using Core.Framework.Service;
// using Dapper;
// using System;
// using System.Collections;
// using System.Collections.Concurrent;
// using System.Collections.Generic;
// using System.Data;
// using System.Data.Common;
// using System.Linq;
// using System.Reflection;
// using System.Reflection.Emit;
// using System.Text;
// using System.Threading;


// namespace Core.Framework.Repository
// {
//     public class BaseRepositoryOracle<TEntity> : IRepository<TEntity> where TEntity : class
//     {

//         protected DbConnection Connection;
//         protected int userId;
//         internal static Dictionary<Type, DbType> oracleDbTypeMap;
//         protected string TableName;
//         public BaseRepositoryOracle(DbConnection connection) => init(connection, 0);
//         public BaseRepositoryOracle(DbConnection connection, int userId) => init(connection, userId);

//         public BaseRepositoryOracle(DbConnection connection, string user_Id)
//         {        
//             int.TryParse(user_Id, out userId);
//             init(connection, userId);
//         }
//         private void init(DbConnection connection, int userId)
//         {
//             Connection = connection;
//             TableName = GetTableName(typeof(TEntity));
//             oracleDbTypeMap = new Dictionary<Type, DbType>
//             {
//                 [typeof(byte)] = DbType.Byte,
//                 [typeof(sbyte)] = DbType.SByte,
//                 [typeof(short)] = DbType.Int16,
//                 [typeof(ushort)] = DbType.UInt16,
//                 [typeof(int)] = DbType.Int32,
//                 [typeof(Int64)] = DbType.Decimal,
//                 [typeof(uint)] = DbType.UInt32,
//                 [typeof(long)] = DbType.Int64,
//                 [typeof(ulong)] = DbType.UInt64,
//                 [typeof(float)] = DbType.Single,
//                 [typeof(double)] = DbType.Double,
//                 [typeof(decimal)] = DbType.Decimal,
//                 [typeof(bool)] = DbType.Decimal,
//                 [typeof(string)] = DbType.String,
//                 [typeof(char)] = DbType.StringFixedLength,
//                 [typeof(Guid)] = DbType.Guid,
//                 [typeof(DateTime)] = DbType.DateTime,
//                 [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
//                 [typeof(TimeSpan)] = DbType.Time,
//                 [typeof(byte[])] = DbType.Binary,
//                 [typeof(byte?)] = DbType.Byte,
//                 [typeof(sbyte?)] = DbType.SByte,
//                 [typeof(short?)] = DbType.Int16,
//                 [typeof(ushort?)] = DbType.UInt16,
//                 [typeof(int?)] = DbType.Int32,
//                 [typeof(uint?)] = DbType.UInt32,
//                 [typeof(long?)] = DbType.Int64,
//                 [typeof(ulong?)] = DbType.UInt64,
//                 [typeof(float?)] = DbType.Single,
//                 [typeof(double?)] = DbType.Double,
//                 [typeof(decimal?)] = DbType.Decimal,
//                 [typeof(bool?)] = DbType.Decimal,
//                 [typeof(char?)] = DbType.StringFixedLength,
//                 [typeof(Guid?)] = DbType.Guid,
//                 [typeof(DateTime?)] = DbType.DateTime,
//                 [typeof(DateTimeOffset?)] = DbType.DateTimeOffset,
//                 [typeof(TimeSpan?)] = DbType.Time,
//                 [typeof(object)] = DbType.Object
//             };
//         }

//         #region CRUD operations

//         /// <summary>
//         /// Verilen BaseEntity turungeki objeyi veritabanina insert yapar.
//         /// </summary>
//         /// <param name="obj">BaseEntity turungeki obje</param>
//         /// <returns></returns>
//         public virtual long Insert(TEntity obj)
//         {
//             BaseEntity baseEntity = setDefaults(obj, SqlType.Insert);
//             string sql = InsertSqlCache(typeof(TEntity), true);
//             DynamicParameters paramList = GenerateDynamicParameters(baseEntity, SqlType.Insert);
//             LogSql(sql, paramList); //TODO:Atilla => Daha sonra iptal edilecek
//             Connection.Execute(sql, paramList);
//             return paramList.Get<long>("Id");
//         }

//         /// <summary>
//         /// id degeri verilen tek bir entity getirir.
//         /// T olarak verilen entity objesi, daha once veritabanindan alinmis ise,
//         /// objede herhangi bir degisiklik oldugu Dapper.Contrip tarafindan, otomatik olarak takip edilmektedir.
//         /// Update() islemlerinde degisiklik olmayan objeler update yapilmayacaktir.
//         /// IsDirty=true, degisiklik yapildigni ifade eder.
//         /// </summary>
//         /// <param name="id">Veritabanindaki Id degeri</param>
//         /// <returns>Tek Entity getirir.</returns>
//         public virtual TEntity Get(int id)
//         {
//             var type = typeof(TEntity);

//             string sql = $"SELECT * FROM {GetTableName(type)} WHERE ISACTIVE=1 AND ID = :ID ";
//             var dynParms = new DynamicParameters();
//             dynParms.Add(name: "Id", value: id, dbType: DbType.Int32, direction: ParameterDirection.Input);

//             TEntity obj;
//             if (!IsInterface(type)) return obj = Connection.Query<TEntity>(sql, dynParms).FirstOrDefault();

//             //TODO:Atilla => Asagisi daha sonra calisir hale getirilecek
//             var res = Connection.Query(sql, dynParms).FirstOrDefault() as IDictionary<string, object>;

//             if (res == null) return null;

//             return res as TEntity;
//         }

//         /// <summary>
//         /// Verilen objeyi veritabaninda gunceller
//         /// Objede bir degisiklik yok ise, guncelleme yapmaz
//         /// </summary>
//         /// <param name="obj">Guncellenecek obje</param>
//         /// <returns></returns>
//         public virtual int Update(TEntity obj)
//         {
//             var proxy = obj as IProxy;
//             if (proxy != null)
//             {
//                 if (!proxy.IsDirty) return 0;
//             }

//             BaseEntity baseEntity = setDefaults(obj, SqlType.Update);
//             string sql = UpdateSqlCache(typeof(TEntity));
//             DynamicParameters paramList = GenerateDynamicParameters(baseEntity, SqlType.Update);
//             LogSql(sql, paramList); //TODO:Atilla => Daha sonra iptal edilecek
//             return Connection.Execute(sql, paramList);
//         }

//         /// <summary>
//         /// Id si verilen kaydin ISACTIVE=0, UPDATEDATE=SYSDATE, UPDATEDBY=User.Id alanlarini gunceller
//         /// </summary>
//         /// <param name="id"></param>
//         /// <returns>Etkilenen kayit sayisi</returns>
//         public virtual int Delete(int id)
//         {

//             return Connection.Execute($@"UPDATE {GetTableName(typeof(TEntity))}
//                                                          SET ISACTIVE=0, UPDATEDATE=SYSDATE, UPDATEDBY=:UpdatedBy
//                                                          WHERE Id=:Id", new { Id = id, UpdatedBy = userId, UpdateDate = DateTime.Now });
//         }

//         /// <summary>
//         /// verilen BaseDto objesine gore kayit listesini doner,
//         /// ilk sayfada pagingDto.count setlenir.
//         /// </summary>
//         /// <param name="baseDto"></param>
//         /// <param name="pagingDto"></param>
//         /// <returns></returns>
//         public virtual IEnumerable<TEntity> List(BaseDto baseDto, ref PagingDto pagingDto)
//         {
//             if (baseDto == null) return null;
//             string sqlSelect;
//             string sqlWhere;
//             DynamicParameters paramList = GenerateDynamicSearchParameters(baseDto, false, "T1", out sqlSelect, out sqlWhere);
//             var tableName = GetTableName(typeof(TEntity));

//             if (pagingDto?.pageNumber == 1)
//             {
//                 //TODO:Atilla => su anda oracle icin yapilamamis hata veriliyor, yapilinca eklencek.ref= https://github.com/StackExchange/dapper-dot-net/issues/491
//                 //sql = $@"SELECT COUNT(ID) FROM {GetTableName(typeof(TEntity))} {sqlWhere};
//                 //          {sqlSelect} {sqlWhere};";
//                 //using (var multi = Connection.QueryMultiple(sql, param: paramList))
//                 //{
//                 //    pagingDto.count = multi.Read<int>().Single();
//                 //    result = multi.Read<TEntity>().ToList();
//                 //}
//                 pagingDto.count = Count(baseDto);
//             }

//             return Connection.Query<TEntity>($"SELECT * FROM {tableName} T1 {sqlWhere} {OrderBy(pagingDto, "T1")} {PagingSql(pagingDto)}", paramList).ToList();
//         }

//         #endregion CRUD operations

//         #region Extensions

//         public virtual int InsertAll(List<TEntity> list)
//         {
//             string sql = InsertSqlCache(typeof(TEntity), false);

//             List<DynamicParameters> paramList = new List<DynamicParameters>();

//             for (int i = 0; i < list.Count; i++)
//             {
//                 BaseEntity baseEntity = list.ElementAt(i) as BaseEntity;
//                 baseEntity = setDefaults(baseEntity, SqlType.Insert);
//                 var dynParams = GenerateDynamicParameters(baseEntity, SqlType.Insert);
//                 paramList.Add(dynParams);
//                 LogSql(sql, dynParams); //TODO:Atilla => Daha sonra iptal edilecek
//             }
//             return Connection.Execute(sql, paramList);
//         }

//         public virtual int UpdateAll(List<TEntity> list)
//         {
//             for (int i = 0; i < list.Count; i++)
//             {
//                 Update(list[i]);//TODO:Atilla Daha sonra degistirilicek http://stackoverflow.com/questions/32635347/how-to-pass-multiple-records-to-update-with-one-sql-statement-in-dapper
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
//                 //SetNotDirty(result);
//                 return result as TEntity;
//             }
//             else
//             {
//                 Update(obj);
//                 IDictionary<string, object> result = obj as IDictionary<string, object>;
//                 //SetNotDirty(result);
//                 return result as TEntity;
//             }
//         }

//         /// <summary>
//         /// Id leri verilen kayitlari getirir.
//         /// </summary>
//         /// <param name="id"></param>
//         /// <returns>Kayitlari getirir.</returns>
//         public virtual IEnumerable<TEntity> Get(int[] id)
//         {
//             return Connection.Query<TEntity>($@"SELECT * FROM {GetTableName(typeof(TEntity))} WHERE ISACTIVE=1 AND ID IN :ID ", new { Id = id });
//         }

//         /// <summary>
//         /// Id leri verilen kayitlarin ISACTIVE=0, UPDATEDATE=SYSDATE, UPDATEDBY=User.Id alanlarini gunceller
//         /// </summary>
//         /// <param name="id"></param>
//         /// <returns>Etkilenen kayit sayisini doner</returns>
//         public virtual int Delete(int[] id)
//         {
//             return Connection.Execute($@"UPDATE {GetTableName(typeof(TEntity))} SET ISACTIVE=0, UPDATEDATE=SYSDATE, UPDATEDBY=:UpdatedBy WHERE Id IN :Id", new { Id = id, UpdatedBy = userId });
//         }

//         /// <summary>
//         /// Id si verilen kaydi, veritabanindan tamamen siler.
//         /// </summary>
//         /// <param name="id"></param>
//         /// <returns>Etkilenen kayit sayisini doner</returns>
//         public virtual int DeleteHard(int id)
//         {
//             return Connection.Execute($@"DELETE FROM {GetTableName(typeof(TEntity))} WHERE Id=:Id", new { Id = id });
//         }

//         /// <summary>
//         /// Id si verilen kayitlari, veritabanindan tamamen siler.
//         /// </summary>
//         /// <param name="id"></param>
//         /// <returns>Etkilenen kayit sayisini doner</returns>
//         public virtual int DeleteHard(int[] id)
//         {
//             return Connection.Execute($@"DELETE FROM {GetTableName(typeof(TEntity))} WHERE Id IN =:Id", new { Id = id });
//         }

//         /// <summary>
//         /// Tum kayitlari getirir.
//         /// </summary>
//         /// <returns>Tum kayitlar</returns>
//         public virtual IEnumerable<TEntity> ListAll()
//         {
//             var type = typeof(TEntity);
//             var cacheType = typeof(List<TEntity>);

//             string sql = $"SELECT * FROM {GetTableName(typeof(TEntity))} WHERE ISACTIVE=1";

//             if (!IsInterface(type)) return Connection.Query<TEntity>(sql);
//             //TODO:Atilla => Asagisi daha sonra calisir hale getirilecek
//             var result = Connection.Query(sql);

//             return SetNotDirty(result);
//         }

//         /// <summary>
//         /// Kayit sayisini getirir.
//         /// </summary>
//         /// <returns>kayit sayisini doner</returns>
//         public virtual int Count()
//         {
//             return Connection.QuerySingleOrDefault($"SELECT count(Id) FROM {GetTableName(typeof(TEntity))} WHERE ISACTIVE=1 ");
//         }

//         //public virtual int Count(Predicate where)
//         //{
//         //    return Connection.QuerySingleOrDefault($"SELECT count(Id) FROM {GetTableName(typeof(TEntity))} where {where}");
//         //}

//         public virtual int Count(BaseDto baseDto)
//         {
//             if (baseDto == null) return 0;
//             string sqlSelect;
//             string sqlWhere;
//             DynamicParameters paramList = GenerateDynamicSearchParameters(baseDto, true, null, out sqlSelect, out sqlWhere);
//             return Connection.Query<int>($"{sqlSelect} {sqlWhere}", paramList).SingleOrDefault();
//         }

//         #endregion Extensions

//         #region Helpers

//         /// <summary>
//         /// Veritabanina gidecek objenin default parametrelerini setler CREATEDBY,CREATEDATE,UPDATEDATEUPDATEDBY
//         /// </summary>
//         /// <param name="obj">Veritabanina gidecek obje</param>
//         /// <param name="stlType">sql cumlesi turu</param>
//         /// <returns></returns>
//         private BaseEntity setDefaults(Object obj, SqlType stlType)
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

//         /// <summary>
//         /// verilen objenin DynamicParameters verisini Sql tpine gore hazirlar
//         /// </summary>
//         /// <param name="obj"></param>
//         /// <param name="SqlType">olusturulacak DynamicParameter turu.</param>
//         /// <returns></returns>
//         private DynamicParameters GenerateDynamicParameters(object obj, SqlType sqlType)
//         {
//             Type type = typeof(TEntity);
//             var tableName = GetTableName(type);

//             if (sqlType.Equals(SqlType.Select)) type = obj.GetType();
//             var paramList = new DynamicParameters();
//             IEnumerable<PropertyInfo> properties;

//             if (sqlType.Equals(SqlType.Insert))
//                 properties = TypePropertiesCache(type).Where(p => !p.Name.ToUpperInvariant().Equals("ID")).ToList();
//             else if (sqlType.Equals(SqlType.Select))
//                 properties = TypePropertiesCache(type).Where(p => !p.Name.ToUpperInvariant().Equals("CREATEDBY") &&
//                                                    !p.Name.ToUpperInvariant().Equals("CREATEDATE") &&
//                                                    !p.Name.ToUpperInvariant().Equals("UPDATEDATE") &&
//                                                    !p.Name.ToUpperInvariant().Equals("UPDATEDBY")).ToList();
//             else
//                 properties = TypePropertiesCache(type);

//             for (var i = 0; i < properties.Count(); i++)
//             {
//                 var property = properties.ElementAt(i);
//                 var propertyValue = property.GetValue(obj);

//                 if (property.PropertyType.BaseType == typeof(BaseEntity)) continue;

//                 if ((sqlType.Equals(SqlType.Insert) || sqlType.Equals(SqlType.Update)) && (propertyValue is ICollection || propertyValue is IEnumerable<object>)) continue;

//                 if (propertyValue == null)
//                 {
//                     paramList.Add(name: property.Name.ToUpperInvariant(), value: null, dbType: oracleDbTypeMap[property.PropertyType], direction: ParameterDirection.Input);
//                     continue;
//                 }

//                 if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(Boolean) || property.PropertyType == typeof(bool?) || property.PropertyType == typeof(Boolean?))
//                 {
//                     paramList.Add(name: property.Name.ToUpperInvariant(), value: (bool)propertyValue ? 1 : 0, dbType: DbType.Decimal, direction: ParameterDirection.Input);
//                     continue;
//                 }

//                 paramList.Add(name: property.Name.ToUpperInvariant(), value: propertyValue, dbType: oracleDbTypeMap[property.PropertyType], direction: ParameterDirection.Input);
//             }

//             if (sqlType.Equals(SqlType.Insert)) paramList.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

//             return paramList;
//         }

//         /// <summary>
//         /// verilen objenin DynamicParameters verisini Sql tpine gore hazirlar
//         /// </summary>
//         /// <param name="baseDto"></param>
//         /// <param name="sqlSelect"></param>
//         /// <returns></returns>
//         private DynamicParameters GenerateDynamicSearchParameters(BaseDto baseDto, bool isCount, string alias, out string sqlSelect, out string sqlWhere)
//         {
//             Type type = baseDto.GetType();
//             var tableName = GetTableName(typeof(TEntity));
//             var sbColumnList = new StringBuilder(null);

//             var paramList = new DynamicParameters();
//             List<PropertyInfo> properties = TypePropertiesCache(type).Where(p => !p.Name.ToUpperInvariant().Equals("CREATEDBY") &&
//                                                   !p.Name.ToUpperInvariant().Equals("CREATEDATE") &&
//                                                   !p.Name.ToUpperInvariant().Equals("UPDATEDATE") &&
//                                                   !p.Name.ToUpperInvariant().Equals("UPDATEDBY") &&
//                                                   !p.Name.ToUpperInvariant().Equals("ISACTIVE")).ToList();

//             if (string.IsNullOrEmpty(alias)) alias = "T1";

//             for (var i = 0; i < properties.Count(); i++)
//             {
//                 var property = properties.ElementAt(i);
//                 var propertyValue = property.GetValue(baseDto);

//                 if (propertyValue == null) continue;

//                 if (property.PropertyType.BaseType == typeof(BaseEntity)) continue;

//                 if (propertyValue is ICollection || propertyValue is IEnumerable<object>) continue;

//                 if (property.PropertyType.BaseType == typeof(BaseDto)) continue;

//                 if (IsNumeric(property.PropertyType) && propertyValue.ToString().Equals("0")) continue;

//                 if (sbColumnList.Length > 0) sbColumnList.Append(" AND ");

//                 if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(Boolean) || property.PropertyType == typeof(bool?) || property.PropertyType == typeof(Boolean?))
//                 {
//                     paramList.Add(name: alias + "_" + property.Name.ToUpperInvariant(), value: (bool)propertyValue ? 1 : 0, dbType: DbType.Decimal, direction: ParameterDirection.Input);
//                     sbColumnList.AppendFormat("{0}.{1}=:{0}_{1}", alias, property.Name.ToUpperInvariant());
//                     continue;
//                 }

//                 if (property.PropertyType == typeof(string) || property.PropertyType == typeof(String))
//                     sbColumnList.AppendFormat("{0}.{1} LIKE :{0}_{1} || '%'", alias, property.Name.ToUpperInvariant());
//                 else
//                     sbColumnList.AppendFormat("{0}.{1}=:{0}_{1}", alias, property.Name.ToUpperInvariant());

//                 paramList.Add(name: alias + "_" + property.Name.ToUpperInvariant(), value: propertyValue, dbType: oracleDbTypeMap[property.PropertyType], direction: ParameterDirection.Input);
//             }

//             sqlWhere = sbColumnList.Length > 0 ? $" WHERE {alias}.ISACTIVE=1 AND {sbColumnList} " : $" WHERE {alias}.ISACTIVE=1";

//             if (isCount)
//                 sqlSelect = $@"SELECT COUNT({alias}.ID) FROM {tableName} {alias}";
//             else
//                 sqlSelect = $@"SELECT * FROM {tableName} {alias}";

//             return paramList;
//         }

//         /// <summary>
//         /// Obje tipine gore sql hazirlar,
//         /// </summary>
//         /// <param name="type"></param>
//         /// <param name="isReturnsPk">insert sonucunda id degerinin donup donmeyecegini belirler</param>
//         /// <returns></returns>
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
//                 sbParameterList.AppendFormat(":{0}", property.Name.ToUpperInvariant());

//                 if (i < allProperties.Count - 1)
//                 {
//                     sbColumnList.Append(", ");
//                     sbParameterList.Append(", ");
//                 }
//             }

//             string insertSqlNew = "";

//             if (isReturnsPk)
//             {
//                 insertSqlNew = $@"
//                                 BEGIN
//                                     INSERT INTO {tableName.ToUpperInvariant()} ({sbColumnList})
//                                            VALUES ({sbParameterList})
//                                     RETURNING ID INTO :Id;
//                                 END;
//                                 ";
//             }
//             else
//             {
//                 insertSqlNew = $@"INSERT INTO {tableName.ToUpperInvariant()} ({sbColumnList}) VALUES ({sbParameterList})";
//             }

//             Cache($"InsertSqls-{isReturnsPk}")[type.Name] = insertSqlNew;

//             return insertSqlNew;
//         }

//         /// <summary>
//         /// Obje tipine gore Update sql cumlesini hazirlar.
//         /// </summary>
//         /// <param name="type"></param>
//         /// <returns></returns>
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
//                 sbColumnList.AppendFormat("{0}= :{0}", property.Name.ToUpperInvariant());

//                 if (i < allProperties.Count - 1)
//                 {
//                     sbColumnList.Append(", ");
//                 }
//             }

//             string updateSqlsNew = $@"UPDATE {tableName.ToUpperInvariant()}
//                                       SET {sbColumnList}
//                                       WHERE Id= :ID";

//             Cache("UpdateSqls")[type.Name] = updateSqlsNew;

//             return updateSqlsNew;
//         }

//         private static void LogSql(string sql, DynamicParameters paramList)
//         {
//             try
//             {
//                 string sqlStr = sql;
//                 foreach (string pName in paramList.ParameterNames)
//                 {
//                     var value = paramList.Get<dynamic>(pName);
//                     if (value != null)
//                     {
//                         if (value is string || value is String || value is DateTime)
//                         {
//                             sqlStr = sqlStr.Replace($":{pName}", $"'{value}'");
//                         }
//                         else
//                         {
//                             sqlStr = sqlStr.Replace($":{pName}", $"{Convert.ToString(value)}");
//                         }
//                     }
//                     else
//                     {
//                         sqlStr = sqlStr.Replace($":{pName}", "null");
//                     }
//                 }

//                 Console.WriteLine(sqlStr);
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine("SQL loglama isleminde hata olustu!", e);
//             }
//         }

//         private static List<TEntity> SetNotDirty(IEnumerable<dynamic> result)
//         {
//             var list = new List<TEntity>();
//             var type = typeof(TEntity);

//             foreach (IDictionary<string, object> res in result)
//             {
//                 list.Add(res as TEntity);
//             }
//             return list;
//         }

//         // private static TEntity SetNotDirty(IDictionary<string, object> result)
//         // {
//         //     var obj = Utils.Dapper.ProxyGenerator.GetInterfaceProxy<TEntity>();

//         //     foreach (var property in TypePropertiesCache(typeof(TEntity)))
//         //     {
//         //         var val = result[property.Name];
//         //         property.SetValue(obj, Convert.ChangeType(val, property.PropertyType), null);
//         //     }

//         //     ((IProxy)obj).IsDirty = false;//Degistirilmemis obje

//         //     return obj;
//         // }

//         public DynamicParameters GenerateSearchDynamicParameters(BaseDto baseDto, out string sqlWhere)
//         {
//             return GenerateSearchDynamicParameters(baseDto, null, out sqlWhere);
//         }

//         public DynamicParameters GenerateSearchDynamicParameters(BaseDto baseDto, string alias, out string sqlWhere)
//         {
//             DynamicParameters result = null;
//             string select = "";
//             result = GenerateDynamicSearchParameters(baseDto, false, alias, out select, out sqlWhere);
//             sqlWhere = sqlWhere.Substring(6);
//             return result;
//         }

//         public DynamicParameters GenerateSearchDynamicParameters(Dictionary<string, BaseDto> baseDtoList, out string sqlWhere)
//         {
//             DynamicParameters result = new DynamicParameters();
//             StringBuilder sbSqlWhere = new StringBuilder();

//             foreach (string alias in baseDtoList.Keys)
//             {
//                 string where = "";
//                 BaseDto baseDto = baseDtoList[alias];
//                 if (baseDto != null) result.AddDynamicParams(GenerateSearchDynamicParameters(baseDto, alias, out where));
//                 if (sbSqlWhere.Length > 1 && where.Length > 0) sbSqlWhere.Append(" AND ");
//                 sbSqlWhere.Append(where);
//             }

//             sqlWhere = sbSqlWhere.ToString();

//             return result;
//         }

//         public string PagingSql(PagingDto pagingDto)
//         {
//             return $" OFFSET ({pagingDto.pageNumber}-1) * {pagingDto.pageSize} ROWS FETCH NEXT {pagingDto.pageSize} ROWS ONLY";
//         }

//         public string OrderBy(PagingDto pagingDto)
//         {
//             return OrderBy(pagingDto, null);
//         }

//         public string OrderBy(PagingDto pagingDto, string alias)
//         {
//             if (!string.IsNullOrEmpty(alias))
//             {
//                 alias = alias + ".";
//             }
//             else
//                 alias = "";

//             return (string.IsNullOrEmpty(pagingDto.orderBy) ? "" : $" ORDER BY {alias}{pagingDto.orderBy} {pagingDto.order}");
//         }

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
//             //return type.IsInterface;
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


//         private static bool IsNumeric(Type type)
//         {
//             if (type == typeof(Int16) ||
//                 type == typeof(Int32) ||
//                 type == typeof(Int64) ||
//                 type == typeof(Decimal) ||
//                 type == typeof(Double) ||
//                 type == typeof(Byte) ||
//                 type == typeof(SByte) ||
//                 type == typeof(UInt16) ||
//                 type == typeof(UInt32) ||
//                 type == typeof(UInt64) ||
//                 type == typeof(Single) ||
//                 type == typeof(float)
//                 )
//             {
//                 return true;
//             }
//             return false;
//         }

//         #endregion Helpers

//     }


// }