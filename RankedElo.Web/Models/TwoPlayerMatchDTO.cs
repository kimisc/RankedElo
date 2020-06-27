using System;
using System.ComponentModel.DataAnnotations;

namespace RankedElo.Web.Models
{
    public class BaseMatchDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
    }

    public class TwoPlayerMatchDto : BaseMatchDto
    {
        [MaxLength(10)]
        public string Player1Name { get; set; }
        [MaxLength(10)]
        public string Player2Name { get; set; }
    }
}