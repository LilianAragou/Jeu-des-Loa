using System.Collections.Generic;
using UnityEngine;

public class SpiritPiece : Piece
{
    public override List<Vector2Int> GetAvailableMoves(BoardManager board)
    {
        List<Vector2Int> moves = new();
        Vector2Int[] directions = {
            new(1, 0), new(-1, 0),  // Droite, Gauche
            new(0, 1), new(0, -1)   // Haut, Bas
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
