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
    /// Репозиторий для работы с БД. Контект идет инъекцией
    /// </summary>
    public class GameRepository : IGameRepository
    {
        private GameContext _DB;
        public GameRepository(GameContext DBContext)
        {
            _DB = DBContext;
        }

        // Сохраняем запись о начале игры при заходе на страницу/обновлении/нажатии на кнопку принудительного начала
        public int AddGame(string SessionId)
        {
            GameInfo Entity = new GameInfo { CreatedOn = DateTime.Now, SessionId = SessionId };
            _DB.GameInfo.Add(Entity);
            _DB.SaveChanges();
            return Entity.Id;
        }

        // Обновляем результат игры, если он выявлен
        public void UpdateGameResult(int GameId, GameResult Winner)
        {
            GameInfo Game = _DB.GameInfo.Where(g => g.Id == GameId).Single();
            Game.Result = (int)Winner;
            _DB.SaveChanges();
        }

        // Добавляем запись о ходе
        public void AddMove(int GameId, CellInfo Cell, CellOwner Owner)
        {
            GameInfo Game = _DB.GameInfo.Where(g => g.Id == GameId).Single();
            MovesInfo Entity = new MovesInfo
            {
                MoveOwner = Owner.ToString(),
                RowX = Cell.X,
                ColY = Cell.Y,
                GameInfo = Game
            };

            _DB.MovesInfo.Add(Entity);
            _DB.SaveChanges();
        }

        /// <summary>
        ///  Получаем список игры (общий или для конкретной сессии)
        /// </summary>
        /// <param name="SesionId">Идентификатор сессии, если нужно найти только игры текущей сессии, оставить пустым, если нужен полный список</param>
        /// <returns>Список игр</returns>
        public IEnumerable<GameInfoView> GetGamesInfo(string SesionId)
        {
            var Temp = _DB.GameInfo.ToList();
            if (SesionId != null)
            {
                Temp = Temp.Where(x => x.SessionId == SesionId).ToList();
            }
            return from game in Temp
                   orderby game.Id descending
                   join move in _DB.MovesInfo on game.Id equals move.GameInfo.Id
                                                 into joined
                   select new GameInfoView
                   {
                       Id = game.Id,
                       Result = Settings.GetOverAllDefinition(game.Result),
                       StartDate = game.CreatedOn.ToShortDateString(),
                       StartTime = game.CreatedOn.ToShortTimeString(),
                       MoveCount = joined.Count()
                   } into model

                   where model.MoveCount > 0
                   select model;
        }

        // Получаем список ходов для определенной игры
        public IEnumerable<MovesInfoView> GetMovesInfo(string StringGameId)
        {
            int GameId = Convert.ToInt32(StringGameId);

            var Tmp = _DB.MovesInfo.Where(x => x.GameInfo.Id == GameId)
                                   .OrderBy(x => x.Id).AsEnumerable()
                                   .Select(game => new MovesInfoView
                                   {
                                       MoveOwner = Settings.PlayersName[(CellOwner)Enum.Parse(typeof(CellOwner), game.MoveOwner)],
                                       Point = "строка=" + game.RowX + " столбец=" + game.ColY
                                   });
            return Tmp;
        }

        // Получение полной статистики
        public IEnumerable<Overall> GetOverall()
        {
            // Приведение к Enumerable для использования функции, иначе нужно дописывать метод
            var List = _DB.GameInfo.GroupBy(p => p.Result)
                                   .AsEnumerable()
                                   .Select(p => new Overall
                                   {
                                       ResultType = Settings.GetOverAllDefinition(p.Key), 
                                       Count = p.Count() 
                                   });


            return List;
        }
    }
}