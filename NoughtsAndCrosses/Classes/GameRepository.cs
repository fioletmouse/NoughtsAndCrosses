using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoughtsAndCrosses.Classes;
using NoughtsAndCrosses.Repository;
using System.Data.Entity;
using NoughtsAndCrosses.Models;

namespace NoughtsAndCrosses.Classes
{
    /// <summary>
    /// Репозитарий для работы с БД. Контект идет инъекцией
    /// </summary>
    public class GameRepository : NoughtsAndCrosses.Classes.IGameRepository
    {
        private GameContext _db;
        public GameRepository(GameContext dbContext)
        {
            _db = dbContext;
        }

        // Сохраняем запись о начале игры при заходе на страницу/обновлении/нажатии на кнопку принудительного начала
        public int AddGame(string sessionId)
        {
            GameInfo entity = new GameInfo { CreatedOn = DateTime.Now, SessionId = sessionId };
            _db.GameInfo.Add(entity);
            _db.SaveChanges();
            return entity.Id;
        }

        // Обновляем результат игры, если он выявлен
        public void UpdateGameResult(int GameId, string Result)
        {
            GameInfo game = _db.GameInfo.Where(g => g.Id == GameId).Single();
            game.Result = Result;
            _db.SaveChanges();
        }

        // Добавляем запись о ходе
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

        // Получаем список игры (общим или для конкретной сессии)
        public IEnumerable<GameInfoView> GetGamesInfo(string SesionId)
        {
            IEnumerable<GameInfo> temp = _db.GameInfo.ToList();
            if (SesionId != null)
            {
                temp = temp.Where(x => x.SessionId == SesionId).ToList();
            }
            return from game in temp
                   join move in _db.MovesInfo on game.Id equals move.GameInfo.Id
                                                 into joined
                   select new GameInfoView
                   {
                       Id = game.Id,
                       PlayerName = game.PlayerName,
                       Result = game.Result ?? "Игра не закончена",
                       CreatedOn = game.CreatedOn,
                       MoveCount = joined.Count()
                   } into model

                   where model.MoveCount > 0
                   select model;
        }

        // Получаем список ходов для определенной игры
        public IEnumerable<MovesInfoView> GetMovesInfo(string GameId)
        {
            int gameId = Convert.ToInt32(GameId);

            var tmp = from game in _db.MovesInfo
                  where game.GameInfo.Id == gameId
                  select new MovesInfoView
                   {
                       MoveOwner = game.MoveOwner,
                       Point = "строка=" + game.RowX + " столбец=" + game.ColY
                   };
            return tmp;
        }

        // Получение полной статистики
        public IEnumerable<Overall> GetOverall()
        {
            return from game in _db.GameInfo
                      group game by game.Result into model
                   select new Overall { ResultType = model.Key ?? "Игра не закончена", Count = model.Count() };
           
        }
    }
}