using System.ComponentModel.DataAnnotations;

namespace UltimateTicTacToe.Components.Models;

public class MoveInput {
    [Required] [Range(0, 2)] public int X { get; set; }
    [Required] [Range(0, 2)] public int Y { get; set; }
    [Required] [Range(0, 2)] public int I { get; set; }
    [Required] [Range(0, 2)] public int J { get; set; }
}
