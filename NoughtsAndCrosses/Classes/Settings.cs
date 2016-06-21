using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoughtsAndCrosses.Classes
{
    /// <summary>
    /// Настройки игры. Сообщения и приведение переменных к пользовательскому виду
    /// </summary>
    public static class Settings
    {
        private static Dictionary<GameResult, string> DefaultGameResult = new Dictionary<GameResult, string>
        {
            { GameResult.X, "Победa Котикa"},
            { GameResult.O, "Победa Игрока"},
            { GameResult.Unknown, "Ничья"},
        };
        private const string NotFinished = "Игра не закончена";
        
        // Получение результата игры, вынесено методом для обработки пустого значение
        public static string GetOverAllDefinition(int? Result)
        {
            string Ret = NotFinished;
            if (Result.HasValue)
            {
                Ret = DefaultGameResult[(GameResult)Result];
            }
            return Ret;
        }

        public static Dictionary<GameResult, string> DefaultMessage = new Dictionary<GameResult, string>
        {
            { GameResult.X, "Котик выиграл."},
            { GameResult.O, "Вы выиграли."},
            { GameResult.Unknown, "Ничья!"},
        };

        public static Dictionary<CellOwner, string> PlayersName = new Dictionary<CellOwner, string>
        {
            { CellOwner.O, "Игрок"},
            { CellOwner.X, "Котик"}
        };
    }
}