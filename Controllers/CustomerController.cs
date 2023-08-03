using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using WebApiSample.Models;
using WebApiSample.Services;

namespace WebApiSample.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        CustomerServices xservices;

        public CustomerController(CustomerServices xservices)
        {
            this.xservices = xservices;

        }

        [HttpGet]
        //[Authorize]
        public async Task<List<customer>> Customer()
        {
            var ret = await xservices.Customer();
            return ret;
        }

        [HttpPost]
        //[Authorize]
        public async Task<int> AddCustomer([FromBody] customer _customer)
        {
            var ret = await xservices.AddCustomer(_customer);
            return ret;
        }

        [HttpPut]
        //[Authorize]
        public async Task<int> UpdateCustomer([FromBody] customer _customer)
        {
            var ret = await xservices.UpdateCustomer(_customer);
            return ret;
        }

        [HttpDelete]
        //[Authorize]
        public async Task<int> DeleteCustomer([FromBody] customer _customer)
        {
            var ret = await xservices.DeleteCustomer(_customer);
            return ret;
        }
    }
}
