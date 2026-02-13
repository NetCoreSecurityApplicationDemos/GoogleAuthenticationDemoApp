using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoogleAuthenticationDemoApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController 
        : ControllerBase
    {
        public SecurityController() { }


        [HttpGet]
        public async Task<string> GetPasswordHash(string passWord="12345678")
        {


            var hasher = new PasswordHasher<object>();
            var hash = hasher.HashPassword(null, passWord);

            return hash;
        }


    }
}
