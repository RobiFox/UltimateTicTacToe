namespace UltimateTicTacToe.Shared.Models;

public class GameState {
    public GameBoard[,] Board { get; set; } = new GameBoard[3, 3];

    public int PlayerTurn { get; set; } = 1;

    public (int, int) AllowedBoard { get; set; } = (-1, -1); // for all

    private readonly Dictionary<string, int> _players = new();

    public int PlayerCount => _players.Count;

    public GameState() {
        if (OperatingSystem.IsBrowser()) return;

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Board[i, j] = new GameBoard();
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
        if (player != PlayerTurn) return false;
        if (AllowedBoard != (x, y) && AllowedBoard != (-1, -1)) {
            Console.WriteLine("invalid small board");
            return false;
        }
        if (Board[x, y].WonBy != -1) {
            Console.WriteLine("board is already won");
            return false;
        }

        if (Board[x, y].MakeMove(player, i, j)) {
            AllowedBoard = Board[i, j].WonBy == -1
                ? (i, j)
                : (-1, -1);
            Console.WriteLine(Board[x, y].Board[i, j]);
            return true;
        }
        return false;
    }

    public int[,] BoardToWinnerBoard() {
        int[,] board = new int[3, 3];

        for (int j = 0; j < 3; j++) {
            for (int i = 0; i < 3; i++) {
                board[i, j] = Board[i, j].WonBy;
            }
        }
        return board;
    }
}
