using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiSample.Models;
using WebApiSample.Services;

namespace WebApiSample.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserServices xservices;

        public UserController(UserServices xservices)
        {
            this.xservices = xservices;

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<List<user>> Login(string user, string pwd)
        {
            var ret = await xservices.Login(user, pwd);
            return ret;
        }
    }
}

