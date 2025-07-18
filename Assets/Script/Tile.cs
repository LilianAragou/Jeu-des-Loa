using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition; // Position sur la grille
    public bool isOccupied = false;
    public bool isBlessed = false;

    public GameObject currentOccupant; // L’unité qui est sur cette case (null si vide)

    public void SetOccupant(GameObject unit)
    {
        currentOccupant = unit;
        isOccupied = (unit != null);
    }
}
