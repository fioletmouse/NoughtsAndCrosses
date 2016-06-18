$(document).ready(function () {
    $("div.col-xs-4").click(function (e) {
        $(this).addClass("player");
        makeComputerMove(this);
    });

    $(function(){
        GetOverall();
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
            if (response.x != -1) {
                $("div[row=" + response.x + "]");
                $("div[row=" + response.x + "][col=" + response.y + "]").addClass("computer");
            }
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

function GetOverall() {
    $.ajax({
        url: 'Game/Overall',
        type: 'GET',
        data: { },

        success: function (response, status, xhr) {
            $('#Overall').html(response);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //show the error somewhere - but this is a bad solution
        }
    })
    /* $.get(ctrl.href, function (data) {
         $('#myModal div.modal-body').html(data);
         $('#myModal').modal('show');
     });*/
}

function UploadStatisticPartialView(ctrl, e, forSession) {
    e.preventDefault();
    $.ajax({
        url: ctrl.href,
        type: 'GET',
        data: { ForSession: forSession },

        success: function (response, status, xhr) {
            $('#myModal div.modal-body').html(response);
            $('#myModal').modal('show');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //show the error somewhere - but this is a bad solution
        }
    })
   /* $.get(ctrl.href, function (data) {
        $('#myModal div.modal-body').html(data);
        $('#myModal').modal('show');
    });*/
}

function getajax(tmp) {
    $.ajax({
        url: 'Game/GetMovesByGameId',
        type: 'GET',
        data: { GameId: tmp.id },

        success: function (response, status, xhr) {
            $('div#' + tmp.id)
            $('div#' + tmp.id + ' div.MoveInformation').html(response);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //show the error somewhere - but this is a bad solution
        }
    })
}