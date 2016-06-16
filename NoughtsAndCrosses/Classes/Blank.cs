using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoughtsAndCrosses.Classes
{
    /// <summary>
    // Варианты, кто занял ячейку
    /// </summary>
    public enum CellOwner
    {
        X = 1,
        O = -1,
        Empty = 0,
    }

    /// <summary>
    /// Ячейка поля
    /// </summary>
    public struct CellInfo
    {
        public int X;
        public int Y;
        public double Rank;

        public CellInfo(int x, int y)
        {
            this.X = x;
            this.Y = y;
            Rank = 0;
        }
    }
    
    /// <summary>
    /// Abstract class for creating new game boards, contains all of the logic needed for the AI to play the game.
    /// </summary>
    public abstract class Blank
    {

        /// Массив со всеми ячейками. Хранится в них могут только значения из перечисления
        public CellOwner[,] cells;

        /// Индексатор для ячейки. Получаем/устанавливаем владельца
        public abstract CellOwner this[int x, int y] { get; set; }

        /// Пустых ячеек нет
        public abstract bool IsFull { get; }


        /// Размер доски
        public abstract int Size { get; }

        /// <summary>
        /// List of the open spaces available on the current board.
        /// </summary>
        public abstract List<CellInfo> OpenSquares { get; }

        /// <summary>
        /// Determines if there is a winner on the current board.
        /// </summary>
        public abstract CellOwner Winner { get; }

        /// <summary>
        /// Makes a deap copy of the current board
        /// </summary>
        public abstract Blank Clone();
    }
}