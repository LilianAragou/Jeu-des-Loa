using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public Vector2Int currentGridPos;
    public bool isRed;
    protected BoardManager board;

    // Bonus temporaire de portée
    private int tempRangeBonus = 0;

    // ─── Hooks d’événements (à override) ───────────────────────────────
    /// <summary>Au début de CHAQUE tour (rouge ou bleu).</summary>
    protected virtual void OnTurnStart() { }

    /// <summary>À la fin de CHAQUE tour.</summary>
    protected virtual void OnTurnEnd()
    {
        // Réinitialisation du bonus de portée en fin de tour
        TempResetRange();
    }

    /// <summary>Appelé quand cette pièce capture quelqu’un.</summary>
    protected virtual void OnCapture(Piece victim) { }

    /// <summary>Appelé quand un allié (même équipe) meurt.</summary>
    protected virtual void OnAllyDeath(Piece ally) { }

    // Filtrage global des événements de capture/destruction
    private void HandleGlobalCapture(Piece attacker, Piece victim)
    {
        if (attacker == this)           OnCapture(victim);
        else if (victim.isRed == isRed) OnAllyDeath(victim);
    }

    private void HandleGlobalDestruction(Piece victim)
    {
        if (victim.isRed == isRed && victim != this)
            OnAllyDeath(victim);
    }

    // ─── Abonnement aux events ────────────────────────────────────────
    protected virtual void Start()
    {
        // TurnManager.Instance est initialisé dans son Awake()
        TurnManager.Instance.OnTurnStart      += OnTurnStart;
        TurnManager.Instance.OnTurnEnd        += OnTurnEnd;
        TurnManager.Instance.OnPieceCaptured  += HandleGlobalCapture;
        TurnManager.Instance.OnPieceDestroyed += HandleGlobalDestruction;
    }

    protected virtual void OnDestroy()
    {
        if (TurnManager.Instance == null) return;
        TurnManager.Instance.OnTurnStart      -= OnTurnStart;
        TurnManager.Instance.OnTurnEnd        -= OnTurnEnd;
        TurnManager.Instance.OnPieceCaptured  -= HandleGlobalCapture;
        TurnManager.Instance.OnPieceDestroyed -= HandleGlobalDestruction;
    }

    // ─── Initialisation sur le plateau ───────────────────────────────
    public void Initialize(Vector2Int startPos, bool isRedTeam, BoardManager boardManager)
    {
        board          = boardManager;
        currentGridPos = startPos;
        isRed          = isRedTeam;

        Tile t = board.GetTileAt(startPos);
        transform.position = t.transform.position;
        t.SetOccupant(gameObject);
    }

    // ─── Déplacements (à override) ───────────────────────────────────
    /// <param name="board">Le plateau pour calculer les déplacements.</param>
    /// <returns>Liste des cases atteignables.</returns>
    public abstract List<Vector2Int> GetAvailableMoves(BoardManager board);

    // ─── Gestion du bonus de portée ───────────────────────────────────
    public void TempIncreaseRange(int amount) => tempRangeBonus += amount;
    public void TempResetRange()           => tempRangeBonus = 0;
    protected int GetEffectiveRange(int baseRange) => baseRange + tempRangeBonus;

    /// <summary>Vérifie si l'autre pièce est une ennemie.</summary>
    public bool IsEnemy(Piece other) => other != null && other.isRed != isRed;
}
