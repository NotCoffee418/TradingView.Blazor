using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingView.Blazor.Models
{
    public class ChartData
    {
        public List<Ohlcv> CandleData {  get; set; }
        public List<Marker> MarkerData { get; set; }
    }
}
