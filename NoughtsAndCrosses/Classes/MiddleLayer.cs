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
        private IGameRepository Repo;

        public Blank Game { get; set; }
        public int GameId { get; set; }

        // Передаем зависимость через конструктор. Класс работает с БД исключительно через репозитарий
        public MiddleLayer(IGameRepository Repository)
        {
            Repo = Repository;
        }

        // Функция, которая вызывается из контроллера при ходе игрока
        public MoveResultModel Move(int x, int y)
        {
            string Txt = null;

            // Записываем сделанный игроком шаг
            CellInfo Cell = new CellInfo(x, y);
            MakeMove(Game, Cell, CellOwner.O);

            // Проверяем, есть ли победитель и возвращаем сообщение
            if (CheckForWinners(out Txt))
            {
                // -1 - чтобы на стороне клиента не отмечать никакую ячейку, если игрок выиграл при своем ходе
                return MoveResult(-1, -1, Txt);
            }

            Cell = ChooseMoveLogic.GetBestMove(Game, CellOwner.X);

            MakeMove(Game, Cell, CellOwner.X);

            if (CheckForWinners(out Txt))
            {
                return MoveResult(Cell.X, Cell.Y, Txt);
            }
            else
            {
                return MoveResult(Cell.X, Cell.Y, Txt);
            }
        }

        #region Private methods
        // Фиксируем шаг и записываем его в БД
        private void MakeMove(Blank Game, CellInfo Cell, CellOwner Owner)
        {
            Game[Cell.X, Cell.Y] = Owner;
            Repo.AddMove(GameId, Cell, Owner);
        }
        
        // Собираем возвращаемый на клиента объект. Тут собираются параметры, связанные с самой игрой
        private MoveResultModel MoveResult(int x, int y, string WinnerInfo)
        {
            MoveResultModel Result = new MoveResultModel();
            Result.x = x;
            Result.y = y;
            Result.WinnerInfo = WinnerInfo;
            return Result;
        }

        // Записываем победителя
        private void WinHandler(GameResult Winner)
        {
            Repo.UpdateGameResult(GameId, Winner);
        }

        /// <summary>
        /// Проверяем текущую игру на наличие победителя
        /// </summary>
        /// <param name="Msg"> выходной параметр - сообщение, отображаемое пользователю</param>
        /// <returns></returns>
        private bool CheckForWinners(out string Msg)
        {
            CellOwner? Player = Game.Winner;

            if (Player == CellOwner.X)
            {
                Msg = Settings.DefaultMessage[GameResult.X];
                // Записываем победителя в БД как результат игры
                WinHandler(GameResult.X);
                return true;
            }
            else if (Player == CellOwner.O)
            {
                Msg = Settings.DefaultMessage[GameResult.O];
                WinHandler(GameResult.O);
                return true;
            }
            else if (Game.IsFull)
            {
                Msg = Settings.DefaultMessage[GameResult.Unknown];
                WinHandler(GameResult.Unknown);
                return true;
            }
            Msg = null;
            return false;
        }
        #endregion

    }
}