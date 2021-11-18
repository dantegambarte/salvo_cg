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

        public Game FindById(long Id)
        {
            return FindByCondition(game => game.Id == Id)
                .Include(game => game.GamePlayers)
                    .ThenInclude(gp => gp.Player)
                .FirstOrDefault();
        }

        public IEnumerable<Game> GetAllGames()
        {
            return FindAll().OrderBy(game => game.CreationDate).ToList();
        }

        public IEnumerable<Game> GetAllGamesWhitPlayers()
        {
            return FindAll(source => source.Include(game => game.GamePlayers)
                .ThenInclude(gamePlayer => gamePlayer.Player)
                    .ThenInclude(player => player.Scores))
                .OrderBy(game => game.CreationDate)
                .ToList();
        }
    }
}
