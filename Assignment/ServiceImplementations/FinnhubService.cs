using System.Text.Json;
using Assignment.ServiceContracts;

namespace Assignment.ServiceImplementations
{
  public class FinnhubService : IFinnhubService
  {
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _token;
    public FinnhubService(IConfiguration configuration, IHttpClientFactory httpHttpClientFactory)
    {
      _configuration = configuration;
      _token = _configuration.GetValue<string>("FinnhubToken");
      _httpClientFactory = httpHttpClientFactory;
    }
    
    public async Task<Dictionary<string, object>> GetCompanyProfile(string stockSymbol)
    {
      using (HttpClient httpClient = _httpClientFactory.CreateClient())
      {
        HttpRequestMessage httpRequest = new()
        {
          Method = HttpMethod.Get,
          RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_token}")
        };

        HttpResponseMessage response = await httpClient.SendAsync(httpRequest);

        string jsonData = new StreamReader(response.Content.ReadAsStream()).ReadToEnd();

        Dictionary<string, object> data = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonData);

        CheckForNullsOrErrors(data);

        return data;
      }
    }

    public async Task<Dictionary<string, object>> GetStockPriceQuote(string stockSymbol)
    {
      using (HttpClient httpClient = _httpClientFactory.CreateClient())
      {
        HttpRequestMessage httpRequest = new()
        {
          RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_token}"),
          Method = HttpMethod.Get
        };

        HttpResponseMessage response = await httpClient.SendAsync(httpRequest);

        var jsonData = new StreamReader(response.Content.ReadAsStream()).ReadToEnd();

        Dictionary<string, object> data = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonData);

        CheckForNullsOrErrors(data);

        return data;
      }
    }

    public void CheckForNullsOrErrors(Dictionary<string, object> data)
    {
      if (data == null)
        throw new ArgumentNullException("Data wass null");
      if (data.ContainsKey("error"))
        throw new InvalidOperationException("Response with error");
    }
  }
}
