using System.Collections.Concurrent;
using UltimateTicTacToe.Components.Models;
using UltimateTicTacToe.Shared.Models;

namespace UltimateTicTacToe.Components.Services;

public class GameService {
    // id
    private ConcurrentDictionary<string, GameState> _games = new ();

    public GameState GetOrCreateGameState(string gameId) {
        return _games.GetOrAdd(gameId, _ => new GameState());
    }

    public GameState? GetGameState(string gameId) {
        return _games[gameId];
    }
}
