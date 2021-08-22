# TradingView.Blazor

Simple component for basic TradingView chart in Blazor supporting OHLC bars, volume and markers.

## Getting Started

Include TradingView's lightweight-charts library in the `<head>`section of your _Host.cs file:
```html
<script src="https://unpkg.com/lightweight-charts/dist/lightweight-charts.standalone.production.js"></script>
```

Add the chart to your page where you would like the chart to display:
```html
<TradingViewChart Id="myChart" Data=data />
```

Ensure that each chart has a unique ID if you have multiple on one page.  

## Known issues
- This is currently for .NET 6 only due to an issue in .NET Preview version
- Markers are not implemented yet