$(document).ready(function () {

    window.addEventListener("resize", resize);
    resize();

    appendTextToChart("Играем! ур...");
    getOverall();

    $("div.col-xs-4").click(function (e) {
        // Ставим картинку и вызываем ход компьютера
        if ($(this).find("img").length == 0) {
            $(this).html("<img src='Content/img/SimonsCross.png' class='img-responsive'/>")
            makeComputerMove(this);
        }
        else {
            appendTextToChart("Уже занято, Мяу!")
        }
    });
});

function resize()
{
    $("div.blankCell").height($("div.blankCell").width());
}

function appendTextToChart(txt)
{
    $('div#Chat').append(txt + "<br />");
}

function makeComputerMove(divTag) {
    $.ajax({
        url: 'Game/Move',
        type: 'GET',
        data: {
            x: $(divTag).attr("row"),
            y: $(divTag).attr("col")
        },

        success: function (response, status, xhr) {
            // Если нужно отметить клетку ходом компьютера
            if (response.x != -1) {
                $("div[row=" + response.x + "][col=" + response.y + "]").html("<img src='Content/img/SimonsNaught.png' class='img-responsive'/>");
            }

            // если игра завершена
            if (response.WinnerInfo != null && response.WinnerInfo != "") {
                appendTextToChart(response.WinnerInfo)
                $('#ShowResult div.modal-body').prepend(response.WinnerInfo);
                $('#ShowResult').modal('show');
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Мяв! Что-то случилось :( Попробуйте начать новую игру или сыграем позже.");
            console.log(errorThrown);
        }
    })
}

// получение общего счета игр (левый блок)
function getOverall() {
    $.ajax({
        url: 'Game/Overall',
        type: 'GET',
        data: { },

        success: function (response, status, xhr) {
            $('#Overall').html(response);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("GetOverall' error occures: " + errorThrown);
        }
    })
}

// Загрузка окна с данными по статистике
// forSession - флаг нужно ли фильтровать по текущей сессии
function gamesListView(e, forSession) {
    e.preventDefault();

    // Загрузка данных
    gamesPageView(null, forSession);

    $('#ReportModal input#IsSession').val(forSession);
    $('#ReportModal').modal('show');
}

// Переход по страницам со статистикой
function pageSwitch(pageId)
{
    var IsSession = $('#ReportModal input#IsSession').val();
    gamesPageView(pageId, IsSession);
}

// Получение данных для страницы со статистикой для каждой страницы
// forSession - флаг нужно ли фильтровать по текущей сессии
function gamesPageView(pageId, forSession) {
    $.ajax({
        url: 'Game/GetGamesList',
        type: 'GET',
        data: {
            Id: pageId,
            ForSession: forSession
        },

        success: function (response, status, xhr) {
            $('#ReportModal div.modal-body').html(response);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $('#ReportModal div.modal-body').html("<div>Котик устал, котик не хочет работать. <br />Лови ошибку: " + errorThrown + "</div>");
        }
    })
}

// Получение данных по  ходам
function getMovesInfo(htmlTag) {
    $.ajax({
        url: 'Game/GetMovesByGameId',
        type: 'GET',
        data: { GameId: htmlTag.id },

        success: function (response, status, xhr) {
            $('div#MoveInfo_' + htmlTag.id).html(response);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $('div#MoveInfo_' + htmlTag.id).html("<div>Объявляю забастовку! Не хочу работать! <br /> Лови ошибку: " + errorThrown + "</div>");
        }
    })
}

