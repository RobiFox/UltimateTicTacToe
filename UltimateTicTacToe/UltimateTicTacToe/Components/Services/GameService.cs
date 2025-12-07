using System.Collections.Concurrent;
using UltimateTicTacToe.Components.Models;
using UltimateTicTacToe.Shared.Models;

namespace UltimateTicTacToe.Components.Services;

public class GameService {
    // id
    private readonly ConcurrentDictionary<string, GameState> _games = new ();

    public GameState GetOrCreateGameState(string gameId) {
        return _games.GetOrAdd(gameId, _ => new GameState());
    }

    public GameState? GetGameState(string gameId) {
        return _games[gameId];
    }

    public void KillGame(string gameId) {
        _games.TryRemove(gameId, out _);
    }

    public void RemoveFromgame(string connectionId) {
        foreach(GameState gs in _games.Values) {
            gs.GetPlayer()
        }
    }
}
