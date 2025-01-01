$(document).ready(function () {
    console.log("Register.js loaded"); // Kiểm tra xem tệp có được tải không

    jQuery.validator.setDefaults({
        debug: true,
        success: function (label) {
            label.attr('id', 'valid');
        },
    });
    
});