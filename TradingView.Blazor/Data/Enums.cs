namespace TradingView.Blazor.Data;

public class Enums
{
    public enum LineStyle
    {
        Solid = 0,
        Dotted = 1,
        Dashed = 2,
        LargeDashed = 3,
        SparseDotted = 4,
    }
    
    public enum ChartType
    {
        Unspecified = 0,
        Candlestick = 1,
        Line = 2,
    }
}