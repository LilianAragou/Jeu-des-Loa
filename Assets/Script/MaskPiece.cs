using System.Collections.Generic;
using UnityEngine;

public class MaskPiece : Piece
{
    public override List<Vector2Int> GetAvailableMoves(BoardManager board)
    {
        List<Vector2Int> moves = new();
        Vector2Int[] directions = {
            new(1, 0), new(-1, 0), new(0, 1), new(0, -1),       // Orthogonales
            new(1, 1), new(1, -1), new(-1, 1), new(-1, -1)      // Diagonales
        };

        foreach (var dir in directions)
        {
            Vector2Int target = currentGridPos + dir;
            Tile tile = board.GetTileAt(target);

            if (tile != null && !tile.isOccupied)
                moves.Add(target);
        }

        return moves;
    }
}
