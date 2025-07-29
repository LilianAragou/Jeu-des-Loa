using System;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [Tooltip("0 = Rouge, 1 = Bleu")]
    public int CurrentPlayer { get; private set; } = 0;

    [Header("UI")]
    [Tooltip("Un TextMeshProUGUI placé dans ta Canvas pour afficher le tour courant")]
    public TextMeshProUGUI turnText;

    // Événements pour les pièces
    public event Action OnTurnStart;
    public event Action OnTurnEnd;
    public event Action<Piece, Piece> OnPieceCaptured; // (attaquant, victime)
    public event Action<Piece> OnPieceDestroyed;       // destruction hors capture

    void Awake()
    {
        // Pattern singleton
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        // Mise à jour de l'affichage UI
        UpdateTurnDisplay();

        // Log du tout premier début de tour
        Debug.Log($"[TurnManager] Début du tout premier tour {(CurrentPlayer==0?"Rouge":"Bleu")}");

        // Déclenche le premier OnTurnStart
        OnTurnStart?.Invoke();
    }

    /// <summary>Appelé en fin de tour pour basculer de joueur</summary>
    public void EndTurn()
    {
        // Log avant de déclencher OnTurnEnd
        Debug.Log($"[TurnManager] Fin du tour {(CurrentPlayer==0?"Rouge":"Bleu")}");
        OnTurnEnd?.Invoke();

        // Changement de joueur
        CurrentPlayer = 1 - CurrentPlayer;
        UpdateTurnDisplay();

        // Log et déclenchement du nouveau tour
        Debug.Log($"[TurnManager] Début du tour {(CurrentPlayer==0?"Rouge":"Bleu")}");
        OnTurnStart?.Invoke();
    }

    /// <summary>Notifie une capture pour déclencher OnPieceCaptured(attacker,victim)</summary>
    public void NotifyCapture(Piece attacker, Piece victim)
        => OnPieceCaptured?.Invoke(attacker, victim);

    /// <summary>Notifie une destruction hors capture pour déclencher OnPieceDestroyed(victim)</summary>
    public void NotifyDestruction(Piece victim)
        => OnPieceDestroyed?.Invoke(victim);

    /// <summary>Met à jour le texte UI avec le joueur courant</summary>
    private void UpdateTurnDisplay()
    {
        if (turnText != null)
            turnText.text = $"Tour : {(CurrentPlayer == 0 ? "Rouge" : "Bleu")}";
        else
            Debug.LogWarning("[TurnManager] turnText n'est pas assigné !");
    }
}
