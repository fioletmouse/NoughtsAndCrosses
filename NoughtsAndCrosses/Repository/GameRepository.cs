using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoughtsAndCrosses.Classes;
using System.Data.Entity;
using NoughtsAndCrosses.Models;

namespace NoughtsAndCrosses.Repository
{
    /// <summary>
    /// Репозитарий для работы с БД. Контект идет инъекцией
    /// </summary>
    public class GameRepository : IGameRepository
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
        public void UpdateGameResult(int GameId, GameResult Winner)
        {
            GameInfo game = _db.GameInfo.Where(g => g.Id == GameId).Single();
            game.Result = (int)Winner;
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

        /// <summary>
        ///  Получаем список игры (общим или для конкретной сессии)
        /// </summary>
        /// <param name="SesionId">Идентификатор сессии, если нужно найти только игры текущей сессии, оставить пустым, если нужен полный список</param>
        /// <returns>Список игр</returns>
        public IEnumerable<GameInfoView> GetGamesInfo(string SesionId)
        {
            IEnumerable<GameInfo> temp = _db.GameInfo.ToList();
            if (SesionId != null)
            {
                temp = temp.Where(x => x.SessionId == SesionId).ToList();
            }
            return from game in temp
                   orderby game.Id descending
                   join move in _db.MovesInfo on game.Id equals move.GameInfo.Id
                                                 into joined
                   select new GameInfoView
                   {
                       Id = game.Id,
                       PlayerName = game.PlayerName,
                       Result = Settings.GetOverAllDefinition(game.Result),
                       StartDate = game.CreatedOn.ToShortDateString(),
                       StartTime = game.CreatedOn.ToShortTimeString(),
                       MoveCount = joined.Count()
                   } into model

                   where model.MoveCount > 0
                   select model;
        }

        // Получаем список ходов для определенной игры
        public IEnumerable<MovesInfoView> GetMovesInfo(string GameId)
        {
            int gameId = Convert.ToInt32(GameId);

            var tmp = _db.MovesInfo.Where(x => x.GameInfo.Id == gameId).OrderBy(x => x.Id).AsEnumerable()
                                    .Select(game => new MovesInfoView
                   {
                       MoveOwner = Settings.PlayersName[(CellOwner)Enum.Parse(typeof(CellOwner), game.MoveOwner)],
                       Point = "строка=" + game.RowX + " столбец=" + game.ColY
                   });
            return tmp;
        }

        // Получение полной статистики
        public IEnumerable<Overall> GetOverall()
        {
            // Приведение к Enumerable для использования функции, иначе нужно дописывать метод
            var list = _db.GameInfo.GroupBy(p => p.Result)
                                   .AsEnumerable()
                                   .Select(p => new Overall
                                   {
                                       ResultType = Settings.GetOverAllDefinition(p.Key), 
                                       Count = p.Count() 
                                   });


            return list;
        }
    }
}