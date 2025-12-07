namespace UltimateTicTacToe.Shared.Models;

public class GameState {
    public List<List<GameBoard>> Board { get; set; } = [];

    public int PlayerTurn { get; set; } = 0;

    public (int, int) AllowedBoard { get; set; } = (-1, -1); // for all

    private readonly Dictionary<string, int> _players = new();

    public int PlayerCount => _players.Count;

    public GameState() {
        if (OperatingSystem.IsBrowser()) return;
        for (int i = 0; i < 3; i++) {
            Board.Add([]);
            for (int j = 0; j < 3; j++) {
                Board[i].Add(new GameBoard());
            }
        }
    }

    public int RegisterPlayer(string id) {
        int c = PlayerCount + 1;
        _players.Add(id, c);
        return c;
    }

    public int? GetPlayer(string id) {
        return _players.TryGetValue(id, out int player)
            ? player
            : null;
    }

    public bool MakeMove(int player, int x, int y, int i, int j) {
        //if (player != PlayerTurn) return false;
        if (Board[x][y].WonBy != -1) {
            Console.WriteLine("board is already won");
            return false;
        }

        if (Board[x][y].MakeMove(player, i, j)) {
            AllowedBoard = Board[i][j].WonBy == -1
                ? (i, j)
                : (-1, -1);
            Console.WriteLine(Board[x][y].Board[i][j]);
            return true;
        }
        return false;
    }

    public List<List<int>> BoardToWinnerBoard() {
        List<List<int>> board = [];
        for (int i = 0; i < 3; i++) {
            board.Add([]);
            for (int j = 0; j < 3; j++) {
                board[i].Add(Board[i][j].WonBy);
            }
        }
        return board;
    }
}
