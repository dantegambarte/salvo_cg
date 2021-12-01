using Microsoft.EntityFrameworkCore;
using SalvoCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalvoCG.Repositories
{
    public class GamePlayerRepository : RepositoryBase<GamePlayer>, IGamePlayerRepository
    {
        public GamePlayerRepository(SalvoContext repositoryContext): base(repositoryContext)
        {

        }

        public GamePlayer FindById(long Id)
        {
            return FindByCondition(gp => gp.Id == Id)
                .Include(gp => gp.Game)
                    .ThenInclude(game => game.GamePlayers)
                        .ThenInclude(gp => gp.Salvos)
                .Include(gp => gp.Game)
                    .ThenInclude(game => game.GamePlayers)
                        .ThenInclude(gp => gp.Ships)
                .Include(gp => gp.Player)
                .Include(gp => gp.Ships)
                .Include(gp => gp.Salvos)
                .FirstOrDefault();
        }

        public GamePlayer GetGamePlayerView(long idGamePlayer)
        {
            return FindAll(
                source => source
                .Include(gamePlayer => gamePlayer.Ships)
                    .ThenInclude(ship => ship.Locations)
                .Include(gamePlayer => gamePlayer.Salvos)
                    .ThenInclude(salvo => salvo.Locations)
                .Include(gamePlayer => gamePlayer.Game)
                    .ThenInclude(game => game.GamePlayers)
                        .ThenInclude(gp => gp.Player)
                .Include(gamePlayer => gamePlayer.Game)
                    .ThenInclude(game => game.GamePlayers)
                        .ThenInclude(gp => gp.Salvos)
                            .ThenInclude(salvo => salvo.Locations)
                 .Include(gamePlayer => gamePlayer.Game)
                    .ThenInclude(game => game.GamePlayers)
                        .ThenInclude(gp => gp.Ships)
                            .ThenInclude(ship => ship.Locations)
            )
                .Where(gamePlayer => gamePlayer.Id == idGamePlayer)
                .OrderBy(game => game.JoinDate)
                .FirstOrDefault();
        }

        public void Save(GamePlayer gamePlayer)
        {
            if (gamePlayer.Id == 0) Create(gamePlayer);
            else Update(gamePlayer);
            SaveChanges();
        }

    }
}
