using System.Collections.Concurrent;
using UltimateTicTacToe.Shared.Models;

namespace UltimateTicTacToe.Components.Services;

public class GameService {
    // id
    private readonly ConcurrentDictionary<string, GameState> _games = new ();
    public readonly ConcurrentStack<string?> PublicLobbies = new();

    public GameState CreateGameState(string gameId) {
        return _games.GetOrAdd(gameId, _ => new GameState());
    }

    public GameState? GetGameState(string gameId) {
        return _games.GetValueOrDefault(gameId);
    }

    public void KillGame(string gameId) {
        _games.TryRemove(gameId, out _);
    }

    public void RemoveFromGame(string connectionId) {
        foreach(KeyValuePair<string, GameState> gs in _games) {
            if (gs.Value.UnregisterPlayer(connectionId)) {
                if (gs.Value.PlayerCount == 0) {
                    KillGame(gs.Key);
                }
                break;
            }
        }
    }
}
