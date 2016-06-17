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
        private GameRepository repo;

        public GameController()
        {
            repo = new GameRepository();
        }

        public ActionResult Index()
        {
            // Объект новой игры
            Game = new Blank3x3();
            Session["Game"] = Game;
            Session["GameId"] = repo.AddGame(Session.SessionID);

            return View();
        }


        private void WinHandler(string Text)
        {
            repo.UpdateGameResult(Convert.ToInt32(Session["GameId"]), Text);
        }

        public bool CheckForWinners(out string msg)
        {
            CellOwner? p = Game.Winner;

            if (p == CellOwner.X)
            {
                msg = "Computer Wins";
                WinHandler(msg);
                return true;
            }
            else if (p == CellOwner.O)
            {
                msg = "You Win!";
                WinHandler(msg);
                return true;
            }
            else if (Game.IsFull)
            {
                msg = "Cat's Game";
                WinHandler(msg);
                return true;
            }
            msg = null;
            return false;
        }


        [ChildActionOnly]
        private JsonResult MoveResult(int x, int y, string WinnerInfo)
        {
            MoveResultModel result = new MoveResultModel();
            result.RedirectLink = Url.Action("Index", "Game");
            result.x = x;
            result.y = y;
            result.WinnerInfo = WinnerInfo;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private void MakeMove(Blank game, CellInfo s, CellOwner owner)
        {
            game[s.X, s.Y] = owner;
            repo.AddMove(Convert.ToInt32(Session["GameId"]), s, owner);
        }

        public ActionResult Move(int x, int y)
        {
            // получаем текущую игру
            Game = (Blank)Session["Game"];

            // Записываем сделанный игроком шаг
            CellInfo s = new CellInfo(x, y);
            MakeMove(Game, s, CellOwner.O);

            string txt = null;

            if (CheckForWinners(out txt))
            {
                return MoveResult(-1, -1, txt);
            }


            if (Game.EmptyCells.Count == Game.Size)
            {
                Random r = new Random();
                s = new CellInfo(r.Next(0, 3), r.Next(0, 3));
            }
            else
            {
                s = ChooseMoveLogic.GetBestMove(Game, CellOwner.X);
            }

            MakeMove(Game, s, CellOwner.X);

            if (CheckForWinners(out txt))
            {
                return MoveResult(s.X, s.Y, txt);
            }
            else
            {
                return MoveResult(s.X, s.Y, txt);
            }
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
