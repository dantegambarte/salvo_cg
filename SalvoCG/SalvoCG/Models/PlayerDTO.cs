using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalvoCG.Models
{
    public class PlayerDTO
    {
        public long Id { get; set; }
        //[EmailAddress(ErrorMessage = "Ingrese bien su email")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
    }
}
