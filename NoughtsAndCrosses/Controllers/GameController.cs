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
        private IGameRepository repo; // репозиторий
        private MiddleLayer ml; // слой для работы игра+репозаторий совместно

        public GameController()
        {
            repo = new GameRepository(new GameContext());
            ml = new MiddleLayer(repo);
        }

        // метод для начала игры
        public ActionResult Index()
        {
            // Объект новой игры
            Game = new Blank3x3();
            Session["Game"] = Game;
            Session["GameId"] = repo.AddGame(Session.SessionID);

            return View();
        }

        [ChildActionOnly]
        private JsonResult MoveResult(MoveResultModel result)
        {
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // ход игрока
        public ActionResult Move(int x, int y)
        {
            ml.Game = (Blank)Session["Game"];
            ml.GameId = Convert.ToInt32(Session["GameId"]);
            MoveResultModel result = ml.Move(x, y);
            return MoveResult(result);
        }

        // Получение списка цитат для полного вывода
        private IEnumerable<GameInfoView> GetItemsPerPage(int page, IEnumerable<GameInfoView> list)
        {
            int pageSize = 20;
            var itemsToSkip = page * pageSize;

            return list.Skip(itemsToSkip).Take(pageSize).ToList();
        }

        // Методы для получения статистики
        public ActionResult GetGamesList(int? id, string ForSession)
        {
            string UserSession = (ForSession == "true") ? Session.SessionID : null;
            int page = id ?? 0;

            // Получаем полный набор элементов
            var FullList = repo.GetGamesInfo(UserSession);

            var FilteredList = GetItemsPerPage(page, FullList);

            return PartialView("_GamesInfo", FilteredList);
        }

        public ActionResult GetMovesByGameId(string GameId)
        {
            var list = repo.GetMovesInfo(GameId).ToList();
            return PartialView("_MovesInfo", list);
        }

        public ActionResult Overall()
        {
            var list = repo.GetOverall().ToList();
            return PartialView("_Overall", list);
        }
    }
}
