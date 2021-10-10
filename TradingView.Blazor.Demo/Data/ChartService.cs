using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TradingView.Blazor.Models;

namespace TradingView.Blazor.Demo.Data;
public class ChartService
{
    public List<Ohlcv> GetSampleData()
    {
        using (var reader = new StreamReader("sample-data.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                return csv.GetRecords<Ohlcv>().ToList();
    }
}
