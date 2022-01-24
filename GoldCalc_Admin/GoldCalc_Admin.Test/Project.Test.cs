using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using GoldCalc.Api.Controllers;
using GoldCalc.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldCalc.Test
{
    public class ProjectTest
    {
        private GoldCalcController controller;
        private Mock<ILogger<GoldCalcController>> loggerMock;
      
        [SetUp]
        public void Setup()
        {
            
           
            loggerMock = new Mock<ILogger<GoldCalcController>>();
            controller = new GoldCalcController(loggerMock.Object);
        }

        [Test]
        public async Task Test1()
        {
            var req = new GoldModel { Rate = 3000, Weight = 10 };
            var res = await controller.Post(req);
            var k = res as OkObjectResult;
            
            Assert.IsAssignableFrom<OkObjectResult>(res);
            Assert.IsNotNull(k.Value);
   
        }
        
        [Test]
        public async Task Test2()
        {
            var req = new GoldModel { Rate = 3000, Weight=2, Discount=1 };
            var res = await controller.Post(req);
            var k = res as OkObjectResult;
            
            Assert.IsAssignableFrom<OkObjectResult>(res);
            var prop = k.Value.GetType().GetProperty("TotalAmount");
            var val = prop.GetValue(k.Value) as double?;
            Assert.AreEqual(5940.0d, val);

        }


        
    }
}