namespace TradingView.Blazor.WasmDemo.Data;
public class ChartService
{
    public List<Ohlcv> GetSmallSampleData()
    {
        using (var reader = new StreamReader("/sample-data/small-sample-data.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            return csv.GetRecords<Ohlcv>().ToList();
    }
    
    public List<Ohlcv> GetSampleData()
    {
        using (var reader = new StreamReader("/sample-data/sample-data.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                return csv.GetRecords<Ohlcv>().ToList();
    }
    
    public List<Marker> GetSampleMarkers()
    {
        using (var reader = new StreamReader("/sample-data/sample-markers.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                return csv.GetRecords<Marker>().ToList();
    }
}
