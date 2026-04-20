using Microsoft.AspNetCore.Mvc;
using PROG7311_GLMSApp.Services;

namespace PROG7311_GLMSApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : Controller
    {
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        public IActionResult Index()
        {
            return View();
        }
      
        [HttpPost("convert")]
        public async Task<IActionResult> ConvertCurrency(double amount)
        {           
            var exchangeRate = await _currencyService.ConvertCurrencyAsync(amount);
            if (exchangeRate != null)
            {
                return Ok(exchangeRate);
            }
            return NotFound("Unable to convert currency.");

        }
    }
}
