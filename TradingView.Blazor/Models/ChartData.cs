using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingView.Blazor.Models;
public class ChartData
{
    /// <summary>
    /// Fill this object with chart entry data such as Ohlcv or PricePoint
    /// </summary>
    public List<IChartEntry> ChartEntries { get; set; }
    
        
    /// <summary>
    /// Optional marker arrow to be displayed in addition to the primary chart data
    /// </summary>
    public List<Marker> MarkerData { get; set; }
    
    
    [Obsolete(
        "Property CandleData has been deprecated. Please use ChartEntries instead. " +
        "You may need to use .Cast<IChartEntry>() to convert a list of List<Ohlcv> into a List<IChartEntry>.",
        true)]
    public List<Ohlcv> CandleData { get; set; }
}