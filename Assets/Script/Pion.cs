using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2Int currentGridPos;      // Position sur la grille
    protected BoardManager board;          // Référence au plateau
    



    public virtual bool IsRed()
    {
        return false;
    }

public bool IsEnemy(Piece other)
{
    return this.IsRed() != other.IsRed();
}


    protected virtual void Start()
    {
        board = FindFirstObjectByType<BoardManager>();
    }

    public void MoveTo(Vector2Int newPos)
{
    Tile newTile = board.GetTileAt(newPos);
    if (newTile != null)
    {
        // Vérifie si la case est occupée par un pion adverse
        if (newTile.isOccupied)
        {
            GameObject occupant = newTile.currentOccupant;
            Piece otherPiece = occupant?.GetComponent<Piece>();

            // Ne mange que si c'est un ennemi
            if (otherPiece != null && otherPiece.IsEnemy(this))
            {
                Destroy(occupant); // On détruit le pion adverse
            }
            else
            {
                // Si ce n’est pas un ennemi, on ne bouge pas
                Debug.Log("Case occupée par un allié ou vide.");
                return;
            }
        }

        // Libère l’ancienne case
        Tile oldTile = board.GetTileAt(currentGridPos);
        if (oldTile != null)
            oldTile.SetOccupant(null);

        // Déplace le pion
        transform.position = newTile.transform.position;
        newTile.SetOccupant(gameObject);
        currentGridPos = newPos;
    }
}


    // 👉 C’est cette méthode que chaque pion va personnaliser :
    public virtual List<Vector2Int> GetAvailableMoves(BoardManager board)
    {
        return new List<Vector2Int>();
    }
}
