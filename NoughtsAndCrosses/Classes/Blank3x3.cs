using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoughtsAndCrosses.Classes
{

    /// <summary>
    /// Contains all of the functions and logic needed for the game of TicTacToe
    /// </summary>
    class Blank3x3 : Blank
    {
        private const int currentSize = 3;
        
        public Blank3x3()
        {
            cells = new CellOwner[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        }

        public override int Size { get { return currentSize * currentSize; } }

        public override CellOwner this[int x, int y]
        {
            get
            {
                return cells[x, y];
            }

            set
            {
                cells[x, y] = value;
            }
        }

        public override bool IsFull
        {
            get
            {
                foreach (CellOwner i in cells)
                    if (i == CellOwner.Empty) { return false; }
                return true;
            }
        }

        public override List<CellInfo> OpenSquares
        {
            get
            {
                List<CellInfo> openSquares = new List<CellInfo>();

                for (int x = 0; x <= 2; x++)
                    for (int y = 0; y <= 2; y++)
                        if (cells[x, y] == CellOwner.Empty)
                            openSquares.Add(new CellInfo(x, y));

                return openSquares;
            }
        }


        public override CellOwner Winner
        {
          /*  get 
            {
                // проверяем строки/столбцы
                for (int i = 0; i < currentSize; i++)
                {
                    if (((int)cells[i, 0] + (int)cells[i, 1] + (int)cells[i, 2]) == Math.Abs(3))
                        return cells[i, 0];

                    if (((int)cells[0, i] + (int)cells[1, i] + (int)cells[2, i]) == Math.Abs(3))
                        return cells[0, i];
                }

                // проверяем диагональ
                if (((int)cells[0, 0] + (int)cells[1, 1] + (int)cells[2, 2]) == Math.Abs(3))
                    return cells[0, 0];

                if (((int)cells[0, 2] + (int)cells[1, 1] + (int)cells[2, 0]) == Math.Abs(3))
                    return cells[0, 2];

                return CellOwner.Empty;
            }*/
            get
            {
                int count = 0;

                //columns
                for (int x = 0; x < currentSize; x++)
                {
                    count = 0;

                    for (int y = 0; y < currentSize; y++)
                        count += (int)cells[x, y];

                    if (count == 3)
                        return CellOwner.X;
                    if (count == -3)
                        return CellOwner.O;
                }

                //rows
                for (int x = 0; x < 3; x++)
                {
                    count = 0;

                    for (int y = 0; y < 3; y++)
                        count += (int)cells[y, x];

                    if (count == 3)
                        return CellOwner.X;
                    if (count == -3)
                        return CellOwner.O;
                }

                //diagnols right to left
                count = 0;
                count += (int)cells[0, 0];
                count += (int)cells[1, 1];
                count += (int)cells[2, 2];
                if (count == 3)
                    return CellOwner.X;
                if (count == -3)
                    return CellOwner.O;

                //diagnols left to right
                count = 0;
                count += (int)cells[0, 2];
                count += (int)cells[1, 1];
                count += (int)cells[2, 0];
                if (count == 3)
                    return CellOwner.X;
                if (count == -3)
                    return CellOwner.O;

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