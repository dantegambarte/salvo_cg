using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalvoCG.Models;
using SalvoCG.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SalvoCG.Controllers
{
    [Route("api/gamePlayers")]
    [ApiController]
    [Authorize("PlayerOnly")]
    public class GamePlayersController : ControllerBase
    {
        private IGamePlayerRepository _repository;
        private IPlayerRepository _playerRepository;
        public GamePlayersController(IGamePlayerRepository repository, IPlayerRepository playerRepository)
        {
            _repository = repository;
            _playerRepository = playerRepository;
        }

        // GET api/<GamePlayersController>/5
        [HttpGet("{id}", Name ="GetGameView")]
        public IActionResult GetGameView(long id)
        {
            try
            {
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";

                var gp = _repository.GetGamePlayerView(id);
                //veririficar si el gp corresponde al mismo emaild el usuarioa utenticado
                if (gp.Player.Email != email) return Forbid();
                var gameView = new GameViewDTO
                {
                    Id = gp.Id,
                    CreationDate = gp.Game.CreationDate,
                    Ships = gp.Ships.Select(ship => new ShipDTO
                    {
                        Id = ship.Id,
                        Type = ship.Type,
                        Locations = ship.Locations.Select(shipLocation => new ShipLocationDTO { 
                            Id = shipLocation.Id,
                            Location = shipLocation.Location
                        }).ToList()
                    }).ToList(),
                    GamePlayers = gp.Game.GamePlayers.Select(gps => new GamePlayerDTO
                    {
                        Id = gps.Id,
                        JoinDate = gps.JoinDate,
                        Player = new PlayerDTO
                        {
                            Id = gps.Player.Id,
                            Email = gps.Player.Email
                        }
                    }).ToList(),
                    Salvos = gp.Game.GamePlayers.SelectMany(gps => gps.Salvos.Select(salvo => new SalvoDTO
                    {
                        Id = salvo.Id,
                        Turn =salvo.Turn,
                        Player=new PlayerDTO
                        {
                            Id=gps.Player.Id,
                            Email=gps.Player.Email
                        },
                        Locations = salvo.Locations.Select(salvoLocation => new SalvoLocationDTO
                        {
                            Id = salvoLocation.Id,
                            Location = salvoLocation.Location
                        }).ToList()
                    })).ToList()
                };

                return Ok(gameView);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{id}/ships")]
        public IActionResult Post(long Id, [FromBody] List<ShipDTO> ships)
        {
            try
            {
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";
                Player player = _playerRepository.FindByEmail(email);
                GamePlayer gamePlayer = _repository.FindById(Id);

                if (gamePlayer == null) return StatusCode(403, "No existe el juego");
                if (gamePlayer.Player.Id != player.Id) return StatusCode(403, "El usuario no se encuentra en el juego");
                if (gamePlayer.Ships.Count == 5) return StatusCode(403, "Ya se posicionaron los barcos");

                gamePlayer.Ships = ships.Select(ship => new Ship
                {
                    GamePlayerId = gamePlayer.Id,
                    Type = ship.Type,
                    Locations = ship.Locations.Select(location => new ShipLocation
                    {
                        ShipId = ship.Id,
                        Location = location.Location
                    }).ToList()
                }).ToList();

                _repository.Save(gamePlayer);
                return StatusCode(201, "Creado");
            }
            catch(Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("{id}/salvos")]
        public IActionResult Post(long Id, [FromBody] SalvoDTO salvo)
        {
            try
            {
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";
                Player player = _playerRepository.FindByEmail(email);
                GamePlayer gamePlayer = _repository.FindById(Id); //gameplayer que esta logeado
                GamePlayer opponent = gamePlayer.GetOpponent();
                opponent = _repository.FindById(opponent.Id);
                if (gamePlayer.Game.GamePlayers.Count() != 2) return StatusCode(403, "No hay a quien disprar");
                if (gamePlayer == null) return StatusCode(403, "No existe el juego");
                if (gamePlayer.Player.Id != player.Id) return StatusCode(403, "El jugador no se encuentra en este juego");
                if (gamePlayer.Player.Email != email) return StatusCode(403, "El jugador no se encuentra en este juego");
                if (opponent == null) return StatusCode(403, "No existe oponente");
                if (gamePlayer.Ships.Count() == 0) return StatusCode(403, "El usuario logueado no ha posicionado los barcos");
                if (opponent.Ships.Count() == 0) return StatusCode(403, "El oponente no ha posicionado los barcos");
                if (gamePlayer.Ships.Count != 5) return StatusCode(403, "Las naves no estan posicionadas");
                if (opponent.Ships.Count != 5) return StatusCode(403, "Las naves del oponente no estan posicionadas");
                if (gamePlayer.Salvos.Count > opponent.Salvos.Count) return StatusCode(403, "No es tu turno");
                if ((gamePlayer.Salvos.Count == opponent.Salvos.Count) && gamePlayer.JoinDate > opponent.JoinDate)
                    return StatusCode(403, "No es tu turno");

                gamePlayer.Salvos.Add(new Salvo 
                {
                    GamePlayerId = Id,
                    Turn = gamePlayer.Salvos.Count + 1,
                    Locations = salvo.Locations.Select(location => new SalvoLocation
                    {
                        Location = location.Location
                    }).ToList()
                });
                _repository.Save(gamePlayer);
                return StatusCode(201, "Creado");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
