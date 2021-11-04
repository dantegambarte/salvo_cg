using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalvoCG.Models
{
    public class GamePlayerDTO
    {
        public long Id { get; set; }
        public DateTime? JoinDate { get; set; }
        public PlayerDTO Player{ get; set; }
    }
}
