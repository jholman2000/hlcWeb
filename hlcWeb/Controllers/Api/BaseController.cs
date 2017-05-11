using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Http;
using Dapper;

namespace hlcWeb.Controllers.Api
{
    public class BaseController : ApiController
    {
        private SqlConnection _conn;
        private Dictionary<string, object> _parameters;

        public BaseController()
        {
            _conn = new SqlConnection(GetConnectionString());
            _parameters = new Dictionary<string, object>();
        }

        protected SqlConnection Connection
        {
            get
            {
                return _conn;
            }
        }

        private string GetConnectionString()
        {
            ObjectCache cache = MemoryCache.Default;
            var connString = cache["HLCConnection"]?.ToString();

            if (string.IsNullOrEmpty(connString))
            {
                // To develop against the local .mdf, set an APP_ENVIRONMENT=DEV environment variable in Windows and 
                // change the path below
                var environment = Environment.GetEnvironmentVariable("APP_ENVIRONMENT");
                switch (environment)
                {
                    case "DEV":
                        connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\_Sandbox\hlcWeb\hlcWeb\App_Data\hlcWeb_local.mdf;Integrated Security=True";
                        break;

                    default:
                        connString = @"Data Source=tcp:quagv1i08c.database.windows.net,1433;Initial Catalog=HLComm;User ID=HLComm@quagv1i08c;Password=HLCnoblood2015";
                        break;
                }

                cache.Add("HLCConnection", connString, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable });
            }

            return connString;
        }

        protected T GetMemberFromSql<T>(string sql) where T : class
        {
            using (_conn)
            {
                _conn.Open();
                return _conn.Query<T>(sql).FirstOrDefault();
            }

        }

        protected List<T> GetListFromSql<T>(string sql) where T: class
        {
            using (_conn)
            {
                _conn.Open();
                return _conn.Query<T>(sql).ToList();
            }
        }

        protected T GetMemberFromId<T>(int id) where T: class
        {
            using (_conn)
            {
                string tableName = typeof(T).Name;

                string sql = $"select * from hlc_{tableName} where ID={id}";
                
                _conn.Open();

                return _conn.Query<T>(sql).FirstOrDefault();

            }

        }

        protected T GetMemberFromId<T>(string id) where T : class
        {
            using (_conn)
            {
                string tableName = typeof(T).Name;
                string sql;

                //TODO: Consider changing hlc_User.UserId to ID to avoid this check
                if (tableName == "User")
                {
                    sql = $"select * from hlc_User where UserId='{id}'";
                }
                else
                {
                    sql = $"select * from hlc_{tableName} where ID='{id}'";
                }                

                _conn.Open();

                return _conn.Query<T>(sql).FirstOrDefault();

            }

        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        protected List<T> GetListFromSP<T>(string spName)
        {
            if (string.IsNullOrEmpty(spName))
                throw new ArgumentNullException(nameof(spName));

            using (_conn)
            {
                _conn.Open();

                if (_parameters.Count() > 0)
                {
                    var parms = new DynamicParameters();
                    DbType type = new DbType();

                    foreach (var p in _parameters)
                    {
                        switch (p.Value.GetType().Name)
                        {
                            case "Int32":
                                type = DbType.Int32;
                                break;
                            case "String":
                                type = DbType.String;
                                break;
                            case "DateTime":
                                type = DbType.DateTime;
                                break;
                        }
                        parms.Add(p.Key, p.Value, dbType: type, direction: ParameterDirection.Input);
                    }

                    return _conn.Query<T>(sql: spName, param: parms, commandType: CommandType.StoredProcedure).ToList();
                }
                else
                {                                         
                    return  _conn.Query<T>(sql: spName, commandType: CommandType.StoredProcedure).ToList();                        
                }
            }
        }

        protected int ExecuteSql(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentNullException(nameof(sql));

            using (_conn)
            {
                return _conn.Execute(sql, commandType: CommandType.Text);
            }
        }

        protected void AddParameter(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _parameters.Add(name, value);
        }

        protected void ClearParameters()
        {
            _parameters.Clear();
        }


        //TODO: Add method to write out exception to a table
    }
}
