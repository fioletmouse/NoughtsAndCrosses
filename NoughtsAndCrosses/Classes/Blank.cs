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
    /// Перечисление с результатами игры
    /// </summary>
    public enum GameResult 
    {
        X,
        O,
        Unknown       
    }
    /// <summary>
    /// Абстрактный класс для игрового поля
    /// </summary>
    public abstract class Blank
    {
        // Массив со всеми ячейками. Храниться в них могут только значения из перечисления CellOwner
        public CellOwner[,] Cells;

        // Индексатор для ячейки. Получаем/устанавливаем владельца
        public  CellOwner this[int x, int y]
        {
            get
            {
                return Cells[x, y];
            }

            set
            {
                Cells[x, y] = value;
            }
        }

        // Пустых ячеек нет
        public bool IsFull
        { 
            get 
            { 
                return !Cells.Cast<CellOwner>().Any(x => x == CellOwner.Empty); 
            } 
        }

        // Полный размер игрового поля
        public abstract int Size { get; }

        // Список с пустыми ячейками
        public abstract List<CellInfo> EmptyCells { get; }

        // Определяем победителя
        public abstract CellOwner Winner { get; }

        // Создание полной копии игрового поля. Нужно для выбора оптимального хода
        public abstract Blank Clone();
    }
}