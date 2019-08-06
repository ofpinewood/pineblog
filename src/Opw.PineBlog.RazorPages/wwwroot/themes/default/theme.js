$(function () {
    $("[data-toggle=popover]").popover({
        html: true,
        sanitize: false,
        content: function () {
            var content = $(this).attr("data-popover-content");
            return $(content).children(".popover-body").html();
        },
        title: function () {
            var content = $(this).attr("data-popover-content");
            return $(content).children(".popover-heading").html();
        }
    });
});

//// logout
//function profileLogOut() {
//    $("#frmLogOut").submit();
//}

//// tooltip
//$('[data-toggle="tooltip"]').tooltip();

//// Create the measurement node for scrollbar
//var scrollDiv = document.createElement("div");
//scrollDiv.className = "scrollbar-measure";
//document.body.appendChild(scrollDiv);
//var scrollbarWidth = scrollDiv.offsetWidth - scrollDiv.clientWidth;
//document.body.removeChild(scrollDiv);

//$('#blog-search').on('show.bs.modal', function () {
//    $(".blog-header").css({
//        "right": scrollbarWidth
//    });
//})
//$('#blog-search').on('hidden.bs.modal', function () {
//    $(".blog-header").css({
//        "right": "0"
//    });
//})
//$('#blog-search').on('shown.bs.modal', function () {
//    $('#blog-search .form-control').trigger('focus');
//})
