using PlayerStoreApi.Models;
using PlayerStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlayerStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly PlayerService _PlayersService;

    public PlayersController(PlayerService PlayersService) =>
        _PlayersService = PlayersService;

    [HttpGet]
    public async Task<List<Player>> Get() =>
        await _PlayersService.GetAsync();

    // Endpoint para obtener los 10 mejores jugadores
    [HttpGet("top10")]
    public async Task<List<Player>> GetTop10Players()
    {

        var topPlayers = await _PlayersService.GetTop10PlayersAsync();
        return topPlayers; // Devuelve la lista de los 10 mejores jugadores
        
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Player>> Get(string id)
    {
        var Player = await _PlayersService.GetAsync(id);

        if (Player is null)
        {
            return NotFound();
        }

        return Player;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Player newPlayer)
    {
        await _PlayersService.CreateAsync(newPlayer);

        return CreatedAtAction(nameof(Get), new { id = newPlayer.Id }, newPlayer);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Player updatedPlayer)
    {
        var Player = await _PlayersService.GetAsync(id);

        if (Player is null)
        {
            return NotFound();
        }

        updatedPlayer.Id = Player.Id;

        await _PlayersService.UpdateAsync(id, updatedPlayer);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var Player = await _PlayersService.GetAsync(id);

        if (Player is null)
        {
            return NotFound();
        }

        await _PlayersService.RemoveAsync(id);

        return NoContent();
    }
}