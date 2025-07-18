using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    public int width = 9;
    public int height = 9;
    public float tileSize = 1f;

    public GameObject tilePrefab;
    public GameObject spiritRedPrefab;
    public GameObject spiritBluePrefab;
    public GameObject maskRedPrefab;
    public GameObject maskBluePrefab;
    public GameObject coordTextPrefab;

    private Tile[,] tiles;

    void Start()
    {
        GenerateBoard();
    }

    public void GenerateBoard()
    {
        // Centrage du plateau autour de (0,0)
        float offsetX = -((width - 1) * tileSize) / 2f;
        float offsetY = -((height - 1) * tileSize) / 2f;

        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 worldPos = new Vector2(x * tileSize + offsetX, y * tileSize + offsetY);
                GameObject tileObj = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);
                Tile tile = tileObj.GetComponent<Tile>();
                tile.gridPos = new Vector2Int(x, y);
                tiles[x, y] = tile;

                // Coordonnées visibles (facultatif)
                if (coordTextPrefab != null)
                {
                    GameObject text = Instantiate(coordTextPrefab, worldPos, Quaternion.identity, tileObj.transform);
                    TextMeshPro tmp = text.GetComponent<TextMeshPro>();
                    if (tmp != null)
                        tmp.text = $"{(char)('A' + x)}{y + 1}";
                }
            }
        }

        // Placement des pions ROUGES (haut)
        PlacePiece(spiritRedPrefab, 2, 0); // C1
        PlacePiece(spiritRedPrefab, 3, 0); // D1
        PlacePiece(maskRedPrefab, 4, 0); // E1
        PlacePiece(spiritRedPrefab, 5, 0); // F1
        PlacePiece(spiritRedPrefab, 6, 0); // G1
        PlacePiece(spiritRedPrefab, 3, 1); // D2
        PlacePiece(spiritRedPrefab, 5, 1); // F2

        // Placement des pions BLEUS (bas)
        PlacePiece(spiritBluePrefab, 2, 8); // C9
        PlacePiece(spiritBluePrefab, 3, 8); // D9
        PlacePiece(maskBluePrefab, 4, 8); // E9
        PlacePiece(spiritBluePrefab, 5, 8); // F9
        PlacePiece(spiritBluePrefab, 6, 8); // G9
        PlacePiece(spiritBluePrefab, 3, 7); // D8
        PlacePiece(spiritBluePrefab, 5, 7); // F8

    }

    private void PlacePiece(GameObject prefab, int x, int y)
    {
        if (tiles[x, y] == null) return;

        // Instancie le pion sans encore le marquer comme occupant
        Vector2 pos = tiles[x, y].transform.position;
        GameObject piece = Instantiate(prefab, pos, Quaternion.identity, transform);

        Piece pieceComp = piece.GetComponent<Piece>();
        if (pieceComp != null)
        {
            pieceComp.currentGridPos = new Vector2Int(x, y);

        }

    }


    public Tile GetTileAt(Vector2Int gridPos)
    {
        if (gridPos.x < 0 || gridPos.x >= width || gridPos.y < 0 || gridPos.y >= height)
            return null;

        return tiles[gridPos.x, gridPos.y];
    }
    

    
}
