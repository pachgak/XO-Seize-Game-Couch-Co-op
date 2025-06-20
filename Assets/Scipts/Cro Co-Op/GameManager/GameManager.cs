using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : NetworkBehaviour
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

    public static event Action OnResetBoard;
    public static event Action OnResetScore;
    public static event Action OnTrueChange;
    public static event Action<PlayerBase> OnChangPlayerFrist;
    public static event Action<PlayerBase> OnEndGame3Line;
    public static event Action<PlayerBase> OnEndGame3LineAndEmptyPoint;
    public static event Action<PlayerBase> OnEndGameEmptyPoint;
    public static event Action<Slot> OnSetSlot;
    public static event Action<List<Slot>> OnDagerSlot;
    //public static event Action<List<Slot>> OnResetDagerSlot;
    //public static event Action<Slot> OnWonTrigerSlot;

    //gameData
    public int pointBase = 10;
    public int wonSlotCount = 3;
    //public int wonTrunCount = 1;
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
    public PlayerBase playerBaseTrun;
    public PlayerBase playerBaseNext;
    public bool isReadyClick;
    public bool isGameEnd;

    //click
    //public bool haveClick = false;
    //public Slot clickSlot;

    //win
    public Slot slotTrigerWin; 

    [System.Serializable]
    public class Row
    {
        public Slot[] col;
    }

    public enum PlayerType
    {
        X, O
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(OnAftertStart), 0.1f);
    }

    private void OnAftertStart()
    {
        playerX.typeIs = PlayerType.X;
        playerO.typeIs = PlayerType.O;

        //firstPlayer = playerX;

        //playerTrun = firstPlayer.typeIs;
        //SetTurn(playerTrun,false);


        SetPlayerFrist(playerX);
        //ResetScore();

        for (int i = 0; i < row.Length; i++)
        {
            for (int j = 0; j < row[i].col.Length; j++)
            {
                row[i].col[j].rowSlot = i;
                row[i].col[j].colSlot = j;
            }
        }
    }

    [Rpc(SendTo.Server)]
    public void CheckClientClickSlotServerRpc()
    {
            Debug.Log($"Not Your Trun");
 
    }

    [Rpc(SendTo.Server)]
    public void CheckClientClickSlotServerRpc(int slotClickRow, int slotClickCol, GameManager.PlayerType clickBy)
    {
        if (playerBaseTrun.typeIs != clickBy)
        {
            Debug.Log($"Not Your Trun");
            return;
        }
        if (!isReadyClick) return;

        ClickSlotRpc(slotClickRow, slotClickCol);
    }

    [Rpc(SendTo.Everyone)]
    public void ClickSlotRpc(int slotClickRow, int slotClickCol)
    {
        Slot slotClick = GameManager.instance.row[slotClickRow].col[slotClickCol];
        //if (playerBaseTrun.typeIs != clickBy)
        //{
        //    Debug.Log($"Not Your Trun");
        //    return;
        //}
        //if (!isReadyClick) return;

        PlayerBase trunPlayer = playerBaseTrun;

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
        Debug.Log($"CheckWin");
        if (slotTrigerWin == null)
        {
            if(CheckSlotTrigerWon2(slotCheck)) slotTrigerWin = slotCheck;
        }
        else
        {
            if (!CheckSlotTrigerWon2(slotTrigerWin))
            {
                OnDagerSlot?.Invoke(GetDangerSlots(slotTrigerWin));
                slotTrigerWin = null;

                if (CheckSlotTrigerWon2(slotCheck)) slotTrigerWin = slotCheck;
            }
            else OnDagerSlot?.Invoke(GetDangerSlots(slotTrigerWin));

        }
    }
    /*
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
    */
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

    public void ChangeTurn(bool checkWin)
    {
        Debug.Log("------ ChangeTurn");

        switch (playerTrun)
        {
            case PlayerType.X:
                SetTurn(PlayerType.O, checkWin);

                break;
            case PlayerType.O:
                SetTurn(PlayerType.X, checkWin);
                break;
        }

        OnTrueChange?.Invoke();
    }

    public void SetTurn(PlayerType setPlayerTrun , bool checkWin)
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

        if(checkWin) checkWiner();

    }

    public void checkWiner()
    {
        if (slotTrigerWin == null)
        {
            if (!CheckPointCanPlayer())
            {
                isGameEnd = true;
                OnEndGameEmptyPoint?.Invoke(GameManager.instance.playerBaseNext);
            }
            else
            {
                isReadyClick = true;
            }
        }
        else
        {
            
            if (slotTrigerWin.owner == playerBaseTrun)
            {
                OnDagerSlot?.Invoke(GetDangerSlots(slotTrigerWin));
                isGameEnd = true;
                OnEndGame3Line?.Invoke(GameManager.instance.playerBaseTrun);
            }
            else
            {
                if (!CheckPointCanPlayer())
                {
                    isGameEnd = true;
                    OnEndGame3LineAndEmptyPoint?.Invoke(GameManager.instance.playerBaseNext);
                }
                else
                {
                    OnDagerSlot?.Invoke(GetDangerSlots(slotTrigerWin));
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

    public void ShowDagerSlot(List<Slot> slot)
    {
        OnDagerSlot?.Invoke(slot);
    }
    public void ResetBoard()
    {
        ShowDagerSlot(null);
        SetTurn(firstPlayer.typeIs,false);
        OnResetBoard?.Invoke();

        slotTrigerWin = null;
    }
    public void ChangPlayerFrist()
    {
        switch (firstPlayer.typeIs)
        {
            case PlayerType.X:
                firstPlayer = playerO;

                break;
            case PlayerType.O:
                firstPlayer = playerX;
                break;
        }

        SetPlayerFrist(firstPlayer);
    }

    public void SetPlayerFrist(PlayerBase player)
    {
        isReadyClick = true;
        firstPlayer = player;
        OnChangPlayerFrist?.Invoke(firstPlayer);
        ResetBoard();
        SetTurn(firstPlayer.typeIs,false);
    }
    public void ResetScore()
    {
        OnResetScore?.Invoke();
        ResetBoard();
    }


}
