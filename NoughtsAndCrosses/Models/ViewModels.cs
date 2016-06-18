using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoughtsAndCrosses.Models
{
    public class MoveResultModel
    {
        public string RedirectLink { get; set; }
        public string WinnerInfo { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class GameInfoView
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public string Result { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public int MoveCount { get; set; }
    }

    public class MovesInfoView
    {
        public string MoveOwner { get; set; }
        public string Point { get; set; }
    }

    public class Overall
    {
        public string ResultType { get; set; }
        public int Count { get; set; }
    }

}