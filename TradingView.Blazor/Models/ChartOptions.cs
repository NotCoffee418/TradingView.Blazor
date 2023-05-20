namespace TradingView.Blazor.Models;
public class ChartOptions
{
    /// <summary>
    /// Setting width to a negative value will cause it to fill the container
    /// </summary>
    public int Width { get; set; } = -1;
    public int Height { get; set; } = 300;

    // -- Layout
    public string LayoutBackgroundColor { get; set; } = "#000";
    public string LayoutTextColor { get; set; } = "rgba(255, 255, 255, 0.9)";

    // -- Grid
    public string VertLinesColor { get; set; } = "rgba(197, 203, 206, 0.5)";
    public string HorzLinesColor { get; set; } = "rgba(197, 203, 206, 0.5)";

    // -- RightPriceScale
    public string RightPriceScaleBorderColor { get; set; } = "rgba(197, 203, 206, 0.8)";
    public int RightPriceScaleDecimalPrecision { get; set; } = 2;

    // -- Timescale
    public string TimeScaleBorderColor { get; set; } = "rgba(197, 203, 206, 0.8)";
    public bool TimeScaleTimeVisible { get; set; } = true;
    public bool TimeScaleSecondsVisible { get; set; } = false;

    // -- Volume
    public string VolumeColorUp { get; set; } = "rgba(0, 150, 136, 0.8)";
    public string VolumeColorDown { get; set; } = "rgba(255,82,82, 0.8)";

    // -- Marker
    public string MarkerBuyColor { get; set; } = "#2196F3";
    public string MarkerSellColor { get; set; } = "#e91e63";
    public int MarkerSize { get; set; } = 1;
    
    // -- Line style
    public LineStyle LineStyle { get; set; } = LineStyle.Solid;

    // -- Custom definitions
    /// <summary>
    /// Options defined here will be added to the chart when calling createChart()
    /// Example usage:
    /// options.CustomChartDefinitions["height"] = 500;
    /// </summary>
    public Dictionary<string, dynamic> CustomChartDefinitions { get; set; } = new();

    /// <summary>
    /// Options defined here will be added to the chart when calling addCandlestickSeries()
    /// Example usage:
    /// options.CustomCandleSeriesDefinitions["borderVisible"] = false;
    /// </summary>
    public Dictionary<string, dynamic> CustomCandleSeriesDefinitions { get; set; } = new();
    
    /// <summary>
    /// Options defined here will be added to the chart when calling addLineSeries()
    /// Example usage:
    /// options.CustomLineSeriesDefinitions["borderVisible"] = false;
    /// </summary>
    public Dictionary<string, dynamic> CustomLineSeriesDefinitions { get; set; } = new();

    /// <summary>
    /// Options defined here will be added to the chart when calling addHistogramSeries()
    /// Example usage:
    /// options.CustomVolumeSeriesDefinitions["color"] = "#26a69a";
    /// </summary>
    public Dictionary<string, dynamic> CustomVolumeSeriesDefinitions { get; set; } = new();

}