using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoughtsAndCrosses.Models;
using NoughtsAndCrosses.Repository;

namespace NoughtsAndCrosses.Classes
{
    /// <summary>
    /// Промежуточный класс для работы с экземпляром игры и репозиторием совместно
    /// </summary>
    public class MiddleLayer
    {
        private IGameRepository repo;
        public Blank Game { get; set; }
        public int GameId { get; set; }

        // Передаем зависимостьчерез конструктор. Класс работает с БД исключительно через репозитарий
        public MiddleLayer(IGameRepository Repository)
        {
            repo = Repository;
        }

        // Функция, которая вызывается из контроллера при ходе игрока
        public MoveResultModel Move(int x, int y)
        {
            string txt = null;

            // Записываем сделанный игроком шаг
            CellInfo s = new CellInfo(x, y);
            MakeMove(Game, s, CellOwner.O);

            // Проверяем, есть ли победитель и возвращаем сообщение
            if (CheckForWinners(out txt))
            {
                // -1 - чтобы на стороне клиента не отмечать никакую ячейку, если игрок выиграл при своем ходе
                return MoveResult(-1, -1, txt);
            }


            /*if (Game.EmptyCells.Count == Game.Size)
            {
                Random r = new Random();
                s = new CellInfo(r.Next(0, 3), r.Next(0, 3));
            }
            else*/
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

        #region Private methods
        // Фиксируем шаг и записываем его в БД
        private void MakeMove(Blank game, CellInfo s, CellOwner owner)
        {
            game[s.X, s.Y] = owner;
            repo.AddMove(GameId, s, owner);
        }
        
        // Собираем возвращаемый на клиента объект. Тут собираются параметры, связанные с самой игрой
        private MoveResultModel MoveResult(int x, int y, string WinnerInfo)
        {
            MoveResultModel result = new MoveResultModel();
            result.x = x;
            result.y = y;
            result.WinnerInfo = WinnerInfo;
            return result;
        }

        // Записываем победителя
        private void WinHandler(GameResult Winner)
        {
            repo.UpdateGameResult(GameId, Winner);
        }

        /// <summary>
        /// Проверяем текущую игру на наличие победителя
        /// </summary>
        /// <param name="msg"> выходной параметр - сообщение, отображаемое пользователю</param>
        /// <returns></returns>
        private bool CheckForWinners(out string msg)
        {
            CellOwner? p = Game.Winner;

            if (p == CellOwner.X)
            {
                msg = Settings.DefaultMessage[GameResult.X];
                // Записываем победителя в БД как результат игры
                WinHandler(GameResult.X);
                return true;
            }
            else if (p == CellOwner.O)
            {
                msg = Settings.DefaultMessage[GameResult.O];
                WinHandler(GameResult.O);
                return true;
            }
            else if (Game.IsFull)
            {
                msg = Settings.DefaultMessage[GameResult.Unknown];
                WinHandler(GameResult.Unknown);
                return true;
            }
            msg = null;
            return false;
        }
        #endregion

    }
}