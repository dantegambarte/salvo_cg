using SalvoCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalvoCG.Repositories
{
    public interface IPlayerRepository
    {
        Player FindByEmail(string email);
    }
}
