/* ====== Index ======

1. Page Date (Breadcrumbs)
2. Revenue Overview

====== End ======*/
$(function () {
  "use strict";

  /*======== 1. Page Date (Breadcrumbs) ========*/
  if ($("#pagedate")) {
    var start = moment();
    var end = moment();
    var cb = function (start, end) {
      $("#pagedate .lh-date-range span").html(
        
        "<a href='javascript:void(0)' class='calendar' title='Date'><i class='ri-calendar-line'></i>" + start.format("ll") + " - " + end.format("ll") + "</a>"
      );
    };

    $("#pagedate .lh-date-range").daterangepicker(
      {
        startDate: start,
        endDate: end,
        opens: 'left',
        ranges: {
          Today: [moment(), moment()],
          Yesterday: [
            moment().subtract(1, "days"),
            moment().subtract(1, "days")
          ],
          "Last 7 Days": [moment().subtract(6, "days"), moment()],
          "Last 30 Days": [moment().subtract(29, "days"), moment()],
          "This Month": [moment().startOf("month"), moment().endOf("month")],
          "Last Month": [
            moment()
              .subtract(1, "month")
              .startOf("month"),
            moment()
              .subtract(1, "month")
              .endOf("month")
          ]
        }
      },
      cb
    );
    cb(start, end);
  }

  /*======== 2. Revenue Overview ========*/
  if ($(".revenue-overview")) {
    var start = moment().subtract(1, "days");
    var end = moment().subtract(1, "days");
    var cb = function (start, end) {
      $(".revenue-overview .lh-date-range span").html(
        start.format("ll") + " - " + end.format("ll")
      );
    };

    $(".revenue-overview .lh-date-range").daterangepicker(
      {
        startDate: start,
        endDate: end,
        opens: 'left',
        ranges: {
          Today: [moment(), moment()],
          Yesterday: [
            moment().subtract(1, "days"),
            moment().subtract(1, "days")
          ],
          "Last 7 Days": [moment().subtract(6, "days"), moment()],
          "Last 30 Days": [moment().subtract(29, "days"), moment()],
          "This Month": [moment().startOf("month"), moment().endOf("month")],
          "Last Month": [
            moment()
              .subtract(1, "month")
              .startOf("month"),
            moment()
              .subtract(1, "month")
              .endOf("month")
          ]
        }
      },
      cb
    );
    cb(start, end);
  }

   /*======== 3. Map(index.html), recent order(ecommerce.html), best seller(ecommerce.html),top product(ecommerce.html), Device user(analytics)  ========*/
   if ($("#lhmap, #bookingtbl, #best_seller_tbl, #top_product_tbl, #deviceUser")) {
    var start = moment();
    var end = moment();
    var cb = function (start, end) {
      $("#lhmap .lh-date-range span, #bookingtbl .lh-date-range span, #best_seller_tbl .lh-date-range span, #top_product_tbl .lh-date-range span, #deviceUser .lh-date-range span").html(
        
        "<a href='javascript:void(0)' class='calendar' title='Date'><i class='ri-equalizer-line'></i></a>"
      );
    };

    $("#lhmap .lh-date-range, #bookingtbl .lh-date-range, #best_seller_tbl .lh-date-range, #top_product_tbl .lh-date-range, #deviceUser .lh-date-range").daterangepicker(
      {
        startDate: start,
        endDate: end,
        opens: 'left',
        ranges: {
          Today: [moment(), moment()],
          Yesterday: [
            moment().subtract(1, "days"),
            moment().subtract(1, "days")
          ],
          "Last 7 Days": [moment().subtract(6, "days"), moment()],
          "Last 30 Days": [moment().subtract(29, "days"), moment()],
          "This Month": [moment().startOf("month"), moment().endOf("month")],
          "Last Month": [
            moment()
              .subtract(1, "month")
              .startOf("month"),
            moment()
              .subtract(1, "month")
              .endOf("month")
          ]
        }
      },
      cb
    );
    cb(start, end);
  }

  /*======== 4. Deal table ========*/
  if ($("#dealtbl")) {
    var start = moment();
    var end = moment();
    var cb = function (start, end) {
      $("#dealtbl .lh-date-range span").html(
        
        "<a href='javascript:void(0)' class='calendar' title='Date'><i class='ri-equalizer-line'></i></a>"
      );
    };

    $("#dealtbl .lh-date-range").daterangepicker(
      {
        startDate: start,
        endDate: end,
        opens: 'left',
        ranges: {
          Today: [moment(), moment()],
          Yesterday: [
            moment().subtract(1, "days"),
            moment().subtract(1, "days")
          ],
          "Last 7 Days": [moment().subtract(6, "days"), moment()],
          "Last 30 Days": [moment().subtract(29, "days"), moment()],
          "This Month": [moment().startOf("month"), moment().endOf("month")],
          "Last Month": [
            moment()
              .subtract(1, "month")
              .startOf("month"),
            moment()
              .subtract(1, "month")
              .endOf("month")
          ]
        }
      },
      cb
    );
    cb(start, end);
  }

    /*======== 5. Deal table ========*/
    if ($("#campaigns")) {
      var start = moment();
      var end = moment();
      var cb = function (start, end) {
        $("#campaigns .lh-date-range span").html(
          
          "<a href='javascript:void(0)' class='calendar' title='Date'><i class='ri-equalizer-line'></i></a>"
        );
      };
  
      $("#campaigns .lh-date-range").daterangepicker(
        {
          startDate: start,
          endDate: end,
          opens: 'left',
          ranges: {
            Today: [moment(), moment()],
            Yesterday: [
              moment().subtract(1, "days"),
              moment().subtract(1, "days")
            ],
            "Last 7 Days": [moment().subtract(6, "days"), moment()],
            "Last 30 Days": [moment().subtract(29, "days"), moment()],
            "This Month": [moment().startOf("month"), moment().endOf("month")],
            "Last Month": [
              moment()
                .subtract(1, "month")
                .startOf("month"),
              moment()
                .subtract(1, "month")
                .endOf("month")
            ]
          }
        },
        cb
      );
      cb(start, end);
    }
    /*======== 6. Single date picker (create-project.html) ========*/
    $('input[name="deadline"]').daterangepicker({
      singleDatePicker: true,
      showDropdowns: true,
      minYear: 2022,
      maxYear: 2030,
    }, function(start, end, label) {
      var years = moment().diff(start, 'years');
      // alert("You are " + years + " years old!");
    });
});
