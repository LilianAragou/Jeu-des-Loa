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
                    board.ShowPossibleMoves(piece); // ✅ ICI on affiche les déplacements
                    Debug.Log("Pion sélectionné : " + piece.name);
                }
                else if (selectedPiece != null)
                {
                    // Clique sur une case vide
                    Tile tile = hit.collider.GetComponent<Tile>();
                    if (tile != null && !tile.isOccupied)
{
                        var possibleMoves = selectedPiece.GetAvailableMoves(board);
                        if (possibleMoves.Contains(tile.gridPos))
                        {
                            selectedPiece.MoveTo(tile.gridPos);
                            board.ClearHighlights();
                            selectedPiece = null;
                        }
}
                }
            }
        }
    }
}
