export function loadChart(chartElement, candleData, volumeData, markerData, chartOptions) {
	if (chartElement == null) {
		console.error("ChartElement was null. Please define a reference for your TradingViewChart element.");
		return;
    }

	var chart = LightweightCharts.createChart(chartElement, {
		// negative value subtracts from parent width, otherwise fixed width
		width: chartOptions.width > 0 ?
			chartElement.parentElement.offsetWidth - chartOptions.width :
			chartOptions.width,
		height: chartOptions.height,
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
		upColor: 'rgba(255, 144, 0, 1)',
		downColor: '#000',
		borderDownColor: 'rgba(255, 144, 0, 1)',
		borderUpColor: 'rgba(255, 144, 0, 1)',
		wickDownColor: 'rgba(255, 144, 0, 1)',
		wickUpColor: 'rgba(255, 144, 0, 1)',
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