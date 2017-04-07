using System.Web.Http;
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
        public IHttpActionResult Get()
        {
            string sql = "select * from hlc_User";

            var results = GetListFromSql<User>(sql);

            if (results == null)
                return NotFound();

            return Ok(results);
        }

        public IHttpActionResult Get(string id)
        {
            var results = GetMemberFromId<User>(id);

            if (results == null)
                return NotFound();

            return Ok(results);
        }

    }
}
