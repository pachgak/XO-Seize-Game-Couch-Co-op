using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.UI.GridLayoutGroup;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public PlayerBase owner;
    public int point;
    public GameObject imageXO;
    public TMP_Text countXO;
    public GameObject danger;
    public GameObject protect;

    public GameObject effectXOPrefab;
    public GameObject exprotionPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countXO.gameObject.SetActive(false);
        danger.SetActive(false);
        protect.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        XOManager.instance.ClickSlot(this);
    }

    public void ResetSlot()
    {
        owner = null;
        if(point > 0) DestroyXO();
        countXO.gameObject.SetActive(false);
        danger.SetActive(false);
        protect.SetActive(false);
    }

    public void DestroyXO()
    {
        point = 0;
    }

    public void SetOwner(int point ,PlayerBase owner)
    {

    }
}
