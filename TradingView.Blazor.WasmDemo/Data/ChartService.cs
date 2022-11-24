using System.Net;
using System.Text;

namespace TradingView.Blazor.WasmDemo.Data;
public class ChartService
{
    public ChartService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    HttpClient _httpClient;

    public async Task<List<Ohlcv>> GetSmallSampleData()
        => (await ReadCsvAsync<Ohlcv>("sample-data/small-sample-data.csv")).ToList();

    public async Task<List<Ohlcv>> GetSampleData()
        => (await ReadCsvAsync<Ohlcv>("sample-data/sample-data.csv")).ToList();

    public async Task<List<Marker>> GetSampleMarkers()
        => await ReadCsvAsync<Marker>("sample-data/sample-markers.csv");
    
    private async Task<List<T>> ReadCsvAsync<T>(string url)
    {
        using (Stream receiveStream = await _httpClient.GetStreamAsync(url))
            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                using (var csv = new CsvReader(readStream, CultureInfo.InvariantCulture))
                    return csv.GetRecords<T>().ToList();
    }
}
