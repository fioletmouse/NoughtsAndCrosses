using System;
namespace NoughtsAndCrosses.Classes
{
    public interface IGameRepository
    {
        int AddGame(string sessionId);
        void AddMove(int GameId, CellInfo s, CellOwner owner);
        System.Collections.Generic.IEnumerable<NoughtsAndCrosses.Models.GameInfoView> GetGamesInfo(string SesionId);
        System.Collections.Generic.IEnumerable<NoughtsAndCrosses.Models.MovesInfoView> GetMovesInfo(string GameId);
        System.Collections.Generic.IEnumerable<NoughtsAndCrosses.Models.Overall> GetOverall();
        void UpdateGameResult(int GameId, string Result);
    }
}
