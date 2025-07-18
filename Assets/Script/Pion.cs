using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2Int currentGridPos;      // Position sur la grille
    protected BoardManager board;          // Référence au plateau

    protected virtual void Start()
    {
        board = FindFirstObjectByType<BoardManager>();
    }

    public void MoveTo(Vector2Int newPos)
    {
        Tile oldTile = board.GetTileAt(currentGridPos);
        if (oldTile != null)
            oldTile.SetOccupant(null);

        Tile newTile = board.GetTileAt(newPos);
        if (newTile != null && !newTile.isOccupied)
        {
            transform.position = newTile.transform.position;
            currentGridPos = newPos;
            newTile.SetOccupant(gameObject);
        }
    }

    // 👉 C’est cette méthode que chaque pion va personnaliser :
    public virtual List<Vector2Int> GetAvailableMoves(BoardManager board)
    {
        return new List<Vector2Int>();
    }
}
