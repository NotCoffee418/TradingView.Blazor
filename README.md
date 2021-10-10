# TradingView.Blazor
[![Nuget](https://img.shields.io/nuget/v/TradingView.Blazor?style=for-the-badge "Nuget")](https://www.nuget.org/packages/TradingView.Blazor)

Simple component for basic TradingView chart in Blazor supporting OHLC candle, volume and markers.

## Preview
![image](https://user-images.githubusercontent.com/9306304/130491761-77235d05-e079-4b32-a498-e88cedca8f06.png)

## Getting Started

1\. Include TradingView's lightweight-charts library in the `<head>`section of your _Host.cs file:
```html
<script src="https://unpkg.com/lightweight-charts/dist/lightweight-charts.standalone.production.js"></script>
```

2a\. Add the chart to your page where you would like the chart to display with a reference:
```html
<TradingViewChart @ref=myChart />
```

2b\. Define the reference in the `@code` section of your razor page
```csharp
@code {
	TradingViewChart? myChart;
}
```

3\. Prepare your data in the format of `List<TradingView.Blazor.Models.Ohlcv>`
```csharp
public class Ohlcv
{
    public DateTime Time { get; set; }
    public decimal Open {  get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Volume { get; set; }
}
```

4\. Load the chart after render
```csharp
protected override async Task OnAfterRenderAsync(bool firstRender)
{
    // Only on first render
    if (!firstRender)
        return;

    // Load the data
    ChartData data = new()
    {
        CandleData = chartService.GetSampleData(),
        MarkerData = new(), // todo: NIY
    };

    // Optionally define options
    ChartOptions options = new()
    {
        TimeScaleSecondsVisible = false,

        // Setting width to a negative value makes the chart responsive
        Width = -75,
    };

    // Load the chart
    if (myChart != null)
        await myChart.LoadChartAsync(data, options);
}
```

## Demo
You can view the code to the demo's index page [here](https://github.com/NotCoffee418/TradingView.Blazor/blob/main/TradingView.Blazor.Demo/Pages/Index.razor).
