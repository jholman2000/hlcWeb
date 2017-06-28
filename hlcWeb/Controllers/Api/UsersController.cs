using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Mvc;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class UsersController : BaseController
    {
        public User Logon(string email, string password)
        {
            // User must be IsActive to logon.  Multiple UserIds can have the same EmailAddress for logon
            // as long as they have different passwords associated
            var sql =
                "select * from hlc_User " +
                $"where upper(EmailAddress) = '{email.ToUpper()}' and upper(Password) = '{password.ToUpper()}' " +
                " and IsActive=1";

            var results = GetListFromSql<User>(sql);

            if (results.Count == 0)
                return null;
            
            // User logged on OK.  Update their last logon date
            ExecuteSql($"update hlc_user set DateLastOn = getDate() WHERE UserId='{results[0].UserId}'");

            return results[0];
        }

        public User CheckUserId(User user)
        {
            var sql = $"select UserId, FirstName, LastName from hlc_User where UserId='{user.UserId}'";

            var results = GetListFromSql<User>(sql).FirstOrDefault();

            return results;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/users/search")]
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
                    Connection.Insert(model);
                }
                else
                {
                    Connection.Update(model);
                }
                GetSelectList(true);
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, model);
                return false;
            }
        }

        internal SelectList GetSelectList(bool refresh = false)
        {
            ObjectCache cache = MemoryCache.Default;

            var list = (SelectList)cache["UserSelectList"];

            if (refresh || list == null)
            {
                var items = Search("")
                    .Select(s => new
                    {
                        Text = s.FullName,
                        Value = s.UserId
                    })
                    .ToList();
                list = new SelectList(items, "Value", "Text");

                if (refresh) cache.Remove("UserSelectList");
                cache.Add("UserSelectList", list, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable });
            }

            return list;
        }

    }

}

