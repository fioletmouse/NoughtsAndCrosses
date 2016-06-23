using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoughtsAndCrosses.Models
{
    public class GamesInfoPage
    {
        public IEnumerable<GameInfoView> GamesList { get; set; }
        public Pagination Pagination { get; set; }
    }
}