using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Assignment.Models;
using Assignment.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Assignment.Controllers
{
  public class TradeController : Controller
  {
    private readonly TradingOptions _options;
    private readonly IFinnhubService _finnhubService;
    public TradeController(IOptions<TradingOptions> options, IFinnhubService finnhubService)
    {
      _options = options.Value;
      _finnhubService = finnhubService;
    }

    [Route("/")]
    public IActionResult Index()
    {

      return View();
    }

    [HttpPost]
    public async Task<IActionResult> CompanyDetail(string? stockSymbol)
    {
      if (string.IsNullOrEmpty(stockSymbol))
      {
        return View("NullOrEmptyInput");
      }

      Dictionary<string, object> companyProfileData = await _finnhubService.GetCompanyProfile(stockSymbol);
      Dictionary<string, object> companyStockPriceQuoteData = await _finnhubService.GetStockPriceQuote(stockSymbol);

      if (companyProfileData.Count <= 0)
      {
        return View("WrongInput");
      }

      CompanyProfile companyProfile = new CompanyProfile()
      {
        Country = companyProfileData["country"].ToString(),
        Currency = companyProfileData["currency"].ToString(),
        Exchange = companyProfileData["exchange"].ToString(),
        IPO = Convert.ToDateTime(companyProfileData["ipo"].ToString()),
        MarketCapitalization = Convert.ToDouble(companyProfileData["marketCapitalization"].ToString(),CultureInfo.InvariantCulture),
        Name = companyProfileData["name"].ToString(),
        Phone = companyProfileData["phone"].ToString(),
        ShareOutstanding = Convert.ToDouble(companyProfileData["shareOutstanding"].ToString(), CultureInfo.InvariantCulture),
        Ticker = companyProfileData["ticker"].ToString(),
        WebUrl = companyProfileData["weburl"].ToString(),
        Logo = companyProfileData["logo"].ToString(),
        FinnhubIndustry = companyProfileData["finnhubIndustry"].ToString()
      };

      StockPriceQuote stockPriceQuote = new StockPriceQuote()
      {
        CurrentPrice = Convert.ToDouble(companyStockPriceQuoteData["c"].ToString(), CultureInfo.InvariantCulture),
        Change = Convert.ToDouble(companyStockPriceQuoteData["d"].ToString(), CultureInfo.InvariantCulture),
        PercentChange = Convert.ToDouble(companyStockPriceQuoteData["dp"].ToString(), CultureInfo.InvariantCulture),
        HighestPriceOfTheDay = Convert.ToDouble(companyStockPriceQuoteData["h"].ToString(), CultureInfo.InvariantCulture),
        LowestPriceOfTheDay = Convert.ToDouble(companyStockPriceQuoteData["l"].ToString(), CultureInfo.InvariantCulture),
        OpenPriceOfTheDay = Convert.ToDouble(companyStockPriceQuoteData["o"].ToString(), CultureInfo.InvariantCulture),
        PreviousClosePrice = Convert.ToDouble(companyStockPriceQuoteData["pc"].ToString(), CultureInfo.InvariantCulture)
      };

      CompanyDetails companyDetails = new CompanyDetails()
      {
        CompanyProfile = companyProfile,
        StockPriceQuote = stockPriceQuote
      };

      return View(companyDetails);
    }
  }
}
