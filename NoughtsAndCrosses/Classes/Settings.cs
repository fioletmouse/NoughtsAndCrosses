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
            { GameResult.X, "Победа X"},
            { GameResult.O, "Победа O"},
            { GameResult.Unknown, "Ничья"},
        };
        private const string NotFinished = "Игра не закончена";
        
        // Получение результата игры, вынесено методом для обработки пустого значение
        public static string GetResultDefinition(int? result)
        {
            string ret = NotFinished;
            if (result.HasValue)
            {
                ret = DefaultGameResult[(GameResult)result];
            }
            return ret;
        }

        public static Dictionary<GameResult, string> DefaultMessage = new Dictionary<GameResult, string>
        {
            { GameResult.X, "Компьютер выиграл"},
            { GameResult.O, "Вы выиграли"},
            { GameResult.Unknown, "Ничья!"},
        };
    }
}