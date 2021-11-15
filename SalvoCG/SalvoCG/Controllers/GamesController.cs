﻿using Microsoft.AspNetCore.Authorization;
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
    [Route("api/games")]
    [ApiController]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private IGameRepository _repository;
        public GamesController(IGameRepository repository)
        {
            _repository = repository;
        }
        // GET: api/<GamesController>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try 
            {
                GameListDTO gameList = new GameListDTO
                {
                    Email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest",
                    Games = _repository.GetAllGamesWhitPlayers()
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
                            },
                            Point = gp.GetScore() != null ? (double?)gp.GetScore().Point : null
                        }).ToList()
                    }).ToList()
                };

                return Ok(gameList);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
