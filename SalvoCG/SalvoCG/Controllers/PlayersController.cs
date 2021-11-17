using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalvoCG.Models;
using SalvoCG.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalvoCG.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private IPlayerRepository _repository;
        public PlayersController(IPlayerRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] PlayerDTO player)
        {
            try
            {
                //cantidad de caracteres, mayusculas, numeros | verificar estandar de claves? | revisar expresiones regulares

                if (String.IsNullOrEmpty(player.Email) && String.IsNullOrEmpty(player.Password) && String.IsNullOrEmpty(player.Name))
                    return StatusCode(403, "Campos vacios");
                else
                {
                    if (String.IsNullOrEmpty(player.Email)) return StatusCode(403, "Email vacío");
                    if (String.IsNullOrEmpty(player.Password)) return StatusCode(403, "Password vacío");
                    if (String.IsNullOrEmpty(player.Name)) return StatusCode(403, "Nombre vacío");
                    if (VerificalEmail(player.Email) == false)
                    {
                        return StatusCode(403, "Escriba bien su email");
                    }
                }

                    Player dbPlayer = _repository.FindByEmail(player.Email);
                if(dbPlayer != null) return StatusCode(403, "Email está en uso");

                Player newPlayer = new Player
                {
                    Email = player.Email,
                    Password = player.Password,
                    Name = player.Name
                };

                _repository.Save(newPlayer);
                return StatusCode(201, newPlayer);

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private Boolean VerificalEmail(string email)
        {
            String expresion;
            expresion = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
