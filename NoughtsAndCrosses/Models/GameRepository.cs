using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoughtsAndCrosses.Classes;
using NoughtsAndCrosses.Repository;

namespace NoughtsAndCrosses.Models
{

    public class GameRepository
    {
        private GameContext _db;
        public GameRepository()//(GameContext dbContext)
        {
            _db = new GameContext();//dbContext;
        }

        public int AddGame(string sessionId)
        {
            GameInfo entity = new GameInfo { CreatedOn = DateTime.Now, SessionId = sessionId };
            _db.GameInfo.Add(entity);
            _db.SaveChanges();
            return entity.Id;
        }

        public void UpdateGameResult(int GameId, string Result)
        {
            GameInfo game = _db.GameInfo.Where(g => g.Id == GameId).Single();
            game.Result = Result;
            _db.SaveChanges();
        }

        public void AddMove(int GameId, CellInfo s, CellOwner owner)
        {
            GameInfo game = _db.GameInfo.Where(g => g.Id == GameId).Single();
            MovesInfo entity = new MovesInfo
            {
                MoveOwner = owner.ToString(),
                RowX = s.X,
                ColY = s.Y,
                GameInfo = game
            };

            _db.MovesInfo.Add(entity);
            _db.SaveChanges();
        }
    }
}