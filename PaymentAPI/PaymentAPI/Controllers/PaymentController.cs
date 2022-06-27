using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PaymentAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("TotalMoney")]
        public Task<ResponseModel> TotalMoney([FromBody] RequestModel model)
        {
            var interestRate = _configuration.GetValue<double>("InterestRate");
            var totalInterest = model.RequestedMoney * (interestRate * model.ExpiryMonth / 120);
            var totalAmount = totalInterest + model.RequestedMoney;
            var response = new ResponseModel()
            {
                TotalAmount = Math.Round(totalAmount,2),
                TotalInterest = Math.Round(totalInterest,2),
            };
            return Task.FromResult(response);
        }
        [HttpPost]
        public Task<List<ReponseListModel>> PaymentList([FromBody] RequestModel model)
        {
            var interestRate = _configuration.GetValue<double>("InterestRate");
            var totalInterest = model.RequestedMoney * (interestRate * model.ExpiryMonth / 120);
            var totalAmount = totalInterest + model.RequestedMoney;
            var a = totalAmount / model.ExpiryMonth;
            var res = new List<ReponseListModel>();
            for (int i = 1; i <= model.ExpiryMonth; i++)
            {
                res.Add(new ReponseListModel() { Month = i + ".Ay", Price = Math.Round(a,2) });
            }

            return Task.FromResult(res);


        }

    }
}
