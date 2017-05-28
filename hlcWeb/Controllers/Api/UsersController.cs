using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class UsersController : BaseController
    {
        public User Logon(string email, string password)
        {
            var results = GetListFromSql<User>($"SELECT * FROM hlc_User WHERE UPPER(EmailAddress) = '{email.ToUpper()}' AND UPPER(Password) = '{password.ToUpper()}'");

            if (results.Count == 0)
                return null;

            return results[0];
        }

        public User CheckUserId(User user)
        {
            var sql = $"select UserID, FirstName, LastName from hlc_User where UserId='{user.UserId}'";

            var results = GetListFromSql<User>(sql).FirstOrDefault();

            return results;
        }

        [HttpGet]
        [Route("api/users/search")]
        public List<User> Search(string search)
        {
            var where = search == "*"
                ? "1=1"
                : $"LastName LIKE '%{search}%' OR " +
                  $"FirstName LIKE '%{search}%' ";

            var sql = "select * from hlc_User " +
                      $" WHERE {where} ORDER BY LastName, FirstName";

            var results = GetListFromSql<User>(sql);

            return results;
        }

        //public IHttpActionResult Get()
        //{
        //    string sql = "select * from hlc_User";

        //    var results = GetListFromSql<User>(sql);

        //    if (results == null)
        //        return NotFound();

        //    return Ok(results);
        //}

        internal User Get(string id)
        {
            var sql = $"select * from hlc_User where UserId='{id}'";

            var results = GetListFromSql<User>(sql).FirstOrDefault();

            return (results);
        }

        internal bool Save(User model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.OriginalUserId))
                {
                    var newId = Connection.Insert(model);
                    return newId > 0;
                }
                return Connection.Update(model);
            }
            catch (Exception ex)
            {
                LogException(ex, model);
                return false;
            }
        }

    }
}
