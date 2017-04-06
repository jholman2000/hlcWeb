using System.Collections.Generic;
using System.Web.Http;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class UsersController : BaseController
    {
        public UsersController()
        {

        }

        public User Logon(string email, string password)
        {
            var results = base.GetListFromSQL<User>($"SELECT * FROM hlc_User WHERE UPPER(EmailAddress) = '{email.ToUpper()}' AND UPPER(Password) = '{password.ToUpper()}'");

            if (results.Count == 0)
                return null;

            return results[0];
        }
        public IHttpActionResult Get()
        {
            string sql = "select * from hlc_User";

            var results = base.GetListFromSQL<User>(sql);

            if (results == null)
                return NotFound();

            return Ok(results);
        }

        public IHttpActionResult Get(string id)
        {
            var results = base.GetMemberFromId<User>(id);

            if (results == null)
                return NotFound();

            return Ok(results);
        }

    }
}
