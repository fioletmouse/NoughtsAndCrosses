using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoughtsAndCrosses.Classes;
using NoughtsAndCrosses.Models;

namespace NoughtsAndCrosses.Controllers
{
    public class GameController : Controller
    {
        private Blank Game;

        public ActionResult Index()
        {
            Game = new Blank3x3();
            Session["Game"] = Game;

            return View();
        }


        public bool CheckForWinners(out string msg)
        {
            CellOwner? p = Game.Winner;

            if (p == CellOwner.X)
            {
                msg = "Computer Wins";
                return true;
            }
            else if (p == CellOwner.O)
            {
                msg = "You Win!";
                return true;
            }
            else if (Game.IsFull)
            {
                msg = "Cat's Game";
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

        public ActionResult Move(int x, int y)
        {
            // запоминаем сделанный игроком шаг
            Game = (Blank)Session["Game"];

            CellInfo s = new CellInfo(x, y);
            Game[x, y] = CellOwner.O;

            string txt = null;
            if (CheckForWinners(out txt))
            {
              /*  MoveResultModel mr = new MoveResultModel();
                mr.RedirectLink = Url.Action("Index", "Game");
                mr.WinnerInfo = txt;*/
                return MoveResult(-1, -1, txt);//   Json(mr, JsonRequestBehavior.AllowGet);
            }
                //Form1_Load(null, new EventArgs());  //Winner was found, reload the game

            if (Game.EmptyCells.Count == Game.Size) //if all spaces are open, randomly pick one for excitement
            {
                Random r = new Random();
                s = new CellInfo(r.Next(0, 3), r.Next(0, 3));
            }
            else
            {
                s = ChooseMoveLogic.GetBestMove(Game, CellOwner.X);
            }

            Game[s.X, s.Y] = CellOwner.X;

            //LoadBoard();
            if (CheckForWinners(out txt))
            {
                /*MoveResultModel mr = new MoveResultModel();
                mr.RedirectLink = Url.Action("Index", "Game");
                mr.WinnerInfo = txt;
                mr.x = s.X;
                mr.y = s.Y;
                return Json(mr, JsonRequestBehavior.AllowGet);*/
                return MoveResult(s.X, s.Y, txt);
            }
            else
            {
                /*MoveResultModel mr = new MoveResultModel();
                mr.x = s.X;
                mr.y = s.Y;
                return Json(mr, JsonRequestBehavior.AllowGet);*/
                return MoveResult(s.X, s.Y, txt);
            }
        }
    }
}
