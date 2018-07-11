/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:09:34 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-04-16 15:50:21
 */

using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Core.Framework.Service;
using Microsoft.Data.Sqlite;
using Core.Framework.Repository;

namespace Core.Framework.Middleware
{
    public class DatabaseContextMiddleware
    {
        private readonly InvokeDelegate _next;

        public DbContextOptions DbContextOptions { get; set; }

        public DatabaseContextMiddleware(InvokeDelegate next, Action<DbContextOptions> setupAction)
        {
            this.DbContextOptions = new DbContextOptions();
            setupAction.Invoke(this.DbContextOptions);
            _next = next;
        }
        public async Task Invoke(InvokeContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            object[] customAttributes = (object[])context.Request.Properties["CustomAttributes"];
            DatabaseConnection databaseConnection = customAttributes.FirstOrDefault(t => t.GetType() == typeof(DatabaseConnection)) as DatabaseConnection;
            bool isDatabaseConnectionEnabled = databaseConnection == null ? true : databaseConnection.IsEnabled;

            if (!isDatabaseConnectionEnabled)
            {
                await _next.Invoke(context);
            }
            else
            {
                var appContext = Application.Current.Context;
                Transactional transactional = customAttributes.FirstOrDefault(t => t.GetType() == typeof(Transactional)) as Transactional;
                bool isTransactionEnabled = transactional == null ? false : transactional.IsEnabled;

                DbConnection connection = GetDbConnection(DbContextOptions.DbProviderName);
                DatabaseContext ctx = null;

                if (isTransactionEnabled)
                {
                    connection.ConnectionString = DbContextOptions.WriteConnectionString;

                    using (ctx = new DatabaseContext(true, connection))
                    {
                        ctx.DbProviderName = DbContextOptions.DbProviderName;
                        ServiceContext serviceContext = (ServiceContext)context.Request.Properties["ServiceContext"];
                        serviceContext.AddItem("DatabaseContext", ctx);
                        await _next.Invoke(context);

                        InvokeResult invokeResult = context.Result;

                        if (invokeResult?.Value != null)
                        {
                            dynamic result = invokeResult.Value;
                            if (result.GetType().Name == typeof(ServiceResponse<>).Name)
                            {
                                if (result?.IsSuccess == true)
                                    ctx.Commit();
                                else
                                    ctx.Transaction.Rollback();
                            }
                            else
                            {
                                ctx.Commit();
                            }

                        }
                        else
                        {
                            ctx.Transaction.Rollback();
                        }
                    }
                }
                else
                {
                    connection.ConnectionString = DbContextOptions.ReadConnectionString;

                    using (ctx = new DatabaseContext(false, connection))
                    {
                        ctx.DbProviderName = DbContextOptions.DbProviderName;
                        ServiceContext serviceContext = (ServiceContext)context.Request.Properties["ServiceContext"];
                        serviceContext.AddItem("DatabaseContext", ctx);

                        await _next.Invoke(context);
                    }
                }
            }
        }

        private DbConnection GetDbConnection(string DATABASE_PROVIDER_NAME)
        {
            DbConnection result = null;
            try
            {
                switch (DATABASE_PROVIDER_NAME)
                {
                    case "System.Data.SqlClient":
                        DbConnection connSql = null;
                        result = connSql;
                        break;
                    default:
                        DbConnection connSqlite = new SqliteConnection();
                        result = connSqlite;
                        break;
                }
            }
            catch (System.Exception)
            {
                throw new ServiceException($"Cannot create Repository with providerName ={DbContextOptions.DbProviderName}");
            }

            if (result == null)
                throw new ServiceException($"Cannot create Repository with providerName ={DbContextOptions.DbProviderName}");

            return result;
        }

    }

    public static class DatabaseContextMiddlewareExtension
    {
        public static IApplicationBuilder UseDatabaseContext(this IApplicationBuilder app, Action<DbContextOptions> setupAction)
        {
            app.UseMiddleware<DatabaseContextMiddleware>(setupAction);
            return app;
        }
    }


    public class DbContextOptions
    {
        public string DbProviderName { get; set; }
        public string WriteConnectionString { get; set; }
        public string ReadConnectionString { get; set; }
    }

    public class DatabaseConnection : System.Attribute
    {
        public bool IsEnabled { get; set; }

        public DatabaseConnection()
        {
            IsEnabled = true;
        }

        public DatabaseConnection(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }

    public class Transactional : System.Attribute
    {
        public bool IsEnabled { get; set; }
        public Transactional()
        {
            IsEnabled = true;
        }

        public Transactional(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}