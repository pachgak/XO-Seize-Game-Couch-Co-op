using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public AudioClip warningSlotCantSetSound;
    public AudioClip boomSwichSound;
    public AudioSource gameOverSource;

    public PlayerBase pleyerWiner;
    public List<Slot> winerSlot = new List<Slot>();
    public PlayerBase playerLosser;
    public List<Slot> losserSlot = new List<Slot>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverSource = GetComponent<AudioSource>();

        GameManager.OnEndGameEmptyPoint += WinByEmptyPoint;
        GameManager.OnEndGame3Line += WinBy3Line;
        GameManager.OnEndGame3LineAndEmptyPoint += WinBy3LineAndEmptyPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetSlotList()
    {
        winerSlot.Clear();
        losserSlot.Clear();
    }

    public void WinBy3Line(PlayerBase winer)
    {
        SetWiner(winer);

        Debug.Log($"================== GameOver : Player {GameManager.instance.playerBaseTrun.typeIs} Win by 3 Slot Line =======================");
        StartCoroutine(AniWinBy3Line());
    }

    IEnumerator AniWinBy3Line()
    {
        yield return new WaitForSeconds(1f);
        //gameOverSource.PlayOneShot(warningSlotCantSetSound);
        //GameManager.instance.ShowDagerSlot(null);
        //yield return new WaitForSeconds(0.35f);
        //GameManager.instance.ShowDagerSlot(GameManager.instance.GetDangerSlots(GameManager.instance.slotTrigerWin));
        //yield return new WaitForSeconds(0.35f);

        //gameOverSource.PlayOneShot(warningSlotCantSetSound);
        //GameManager.instance.ShowDagerSlot(null);
        //yield return new WaitForSeconds(0.35f);
        //GameManager.instance.ShowDagerSlot(GameManager.instance.GetDangerSlots(GameManager.instance.slotTrigerWin));
        //yield return new WaitForSeconds(0.35f);

        //gameOverSource.PlayOneShot(warningSlotCantSetSound);
        //GameManager.instance.ShowDagerSlot(null);
        //yield return new WaitForSeconds(0.35f);
        //GameManager.instance.ShowDagerSlot(GameManager.instance.GetDangerSlots(GameManager.instance.slotTrigerWin));
        //yield return new WaitForSeconds(0.35f);

        gameOverSource.PlayOneShot(boomSwichSound);

        yield return new WaitForSeconds(0.5f);

        foreach (Slot slot in losserSlot)
        {
            slot.DestroySlot();
        }
        //yield return StartCoroutine(DestroyLossSlotLoop(losserSlot));

        yield return new WaitForSeconds(1f);

        int pointRe = 0;
        foreach (Slot slot in winerSlot)
        {
            pointRe += slot.point;
            slot.RetrueToScore();
        }
        Debug.Log($"pointRe : {pointRe}");
        ItemDropManager.instance.TrigerCountDownItemDrop(pointRe, () =>
        {
            ScoreManager.instance.AddScorePlayer(pleyerWiner);
            GameManager.instance.ChangPlayerFrist();
            ResetSlotList();
        });
        //PointManager.instance.ReSetPoinBase();
        GameManager.instance.isGameEnd = false;
    }

    public void WinByEmptyPoint(PlayerBase winer)
    {
        SetWiner(winer);

        Debug.Log($"================== GameOver : Player {GameManager.instance.playerBaseNext.typeIs} Win by {GameManager.instance.playerBaseTrun.typeIs} No point  =======================");

        StartCoroutine(AniWinByEmptyPoint());
    }

    IEnumerator AniWinByEmptyPoint()
    {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(ShowDagerSlotLoop(winerSlot));
        //yield return StartCoroutine(CountdownLoop(5));
        //GameManager.instance.ShowDagerSlot(winerSlot);

        yield return new WaitForSeconds(1f);

        //foreach (Slot slot in losserSlot)
        //{
        //    slot.DestroySlot();
        //}

        gameOverSource.PlayOneShot(boomSwichSound);

        yield return new WaitForSeconds(0.5f);

        foreach (Slot slot in losserSlot)
        {
            slot.DestroySlot();
        }

        //yield return StartCoroutine(DestroyLossSlotLoop(losserSlot));

        yield return new WaitForSeconds(1f);

        int pointRe = 0;
        foreach (Slot slot in winerSlot)
        {
            pointRe += slot.point;
            slot.RetrueToScore();
        }
        Debug.Log($"pointRe : {pointRe}");
        ItemDropManager.instance.TrigerCountDownItemDrop(pointRe, () =>
        {
            ScoreManager.instance.AddScorePlayer(pleyerWiner);
            GameManager.instance.ChangPlayerFrist();
            ResetSlotList();
        });
        //PointManager.instance.ReSetPoinBase();
        GameManager.instance.isGameEnd = false;
    }

    public void WinBy3LineAndEmptyPoint(PlayerBase winer)
    {
        SetWiner(winer);

        Debug.Log($"================== GameOver : Player {GameManager.instance.playerBaseNext.typeIs} Win by {GameManager.instance.playerBaseTrun.typeIs} No point and skip Trun =======================");

        StartCoroutine(AniWinBy3LineAndEmptyPointt());
    }

    IEnumerator AniWinBy3LineAndEmptyPointt()
    {
        yield return new WaitForSeconds(1f);

        //GameManager.instance.ShowDagerSlot(GameManager.instance.GetDangerSlots(GameManager.instance.slotTrigerWin));
        yield return StartCoroutine(ShowDagerSlotLoop(GameManager.instance.GetDangerSlots(GameManager.instance.slotTrigerWin)));

        yield return new WaitForSeconds(1f);

        //foreach (Slot slot in losserSlot)
        //{
        //    slot.DestroySlot();
        //}

        gameOverSource.PlayOneShot(boomSwichSound);

        yield return new WaitForSeconds(0.5f);
        
        foreach (Slot slot in losserSlot)
        {
            slot.DestroySlot();
        }
        //yield return StartCoroutine(DestroyLossSlotLoop(losserSlot));

        yield return new WaitForSeconds(1f);

        int pointRe = 0;
        foreach (Slot slot in winerSlot)
        {
            pointRe += slot.point;
            slot.RetrueToScore();
        }
        Debug.Log($"pointRe : {pointRe}");
        ItemDropManager.instance.TrigerCountDownItemDrop(pointRe, () =>
        {
            ScoreManager.instance.AddScorePlayer(pleyerWiner);
            GameManager.instance.ChangPlayerFrist();
            ResetSlotList();
        });
        //PointManager.instance.ReSetPoinBase();
        GameManager.instance.isGameEnd = false;
    }
    IEnumerator ShowDagerSlotLoop(List<Slot> CantSetSlots)
    {
        List<Slot> currutShowCantSetSlots = new List<Slot>();

        for (int i = 0; i < CantSetSlots.Count; i++)
        {
            currutShowCantSetSlots.Add(CantSetSlots[i]);
            GameManager.instance.ShowDagerSlot(currutShowCantSetSlots);
            gameOverSource.PlayOneShot(warningSlotCantSetSound);

            yield return new WaitForSeconds(0.75f); // รอ 1 วินาที ก่อนแสดงตัวเลขถัดไป
        }
    }

    IEnumerator DestroyLossSlotLoop(List<Slot> lossSlots)
    {
        List<Slot> listLossSlots = lossSlots;

        for (int i = 0; i < lossSlots.Count; i++)
        {
            listLossSlots[i].DestroySlot();

            yield return new WaitForSeconds(0.1f); // รอ 1 วินาที ก่อนแสดงตัวเลขถัดไป
        }
    }

    IEnumerator CountdownLoop(int count)
    {
        for (int i = count; i >= 1; i--)
        {
            Debug.Log("Countdown: " + i);
            yield return new WaitForSeconds(1f); // รอ 1 วินาที ก่อนแสดงตัวเลขถัดไป
        }
        Debug.Log("Countdown complete!");
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
