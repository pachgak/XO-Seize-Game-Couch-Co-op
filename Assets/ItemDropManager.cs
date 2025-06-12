using UnityEngine;
using System;

public class ItemDropManager : MonoBehaviour
{
    public static ItemDropManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public int pointRetrun;
    public int curentRetrun;
    public bool isChecker = false;

    public event Action endHitAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TrigerCountDownItemDrop(int addPointRetrun , System.Action action)
    {
        endHitAction += action;
        if (addPointRetrun == 0)
        {
            endHitAction?.Invoke();
        }
        else
        {
            pointRetrun = addPointRetrun;
            isChecker = true;
        }
    }

    public void checkPoint()
    {
        if (!isChecker) return;
        curentRetrun++;

        if (pointRetrun == curentRetrun)
        {
            endHitAction?.Invoke();
            Debug.Log("------ pointRetrun == curentRetrun");
            isChecker = false;
            curentRetrun = 0;
            pointRetrun = 0;
            endHitAction = null;
        }
    }
}
