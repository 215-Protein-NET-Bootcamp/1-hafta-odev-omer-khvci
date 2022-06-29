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
            var interestRate = _configuration.GetValue<double>("InterestRate")/100;
            var totalInterest = (Math.Pow(1 + interestRate, model.ExpiryMonth)-1);
            var totalAmount = (((totalInterest + 1) * model.RequestedMoney) / (totalInterest * 100))*model.ExpiryMonth;
            var response = new ResponseModel()
            {
                TotalAmount = Math.Round(totalAmount,2),
                TotalInterest = Math.Round(totalInterest*model.RequestedMoney,2),
            };
            return Task.FromResult(response);
        }
        [HttpPost("MonthlyPayment")]
        public Task<List<ReponseListModel>> PaymentList([FromBody] RequestModel model)
        {
            var interestRate = _configuration.GetValue<double>("InterestRate")/100;
            var totalInterest = (Math.Pow(1 + interestRate, model.ExpiryMonth) - 1);
            var monthIsterest = model.RequestedMoney * interestRate;
            var monthPrice = ((totalInterest + 1) * model.RequestedMoney)/(totalInterest * 100);
            var response = new List<ReponseListModel>();
            for (int i = 1; i <= model.ExpiryMonth; i++)
            {
                monthIsterest = model.RequestedMoney * interestRate;
                model.RequestedMoney = model.RequestedMoney - (monthPrice - monthIsterest);
                response.Add(new ReponseListModel() { Month = i + ".Taksit", Price = Math.Round(monthPrice, 2), RemainingPayment = Math.Round(model.RequestedMoney, 2), MonthlyInterestPaid = Math.Round(monthIsterest, 2)  });
            }

            return Task.FromResult(response);


        }

    }
}
