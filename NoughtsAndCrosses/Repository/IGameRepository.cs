using System;
using System.Collections.Generic;
using NoughtsAndCrosses.Models;
using NoughtsAndCrosses.Classes;

namespace NoughtsAndCrosses.Repository
{
    public interface IGameRepository
    {
        int AddGame(string sessionId);
        void AddMove(int GameId, CellInfo s, CellOwner owner);
        IEnumerable<GameInfoView> GetGamesInfo(string SesionId);
        IEnumerable<MovesInfoView> GetMovesInfo(string GameId);
        IEnumerable<Overall> GetOverall();
        void UpdateGameResult(int GameId, GameResult Winner);
    }
}
