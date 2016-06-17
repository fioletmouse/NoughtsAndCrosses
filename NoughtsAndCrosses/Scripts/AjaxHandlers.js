$(document).ready(function () {
    $("div.col-xs-4").click(function (e) {
        $(this).addClass("player");
        makeComputerMove(this);
    });
});

function makeComputerMove(divTag) {
    $.ajax({
        url: 'Game/Move',
        type: 'GET',
        data: {
            x: $(divTag).attr("row"),
            y: $(divTag).attr("col")
        },

        success: function (response, status, xhr) {
            $("div[row=" + response.x + "]");
            $("div[row=" + response.x + "][col=" + response.y + "]").addClass("computer");
            if (response.WinnerInfo != null && response.WinnerInfo != "") {
                alert(response.WinnerInfo);
                window.location.href = response.RedirectLink;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //show the error somewhere - but this is a bad solution
        }
    })
}