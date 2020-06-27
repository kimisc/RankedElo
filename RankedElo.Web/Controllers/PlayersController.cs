using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RankedElo.Core.Interfaces;

namespace RankedElo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        public readonly IPlayerRepository PlayerRepository; 

        public PlayersController(IPlayerRepository playerRepository)
        {
            PlayerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await PlayerRepository.GetAllPlayersAsync());
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var player = await PlayerRepository.GetPlayerByNameAsync(name);

            if(player is null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpGet("topten")]
        public async Task<IActionResult> GetTopTenAsync()
        {
            return Ok(await PlayerRepository.GetTopTenAsync());
        }
    }
}