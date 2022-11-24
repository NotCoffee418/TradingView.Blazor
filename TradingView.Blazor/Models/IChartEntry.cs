using System;

namespace TradingView.Blazor.Models;

public interface IChartEntry
{
    public DateTime Time { get; set; }
    public decimal Volume { get; set; }
    public decimal DisplayPrice { get; }
}