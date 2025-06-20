using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerMultiplayerData : NetworkBehaviour
{
    public static PlayerMultiplayerData instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public int playerNumber;
    public GameManager.PlayerType playerTypeIs;



    public PlayerBase playerBaseLeft;
    public TMP_Text playerWinLeftText;
    public PlayerBase playerBaseRight;
    public TMP_Text playerWinRightText;

    [Header("IconIs")]
    public Sprite iconX;
    public Sprite iconO;
    public override void OnNetworkSpawn()
    {
        playerNumber = NetworkManager.Singleton.ConnectedClients.Count;

        

        if (IsHost)
        {
            SetUpPlayer(GameManager.PlayerType.X);
        }
        else if (IsClient)
        {
            SetUpPlayer(GameManager.PlayerType.O);
        }
    }

    public void SetUpPlayer(GameManager.PlayerType isType)
    {
        switch (isType) 
        {
            case GameManager.PlayerType.X:

                playerBaseLeft.playerIcon.sprite = iconX;
                playerBaseRight.playerIcon.sprite = iconO;

                playerBaseLeft.typeIs = GameManager.PlayerType.X;
                playerBaseRight.typeIs = GameManager.PlayerType.O;

                playerBaseLeft.WinText = playerWinLeftText;
                playerBaseRight.WinText = playerWinRightText;

                GameManager.instance.playerX = playerBaseLeft;
                GameManager.instance.playerO = playerBaseRight;

                break;
            case GameManager.PlayerType.O:

                playerBaseLeft.playerIcon.sprite = iconO;
                playerBaseRight.playerIcon.sprite = iconX;

                playerBaseLeft.typeIs = GameManager.PlayerType.O;
                playerBaseRight.typeIs = GameManager.PlayerType.X;

                playerBaseLeft.WinText = playerWinLeftText;
                playerBaseRight.WinText = playerWinRightText;

                GameManager.instance.playerO = playerBaseLeft;
                GameManager.instance.playerX = playerBaseRight;

                break;
        }
        playerTypeIs = isType;

        GameManager.instance.playerO.SetEvent();
        GameManager.instance.playerX.SetEvent();



    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
