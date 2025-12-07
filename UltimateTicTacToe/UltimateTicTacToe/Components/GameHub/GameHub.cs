using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using UltimateTicTacToe.Components.Models;
using UltimateTicTacToe.Components.Services;
using UltimateTicTacToe.Shared.Models;

namespace UltimateTicTacToe.Components.GameHub;

public class GameHub(GameService games) : Hub {
    public async Task JoinGame(string gameId) {
        Console.WriteLine($"{Context.ConnectionId} is joining {gameId}");
        GameState gs = games.GetOrCreateGameState(gameId);
        int? p = gs.GetPlayer(Context.ConnectionId);
        if (p != null) {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.Client(Context.ConnectionId).SendAsync("InitData",
                JsonConvert.SerializeObject(new InitData { MyPlayer = p.Value }));
            await Clients.Client(Context.ConnectionId).SendAsync("UpdateState", JsonConvert.SerializeObject(gs));
            Console.WriteLine("Player exists, updating");
            return;
        }
        if (gs.PlayerCount >= 2) {
            // spectator?
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.Client(Context.ConnectionId).SendAsync("UpdateState", JsonConvert.SerializeObject(gs));
        }

        int c = gs.RegisterPlayer(Context.ConnectionId);
        Console.WriteLine($"{Context.ConnectionId} registered {gameId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        await Clients.Client(Context.ConnectionId)
            .SendAsync("InitData", JsonConvert.SerializeObject(new InitData { MyPlayer = c }));
        await Clients.Client(Context.ConnectionId).SendAsync("UpdateState", JsonConvert.SerializeObject(gs));
    }

    public async Task MakeMove(string gameId, MoveInput moveInput) {
        Console.WriteLine("received make move");
        List<ValidationResult> results = [];
        bool valid = Validator.TryValidateObject(moveInput, new ValidationContext(moveInput), results, true);
        if (!valid) {
            throw new HubException(string.Join(", ", results.Select(r => r.ErrorMessage)));
        }
        GameState? gs = games.GetGameState(gameId);
        int? player = gs?.GetPlayer(Context.ConnectionId);
        if (player == null) {
            Console.WriteLine("player not found");
            return;
        }

        bool validMove = gs!.MakeMove(player.Value, moveInput.I, moveInput.J, moveInput.X, moveInput.Y);
        if (validMove) {
            gs.PlayerTurn++;
            if (gs.PlayerTurn > gs.PlayerCount) {
                gs.PlayerTurn = 1;
            }
            await Clients.Group(gameId).SendAsync("UpdateState", JsonConvert.SerializeObject(gs));
        } else {
            Console.WriteLine("invalid move");
        }
    }
}
