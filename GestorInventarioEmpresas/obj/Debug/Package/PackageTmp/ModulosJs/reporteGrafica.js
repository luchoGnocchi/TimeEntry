!function($) {
    "use strict";
    var MorrisCharts = function () { };
    var Dashboard = function () {
        this.$body = $("body")
        this.$realData = []
    };
 function initMorrins() {
        MorrisCharts.prototype.init;
    }
    //creates line chart
 MorrisCharts.prototype.createLineChart = function (element, data, xkey, ykeys, labels, lineColors) {
     Morris.Line({
         element: element,
         data: data,
         xkey: xkey,
         ykeys: ykeys,
         labels: labels,
         gridLineColor: '#eef0f2',
         resize: true, //defaulted to true
         lineColors: lineColors
     });
 },
 MorrisCharts.prototype.createAreaChart = function (element, pointSize, lineWidth, data, xkey, ykeys, labels, lineColors) {
     Morris.Area({
         element: element,
         pointSize: 3,
         lineWidth: 0,
         data: data,
         xkey: xkey,
         ykeys: ykeys,
         labels: labels,
         gridLineColor: '#eef0f2',
         lineColors: lineColors,
         xLabels: 'day',
         xLabelAngle: 45,
         hideHover: 'auto', 
         xLabelFormat: function (d) {
             var weekdays = new Array(7);
             weekdays[0] = "Dom";
             weekdays[1] = "Lun";
             weekdays[2] = "Mar";
             weekdays[3] = "Mir";
             weekdays[4] = "Jue";
             weekdays[5] = "Vie";
             weekdays[6] = "Sab";

             return weekdays[d.getDay()] + ' ' +
                  ("0" + (d.getDate())).slice(-2) +   '-' +
                     ("0" + (d.getMonth() + 1)).slice(-2)  ;
         },
     });
 },
 Dashboard.prototype.createPieGraph = function (selector, labels, datas, colors) {
      
       var options = {
           series: {
               pie: {
                   show: true,
               }
           },
           legend: {
               show: false
           },
           grid: {
               hoverable: true//,
               //clickable: true
           },
           colors: colors,
           tooltip: true,
          
           tooltipOpts: {
           cssClass: "flotTip",
           content: "%s, %p.0%",
       shifts: {
           x: 20,
           y: 0
       },
       defaultTheme: false
   },
       };

       $.plot($(selector), datas, options);
   },
 MorrisCharts.prototype.init = function (data) {

     ////create line chart
     //var $data = [
     //    { y: '2009', a: 100, b: 90 },
     //    { y: '2010', a: 75, b: 65 },
     //    { y: '2011', a: 50, b: 40 },
     //    { y: '2012', a: 75, b: 65 },
     //    { y: '2013', a: 50, b: 40 },
     //    { y: '2014', a: 75, b: 65 },
     //    { y: '2015', a: 100, b: 90 }
     //];
     //this.createLineChart('morris-line-example', $data, 'y', ['a', 'b'], ['Series A', 'Series B'], ['#6e8cd7', '#dcdcdc']);

     //creating area chart
   //  var $areaData = [{ "y": "2018-08-01" }, { "y": "2018-08-02" }, { "y": "2018-08-03" }, { "y": "2018-08-04" }, { "y": "2018-08-05" }, { "y": "2018-08-06", "EWS - Strongbox Implementation-C53": "7.00", "Berries - Executive Dashboard-C03,C06,C07,C09,C14,C28": "1.00" }, { "y": "2018-08-07", "EWS - Strongbox Implementation-C53": "0.00", "Berries - Executive Dashboard-C03,C06,C07,C09,C14,C28": "0.00" }, { "y": "2018-08-08", "EWS - Strongbox Implementation-C53": "0.00", "Berries - Executive Dashboard-C03,C06,C07,C09,C14,C28": "0.00" }, { "y": "2018-08-09", "EWS - Strongbox Implementation-C53": "0.00", "Berries - Executive Dashboard-C03,C06,C07,C09,C14,C28": "0.00" }, { "y": "2018-08-10", "EWS - Strongbox Implementation-C53": "8.00", "Berries - Executive Dashboard-C03,C06,C07,C09,C14,C28": "0.00" }, { "y": "2018-08-11", "EWS - Strongbox Implementation-C53": "0.00", "Berries - Executive Dashboard-C03,C06,C07,C09,C14,C28": "0.00" }, { "y": "2018-08-12", "EWS - Strongbox Implementation-C53": "0.00", "Berries - Executive Dashboard-C03,C06,C07,C09,C14,C28": "0.00" }, { "y": "2018-08-13", "EWS - Strongbox Implementation-C53": "4.00" }, { "y": "2018-08-14", "EWS - Strongbox Implementation-C53": "0.00" }, { "y": "2018-08-15", "EWS - Strongbox Implementation-C53": "0.00" }, { "y": "2018-08-16", "EWS - Strongbox Implementation-C53": "0.00" }, { "y": "2018-08-17", "EWS - Strongbox Implementation-C53": "0.00" }, { "y": "2018-08-18", "EWS - Strongbox Implementation-C53": "9.00" }, { "y": "2018-08-19", "EWS - Strongbox Implementation-C53": "0.00" }, { "y": "2018-08-20" }, { "y": "2018-08-21" }, { "y": "2018-08-22" }, { "y": "2018-08-23" }, { "y": "2018-08-24" }, { "y": "2018-08-25" }, { "y": "2018-08-26" }, { "y": "2018-08-27" }, { "y": "2018-08-28" }, { "y": "2018-08-29" }, { "y": "2018-08-30" }];
     
     this.createAreaChart('morris-area-example', 0, 0, data.data, 'y', data.Keys, data.Keys, data.colores);

 },
 Dashboard.prototype.init = function (data) {
    
         //Pie graph data
        // var colors = ["red", "rgba(41, 182, 246, 0.85)", "rgba(126, 87, 194, 0.85)"];
         this.createPieGraph("#pie-chart #pie-chart-container", data.Keys, data.datosPieGrafic, data.colores);
         BlockUIManager.unblock()
     },
 $.MorrisCharts = new MorrisCharts, $.MorrisCharts.Constructor = MorrisCharts;
 $.Dashboard = new Dashboard, $.Dashboard.Constructor = Dashboard
}(window.jQuery);

