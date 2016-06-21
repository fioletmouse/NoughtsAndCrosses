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
        private const int CurrentSize = 3;
        
        public Blank3x3()
        {
            Cells = new CellOwner[3, 3];
        }

        public override int Size 
        { 
            get { return CurrentSize * CurrentSize; } 
        }

        // Получаем список со всеми пустыми ячейками
        public override List<CellInfo> EmptyCells
        {
            get
            {
                List<CellInfo> EmptyCells = new List<CellInfo>();

                for (int x = 0; x < CurrentSize; x++)
                {
                    for (int y = 0; y < CurrentSize; y++)
                    {
                        if (Cells[x, y] == CellOwner.Empty)
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
                int Count = 0;

                // столбцы
                for (int x = 0; x < CurrentSize; x++)
                {
                    Count = 0;
                    for (int y = 0; y < CurrentSize; y++) Count += (int)Cells[x, y];
                    if (Count == 3) return CellOwner.X;
                    if (Count == -3) return CellOwner.O;
                }

                // строчки
                for (int x = 0; x < 3; x++)
                {
                    Count = 0;
                    for (int y = 0; y < 3; y++) Count += (int)Cells[y, x];
                    if (Count == 3) return CellOwner.X;
                    if (Count == -3) return CellOwner.O;
                }

                // диагонали
                Count = (int)Cells[0, 0] + (int)Cells[1, 1] + (int)Cells[2, 2];
                if (Count == 3) return CellOwner.X;
                if (Count == -3) return CellOwner.O;

                Count = (int)Cells[0, 2] + (int)Cells[1, 1] + (int)Cells[2, 0];
                if (Count == 3) return CellOwner.X;
                if (Count == -3) return CellOwner.O;

                return CellOwner.Empty;
            }
        }

        public override Blank Clone()
        {
            Blank B = new Blank3x3();
            B.Cells = (CellOwner[,])this.Cells.Clone();
            return B;
        }
    }
}