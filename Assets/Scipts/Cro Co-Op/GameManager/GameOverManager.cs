using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public PlayerBase pleyerWiner;
    public List<Slot> winerSlot = new List<Slot>();
    public PlayerBase playerLosser;
    public List<Slot> losserSlot = new List<Slot>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.OnEndGameEmptyPoint += WinByEmptyPoint;
        GameManager.OnEndGame3Line += WinBy3Line;
        GameManager.OnEndGame3LineAndEmptyPoint += WinBy3LineAndEmptyPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WinByEmptyPoint(PlayerBase winer)
    {
        SetWiner(winer);

        Debug.Log($"================== GameOver : Player {GameManager.instance.playerBaseNext.typeIs} Win by {GameManager.instance.playerBaseTrun.typeIs} No point  =======================");
        GameManager.instance.ShowDagerSlot(winerSlot);

        foreach (Slot slot in losserSlot)
        {
            slot.DestroySlot();
        }
        foreach (Slot slot in winerSlot)
        {
            slot.RetrueToScore();
        }
    }
    public void WinBy3Line(PlayerBase winer)
    {
        SetWiner(winer);

        Debug.Log($"================== GameOver : Player {GameManager.instance.playerBaseTrun.typeIs} Win by 3 Slot Line =======================");
    }
    public void WinBy3LineAndEmptyPoint(PlayerBase winer)
    {
        SetWiner(winer);

        Debug.Log($"================== GameOver : Player {GameManager.instance.playerBaseTrun.typeIs} Win by {GameManager.instance.playerBaseNext.typeIs} No point and skip Trun =======================");
    }

    private void SetWiner(PlayerBase winer)
    {
        pleyerWiner = winer;
        if (GameManager.instance.playerX == winer) playerLosser = GameManager.instance.playerO;
        else playerLosser = GameManager.instance.playerX;

        //winerSlot
        for (int i = 0; i < GameManager.instance.row.Length; i++)
        {
            for (int j = 0; j < GameManager.instance.row[i].col.Length; j++)
            {
                if (GameManager.instance.row[i].col[j].owner == pleyerWiner) winerSlot.Add(GameManager.instance.row[i].col[j]);
            }
        }
        //lossSlot
        for (int i = 0; i < GameManager.instance.row.Length; i++)
        {
            for (int j = 0; j < GameManager.instance.row[i].col.Length; j++)
            {
                if (GameManager.instance.row[i].col[j].owner == playerLosser) losserSlot.Add(GameManager.instance.row[i].col[j]);
            }
        }
    }

}
