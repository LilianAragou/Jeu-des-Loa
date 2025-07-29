using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    // ─── Singleton ─────────────────────────────────────────────────────
    public static BoardManager Instance { get; private set; }

    // ─── Paramètres du plateau ─────────────────────────────────────────
    public int width = 9;
    public int height = 9;
    public float tileSize = 1f;

    // ─── Prefabs à assigner dans l’Inspecteur ─────────────────────────
    public GameObject tilePrefab;
    public GameObject spiritRedPrefab;
    public GameObject spiritBluePrefab;
    public GameObject maskRedPrefab;
    public GameObject maskBluePrefab;
    public GameObject coordTextPrefab;

    // Evolve Baron Samedi
    public GameObject revenantPrefab;
    public GameObject Flm_ViolettePrefab;

    public GameObject Devoreur_AmePrefab;

    public GameObject Hurleur_CreuxPrefab;
    public GameObject baronSamediMaskPrefab;

    public GameObject Egaree_ProfondeurPrefab;

    // ─── Données internes ──────────────────────────────────────────────
    private Tile[,] tiles;

    // ─── Awake pour le singleton ──────────────────────────────────────
    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    // ─── Start ─────────────────────────────────────────────────────────
    void Start()
    {
        GenerateBoard();
    }

    // ─── Génération du plateau et placement initial des pions ─────────
    public void GenerateBoard()
    {
        float offsetX = -((width - 1) * tileSize) / 2f;
        float offsetY = -((height - 1) * tileSize) / 2f;

        tiles = new Tile[width, height];

        // Création des tuiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 worldPos = new Vector2(x * tileSize + offsetX, y * tileSize + offsetY);
                GameObject tileObj = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);
                Tile tile = tileObj.GetComponent<Tile>();
                tile.gridPos = new Vector2Int(x, y);
                tiles[x, y] = tile;

                if (coordTextPrefab != null)
                {
                    GameObject text = Instantiate(coordTextPrefab, worldPos, Quaternion.identity, tileObj.transform);
                    TextMeshPro tmp = text.GetComponent<TextMeshPro>();
                    if (tmp != null)
                        tmp.text = $"{(char)('A' + x)}{y + 1}";
                }
            }
        }

        // Placement des pions ROUGES (exemple avec Revenant et Flamme Violette)
        PlacePiece(spiritRedPrefab,    2, 0, true);
        PlacePiece(Egaree_ProfondeurPrefab,    3, 0, true);
        PlacePiece(baronSamediMaskPrefab,      4, 0, true);
        PlacePiece(spiritRedPrefab,    5, 0, true);
        PlacePiece(spiritRedPrefab,    6, 0, true);
        PlacePiece(spiritRedPrefab,    3, 1, true);
        PlacePiece(spiritRedPrefab,    5, 1, true);

        // Placement des pions BLEUS
        PlacePiece(spiritBluePrefab,   2, 8, false);
        PlacePiece(revenantPrefab,   3, 8, false);
        PlacePiece(maskBluePrefab,     4, 8, false);
        PlacePiece(spiritBluePrefab,   5, 8, false);
        PlacePiece(spiritBluePrefab,   6, 8, false);
        PlacePiece(spiritBluePrefab,   3, 7, false);
        PlacePiece(spiritBluePrefab,   5, 7, false);
    }

    // ─── Instanciation et initialisation d’une pièce ──────────────────
    private void PlacePiece(GameObject prefab, int x, int y, bool isRed)
    {
        if (tiles[x, y] == null)
        {
            Debug.LogError($"Aucune tuile à [{x},{y}]");
            return;
        }

        // Si déjà occupée, on détruit l’ancien occupant
        if (tiles[x, y].currentOccupant != null)
        {
            Debug.LogWarning($"La tuile [{x},{y}] était déjà occupée par {tiles[x, y].currentOccupant.name}. Suppression.");
            Destroy(tiles[x, y].currentOccupant);
            tiles[x, y].SetOccupant(null);
        }

        // Instanciation
        Vector2 pos = tiles[x, y].transform.position;
        GameObject pieceObj = Instantiate(prefab, pos, Quaternion.identity, transform);
        Piece pieceComp = pieceObj.GetComponent<Piece>();
        if (pieceComp != null)
        {
            pieceComp.Initialize(new Vector2Int(x, y), isRed, this);
            Debug.Log($"Placement de {pieceObj.name} ({(isRed ? "Rouge" : "Bleu")}) en [{x},{y}]");
        }
        else
        {
            Debug.LogError($"Prefab {prefab.name} n’a pas de composant Piece.");
        }
    }

    // ─── Accès à une tuile selon ses coordonnées ───────────────────────
    public Tile GetTileAt(Vector2Int gridPos)
    {
        if (gridPos.x < 0 || gridPos.x >= width ||
            gridPos.y < 0 || gridPos.y >= height)
            return null;
        return tiles[gridPos.x, gridPos.y];
    }

    // ─── Affichage des déplacements possibles ──────────────────────────
    public void ShowPossibleMoves(Piece piece)
    {
        ClearHighlights();
        List<Vector2Int> validMoves = piece.GetAvailableMoves(this);
        foreach (var move in validMoves)
        {
            Tile targetTile = GetTileAt(move);
            if (targetTile != null)
                targetTile.Highlight(Color.yellow);
        }
    }

    public void ClearHighlights()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                tiles[x, y].ResetHighlight();
    }

    // ─── Mouvement, capture et fin de tour ────────────────────────────
    public void MovePiece(Piece piece, Vector2Int targetPos)
    {
        Tile fromTile = GetTileAt(piece.currentGridPos);
        Tile toTile   = GetTileAt(targetPos);
        if (fromTile == null || toTile == null) return;

        // Capture éventuelle
        if (toTile.currentOccupant != null)
        {
            Piece victim = toTile.currentOccupant.GetComponent<Piece>();
            if (victim != null && piece.IsEnemy(victim))
            {
                // Notifier la capture
                TurnManager.Instance.NotifyCapture(piece, victim);
                // Retirer la référence avant destruction
                toTile.SetOccupant(null);
                // Détruire la pièce capturée
                Destroy(victim.gameObject);
            }
        }

        // Déplacement de la pièce
        fromTile.SetOccupant(null);
        toTile.SetOccupant(piece.gameObject);
        piece.currentGridPos = targetPos;
        piece.transform.position = toTile.transform.position;

        // Fin de tour
        TurnManager.Instance.EndTurn();
    }
}
