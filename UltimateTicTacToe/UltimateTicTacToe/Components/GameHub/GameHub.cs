using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using UltimateTicTacToe.Components.Models;
using UltimateTicTacToe.Components.Services;
using UltimateTicTacToe.Shared.Models;

namespace UltimateTicTacToe.Components.GameHub;

public class GameHub(GameService games) : Hub {
    public async Task JoinGame(string gameId) {
        GameState gs = games.GetOrCreateGameState(gameId);
        int? p = gs.GetPlayer(Context.ConnectionId);
        if (p != null) {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.Client(Context.ConnectionId).SendAsync("InitData", new InitData { MyPlayer = p.Value});
            await Clients.Client(Context.ConnectionId).SendAsync("UpdateState", JsonConvert.SerializeObject(gs));
            return;
        }
        if (gs.PlayerCount >= 2) return;
        int c = gs.RegisterPlayer(Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        await Clients.Client(Context.ConnectionId).SendAsync("InitData", new InitData { MyPlayer = c});
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
        
        bool validMove = gs!.MakeMove(player.Value, moveInput.X, moveInput.Y, moveInput.I, moveInput.J);
        if (validMove) {
            Console.WriteLine("sending update");
            Console.WriteLine(gs.Board[moveInput.X][moveInput.Y].Board[moveInput.I][moveInput.J]);
            await Clients.Group(gameId).SendAsync("UpdateState", JsonConvert.SerializeObject(gs));
        } else {
            Console.WriteLine("invalid move");
        }
    }
}
