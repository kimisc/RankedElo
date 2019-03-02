using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RankedElo.Core.Entities;
using RankedElo.Core.Interfaces;

namespace RankedElo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IRepository _matchRepository;

        public MatchesController(IRepository repository)
        {
            _matchRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int id)
        {
            return Ok(await _matchRepository.List<Match>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatch(int id)
        {
            return Ok(await _matchRepository.GetById<Match>(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddMatch(Match match)
        {
            await _matchRepository.Add(match);
            return Ok();
        }

    }
}
