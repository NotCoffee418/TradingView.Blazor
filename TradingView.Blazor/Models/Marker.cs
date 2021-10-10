using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingView.Blazor.Models
{
    public class Marker
    {
        public DateTime Time {  get; set; }
        public decimal Price {  get; set; }
        public Direction MarkerDirection { get; set; }
        public string Text { get; set; }


        public enum Direction
        {
            Buy = 1,
            Sell = 2,
        }
    }
}
