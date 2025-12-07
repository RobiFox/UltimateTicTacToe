namespace UltimateTicTacToe.Shared.Models;

public class GameBoard {
    public List<List<int>> Board = [];
    public int WonBy = -1;
    
    public GameBoard() {
        if (OperatingSystem.IsBrowser()) return;
        for (int i = 0; i < 3; i++) {
            Board.Add([]);
            for (int j = 0; j < 3; j++) {
                Board[i].Add(-1);
            }
        }
    }

    public bool MakeMove(int player, int i, int j) {
        if (Board[i][j] != -1) {
            Console.WriteLine("already occupied");
            return false;
        }

        Console.WriteLine("Placing " + player);
        Board[i][j] = player;
        Console.WriteLine(Board[i][j]);
        if (UtilHelper.IsWinner(player, Board)) {
            WonBy = player;
        }
        return true;
    }
}
