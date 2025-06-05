using Unity.Netcode;
using UnityEngine;

public class PlayerData : NetworkBehaviour
{
    public int playerNumber;

    public override void OnNetworkSpawn()
    {
        playerNumber = NetworkManager.Singleton.ConnectedClients.Count;
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
