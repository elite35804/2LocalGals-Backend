using Nexus;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using TwoLocalGals.DTO;

namespace TwoLocalGals.APIs
{
    [RoutePrefix("api/v1")]
    public class AccountController : ApiController
    {
        [HttpPost]
        [Route("user/login")]
        [EnableCors(origins: "*", headers:"*", methods: "*")]
        public IHttpActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null) return BadRequest("Invalid user data");

            UserStruct user = Database.ValidateUser(request.Username.Trim(), request.Password ?? "");
            if (string.IsNullOrEmpty(user.username))
            {
                return BadRequest("Incorrect Username or Password.");

            }
            if (user.access > 0 && user.contractorID > 0)
            {
                var contractorActive = Database.GetContractorByID(user.contractorID);
                if (contractorActive.active)
                {
                    var token = JwtManager.GenerateToken(request.Username, user.contractorID, user.access, user.franchiseMask);
                    return Ok(new { Token = token });
                }
                else
                {
                    return BadRequest("Invalid login attempt. Please contact the main office");
                }
            }
            else
            {
                return BadRequest("Invalid login attempt. Please contact the main office");
            }
        }

        [Authorize]
        [HttpGet]
        [Route(("users/{userID}"))]
        public IHttpActionResult GetUserById(string userID)
        {
            UsersDTO user = Database.GetUserById(userID);

            return Ok(user);
        }





    }
}