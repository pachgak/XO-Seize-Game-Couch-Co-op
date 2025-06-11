using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public static event Action<int> OnScoreXChange;
    public static event Action<int> OnScoreOChange;

    public GameObject effectWinChangePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(OnAftertStart),0.1f);
    }

    private void OnAftertStart()
    {
        ResetScore();
    }

    public void AddScorePlayer(PlayerBase player)
    {
        player.winCount++;
        if (player.typeIs == GameManager.PlayerType.X) OnScoreXChange?.Invoke(GameManager.instance.playerX.winCount);
        if (player.typeIs == GameManager.PlayerType.O) OnScoreOChange?.Invoke(GameManager.instance.playerO.winCount);
    }

    public void ResetScore()
    {
        GameManager.instance.playerX.winCount = 0;
        OnScoreXChange?.Invoke(GameManager.instance.playerX.winCount);
        GameManager.instance.playerO.winCount = 0;
        OnScoreOChange?.Invoke(GameManager.instance.playerO.winCount);
    }
}
