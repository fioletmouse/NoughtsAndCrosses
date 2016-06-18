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
        private Blank Game;
        private IGameRepository repo;
        private MiddleLayer ml;

        public GameController()
        {
            repo = new GameRepository(new GameContext());
            ml = new MiddleLayer(repo);
        }

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

        public ActionResult Move(int x, int y)
        {
            ml.Game = (Blank)Session["Game"];
            ml.GameId = Convert.ToInt32(Session["GameId"]);
            MoveResultModel result = ml.Move(x, y);
            return MoveResult(result);
        }

        public ActionResult GetGamesList(string ForSession)
        {
            string UserSession = (ForSession == "true") ? Session.SessionID : null;

            var list = repo.GetGamesInfo(UserSession).ToList();
            return PartialView("_GamesInfo", list);
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
