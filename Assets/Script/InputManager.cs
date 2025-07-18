using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Piece selectedPiece;
    private BoardManager board;

    void Start()
    {
        board = FindFirstObjectByType<BoardManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clic gauche
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                // Clique sur un pion
                Piece piece = hit.collider.GetComponent<Piece>();
                if (piece != null)
                {
                    selectedPiece = piece;
                    Debug.Log("Pion sélectionné : " + piece.name);
                }
                else if (selectedPiece != null)
                {
                    // Clique sur une case
                    Tile tile = hit.collider.GetComponent<Tile>();
                    if (tile != null && !tile.isOccupied)
                    {
                        selectedPiece.MoveTo(tile.gridPos);
                        selectedPiece = null;
                    }
                }
            }
        }
    }
}
