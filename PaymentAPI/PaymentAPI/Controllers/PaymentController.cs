
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PaymentAPI.Models;
using System.Threading.Tasks;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController:ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("TotalMoney")]
        public Task<ResponseModel> TotalMoney([FromBody]RequestModel model)
        {
            var interestRate= _configuration.GetValue<double>("InterestRate");
            var totalInterest = model.RequestedMoney * (interestRate *model.Expiry/36500);
            var totalAmount = totalInterest + model.RequestedMoney;
            var response = new ResponseModel()
            {
                TotalAmount = totalAmount,
                TotalInterest = totalInterest,
            };
            return Task.FromResult(response);
        }

    }
}
