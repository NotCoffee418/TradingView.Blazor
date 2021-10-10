namespace TradingView.Blazor.Demo.Data;
public class ChartService
{
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
}
