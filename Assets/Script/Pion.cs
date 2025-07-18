using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2Int currentGridPos; // Position logique sur la grille
    private BoardManager board;

    void Start()
    {
        board = FindFirstObjectByType<BoardManager>();
        MoveTo(currentGridPos); // Place le pion à la bonne position dès le début
    }

    public void MoveTo(Vector2Int newPos)
    {
        Tile newTile = board.GetTileAt(newPos);
        if (newTile != null && !newTile.isOccupied)
        {
            // Libérer l’ancienne case
            Tile oldTile = board.GetTileAt(currentGridPos);
            if (oldTile != null)
                oldTile.SetOccupant(null);

            // Déplacer le pion
            transform.position = newTile.transform.position;
            newTile.SetOccupant(gameObject);
            currentGridPos = newPos;
        }
    }
}
