using System;
using UnityEngine;
using static GameManager;

public class ItemIconManager : MonoBehaviour
{
    public static ItemIconManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public Sprite iconX;
    public Sprite iconO;

    public GameObject[] itemIconsPrefab;
     
    public GameObject SpawnItemIcon(PlayerType type , int point , Transform parant)
    {

        if (point < 10)
        {
            GameObject clone = Instantiate(itemIconsPrefab[point - 1], parant);
            SetIconItemByWho(type, clone.GetComponent<Item>());
            return clone;
        }
        if (point >= 10)
        {
            GameObject clone = Instantiate(itemIconsPrefab[10 - 1], parant);
            SetIconItemByWho(type, clone.GetComponent<Item>());
            return clone;
        }

        return null;
    }

    private void SetIconItemByWho(PlayerType type ,Item thisItem)
    {
        if (type == PlayerType.X) thisItem.SetXIcon();
        if (type == PlayerType.O) thisItem.SetOIcon();
    }
}

