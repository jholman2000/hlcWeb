using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Http;
using System.Web.Script.Serialization;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Configuration;

namespace hlcWeb.Controllers.Api
{
    public class BaseController : ApiController
    {
        private SqlConnection _conn;
        private readonly Dictionary<string, object> _parameters;

        public BaseController()
        {
            _conn = new SqlConnection(GetConnectionString());
            _parameters = new Dictionary<string, object>();
        }

        protected SqlConnection Connection
        {
            get
            {
                if (string.IsNullOrEmpty(_conn?.ConnectionString))
                {
                    _conn = new SqlConnection(GetConnectionString());
                }
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
                    case "LOCAL":
                        connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\CorpApps\Sandbox\hlcWeb\hlcWeb\App_Data\hlcWeb_local.mdf;Integrated Security=True";
                        break;

                    default:
                        // Connection string is stored in Azure AppSettings section, which gets injected into
                        // Web.config
                        connString = ConfigurationManager.AppSettings["HLC_CONNECTION"] ?? Environment.GetEnvironmentVariable("HLC_CONNECTION");

                        break;
                }

                if (connString != null)
                    cache.Add("HLCConnection", connString,
                        new CacheItemPolicy {Priority = CacheItemPriority.NotRemovable});
            }

            return connString;
        }

        #region Generic SQL calls

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
            using (Connection)
            {
                Connection.Open();
                return Connection.Query<T>(sql).ToList();
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
                sql = tableName == "User" ? $"select * from hlc_User where UserId='{id}'" : $"select * from hlc_{tableName} where ID='{id}'";                

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

                if (_parameters.Any())
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

                return Connection.Execute(sql, commandType: CommandType.Text);
        }

        protected void AddParameter(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _parameters.Add(name, value);
        }

        protected void ClearParameters()
        {
            _parameters.Clear();
        }

        #endregion

        #region Exception processing

        protected void LogException(Exception ex, object data = null)
        {
            try
            {
                var json = new JavaScriptSerializer().Serialize(data);

                var error = new HlcError()
                {
                    DateError = DateTime.Now,
                    Message = ex.Message.Length < 5000 ? ex.Message : ex.Message.Substring(0, 5000),
                    Data = json.Length < 5000 ? json : json.Substring(0,5000),
                    StackTrace = ex.StackTrace.Length < 5000 ? ex.StackTrace : ex.StackTrace.Substring(0,5000)
                };

                _conn.Insert(error);
            }
            catch 
            {
                // ignored
            }
        }
        #endregion
    }

    [Table("hlc_ErrorLog")]
    internal class HlcError
    {
        public DateTime DateError { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public string StackTrace { get; set; }
    }
}
