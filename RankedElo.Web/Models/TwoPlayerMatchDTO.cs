using System;
using System.ComponentModel.DataAnnotations;

namespace RankedElo.Web.Models
{
    public class TwoPlayerMatchDTO
    {
        [MaxLength(10)]
        public string Player1Name { get; set; }
        [MaxLength(10)]
        public string Player2Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }

    }
}