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
        public static CellInfo GetBestMove(Blank game, CellOwner point)
        {
            // если не нашли лучший ход, то данные остаются пустыми, т.к. это структура указывает nullable тип
            CellInfo? bestCell = null;  
            List<CellInfo> emptyCells = game.EmptyCells;
            Blank emulationGame;    // хранит копию игры с текущего шага

            // Проверяем, проходя по дереву, для всех пустых ячеек
            for (int i = 0; i < emptyCells.Count; i++)
            {
                emulationGame = game.Clone();
                CellInfo currentCell = emptyCells[i];

                emulationGame[currentCell.X, currentCell.Y] = point;

                // Если нет победителя, но еще есть свободные ячейки, спускаемся ниже по дереву
                if (emulationGame.Winner == CellOwner.Empty && emulationGame.EmptyCells.Count > 0)
                {
                    CellInfo tempMove = GetBestMove(emulationGame, ((CellOwner)(-(int)point)));  // переключаем игрока (ходы через одного)
                    currentCell.Rank = tempMove.Rank;
                }
                else
                {
                    // оцениваем ход
                    if (emulationGame.Winner == CellOwner.Empty)
                        currentCell.Rank = 0;
                    else if (emulationGame.Winner == CellOwner.X)
                        currentCell.Rank = -1;
                    else if (emulationGame.Winner == CellOwner.O)
                        currentCell.Rank = 1;
                }

                // Если текущий шаг лучше, чем найденный ранее, выбираем его
                if (bestCell == null ||
                   (point == CellOwner.X && currentCell.Rank < ((CellInfo)bestCell).Rank) ||
                   (point == CellOwner.O && currentCell.Rank > ((CellInfo)bestCell).Rank))
                {
                    bestCell = currentCell;
                }
            }

            return (CellInfo)bestCell;
        }
    }
}