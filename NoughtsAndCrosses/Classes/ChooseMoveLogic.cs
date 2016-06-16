using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoughtsAndCrosses.Classes
{
    public class ChooseMoveLogic
    {
        /// <summary>
        /// Implementation of the minimax algorithm.  Determines the best move for the current 
        /// board by playing every move combination until the end of the game.
        /// </summary>
        public static CellInfo GetBestMove(Blank gb, CellOwner p)
        {
            CellInfo? bestSpace = null;
            List<CellInfo> openSpaces = gb.OpenSquares;
            Blank newBoard;

            for (int i = 0; i < openSpaces.Count; i++)
            {
                newBoard = gb.Clone();
                CellInfo newSpace = openSpaces[i];

                newBoard[newSpace.X, newSpace.Y] = p;

                if (newBoard.Winner == CellOwner.Empty && newBoard.OpenSquares.Count > 0)
                {
                    CellInfo tempMove = GetBestMove(newBoard, ((CellOwner)(-(int)p)));  //a little hacky, inverts the current player
                    newSpace.Rank = tempMove.Rank;
                }
                else
                {
                    if (newBoard.Winner == CellOwner.Empty)
                        newSpace.Rank = 0;
                    else if (newBoard.Winner == CellOwner.X)
                        newSpace.Rank = -1;
                    else if (newBoard.Winner == CellOwner.O)
                        newSpace.Rank = 1;
                }

                //If the new move is better than our previous move, take it
                if (bestSpace == null ||
                   (p == CellOwner.X && newSpace.Rank < ((CellInfo)bestSpace).Rank) ||
                   (p == CellOwner.O && newSpace.Rank > ((CellInfo)bestSpace).Rank))
                {
                    bestSpace = newSpace;
                }
            }

            return (CellInfo)bestSpace;
        }
    }
}