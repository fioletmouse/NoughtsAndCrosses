using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoughtsAndCrosses.Classes
{
    public class ChooseMoveLogic
    {
        /// <summary>
        /// Реализация алгоритма минимакса. Проходим все шаги до конца игры в поисках лучшего хода.
        /// </summary>
        public static CellInfo GetBestMove(Blank Game, CellOwner Point)
        {
            // если не нашли лучший ход, то данные остаются пустыми, т.к. это структура указывает nullable тип
            CellInfo? BestCell = null;  
            List<CellInfo> EmptyCells = Game.EmptyCells;
            Blank EmulationGame;    // хранит копию игры с текущего шага

            // Проверяем, проходя по дереву, для всех пустых ячеек
            for (int i = 0; i < EmptyCells.Count; i++)
            {
                EmulationGame = Game.Clone();
                CellInfo CurrentCell = EmptyCells[i];

                EmulationGame[CurrentCell.X, CurrentCell.Y] = Point;

                // Если нет победителя, но еще есть свободные ячейки, спускаемся ниже по дереву
                if (EmulationGame.Winner == CellOwner.Empty && EmulationGame.EmptyCells.Count > 0)
                {
                    CellInfo TempMove = GetBestMove(EmulationGame, ((CellOwner)(-(int)Point)));  // переключаем игрока (ходы через одного)
                    CurrentCell.Rank = TempMove.Rank;
                }
                else
                {
                    // оцениваем ход
                    if (EmulationGame.Winner == CellOwner.Empty)
                        CurrentCell.Rank = 0;
                    else if (EmulationGame.Winner == CellOwner.X)
                        CurrentCell.Rank = -1;
                    else if (EmulationGame.Winner == CellOwner.O)
                        CurrentCell.Rank = 1;
                }

                // Если текущий шаг лучше, чем найденный ранее, выбираем его
                if (BestCell == null ||
                   (Point == CellOwner.X && CurrentCell.Rank < ((CellInfo)BestCell).Rank) ||
                   (Point == CellOwner.O && CurrentCell.Rank > ((CellInfo)BestCell).Rank))
                {
                    BestCell = CurrentCell;
                }
            }

            return (CellInfo)BestCell;
        }
    }
}