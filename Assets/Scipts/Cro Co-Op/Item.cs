using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public List<Image> icons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void SetXIcon()
    {
        foreach (var item in icons)
        {
            item.sprite = ItemIconManager.instance.iconX;
        }
    }

    public void SetOIcon()
    {
        foreach (var item in icons)
        {
            item.sprite = ItemIconManager.instance.iconO;
        }
    }
}
