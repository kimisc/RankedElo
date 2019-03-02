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
        public readonly IPlayerService _playerService; 

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService ?? throw new ArgumentNullException(nameof(playerService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _playerService.GetAllPlayersAsync());
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var player = await _playerService.GetPlayerByNameAsync(name);

            if(player is null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpGet("topten")]
        public async Task<IActionResult> GetTopTenAsync()
        {
            return Ok(await _playerService.GetTopTenAsync());
        }
    }
}