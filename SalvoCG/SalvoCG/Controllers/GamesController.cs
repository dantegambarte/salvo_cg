﻿using Microsoft.AspNetCore.Mvc;
using SalvoCG.Models;
using SalvoCG.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SalvoCG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private IGameRepository _repository;
        public GamesController(IGameRepository repository)
        {
            _repository = repository;
        }
        // GET: api/<GamesController>
        [HttpGet]
        public IActionResult Get()
        {
            try 
            {
                //var games = _repository.GetAllGames();
                //se necesita devolver una lista de game que finalmente es una lista de gamesDTO
                var games = _repository.GetAllGamesWhitPlayers()
                    .Select(g => new GameDTO
                    {
                        Id = g.Id,
                        CreationDate = g.CreationDate,
                        GamePlayers = g.GamePlayers.Select(gp => new GamePlayerDTO
                        {
                            Id = gp.Id,
                            JoinDate = gp.JoinDate,
                            Player = new PlayerDTO
                            {
                                Id = gp.Player.Id,
                                Email = gp.Player.Email
                            }
                        }).ToList()
                    }).ToList();
                return Ok(games);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
