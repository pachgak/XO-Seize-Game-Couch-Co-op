using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UINetworkManager : MonoBehaviour
{
    public Button hostBtn;
    public GameObject hostIpCanvas;
    public TMP_Text hostIpText;

    public TMP_InputField ipSendIpf;
    public Button cilentSendBtn;

    private void Awake()
    {
        hostBtn.onClick.AddListener(() => 
        {
            NetworkManager.Singleton.StartHost();
        });

        cilentSendBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
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
