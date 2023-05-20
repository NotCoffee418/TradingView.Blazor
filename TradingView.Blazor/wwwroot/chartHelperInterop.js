window.charts = {};

export function loadChart(chartElement, chartRefId, chartType, mainSeriesData, volumeData, markerData, chartOptions) {
	if (chartElement == null) {
		console.error("ChartElement was null. Please define a reference for your TradingViewChart element.");
		return;
	}

	// Prepare chart element
	window.charts[chartRefId] = LightweightCharts.createChart(chartElement, {
		// negative value handled by resize script
		// note: keep width/height hard values above 0 or markers will break
		width: chartOptions.width > 0 ?
			chartOptions.width : 0,
		height: chartOptions.height > 0 ?
			chartOptions.height : 0,
		layout: {
			background: {
				color: chartOptions.layoutBackgroundColor,
			},
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
			borderColor: chartOptions.rightPriceScaleBorderColor,
		},
		timeScale: {
			borderColor: chartOptions.timeScaleBorderColor,
			timeVisible: chartOptions.timeScaleTimeVisible,
			secondsVisible: chartOptions.timeScaleSecondsVisible
		},
		...chartOptions.customChartDefinitions
	});

	// Define chart layout
	if (chartType === 1) // Candle
	{
		window.charts[chartRefId]["MainSeries"] = window.charts[chartRefId].addCandlestickSeries({
			upColor: 'rgb(38,166,154)',
			downColor: 'rgb(255,82,82)',
			wickUpColor: 'rgb(38,166,154)',
			wickDownColor: 'rgb(255,82,82)',
			borderVisible: true,
			priceFormat: {
				type: 'price',
				precision: chartOptions.rightPriceScaleDecimalPrecision,
				minMove: 1 / (10 ** chartOptions.rightPriceScaleDecimalPrecision),
			},
			...chartOptions.customCandleSeriesDefinitions
		});
		window.charts[chartRefId]["MainSeries"].setData(mainSeriesData);
	}
	else if (chartType === 2) // Line
	{
		window.charts[chartRefId]["MainSeries"] = window.charts[chartRefId].addLineSeries({
			color: '#00C',
			lineWidth: 2,
			lineStyle: chartOptions.lineStyle,
			axisLabelVisible: true,
			borderVisible: true,
			priceFormat: {
				type: 'price',
				precision: chartOptions.rightPriceScaleDecimalPrecision,
				minMove: 1 / (10 ** chartOptions.rightPriceScaleDecimalPrecision),
			},
			...chartOptions.customLineSeriesDefinitions
		});
		window.charts[chartRefId]["MainSeries"].setData(mainSeriesData);
		window.charts[chartRefId].timeScale().fitContent();
	}
	else console.error("ChartType was not defined or invalid. This is probably a C# bug.");
	

	// Define volume for chart layout
	window.charts[chartRefId]["VolumeSeries"] = window.charts[chartRefId].addHistogramSeries({
		color: '#26a69a',
		priceFormat: {
			type: 'volume',
		},
		priceScaleId: '',
		scaleMargins: {
			top: 0.8,
			bottom: 0,
		},
		...chartOptions.customVolumeSeriesDefinitions
	});
	window.charts[chartRefId]["VolumeSeries"].setData(volumeData);

	// Bind data
	window.charts[chartRefId]["MainSeries"].setMarkers(markerData); // <- fix me

	// Force resize if applicable
	var timerID;
	if (chartOptions.width < 0)
	{
		// Set size on initial load
		window.charts[chartRefId].resize(chartElement.parentElement.offsetWidth - (chartOptions.width*-1), chartOptions.height);

		// Regular check
		document.body.onresize = function () {
			if (timerID) clearTimeout(timerID);
			timerID = setTimeout(function () {
				window.charts[chartRefId].resize(chartElement.parentElement.offsetWidth - (chartOptions.width*-1), chartOptions.height);
			}, 200);
		}
	}

	// Fit the chart
	window.charts[chartRefId].timeScale().fitContent();

    // success
	return true;
}

export function replaceChartData(chartRefId, chartType, mainSeriesData, volumeData, markerData) {
	window.charts[chartRefId]["MainSeries"].setData(mainSeriesData);
	window.charts[chartRefId]["VolumeSeries"].setData(volumeData);
	window.charts[chartRefId]["MainSeries"].setMarkers(markerData);
}