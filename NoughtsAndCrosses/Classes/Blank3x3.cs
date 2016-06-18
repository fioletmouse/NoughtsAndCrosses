using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoughtsAndCrosses.Classes
{
    /// <summary>
    // Класс для игры 3 на 3 клетки
    /// </summary>
    class Blank3x3 : Blank
    {
        // размерность
        private const int currentSize = 3;
        
        public Blank3x3()
        {
            cells = new CellOwner[3, 3];
        }

        public override int Size 
        { 
            get { return currentSize * currentSize; } 
        }

        // Получаем список со всеми пустыми ячейками
        public override List<CellInfo> EmptyCells
        {
            get
            {
                List<CellInfo> EmptyCells = new List<CellInfo>();

                for (int x = 0; x < currentSize; x++)
                {
                    for (int y = 0; y < currentSize; y++)
                    {
                        if (cells[x, y] == CellOwner.Empty)
                        {
                            EmptyCells.Add(new CellInfo(x, y));
                        }
                    }
                }

                return EmptyCells;
            }
        }

        // Проверяем последовательно - столбцы, строки, диагонали на наличие ряда
        public override CellOwner Winner
        {
            get
            {
                int count = 0;

                // столбцы
                for (int x = 0; x < currentSize; x++)
                {
                    count = 0;
                    for (int y = 0; y < currentSize; y++) count += (int)cells[x, y];
                    if (count == 3) return CellOwner.X;
                    if (count == -3) return CellOwner.O;
                }

                // строчки
                for (int x = 0; x < 3; x++)
                {
                    count = 0;
                    for (int y = 0; y < 3; y++) count += (int)cells[y, x];
                    if (count == 3) return CellOwner.X;
                    if (count == -3) return CellOwner.O;
                }

                // диагонали
                count = (int)cells[0, 0] + (int)cells[1, 1] + (int)cells[2, 2];
                if (count == 3) return CellOwner.X;
                if (count == -3) return CellOwner.O;

                count = (int)cells[0, 2] + (int)cells[1, 1] + (int)cells[2, 0];
                if (count == 3) return CellOwner.X;
                if (count == -3) return CellOwner.O;

                return CellOwner.Empty;
            }
        }

        public override Blank Clone()
        {
            Blank b = new Blank3x3();
            b.cells = (CellOwner[,])this.cells.Clone();
            return b;
        }
    }
}