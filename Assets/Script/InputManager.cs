using UnityEngine;

using System.Collections.Generic;


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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPos = new Vector2Int(
                Mathf.RoundToInt(mouseWorldPos.x + (board.width - 1) / 2f),
                Mathf.RoundToInt(mouseWorldPos.y + (board.height - 1) / 2f)
            );

            Tile clickedTile = board.GetTileAt(gridPos);

            if (clickedTile != null)
            {
                // Si un pion est sélectionné et qu'on clique sur une case valide, on essaie de bouger
                if (selectedPiece != null)
                {
                    List<Vector2Int> validMoves = selectedPiece.GetAvailableMoves(board);
                    if (validMoves.Contains(gridPos))
                    {
                        selectedPiece.MoveTo(gridPos);
                        board.ClearHighlights();
                        selectedPiece = null;
                        return;
                    }
                }

                // Sinon, essaie de sélectionner un pion
                Piece piece = clickedTile.currentOccupant != null ? clickedTile.currentOccupant.GetComponent<Piece>() : null;

                if (piece != null)
                {
                    // S'il y avait déjà un pion sélectionné, on désélectionne
                    if (selectedPiece != null)
                    {
                        board.ClearHighlights();
                    }

                    selectedPiece = piece;
                    board.ShowPossibleMoves(piece);
                    Debug.Log("Pion sélectionné : " + piece.name);
                    return;
                }
            }

            // Si on clique ailleurs (aucune case ou pas de pion), on annule la sélection
            if (selectedPiece != null)
            {
                board.ClearHighlights();
                selectedPiece = null;
            }
        }
    }

}
