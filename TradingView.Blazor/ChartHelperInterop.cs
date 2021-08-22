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
        public async Task LoadChart(string elementId, ChartData data, ChartOptions options)
        {
            // Extract candle data
            var candleData = data.CandleData
                .Select(x => new
                {
                    time = new DateTimeOffset(x.Time, TimeSpan.Zero).ToUnixTimeSeconds(),
                    open = x.Open,
                    high = x.High,
                    low = x.Low,
                    close = x.Close,
                });

            // Extract volume data
            var volumeData = data.CandleData
                .Select(x => new
                {
                    time = new DateTimeOffset(x.Time, TimeSpan.Zero).ToUnixTimeSeconds(),
                    value = x.Volume,
                    color = x.Close > x.Open ? options.VolumeColorUp : options.VolumeColorDown
                });

            // Shape marker data
            var markerData = data.MarkerData
                .Select(x => new
                {
                    time = new DateTimeOffset(x.Time, TimeSpan.Zero).ToUnixTimeSeconds(),
                    position = x.MarkerDirection == Marker.Direction.Buy ?
                        "belowBar" : "aboveBar",
                    color = x.MarkerDirection == Marker.Direction.Buy ?
                        options.MarkerBuyColor : options.MarkerSellColor,
                    shape = x.MarkerDirection == Marker.Direction.Buy ?
                        "arrowUp" : "arrowDown",
                    text = x.Text
                });

            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("loadChart", 
                elementId, candleData, volumeData, markerData, options);
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
