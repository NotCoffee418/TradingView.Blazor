using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;
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
            (object candleData, object volumeData, object markerData) =
                ShapeChartData(data, options);

            // Call loadChart JS function
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("loadChart", 
                eleRef, eleRef.Id, candleData, volumeData, markerData, options);
        }

        public async Task UpdateChart(ElementReference eleRef, ChartData data, ChartOptions options)
        {
            // Shape updated data using cached options
            (object candleData, object volumeData, object markerData) =
                ShapeChartData(data, options);

            // Call JS function
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync(
                "replaceChartData", eleRef.Id, candleData, volumeData, markerData);
        }

        /// <summary>
        /// Shape data to be usable by JS
        /// </summary>
        /// <param name="data"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private (object candleData, object volumeData, object markerData) 
            ShapeChartData(ChartData data, ChartOptions options)
        {
            // Extract candle data
            object candleData = data.CandleData
                .Select(x => new
                {
                    time = new DateTimeOffset(x.Time, TimeSpan.Zero).ToUnixTimeSeconds(),
                    open = x.Open,
                    high = x.High,
                    low = x.Low,
                    close = x.Close,
                });

            // Extract volume data
            object volumeData = data.CandleData
                .Select(x => new
                {
                    time = new DateTimeOffset(x.Time, TimeSpan.Zero).ToUnixTimeSeconds(),
                    value = x.Volume,
                    color = x.Close > x.Open ? options.VolumeColorUp : options.VolumeColorDown
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

            return (candleData, volumeData, markerData);
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
