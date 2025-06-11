using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public static event Action OnResetGame;
    public static event Action OnEndGame;
    public static event Action OnTrueChange;
    public static event Action<Slot> OnSetSlot;
    public static event Action<List<Slot>> OnDagerSlot;
    public static event Action<List<Slot>> OnResetDagerSlot;
    public static event Action<Slot> OnWonTrigerSlot;

    //gameData
    public int pointBase = 10;
    public int wonSlotCount = 3;
    public int wonTrunCount = 1;
    public PlayerBase playerX;
    public PlayerBase playerO;

    [Header("XO Slot")]
    public Row[] row;
    //public GameObject[] iconPrefab = new GameObject[9];

    //public List<Slot> allSlot;
    [Header("System")]
    //trun
    public PlayerType playerTrun;
    public PlayerBase firstPlayer;
    public GameStep gameStep;
    public PlayerBase playerBaseTrun;
    public PlayerBase playerBaseNext;
    public bool isReadyClick;
    //click
    //public bool haveClick = false;
    //public Slot clickSlot;

    //win
    public Slot slotTrigerWin;

    public GameObject dangerTopUI;
    public GameObject dangerDownUI;

    public List<Slot> dagerSlots;

    [System.Serializable]
    public class Row
    {
        public Slot[] col;
    }

    public enum PlayerType
    {
        X, O
    }

    public enum GameStep
    {
        ReSetGame, PlayTrun, CheckSlotWon, ChangTrun, SlotWonWarning, EndGame
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerX.typeIs = PlayerType.X;
        playerO.typeIs = PlayerType.O;

        firstPlayer = playerX;

        playerTrun = firstPlayer.typeIs;
        SetTurn(playerTrun);

        for (int i = 0; i < row.Length; i++)
        {
            for (int j = 0; j < row[i].col.Length; j++)
            {
                row[i].col[j].rowSlot = i;
                row[i].col[j].colSlot = j;
            }
        }

        isReadyClick = true;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void SetGameStep(GameStep step)
    {
        switch (step)
        {
            case GameStep.ReSetGame:
                OnResetGame?.Invoke();
                break;
            case GameStep.PlayTrun:
                break;
            case GameStep.CheckSlotWon:
                break;
            case GameStep.ChangTrun:
                OnTrueChange?.Invoke();
                break;
            case GameStep.SlotWonWarning:
                break;
            case GameStep.EndGame:
                break;
        }
    }

    public void ClickSlot(Slot slotClick)
    {
        if (!isReadyClick) return;

        PlayerBase trunPlayer = playerBaseTrun;
        //PlayerBase trunPlayer = null;
        //switch (playerTrun)
        //{
        //    case PlayerType.X:
        //        trunPlayer = playerX;
        //        break;
        //    case PlayerType.O:
        //        trunPlayer = playerO;
        //        break;
        //}

        if (slotClick.owner == null)
        {
            XOSetEmptySlot(slotClick);
        }
        else if (slotClick.owner != trunPlayer)
        {
            if (slotClick.isProtect)
            {
                XOProtection(slotClick);
            }
            else if (trunPlayer.point > slotClick.point)
            {
                XOGetEmemySlot(slotClick);
            }
            else
            {
                XONotEnoughPoint(slotClick);
            }
        }
        else
        {
            Debug.Log($"this is your own yep");
        }
    }

    private void XOSetEmptySlot(Slot slotClick)
    {
        OnSetSlot?.Invoke(slotClick);
        isReadyClick = false;
    }

    private void XOGetEmemySlot(Slot slotClick)
    {
        OnSetSlot?.Invoke(slotClick);
        isReadyClick = false;
    }

    private void XONotEnoughPoint(Slot slotClick)
    {
        Debug.Log($"Your point not enough for Get This Slot");
    }

    private void XOProtection(Slot slotClick)
    {
        Debug.Log($"This Slot have protection");
    }
    /*
    private void CheckWin(Slot slotClick)
    {
        //Check Rows
        List<Slot> checkRow = new List<Slot>();
        for (int i = 0; i < slotClick.colSlot; i++)
        {
            if (row[slotClick.rowSlot].col[i].owner = slotClick.owner)
            {
                checkRow.Add(row[slotClick.rowSlot].col[i]);
            }
            else if(checkRow.Count < 3)
            {
                checkRow.Clear();
            }
        }
        if (checkRow.Count >= 3) Debug.Log("Won Row");


        // Check Columns
        List<Slot> checkCol = new List<Slot>();
        for (int i = 0; i < slotClick.rowSlot; i++)
        {
            if (row[i].col[slotClick.colSlot].owner = slotClick.owner)
            {
                checkRow.Add(row[i].col[slotClick.colSlot]);
            }
            else if (checkRow.Count < 3)
            {
                checkRow.Clear();
            }
        }
        if (checkRow.Count >= 3) Debug.Log("Won Col");

        // diagonal (top-left to bottom-right)
        List<Slot> checkDLeftPo = new List<Slot>();
        for (int i = 0; slotClick.colSlot - i > 0 && slotClick.rowSlot - i > 0; i++)
        {
            if (row[slotClick.rowSlot - i].col[slotClick.colSlot - i].owner == slotClick.owner)
            {
                checkDLeftPo.Add(row[slotClick.rowSlot + i].col[slotClick.colSlot + i]);
            }
            else
            {
                break;
            }
        }
        List<Slot> checkDLeftNe = new List<Slot>();
        for (int i = 0; slotClick.colSlot - i > 0 && slotClick.rowSlot + i > 0; i++)
        {
            if (row[slotClick.rowSlot - i].col[slotClick.colSlot - i].owner == slotClick.owner)
            {
                checkDLeftPo.Add(row[slotClick.rowSlot + i].col[slotClick.colSlot + i]);
            }
            else
            {
                break;
            }
        }
        List<Slot> checkDLeft = new List<Slot>();
        // Anti-diagonal (top-right to bottom-left)

    }
    */

    public void CheckWin(Slot slotCheck)
    {
        if (slotTrigerWin == null)
        {
            CheckSlotTrigerWon(slotCheck);
        }
        else
        {
            if (CheckSlotTrigerWon(slotTrigerWin))
            {
                //ChangeTurn();
            }
            else
            {
                slotTrigerWin = null;
                OnWonTrigerSlot?.Invoke(slotTrigerWin);

                CheckSlotTrigerWon(slotCheck);
            }

        }
    }

    public bool CheckSlotTrigerWon(Slot slotCheck)
    {

        List<Slot> dangerSlot = new List<Slot>();
        bool isRowWon = false;
        bool isColWon = false;
        bool isDLeftWon = false;
        bool isDRightWon = false;

        //Check Rows
        //Check RowsPositive
        List<Slot> checkRowP = new List<Slot>();
        for (int i = 1; slotCheck.colSlot + i < row[0].col.Length; i++)
        {
            if (row[slotCheck.rowSlot].col[slotCheck.colSlot + i].owner == slotCheck.owner)
            {
                checkRowP.Add(row[slotCheck.rowSlot].col[slotCheck.colSlot + i]);
            }
            else break;
        }
        //Check RowsNegative
        List<Slot> checkRowN = new List<Slot>();
        for (int i = 1; slotCheck.colSlot - i >= 0; i++)
        {
            if (row[slotCheck.rowSlot].col[slotCheck.colSlot - i].owner == slotCheck.owner)
            {
                checkRowN.Add(row[slotCheck.rowSlot].col[slotCheck.colSlot - i]);
            }
            else break;
        }
        //Check RowsWon
        if (checkRowP.Count + checkRowN.Count + 1 >= wonSlotCount)
        {
            isRowWon = true;
            Debug.Log($"{slotCheck.owner.typeIs} isRowWon");

            foreach (Slot slot in checkRowP)
            {
                dangerSlot.Add(slot);
            }
            foreach (Slot slot in checkRowN)
            {
                dangerSlot.Add(slot);
            }
        }

        //Check Col
        //Check ColPositive
        List<Slot> checkColP = new List<Slot>();
        for (int i = 1; slotCheck.rowSlot + i < row.Length; i++)
        {
            if (row[slotCheck.rowSlot + i].col[slotCheck.colSlot].owner == slotCheck.owner)
            {
                checkColP.Add(row[slotCheck.rowSlot + i].col[slotCheck.colSlot]);
            }
            else break;
        }
        //Check RowsNegative
        List<Slot> checkColN = new List<Slot>();
        for (int i = 1; slotCheck.rowSlot - i >= 0; i++)
        {
            if (row[slotCheck.rowSlot - i].col[slotCheck.colSlot].owner == slotCheck.owner)
            {
                checkColN.Add(row[slotCheck.rowSlot - i].col[slotCheck.colSlot]);
            }
            else break;
        }
        //Check RowsWon
        if (checkColP.Count + checkColN.Count + 1 >= wonSlotCount)
        {
            isColWon = true;
            Debug.Log($"{slotCheck.owner.typeIs} isColWon");

            foreach (Slot slot in checkColP)
            {
                dangerSlot.Add(slot);
            }
            foreach (Slot slot in checkColN)
            {
                dangerSlot.Add(slot);
            }
        }

        //Check DLeft
        //Check DLeftPositive
        List<Slot> checkLeftP = new List<Slot>();
        for (int i = 1; slotCheck.rowSlot - i >= 0 && slotCheck.colSlot - i >= 0; i++)
        {
            if (row[slotCheck.rowSlot - i].col[slotCheck.colSlot - i].owner == slotCheck.owner)
            {

                checkLeftP.Add(row[slotCheck.rowSlot - i].col[slotCheck.colSlot - i]);
            }
            else
            {
                break;
            }
        }
        //Check DLeftNegative
        List<Slot> checkLeftN = new List<Slot>();
        for (int i = 1; slotCheck.rowSlot + i < row.Length && slotCheck.colSlot + i < row[0].col.Length; i++)
        {
            if (row[slotCheck.rowSlot + i].col[slotCheck.colSlot + i].owner == slotCheck.owner)
            {
                checkLeftN.Add(row[slotCheck.rowSlot + i].col[slotCheck.colSlot + i]);
            }
            else
            {
                break;
            }
        }
        //Check DLeft
        if (checkLeftP.Count + checkLeftN.Count + 1 >= wonSlotCount)
        {
            isDLeftWon = true;
            Debug.Log($"{slotCheck.owner.typeIs} isDLeftWon");

            foreach (Slot slot in checkLeftP)
            {
                dangerSlot.Add(slot);
            }
            foreach (Slot slot in checkLeftN)
            {
                dangerSlot.Add(slot);
            }
        }

        //Check DRight
        //Check DRightPositive
        List<Slot> checkRightP = new List<Slot>();
        for (int i = 1; slotCheck.rowSlot - i >= 0 && slotCheck.colSlot + i < row[0].col.Length; i++)
        {
            if (row[slotCheck.rowSlot - i].col[slotCheck.colSlot + i].owner == slotCheck.owner)
            {
                checkRightP.Add(row[slotCheck.rowSlot - i].col[slotCheck.colSlot + i]);
            }
            else break;
        }
        //Check DRightNegative
        List<Slot> checkRightN = new List<Slot>();
        for (int i = 1; slotCheck.rowSlot + i < row.Length && slotCheck.colSlot - i >= 0; i++)
        {
            if (row[slotCheck.rowSlot + i].col[slotCheck.colSlot - i].owner == slotCheck.owner)
            {
                checkRightN.Add(row[slotCheck.rowSlot + i].col[slotCheck.colSlot - i]);
            }
            else break;
        }
        //Check DRight
        if (checkRightP.Count + checkRightN.Count + 1 >= wonSlotCount)
        {
            isDRightWon = true;
            Debug.Log($"{slotCheck.owner.typeIs} isDLeftWon");

            foreach (Slot slot in checkRightP)
            {
                dangerSlot.Add(slot);
            }
            foreach (Slot slot in checkRightN)
            {
                dangerSlot.Add(slot);
            }
        }

        if (dangerSlot.Count > 0)
        {
            slotTrigerWin = slotCheck;
            OnWonTrigerSlot?.Invoke(slotTrigerWin);

            dangerSlot.Add(slotCheck);
            OnDagerSlot?.Invoke(dangerSlot);
            return true;
        }
        else
        {
            OnDagerSlot?.Invoke(null);
            return false;
        }
    }

    public bool CheckSlotTrigerWon2(Slot slotCheck)
    {
        if (GetDangerSlots(slotCheck).Count > 0)
        {
            return true;
        }
        else return false;
    }

    public List<Slot> GetDangerSlots(Slot slotCheck)
    {

        List<Slot> dangerSlot = new List<Slot>();
        bool isRowWon = false;
        bool isColWon = false;
        bool isDLeftWon = false;
        bool isDRightWon = false;

        //Check Rows
        //Check RowsPositive
        List<Slot> checkRowP = new List<Slot>();
        for (int i = 1; slotCheck.colSlot + i < row[0].col.Length; i++)
        {
            if (row[slotCheck.rowSlot].col[slotCheck.colSlot + i].owner == slotCheck.owner)
            {
                checkRowP.Add(row[slotCheck.rowSlot].col[slotCheck.colSlot + i]);
            }
            else break;
        }
        //Check RowsNegative
        List<Slot> checkRowN = new List<Slot>();
        for (int i = 1; slotCheck.colSlot - i >= 0; i++)
        {
            if (row[slotCheck.rowSlot].col[slotCheck.colSlot - i].owner == slotCheck.owner)
            {
                checkRowN.Add(row[slotCheck.rowSlot].col[slotCheck.colSlot - i]);
            }
            else break;
        }
        //Check RowsWon
        if (checkRowP.Count + checkRowN.Count + 1 >= wonSlotCount)
        {
            isRowWon = true;
            Debug.Log($"{slotCheck.owner.typeIs} isRowWon");

            foreach (Slot slot in checkRowP)
            {
                dangerSlot.Add(slot);
            }
            foreach (Slot slot in checkRowN)
            {
                dangerSlot.Add(slot);
            }
        }

        //Check Col
        //Check ColPositive
        List<Slot> checkColP = new List<Slot>();
        for (int i = 1; slotCheck.rowSlot + i < row.Length; i++)
        {
            if (row[slotCheck.rowSlot + i].col[slotCheck.colSlot].owner == slotCheck.owner)
            {
                checkColP.Add(row[slotCheck.rowSlot + i].col[slotCheck.colSlot]);
            }
            else break;
        }
        //Check RowsNegative
        List<Slot> checkColN = new List<Slot>();
        for (int i = 1; slotCheck.rowSlot - i >= 0; i++)
        {
            if (row[slotCheck.rowSlot - i].col[slotCheck.colSlot].owner == slotCheck.owner)
            {
                checkColN.Add(row[slotCheck.rowSlot - i].col[slotCheck.colSlot]);
            }
            else break;
        }
        //Check RowsWon
        if (checkColP.Count + checkColN.Count + 1 >= wonSlotCount)
        {
            isColWon = true;
            Debug.Log($"{slotCheck.owner.typeIs} isColWon");

            foreach (Slot slot in checkColP)
            {
                dangerSlot.Add(slot);
            }
            foreach (Slot slot in checkColN)
            {
                dangerSlot.Add(slot);
            }
        }

        //Check DLeft
        //Check DLeftPositive
        List<Slot> checkLeftP = new List<Slot>();
        for (int i = 1; slotCheck.rowSlot - i >= 0 && slotCheck.colSlot - i >= 0; i++)
        {
            if (row[slotCheck.rowSlot - i].col[slotCheck.colSlot - i].owner == slotCheck.owner)
            {

                checkLeftP.Add(row[slotCheck.rowSlot - i].col[slotCheck.colSlot - i]);
            }
            else
            {
                break;
            }
        }
        //Check DLeftNegative
        List<Slot> checkLeftN = new List<Slot>();
        for (int i = 1; slotCheck.rowSlot + i < row.Length && slotCheck.colSlot + i < row[0].col.Length; i++)
        {
            if (row[slotCheck.rowSlot + i].col[slotCheck.colSlot + i].owner == slotCheck.owner)
            {
                checkLeftN.Add(row[slotCheck.rowSlot + i].col[slotCheck.colSlot + i]);
            }
            else
            {
                break;
            }
        }
        //Check DLeft
        if (checkLeftP.Count + checkLeftN.Count + 1 >= wonSlotCount)
        {
            isDLeftWon = true;
            Debug.Log($"{slotCheck.owner.typeIs} isDLeftWon");

            foreach (Slot slot in checkLeftP)
            {
                dangerSlot.Add(slot);
            }
            foreach (Slot slot in checkLeftN)
            {
                dangerSlot.Add(slot);
            }
        }

        //Check DRight
        //Check DRightPositive
        List<Slot> checkRightP = new List<Slot>();
        for (int i = 1; slotCheck.rowSlot - i >= 0 && slotCheck.colSlot + i < row[0].col.Length; i++)
        {
            if (row[slotCheck.rowSlot - i].col[slotCheck.colSlot + i].owner == slotCheck.owner)
            {
                checkRightP.Add(row[slotCheck.rowSlot - i].col[slotCheck.colSlot + i]);
            }
            else break;
        }
        //Check DRightNegative
        List<Slot> checkRightN = new List<Slot>();
        for (int i = 1; slotCheck.rowSlot + i < row.Length && slotCheck.colSlot - i >= 0; i++)
        {
            if (row[slotCheck.rowSlot + i].col[slotCheck.colSlot - i].owner == slotCheck.owner)
            {
                checkRightN.Add(row[slotCheck.rowSlot + i].col[slotCheck.colSlot - i]);
            }
            else break;
        }
        //Check DRight
        if (checkRightP.Count + checkRightN.Count + 1 >= wonSlotCount)
        {
            isDRightWon = true;
            Debug.Log($"{slotCheck.owner.typeIs} isDLeftWon");

            foreach (Slot slot in checkRightP)
            {
                dangerSlot.Add(slot);
            }
            foreach (Slot slot in checkRightN)
            {
                dangerSlot.Add(slot);
            }
        }

        if (dangerSlot.Count > 0)
        {
            dangerSlot.Add(slotCheck);
            return dangerSlot;
        }
        else
        {
            return dangerSlot;
        }
    }

    public void ChangeTurn()
    {
        switch (playerTrun)
        {
            case PlayerType.X:
                SetTurn(PlayerType.O);

                break;
            case PlayerType.O:
                SetTurn(PlayerType.X);
                break;
        }

        OnTrueChange?.Invoke();
    }

    public void SetTurn(PlayerType setPlayerTrun)
    {
        switch (setPlayerTrun)
        {
            case PlayerType.X:
                playerTrun = PlayerType.X;
                playerBaseTrun = playerX;
                playerBaseNext = playerO;

                break;
            case PlayerType.O:
                playerTrun = PlayerType.O;
                playerBaseTrun = playerO;
                playerBaseNext = playerX;
                break;
        }

        //playerBaseTrun.GetComponent<RectTransform>().localScale = Vector3.one;
        //playerBaseTrun.GetComponent<CanvasGroup>().alpha = 1;
        LeanTween.scale(playerBaseTrun.GetComponent<RectTransform>(), Vector3.one, 0.3f);
        LeanTween.alphaCanvas(playerBaseTrun.GetComponent<CanvasGroup>(), 1, 0.3f);

        //playerBaseNext.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
        //playerBaseNext.GetComponent<CanvasGroup>().alpha = 0.6f;
        LeanTween.scale(playerBaseNext.GetComponent<RectTransform>(), new Vector3(0.8f, 0.8f, 0.8f), 0.3f);
        LeanTween.alphaCanvas(playerBaseNext.GetComponent<CanvasGroup>(), 0.6f, 0.3f);

        Debug.Log("================== SetTrun =======================");

        if (slotTrigerWin == null)
        {
            if (!CheckPointCanPlayer())
            {
                Debug.Log("================== GameOver =======================");
            }
            else
            {
                Debug.Log("isReadyClick = True");
                isReadyClick = true;
            }
        }
        else
        {
            CheckSlotTrigerWon(slotTrigerWin);
            if (slotTrigerWin.owner == playerBaseTrun)
            {
                Debug.Log("================== GameOver =======================");
            }
            else
            {
                if (!CheckPointCanPlayer())
                {
                    Debug.Log("================== GameOver =======================");
                }
                else
                {
                    Debug.Log("isReadyClick = True");
                    isReadyClick = true;
                }
            }
        }

    }

    private bool CheckPointCanPlayer()
    {
        for (int i = 0; i < row.Length; i++)
        {
            for (int j = 0; j < row[i].col.Length; j++)
            {
                if (row[i].col[j].owner == null)
                {
                    return true;
                }
                else if (row[i].col[j].owner != playerBaseTrun)
                {
                    if (playerBaseTrun.point > row[i].col[j].point)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void GiveUp()
    {
        switch (playerTrun)
        {
            case PlayerType.X:
                break;
            case PlayerType.O:
                break;
        }
    }
}
