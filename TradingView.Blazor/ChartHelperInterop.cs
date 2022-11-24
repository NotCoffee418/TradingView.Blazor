using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;
using TradingView.Blazor.Data;
using static TradingView.Blazor.Data.Enums;
using TradingView.Blazor.Models;

namespace TradingView.Blazor
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class ChartHelperInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;
        public ChartHelperInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/TradingView.Blazor/chartHelperInterop.js").AsTask());
        }

        /// <summary>
        /// Displays a chart with input data and options
        /// </summary>
        /// <param name="data"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task LoadChart(ElementReference eleRef, ChartData data, ChartOptions options)
        {
            // Shape the data to be compatible with JS library
            (ChartType? chartType, object mainChartData, object volumeData, object markerData) =
                ShapeBarChartData(data, options);

            // Call loadChart JS function
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("loadChart", 
                eleRef, eleRef.Id, chartType, mainChartData, volumeData, markerData, options);
        }

        public async Task UpdateChart(ElementReference eleRef, ChartData data, ChartOptions options)
        {
            // Shape updated data using cached options
            (ChartType? chartType, object mainChartData, object volumeData, object markerData) =
                ShapeBarChartData(data, options);

            // Call JS function
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync(
                "replaceChartData", eleRef.Id, chartType, mainChartData, volumeData, markerData);
        }

        /// <summary>
        /// Shape data to be usable by JS
        /// </summary>
        /// <param name="data"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private (ChartType? chartType,object mainChartData, object volumeData, object markerData) 
            ShapeBarChartData(ChartData data, ChartOptions options)
        {
            // Don't proceed on empty data
            if (data == null || data.ChartEntries == null || data.ChartEntries.Count == 0)
            {
                return (null, null, null, null);
            }
            
            // Extract candle data
            object mainChartData;
            ChartType chartType;
            
            // Extract candle data (if applicable)
            if (data.ChartEntries[0] is Ohlcv)
            {
                chartType = ChartType.Candlestick;
                mainChartData = data.ChartEntries.Cast<Ohlcv>()
                    .Select(x => new
                    {
                        time = new DateTimeOffset(x.Time, TimeSpan.Zero).ToUnixTimeSeconds(),
                        open = x.Open,
                        high = x.High,
                        low = x.Low,
                        close = x.Close,
                    });
            }
            // Extract line data (if applicable)
            else if (data.ChartEntries[0] is PricePoint)
            {
                chartType = ChartType.Line;
                mainChartData = data.ChartEntries.Cast<PricePoint>()
                    .Select(x => new
                    {
                        time = new DateTimeOffset(x.Time, TimeSpan.Zero).ToUnixTimeSeconds(),
                        price = x.Price,
                    });
            }
            else throw new NullReferenceException("No candle or line data is defined.");

            
            // Extract volume data
            decimal lastPrice = 0;
            Func<decimal, string> getVolumeColor = (nextPrice) =>
            {
                bool isLower = lastPrice < nextPrice;
                lastPrice = nextPrice;
                return isLower ? options.VolumeColorUp : options.VolumeColorDown;
            };
            object volumeData = data.ChartEntries
                .Select(x => new
                {
                    time = new DateTimeOffset(x.Time, TimeSpan.Zero).ToUnixTimeSeconds(),
                    value = x.Volume,
                    color = getVolumeColor(x.DisplayPrice),
                });

            // Shape marker data
            object markerData = data.MarkerData
                .Select(x => new
                {
                    time = new DateTimeOffset(x.Time, TimeSpan.Zero).ToUnixTimeSeconds(),
                    position = x.MarkerDirection == Marker.Direction.Buy ?
                        "belowBar" : "aboveBar",
                    color = x.MarkerDirection == Marker.Direction.Buy ?
                        options.MarkerBuyColor : options.MarkerSellColor,
                    shape = x.MarkerDirection == Marker.Direction.Buy ?
                        "arrowUp" : "arrowDown",
                    text = x.Text,
                    size = options.MarkerSize
                });

            return (chartType, mainChartData, volumeData, markerData);
        }


        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
