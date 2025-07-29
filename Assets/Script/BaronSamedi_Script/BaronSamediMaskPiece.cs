using UnityEngine;
using TMPro;

// Hérite de MaskPiece pour réutiliser les déplacements diagonaux+orthogonaux
public class BaronSamediMaskPiece : MaskPiece
{
    [Header("UI")]
    [Tooltip("TextMeshProUGUI pour afficher les Points d'Ombre")]
    public TextMeshProUGUI shadowPointsText;

    // Compteur de Points d'Ombre
    private int shadowPoints = 0;

    protected override void Start()
    {
        base.Start();
        // S'abonner aux événements de capture et destruction
        TurnManager.Instance.OnPieceCaptured  += OnSpiritCaptured;
        TurnManager.Instance.OnPieceDestroyed += OnSpiritDestroyed;

        UpdateShadowUI();
    }

    protected override void OnDestroy()
    {
        if (TurnManager.Instance != null)
        {
            TurnManager.Instance.OnPieceCaptured  -= OnSpiritCaptured;
            TurnManager.Instance.OnPieceDestroyed -= OnSpiritDestroyed;
        }
        base.OnDestroy();
    }

    // Quand un esprit est capturé (Action<Piece,Piece>)
    private void OnSpiritCaptured(Piece attacker, Piece victim)
    {
        GainShadowPoint();
    }

    // Quand un esprit est détruit hors capture (Action<Piece>)
    private void OnSpiritDestroyed(Piece victim)
    {
        GainShadowPoint();
    }
    public int GetShadowPoints() => shadowPoints;
    public void GainShadowPoint()
    {
        shadowPoints++;
        Debug.Log($"{name}: Ombres Persistantes → +1 PO (total = {shadowPoints})");
        UpdateShadowUI();
    }

    private void UpdateShadowUI()
    {
        if (shadowPointsText != null)
            shadowPointsText.text = $"PO : {shadowPoints}";
    }

    // Pas d'override de Start ou GetAvailableMoves (hérités de MaskPiece)
}
