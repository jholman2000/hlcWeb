using System.Web.Http;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class UsersController : BaseController
    {
        public UsersController()
        {

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
