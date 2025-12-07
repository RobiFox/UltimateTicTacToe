namespace UltimateTicTacToe.Shared;

public static class UtilHelper {
    public static bool IsWinner(int i, int[,] board) {
        // TODO jobban kéne megoldani
        return board[0, 0] == i && board[0, 1] == i && board[0, 2] == i
               || board[1, 0] == i && board[1, 1] == i && board[1, 2] == i
               || board[2, 0] == i && board[2, 1] == i && board[2, 2] == i
               || board[0, 0] == i && board[1, 0] == i && board[2, 0] == i
               || board[0, 1] == i && board[1, 1] == i && board[2, 1] == i
               || board[0, 2] == i && board[1, 2] == i && board[2, 2] == i
               || board[0, 0] == i && board[1, 1] == i && board[2, 2] == i
               || board[0, 2] == i && board[1, 1] == i && board[2, 0] == i
            ;
    }
}
