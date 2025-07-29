using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera cam;
    private Piece selectedPiece;
    private BoardManager board;

    void Start()
    {
        cam   = Camera.main;
        // Si tu es sur < Unity 2023.1, utilise FindObjectOfType
        board = FindFirstObjectByType<BoardManager>();
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        // On capte TOUS les colliders sous la souris
        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);
        if (hits.Length == 0)
        {
            Deselect();
            return;
        }

        // Priorité aux Piece, puis Tile
        Piece clickedPiece = null;
        Tile  clickedTile  = null;
        foreach (var hit in hits)
        {
            clickedPiece = hit.GetComponentInParent<Piece>();
            if (clickedPiece != null) break;
            clickedTile = hit.GetComponentInParent<Tile>();
        }

        if (clickedPiece != null)      HandlePieceClick(clickedPiece);
        else if (clickedTile != null)  HandleTileClick(clickedTile);
        else                            Deselect();
    }

    void HandlePieceClick(Piece piece)
    {
        // Désélection si on reclique sur le même
        if (selectedPiece == piece)
        {
            Deselect();
            return;
        }

        // Si on avait un pion sélectionné, et qu'on clique sur un pion
        if (selectedPiece != null)
        {
            // Si c'est une destination valide (capture)
            if (selectedPiece.GetAvailableMoves(board).Contains(piece.currentGridPos))
            {
                board.MovePiece(selectedPiece, piece.currentGridPos);
                selectedPiece = null;
                board.ClearHighlights();
            }
            else
            {
                // Sinon on change la sélection
                SelectPiece(piece);
            }
        }
        else // première sélection
        {
            SelectPiece(piece);
        }
    }

    void HandleTileClick(Tile tile)
    {
        if (selectedPiece == null) return;

        // Si c'est une destination valide (mouvement simple)
        if (selectedPiece.GetAvailableMoves(board).Contains(tile.gridPos))
        {
            board.MovePiece(selectedPiece, tile.gridPos);
            selectedPiece = null;
            board.ClearHighlights();
        }
        else
        {
            Deselect();
        }
    }

    void SelectPiece(Piece piece)
    {
        selectedPiece = piece;
        board.ShowPossibleMoves(piece);
    }

    void Deselect()
    {
        selectedPiece = null;
        board.ClearHighlights();
    }
}
