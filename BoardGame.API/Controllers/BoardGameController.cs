using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGame.API.Services;
using BoardGame.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
namespace BoardGame.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardGameController : ControllerBase
    {
    
        private readonly IRedisService _redisService;
        private readonly IGameHelperService _helperService;

        public BoardGameController(IRedisService redisService,IGameHelperService helperService)
        {
            
            _redisService = redisService;
            _helperService = helperService;
        }

        [HttpPost("CreateBoard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBoard(int size)
        {
            if (size > 0)
            {
                var board = new Board(size);
                
                await _redisService.Set<Board>(board.BoardId.ToString(), board);

                return Ok(board.BoardId);
            }
            return BadRequest("Board cannot be created of size " + size ) ;

        }

        [HttpGet("GetBoard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBoard(Guid boardId)
        { 
            var board = await _redisService.Get<Board>(boardId.ToString());
            if (board != null)
            {
                return Ok(board);
            }
            return NotFound("Unable to get board for request " + boardId.ToString());
        }

        [HttpPost("AddBattleship/{boardId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBattleship(Guid boardId, Battleship ship)
        {
            if (ship == null || ship.Size<=0)
            {
                return BadRequest("Cannot add ship of empty size");
            }

            var board = await _redisService.Get<Board>(boardId.ToString());
            if (board == null)
            {
                return BadRequest("Unable to get board for request " + boardId.ToString());
            }

            if (!_helperService.AreCellsAvailable(board, ship))
            { 
                return BadRequest("Selected position is not available on board " + boardId.ToString());
            }

            var result = _helperService.AddBattleShip(board, ship);

            if (result)
            {
                await _redisService.Set<Board>(board.BoardId.ToString(), board);
            }

            return Ok(new { IsBattleshipAdded =  result });
        }

        [HttpPost("AttackResult/{boardId}/{row:int}/{column:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AttackResult(Guid boardId, int row,int column)
        {

            var board = await _redisService.Get<Board>(boardId.ToString());
            if (board == null)
            {
                return BadRequest("Unable to get board for request " + boardId.ToString());
            }

            var result = _helperService.IsAttacSuccessful(board, row,column);

            if (result)
            {
                await _redisService.Set<Board>(board.BoardId.ToString(), board);
            }

            return Ok(new { IsAttacSuccessful = result });
        }
    }
}