using System.Collections.Generic;
using UnityEngine;

public class MaskPiece : Piece
{
    public bool isRed;

    public override bool IsRed()
    {
        return isRed;
    }

    public override List<Vector2Int> GetAvailableMoves(BoardManager board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right,
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };

        foreach (var dir in directions)
        {
            Vector2Int target = currentGridPos + dir;
            Tile tile = board.GetTileAt(target);

            if (tile != null)
            {
                if (!tile.isOccupied)
                {
                    moves.Add(target);
                }
                else
                {
                    Piece occupant = tile.currentOccupant?.GetComponent<Piece>();
                    if (occupant != null && IsEnemy(occupant))
                    {
                        moves.Add(target); // On peut capturer
                    }
                }
            }
        }

        return moves;
    }
}
