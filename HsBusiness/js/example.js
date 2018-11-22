window.alert = function (str) {
    $.dialog({
        showTitle: false,
        contentHtml: '<p>' + str + '</p>'
    });

}