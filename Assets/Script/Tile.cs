using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPos;
    public bool isOccupied = false;
    public GameObject currentOccupant;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor;

    void Awake()
    {
        defaultColor = new Color(1, 1, 1, 0f); // blanc totalement transparent par défaut
        spriteRenderer.color = defaultColor;
        
    }

    public void SetOccupant(GameObject occupant)
    {
        currentOccupant = occupant;
        isOccupied = occupant != null;
    }
    

    

    public void Highlight(Color color)
{
    if (spriteRenderer != null)
    {
        color.a = 0.5f; // opacité partielle pour la surbrillance
        spriteRenderer.color = color;
    }
}


    public void ResetHighlight()
{
    if (spriteRenderer != null)
    {
        var clear = defaultColor;
        clear.a = 0f; // totalement invisible
        spriteRenderer.color = clear;
    }
}



    
}
