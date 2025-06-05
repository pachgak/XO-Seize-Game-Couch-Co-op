using UnityEngine;

public class XOManager : MonoBehaviour
{
    public static XOManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public PlayerBase playerX;
    public PlayerBase playerO;
    public PlayerTrun playerTrun;

    public enum PlayerTrun
    {
        X , O
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickSlot(Slot slotClick)
    {
        Debug.Log($"Click Slot {slotClick.name}");
        switch (playerTrun)
        {
            case PlayerTrun.X:
                break;
            case PlayerTrun.O:
                break;
        }
    }
}
