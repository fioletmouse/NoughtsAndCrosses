using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoughtsAndCrosses.Classes;
using NoughtsAndCrosses.Models;
using NoughtsAndCrosses.Repository;

namespace NoughtsAndCrosses.Controllers
{
    public class GameController : Controller
    {
        private Blank Game;     // объект с игрой
        private IGameRepository Repo; // репозиторий
        private MiddleLayer MiddleL; // слой для работы игра+репозаторий совместно

        public GameController()
        {
            Repo = new GameRepository(new GameContext());
            MiddleL = new MiddleLayer(Repo);
        }

        // метод для начала игры
        public ActionResult Index()
        {
            // Объект новой игры
            Game = new Blank3x3();
            Session["Game"] = Game;
            Session["GameId"] = Repo.AddGame(Session.SessionID);

            return View();
        }

        [ChildActionOnly]
        private JsonResult MoveResult(MoveResultModel Result)
        {
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        // ход игрока
        public ActionResult Move(int x, int y)
        {
            MiddleL.Game = (Blank)Session["Game"];
            MiddleL.GameId = Convert.ToInt32(Session["GameId"]);
            MoveResultModel Result = MiddleL.Move(x, y);
            return MoveResult(Result);
        }

        // Получение ограниченного списка игр
        private GamesInfoPage GetItemsPerPage(IEnumerable<GameInfoView> List, int? PageId)
        {
            int Page = PageId ?? 1;
            int PageSize = 10;      // По идее значени нужно вынести в настройки
            int itemsToSkip = (Page-1) * PageSize;

            GamesInfoPage GamePage = new GamesInfoPage();
            GamePage.Pagination = new Pagination { PageNumber = Page, PageSize = PageSize, TotalItems = List.Count() };
            GamePage.GamesList = List.Skip(itemsToSkip).Take(PageSize).ToList();

            return GamePage;
        }

        // Методы для получения статистики
        public ActionResult GetGamesList(int? Id, string ForSession)
        {
            string UserSession = (ForSession == "true") ? Session.SessionID : null;

            // Получаем полный набор элементов
            var FullList = Repo.GetGamesInfo(UserSession);

            // Получаем часть списка для вывода на страницу
            var ListPart = GetItemsPerPage(FullList, Id);

            return PartialView("_GamesInfo", ListPart);
        }

        public ActionResult GetMovesByGameId(string GameId)
        {
            var List = Repo.GetMovesInfo(GameId).ToList();
            return PartialView("_MovesInfo", List);
        }

        public ActionResult Overall()
        {
            var List = Repo.GetOverall().ToList();
            return PartialView("_Overall", List);
        }
    }
}
