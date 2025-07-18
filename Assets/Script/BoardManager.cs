using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int width = 7;
    public int height = 7;
    public GameObject tilePrefab; // Référence vers le prefab des cases
    public float tileSize = 1f;

    private Tile[,] tiles; // Stocke toutes les cases du plateau

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * tileSize, y * tileSize, 0);
                GameObject tileObj = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                tileObj.name = $"Tile_{x}_{y}";

                Tile tile = tileObj.GetComponent<Tile>();
                tile.gridPosition = new Vector2Int(x, y);

                tiles[x, y] = tile;
            }
        }
    }

    public Tile GetTileAt(Vector2Int gridPos)
    {
        if (gridPos.x >= 0 && gridPos.x < width && gridPos.y >= 0 && gridPos.y < height)
            return tiles[gridPos.x, gridPos.y];
        return null;
    }
}
