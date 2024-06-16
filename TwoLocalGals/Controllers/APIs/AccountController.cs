using Nexus;
using System.Web.Http;
using TwoLocalGals.DTO;

namespace TwoLocalGals.APIs
{
    [RoutePrefix("api/v1")]
    public class AccountController : ApiController
    {
        [HttpPost]
        [Route("user/login")]
        public IHttpActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null) return BadRequest("Invalid user data");

            UserStruct user = Database.ValidateUser(request.Username.Trim(), request.Password ?? "");
            if (user.access > 0)
            {
                var token = JwtManager.GenerateToken(request.Username, user.contractorID, user.access);
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpGet]
        [Route(("users/{userID}"))]
        public IHttpActionResult GetUserById(string userID)
        {
            Users user = Database.GetUserById(userID);

            return Ok(user);
        }





    }
}