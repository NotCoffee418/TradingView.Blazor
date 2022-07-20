using System.Net;
using System.Text;

namespace TradingView.Blazor.WasmDemo.Data;
public class ChartService
{
    HttpClient httpClient = new HttpClient();

    public async Task<List<Ohlcv>> GetSmallSampleData()
        => await ReadCsvAsync<Ohlcv>("https://localhost:7034/sample-data/small-sample-data.csv");

    public async Task<List<Ohlcv>> GetSampleData()
        => await ReadCsvAsync<Ohlcv>("https://localhost:7034/sample-data/sample-data.csv");

    public async Task<List<Marker>> GetSampleMarkers()
        => await ReadCsvAsync<Marker>("https://localhost:7034/sample-data/sample-markers.csv");

    private async Task<List<T>> ReadCsvAsync<T>(string url)
    {        
        using (Stream receiveStream = await httpClient.GetStreamAsync(url))
            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                using (var csv = new CsvReader(readStream, CultureInfo.InvariantCulture))
                    return csv.GetRecords<T>().ToList();
    }
}
