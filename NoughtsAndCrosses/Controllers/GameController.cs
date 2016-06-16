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
        private Blank gb;
        public GameController()
        {
            

        }
        //
        // GET: /Game/

        public ActionResult Index()
        {
            //gb = new Blank3x3();
            //if (HttpContext.Session["Game"] == null)
            //{
                gb = new Blank3x3();
                Session["Game"] = gb;

            //}

            //LoadBoard();
            return View();
        }

        //
        // GET: /Game/Details/5
        public bool CheckForWinners(out string msg)
        {
            CellOwner? p = gb.Winner;

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
            else if (gb.IsFull)
            {
                msg = "Cat's Game";
                return true;
            }
            msg = null;
            return false;
        }
       
        public ActionResult Test(int x, int y)
        {
            MoveResultModel mr = new MoveResultModel();
            mr.RedirectLink = Url.Action("Index", "Game");
            mr.x = 2;
            mr.y = 2;
            return Json(mr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Move(int x, int y)
        {

            gb = (Blank)Session["Game"];
            CellInfo s = new CellInfo(x, y);//(CellInfo)sender;

            //gb[s.X, s.Y] = Player.O;
            gb[x, y] = CellOwner.O;
            //LoadBoard();
            string txt = null;
            if (CheckForWinners(out txt))
            {
                MoveResultModel mr = new MoveResultModel();
                mr.RedirectLink = Url.Action("Index", "Game");
                mr.WinnerInfo = txt;
                return Json(mr, JsonRequestBehavior.AllowGet);
            }
                //Form1_Load(null, new EventArgs());  //Winner was found, reload the game

            if (gb.OpenSquares.Count == gb.Size) //if all spaces are open, randomly pick one for excitement
            {
                Random r = new Random();
                s = new CellInfo(r.Next(0, 3), r.Next(0, 3));
            }
            else
            {
                s = ChooseMoveLogic.GetBestMove(gb, CellOwner.X);
            }

            gb[s.X, s.Y] = CellOwner.X;

            //LoadBoard();
            if (CheckForWinners(out txt))
            {
                MoveResultModel mr = new MoveResultModel();
                mr.RedirectLink = Url.Action("Index", "Game");
                mr.WinnerInfo = txt;
                mr.x = s.X;
                mr.y = s.Y;
                return Json(mr, JsonRequestBehavior.AllowGet);
            }
            else
            {
                MoveResultModel mr = new MoveResultModel();
                mr.x = s.X;
                mr.y = s.Y;
                return Json(mr, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
