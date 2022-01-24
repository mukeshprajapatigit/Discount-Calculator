using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using GoldCalc.Api.Models;

using GoldCalc.Util;

namespace GoldCalc.Api.Controllers
{
    [Route("api/v1/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        
     
        private readonly IConfiguration configuration;
        private readonly ILogger<AccountController> logger;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration)
        {
           
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            
            try
            {
                login.Password = login.Password.ToHMACSHA1();
                var result = await LoginS(login);
                this.logger.LogInformation($"{login.UserName} looged in successfully");
                return this.Ok(new { Message = "Logged In Successfully", Data = result });
            }
            catch (Exception ex)
            {
                this.logger.LogWarning("Error : " + ex.Message);
                return this.NotFound(new { Message = "Something went wrong" });

            }
        }
        
     


        private async Task<Login> LoginS(Login login)
        {
            try
            {
                var userName = configuration.GetValue<string>("User:name");
                var pass = configuration.GetValue<string>("User:password");
                
              
                if (userName == login.UserName && pass== login.Password)
                {
                    var tokenHandeler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(configuration.GetSection("Credentials:JwtSecretKey").ToString());

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "Admin"),
                            new Claim(ClaimTypes.Version, "V1"),
                            new Claim("UserName", login.UserName),
                            new Claim("ExpDate", DateTime.UtcNow.AddDays(2).ToShortDateString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(2),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
                    };

                    var token = tokenHandeler.CreateToken(tokenDescriptor);
                    login.Token = tokenHandeler.WriteToken(token);
                    login.Password = null;
                    login.Success = true;
                    return login;
                }
                else
                {
                    login.Token = null;
                    login.Success = false;
                    login.Password = null;
                    return login;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
