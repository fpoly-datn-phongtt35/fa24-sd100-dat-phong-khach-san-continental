/* ====== Chart ====== */

(function ($) {
    "use strict";
    function userNumbers() {
        var options = {
            chart: {
                type: "bar",
                height: 50,
                stacked: !0,
                sparkline: {
                    enabled: !0
                },
                dropShadow: {
                    enabled: true,
                    enabledOnSeries: undefined,
                    top: 5,
                    left: 5,
                    blur: 3,
                    color: '#000',
                    opacity: 0.1
                }
            },
            stroke: {
                width: 0
            },
            dataLabels: {
                enabled: !1
            },
            series: [{
                name: "Organic",
                data: [1070, 2250, 1565, 4560, 2850, 5658, 7854, 1565, 4560, 2850, 5658, 7854]
            }, {
                name: "Referal",
                data: [950, 2100, 1265, 4160, 2350, 5258, 7354, 1265, 4160, 2350, 5358, 7554]
            }],
            plotOptions: {
                bar: {
                    horizontal: !1,
                    columnWidth: 25,
                    borderRadius: 0
                }
            },
            xaxis: {
                categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                axisBorder: {
                    show: !1
                },
                axisTicks: {
                    show: !1
                },
                labels: {
                    show: !1
                }
            },
            yaxis: {
                labels: {
                    show: !1
                }
            },
            colors: ["#485568", "#71757b"],
        };
        var userNumbers = new ApexCharts(document.querySelector("#userNumbers"), options);
        userNumbers.render();
    }
    function bookingNumbers() {
        var options = {
            chart: {
                type: "line",
                height: 50,
                sparkline: {
                    enabled: !0
                },
                dropShadow: {
                    enabled: true,
                    enabledOnSeries: undefined,
                    top: 5,
                    left: 5,
                    blur: 3,
                    color: '#000',
                    opacity: 0.1
                }
            },
            series: [{
                data: [1362, 3954, 7152, 4254, 3485, 4956, 3568, 2365, 1050, 1920, 4785, 6856]
            }],
            stroke: {
                curve: "smooth",
                width: 2
            },
            colors: ["#485568"],
            xaxis: {
                categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                axisBorder: {
                    show: !1
                },
                axisTicks: {
                    show: !1
                }
            },
            tooltip: {
                fixed: {
                    enabled: !1
                },
                y: {
                    title: {
                        formatter: function (e) {
                            return ""
                        }
                    }
                }
            },
        };

        var bookingNumbers = new ApexCharts(document.querySelector("#bookingNumbers"), options);
        bookingNumbers.render();
    }
    function revenueNumbers() {
        var options = {
            series: [{
                data: [1070, 2250, 1565, 4560, 2850, 5658, 7854, 1565, 4560, 2850, 5658, 7854]
            }],
            chart: {
                type: "bar",
                height: 50,
                sparkline: {
                    enabled: !0
                },
                dropShadow: {
                    enabled: true,
                    enabledOnSeries: undefined,
                    top: 5,
                    left: 5,
                    blur: 3,
                    color: '#000',
                    opacity: 0.1
                }
            },
            plotOptions: {
                bar: {
                    horizontal: !1,
                    columnWidth: 25,
                    borderRadius: 0
                }
            },

            xaxis: {
                categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                axisBorder: {
                    show: !1
                },
                axisTicks: {
                    show: !1
                }
            },
            tooltip: {
                fixed: {
                    enabled: !1
                },
                y: {
                    title: {
                        formatter: function (e) {
                            return ""
                        }
                    }
                }
            },
            colors: ["#485568"]
        };
        var revenueNumbers = new ApexCharts(document.querySelector("#revenueNumbers"), options);
        revenueNumbers.render();
    }
    function expensesNumbers() {
        var options = {
            chart: {
                type: "line",
                height: 50,
                sparkline: {
                    enabled: !0
                },
                dropShadow: {
                    enabled: true,
                    enabledOnSeries: undefined,
                    top: 5,
                    left: 5,
                    blur: 3,
                    color: '#000',
                    opacity: 0.1
                }
            },
            series: [{
                data: [850, 1920, 1362, 3954, 2485, 4956, 7152, 1254, 3568, 2365, 4785, 6856]
            }],
            stroke: {
                curve: "smooth",
                width: 2
            },
            colors: ["#485568"],
            xaxis: {
                categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                axisBorder: {
                    show: !1
                },
                axisTicks: {
                    show: !1
                }
            },
            tooltip: {
                fixed: {
                    enabled: !1
                },
                y: {
                    title: {
                        formatter: function (e) {
                            return ""
                        }
                    }
                }
            },
        };

        var expensesNumbers = new ApexCharts(document.querySelector("#expensesNumbers"), options);
        expensesNumbers.render();
    }
    function overviewChart() {
        var options = {
            series: [{
                name: 'Bookings',
                type: 'area',
                data: [23, 12, 23, 22, 15, 42, 31, 27, 45, 28, 37]
            }, {
                name: 'Revenue',
                type: 'line',
                data: [44.64, 55.48, 20.15, 30.62, 12.57, 30.38, 41.85, 41.44, 40.56, 25.84, 43.78]
            }, {
                name: 'Expence',
                type: 'line',
                data: [30.55, 24.67, 36.85, 37.08, 42.85, 38.85, 46.64, 45.42, 49.89, 36.56, 38.49]
            }],
            chart: {
                height: 365,
                type: 'line',
                stacked: false,
                foreColor: '#373d3f',
                sparkline: {
                    enabled: !1
                },
                dropShadow: {
                    enabled: true,
                    enabledOnSeries: undefined,
                    top: 5,
                    left: 5,
                    blur: 3,
                    color: '#000',
                    opacity: 0.1
                },
                toolbar: {
                    show: !1
                }
            },
            stroke: {
                width: [2, 2, 2],
                curve: 'smooth'
            },
            fill: {
                opacity: [.5, 1, 1],
            },
            colors: ['#485568', '#87909e', '#7ea0fb'],
            xaxis: {
                categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                axisTicks: {
                    show: !1
                },
                axisBorder: {
                    show: !1
                }
            },
            legend: {
                show: !0,
                horizontalAlign: "center",
                offsetX: 0,
                offsetY: -5,
                markers: {
                    width: 15,
                    height: 10,
                    radius: 6
                },
                itemMargin: {
                    horizontal: 10,
                    vertical: 0
                }
            },
            grid: {
                show: !1,
                xaxis: {
                    lines: {
                        show: !1
                    }
                },
                yaxis: {
                    lines: {
                        show: !1
                    }
                },
                padding: {
                    top: 0,
                    right: -2,
                    bottom: 15,
                    left: 0
                },
            },
            tooltip: {
                shared: !0,
                y: [{
                    formatter: function (e) {
                        return void 0 !== e ? e.toFixed(0) : e
                    }
                }, {
                    formatter: function (e) {
                        return void 0 !== e ? "$" + e.toFixed(2) + "k" : e
                    }
                }, {
                    formatter: function (e) {
                        return void 0 !== e ? "$" + e.toFixed(2) + "k" : e
                    }
                }]
            },
            responsive: [{
                breakpoint: 480,
                options: {
                    chart: {
                        height: '300px',
                    },
                    yaxis: {
                        show: false,
                    },
                }
            }]
        };
        var overviewChart = new ApexCharts(document.querySelector("#overviewChart"), options);
        overviewChart.render();
    }

    jQuery(window).on('load', function () {
        bookingNumbers();
        userNumbers();
        revenueNumbers();
        expensesNumbers();
        overviewChart();
    });

})(jQuery);