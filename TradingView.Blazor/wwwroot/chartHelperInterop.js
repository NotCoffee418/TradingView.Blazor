export function loadChart(chartElement, candleData, volumeData, markerData, chartOptions) {
	if (chartElement == null) {
		console.error("ChartElement was null. Please define a reference for your TradingViewChart element.");
		return;
	}

	// Prepare chart element
	var chart = LightweightCharts.createChart(chartElement, {
		// negative value handled by resize script
		// note: keep width/height hard values above 0 or markers will break
		width: chartOptions.width > 0 ?
			chartOptions.width : 0,
		height: chartOptions.height > 0 ?
			chartOptions.height : 0,
		layout: {
			backgroundColor: chartOptions.layoutBackgroundColor,
			textColor: chartOptions.layoutTextColor,
		},
		grid: {
			vertLines: {
				color: chartOptions.vertLinesColor,
			},
			horzLines: {
				color: chartOptions.horzLinesColor,
			},
		},
		crosshair: {
			mode: LightweightCharts.CrosshairMode.Normal,
		},
		rightPriceScale: {
			borderColor: chartOptions.RightPriceScaleBorderColor,
		},
		timeScale: {
			borderColor: chartOptions.timeScaleBorderColor,
			timeVisible: chartOptions.timeScaleTimeVisible,
			secondsVisible: chartOptions.timeScaleSecondsVisible
		},
	});

	// Define candle options
	// Define chart layout
	var candleSeries = chart.addCandlestickSeries({
		upColor: 'rgb(38,166,154)',
		downColor: 'rgb(255,82,82)',
		wickUpColor: 'rgb(38,166,154)',
		wickDownColor: 'rgb(255,82,82)',
		borderVisible: true,
	});

	// Define volume for chart layout
	var volumeSeries = chart.addHistogramSeries({
		color: '#26a69a',
		priceFormat: {
			type: 'volume',
		},
		priceScaleId: '',
		scaleMargins: {
			top: 0.8,
			bottom: 0,
		},
	});

	// Bind data
	candleSeries.setData(candleData);
	volumeSeries.setData(volumeData);
	candleSeries.setMarkers(markerData);

	// Force resize if applicable
	var timerID;
	if (chartOptions.width < 0)
	{
		// Set size on initial load
		chart.resize(chartElement.parentElement.offsetWidth - (chartOptions.width*-1), chartOptions.height);

		// Regular check
		document.body.onresize = function () {
			if (timerID) clearTimeout(timerID);
			timerID = setTimeout(function () {
				chart.resize(chartElement.parentElement.offsetWidth - (chartOptions.width*-1), chartOptions.height);
			}, 200);
		}
    }

    // success
    return true
}