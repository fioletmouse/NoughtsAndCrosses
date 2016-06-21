using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoughtsAndCrosses.Models
{
    public class MoveResultModel
    {
        public string WinnerInfo { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}