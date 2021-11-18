using SalvoCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalvoCG.Repositories
{
    public interface IGameRepository : IRepositoryBase<Game>
    {
        IEnumerable<Game> GetAllGames();
        IEnumerable<Game> GetAllGamesWhitPlayers();
        Game FindById(long Id);
    }
}
