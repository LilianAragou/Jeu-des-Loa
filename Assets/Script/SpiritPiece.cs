using System.Collections.Generic;
using UnityEngine;

public class SpiritPiece : Piece
{
    
    public bool isRed;


    public override bool IsRed()

    {
        return isRed;
    }

    public override List<Vector2Int> GetAvailableMoves(BoardManager board)
{
    List<Vector2Int> moves = new();
    Vector2Int[] directions = {
        new(1, 0), new(-1, 0),  // droite, gauche
        new(0, 1), new(0, -1)   // haut, bas
    };

    foreach (var dir in directions)
    {
        Vector2Int target = currentGridPos + dir;
        Tile tile = board.GetTileAt(target);

        if (tile == null) continue;

        if (!tile.isOccupied)
        {
            moves.Add(target); // case vide
        }
        else
        {
            Piece occupant = tile.currentOccupant?.GetComponent<Piece>();
            if (occupant != null && IsEnemy(occupant))
            {
                moves.Add(target); // case occupée par un ennemi = possible capture
            }
        }
    }

    return moves;
}

}
