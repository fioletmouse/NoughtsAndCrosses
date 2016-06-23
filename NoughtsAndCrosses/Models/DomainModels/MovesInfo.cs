using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NoughtsAndCrosses.Models
{
    public class MovesInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string MoveOwner { get; set; }
        [Required]
        public int RowX { get; set; }
        [Required]
        public int ColY { get; set; }

        // Обратная ссылка
        [Required]
        public GameInfo GameInfo { get; set; }
    }
}