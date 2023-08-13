namespace Assignment.Models
{
  public class StockPriceQuote
  {
    public double CurrentPrice { get; set; }
    public double Change { get; set; }
    public double PercentChange { get; set; }
    public double HighestPriceOfTheDay { get; set; }
    public double LowestPriceOfTheDay { get; set; }
    public double OpenPriceOfTheDay { get; set; }
    public double PreviousClosePrice { get; set; }

  }
}
