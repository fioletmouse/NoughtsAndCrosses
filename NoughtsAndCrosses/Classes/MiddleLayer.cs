using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoughtsAndCrosses.Models;

namespace NoughtsAndCrosses.Classes
{
    public class MiddleLayer
    {
        private IGameRepository repo;
        public Blank Game { get; set; }
        public int GameId { get; set; }

        public MiddleLayer(IGameRepository Repository)
        {
            repo = Repository;
        }
        
        // Фиксируем шаг и записываем его в БД
        private void MakeMove(Blank game, CellInfo s, CellOwner owner)
        {
            game[s.X, s.Y] = owner;
            repo.AddMove(GameId, s, owner);
        }
        
        private MoveResultModel MoveResult(int x, int y, string WinnerInfo)
        {
            MoveResultModel result = new MoveResultModel();
            result.x = x;
            result.y = y;
            result.WinnerInfo = WinnerInfo;
            return result;
        }

        public MoveResultModel Move(int x, int y)
        {
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

        private void WinHandler(string Text)
        {
            repo.UpdateGameResult(GameId, Text);
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

       

        
    }
}