using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UINetworkManager : NetworkBehaviour
{
    public Button hostBtn;
    public TMP_InputField ipSendIpf;
    public Button cilentSendBtn;
    public GameObject WaitingPlayerUI;
    public GameObject JoinGameUI;
    public GameObject GameCanvas;

    [Header("Nope")]
    public GameObject hostIpCanvas;
    public TMP_Text hostIpText;

    private void Awake()
    {
        hostBtn.onClick.AddListener(() =>
        {
            HostSet();
        });

        cilentSendBtn.onClick.AddListener(() =>
        {
            ClientJoin();
        });
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        JoinGameUI.SetActive(true);
        WaitingPlayerUI.SetActive(false);

        Debug.Log($"UINetworkManager Start");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HostSet()
    {
        NetworkManager.Singleton.StartHost();

        ShowWaiting();
    }

    public void ClientJoin()
    {
        NetworkManager.Singleton.StartClient();

        Invoke(nameof(OnClinetConnectedServerRpc),1f);
    }

    public void playerTest()
    {
        TesterNetRpc();
    }

    [Rpc(SendTo.Server)]
    public void TesterNetRpc()
    {
        Debug.Log($"[Rpc(SendTo.Server)] : TesterNet()");

        TesterEEEERpc();
    }

    [Rpc(SendTo.Everyone)]
    public void TesterEEEERpc()
    {
        Debug.Log($"EEEEEEEEEEEEEEEEEE");
    }

    [Rpc(SendTo.Server)]
    public void OnClinetConnectedServerRpc()
    {

        Debug.Log($"[Rpc(SendTo.Server)]");
        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            ShowBoardClientRpc();
        }
    }

    [Rpc(SendTo.Everyone)]
    public void ShowBoardClientRpc()
    {
        JoinGameUI.SetActive(false);
        WaitingPlayerUI.SetActive(false);

        GameManager.instance.SetPlayerFrist(GameManager.instance.playerX);
    }

    public void ShowWaiting()
    {
        JoinGameUI.SetActive(false);
        WaitingPlayerUI.SetActive(true);
    }
}
