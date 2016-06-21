using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NoughtsAndCrosses.Models
{
    public class GameInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string SessionId { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [MaxLength(100)]
        public string PlayerName { get; set; }
        public int? Result { get; set; }
        
        // навигация на историю ходов
        public List<MovesInfo> Moves { get; set; }
    }
}