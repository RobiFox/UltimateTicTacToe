using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using UltimateTicTacToe.Components.Services;

namespace UltimateTicTacToe.Components.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController(GameService gameService) {
    [HttpGet("public")]
    public string GetPublicGameId() {
        do {
            if (gameService.PublicLobbies.TryPop(out string? publicId) && publicId != null) {
                if (gameService.GetGameState(publicId)?.PlayerCount < 2) {
                    return publicId;
                }
            } else {
                break;
            }
        } while (!gameService.PublicLobbies.IsEmpty);

        string randomId = RandomNumberGenerator.GetHexString(6, true);
        gameService.CreateGameState(randomId);
        gameService.PublicLobbies.Push(randomId);
        return randomId;
    }

    [HttpGet("private")]
    public string GetPrivateGameId() {
        string randomId = RandomNumberGenerator.GetHexString(6, true);
        gameService.CreateGameState(randomId);
        return randomId;
    }

    [HttpGet("is_valid/{gameId}")]
    public bool GetPrivateGameIsValid(string gameId) {
        return gameService.GetGameState(gameId) != null;
    }
}
