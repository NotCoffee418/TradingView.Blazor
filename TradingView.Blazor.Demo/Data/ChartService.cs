namespace TradingView.Blazor.Demo.Data;
public class ChartService
{
    public List<Ohlcv> GetSmallSampleData()
    {
        using (var reader = new StreamReader("small-sample-data.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            return csv.GetRecords<Ohlcv>().ToList();
    }

    public List<Ohlcv> GetSampleData()
    {
        using (var reader = new StreamReader("sample-data.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            return csv.GetRecords<Ohlcv>().ToList();
    }

    public List<Marker> GetSampleMarkers()
    {
        using (var reader = new StreamReader("sample-markers.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            return csv.GetRecords<Marker>().ToList();
    }

    public List<PricePoint> GetSampleLineData()
        => GetSampleData()
        .Select(x => new PricePoint()
        {
            Time = x.Time,
            Price = x.Close,
            Volume = x.Volume
        }).ToList();
}
