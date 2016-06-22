using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoughtsAndCrosses.Models
{
    public class GameInfoView
    {
        public int Id { get; set; }
        public string Result { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public int MoveCount { get; set; }

        //public Pagination Pagination { get; set; }
    }
}