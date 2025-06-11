using System;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public static event Action<int> OnPointXChange;
    public static event Action<int> OnPointOChange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(OnAftertStart), 0.1f);
    }

    private void OnAftertStart()
    {
        SetPlayerPoint(GameManager.instance.playerX, GameManager.instance.pointBase);
        SetPlayerPoint(GameManager.instance.playerO, GameManager.instance.pointBase);
    }

    public void SetPlayerPoint(PlayerBase player, int value)
    {
        player.point = value;

        if (player.typeIs == GameManager.PlayerType.X) OnPointXChange?.Invoke(player.point);
        if (player.typeIs == GameManager.PlayerType.O) OnPointOChange?.Invoke(player.point);
    }

    public void AddPlayerPoint(PlayerBase player , int value)
    {
        player.point += value;

        if (player.typeIs == GameManager.PlayerType.X) OnPointXChange?.Invoke(player.point);
        if (player.typeIs == GameManager.PlayerType.O) OnPointOChange?.Invoke(player.point);
    }

    public void RemovePlayerPoint(PlayerBase player , int value)
    {
        player.point -= value;

        if (player.typeIs == GameManager.PlayerType.X) OnPointXChange?.Invoke(player.point);
        if (player.typeIs == GameManager.PlayerType.O) OnPointOChange?.Invoke(player.point);
    }
}
