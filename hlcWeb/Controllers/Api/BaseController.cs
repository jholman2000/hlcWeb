using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
            //_conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\_Sandbox\hlcWeb\hlcWeb\App_Data\hlcWeb_local.mdf;Integrated Security=True");
            _conn = new SqlConnection(@"Data Source=tcp:quagv1i08c.database.windows.net,1433;Initial Catalog=HLComm;User ID=HLComm@quagv1i08c;Password=HLCnoblood2015");
            _parameters = new Dictionary<string, object>();
        }

        protected List<T> GetListFromSQL<T>(string sql) where T: class
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

        protected List<T> GetListFromSP<T>(string spName)
        {
            if (string.IsNullOrEmpty(spName))
                throw new ArgumentNullException("spName is null or empty");

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
                            default:
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

        public void AddParameter(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name is null or empty");

            _parameters.Add(name, value);
        }
        
        public void ClearParameters()
        {
            _parameters.Clear();
        }
    }
}
