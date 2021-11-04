using Microsoft.EntityFrameworkCore;
using SalvoCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalvoCG.Repositories
{
    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(SalvoContext repositoryContext) : base(repositoryContext)
        {

        }
        public IEnumerable<Game> GetAllGames()
        {
            return FindAll().OrderBy(game => game.CreationDate).ToList();
        }

        public IEnumerable<Game> GetAllGamesWhitPlayers()
        {
            return FindAll(source => source.Include(game => game.GamePlayers)
            .ThenInclude(gamePlayer => gamePlayer.Player)).OrderBy(game => game.CreationDate).ToList();
        }
    }
}
