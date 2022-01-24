using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GoldCalc.Api.Models;

namespace GoldCalc.Api.Controllers
{
    [Route("api/v1/goldcalc")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GoldCalcController : ControllerBase
    {
       
   
        private readonly ILogger _logger;

        public GoldCalcController(ILogger<GoldCalcController> logger)
        {
          
            this._logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Post(GoldModel goldModel)
        {
            try
            {
               
               if(goldModel != null)
                {
                    if(goldModel.Discount.HasValue)
                    {
                        var amount = goldModel.Rate * goldModel.Weight;
                        var totalAmount = amount - (amount * goldModel.Discount.Value / 100);
                        return this.Ok(new { TotalAmount = totalAmount });
                    }
                    else
                    {
                        var amount = goldModel.Rate * goldModel.Weight;
                       return this.Ok(new { TotalAmount = amount });
                    }
                   
                    this._logger.LogInformation("Calculated Rate");
                 
                    return this.Ok(new { Message = "Insufficient Data", Data ="" });
                    
                }

                return this.Ok(new { Message = "Insufficient Data", Data = "" });
            }
            catch (Exception ex)
            {
                this._logger.LogWarning("Error : " + ex.Message);
                return this.NotFound(new {Message = "Something went wrong" });
                
            }
        }
    }
}
