using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DangerSlotManager : MonoBehaviour
{
    public static DangerSlotManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public GameObject dangerTopUI;
    public GameObject dangerDownUI;
    public AudioClip warningSound;
    private AudioSource audioSource;
    public List<Slot> dagerSlots;

    public Vector3 openTopDagerPos;
    public Vector3 closeTopDagerPos;
    public Vector3 openDownDagerPos;
    public Vector3 closeDownDagerPos;
    //public Slot tiggerSlotOld;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (openTopDagerPos == Vector3.zero) openTopDagerPos = dangerTopUI.GetComponent<RectTransform>().position;
        if (openDownDagerPos == Vector3.zero) openDownDagerPos = dangerDownUI.GetComponent<RectTransform>().position;
        dangerTopUI.GetComponent<RectTransform>().position = closeTopDagerPos;
        dangerDownUI.GetComponent<RectTransform>().position = closeDownDagerPos;

        audioSource = GetComponent<AudioSource>();
        GameManager.OnDagerSlot += TrigerDagerSlot;
        GameManager.OnTrueChange += SoundDagerSlot;
        //GameManager.OnDagerSlot += SetDagerSlot;
        //GameManager.OnWonTrigerSlot += CleanOldTriger;
        //GameManager.OnTrueChange += ShowDagerSlot;
        //GameManager.OnTrueChange += ShowDagerSlot;
    }

    private void OnDestroy()
    {
        GameManager.OnDagerSlot -= TrigerDagerSlot;
        GameManager.OnTrueChange -= SoundDagerSlot;
        //GameManager.OnDagerSlot -= SetDagerSlot;
        //GameManager.OnWonTrigerSlot -= CleanOldTriger;
        //GameManager.OnTrueChange -= ShowDagerSlot;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TrigerDagerSlot(List<Slot> Slots)
    {
        if (dagerSlots != null)
        {
            if (dagerSlots.Count > 0)
            {
                foreach (Slot slot in dagerSlots)
                {
                    slot.ShowDanger(false);
                }

            }
        }

        dagerSlots = Slots;

        if (dagerSlots != null && dagerSlots.Count > 0)
        {
            OpenDangerUI();

            foreach (Slot slot in dagerSlots)
            {
                slot.ShowDanger(true);
            }

        }
        else
        {
            CloseDangerUI();
        }

    }

    public void SoundDagerSlot()
    {
        if (dagerSlots != null && dagerSlots.Count > 0 && !GameManager.instance.isGameEnd)
        {
            audioSource.PlayOneShot(warningSound);
        }
    }

    private void OpenDangerUI()
    {
        LeanTween.cancel(dangerTopUI);
        LeanTween.cancel(dangerDownUI);

        LeanTween.move(dangerTopUI.gameObject, openTopDagerPos, 1f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.move(dangerDownUI.gameObject, openDownDagerPos, 1f).setEase(LeanTweenType.easeOutQuint);
        
        //dangerTopUI.SetActive(true);
        //dangerDownUI.SetActive(true);
    }
    private void CloseDangerUI()
    {
        LeanTween.cancel(dangerTopUI);
        LeanTween.cancel(dangerDownUI);

        LeanTween.move(dangerTopUI.gameObject, closeTopDagerPos, 1f).setEase(LeanTweenType.easeInCubic);
        LeanTween.move(dangerDownUI.gameObject, closeDownDagerPos, 1f).setEase(LeanTweenType.easeInCubic);

        //dangerTopUI.SetActive(false);
        //dangerDownUI.SetActive(false);
    }

    /*
    public void SetDagerSlot(List<Slot> Slots)
    {
        if (dagerSlots != null)
        {
            foreach (Slot slot in dagerSlots)
            {
                slot.ShowDanger(false);
            }
        }

        dagerSlots = Slots;
    }

    public void CleanOldTriger(Slot newTiggerSlot)
    {
        Debug.Log($" CleanOldTriger ");
        if (tiggerSlotOld != null)
        {
            
            Debug.Log($" ??????????? tiggerSlotOld == {tiggerSlotOld.name} ????????????");
            //Debug.Log($" ??????????? dagerSlots == {dagerSlots.Count} ????????????");

            //foreach (Slot slot in dagerSlots)
            //{
            //    slot.ShowDanger(false);
            //}
            //GameManager.instance.CheckSlotTrigerWon(tiggerSlotOld);
            //ShowDagerSlot();
        }

        tiggerSlotOld = newTiggerSlot;

    }


    public void ShowDagerSlot()
    {
        if (dagerSlots != null && dagerSlots.Count > 0)
        {
            dangerTopUI.SetActive(true);
            dangerDownUI.SetActive(true);
            foreach (Slot slot in dagerSlots)
            {
                slot.ShowDanger(true);
            }

        }
        else
        {
            dangerTopUI.SetActive(false);
            dangerDownUI.SetActive(false);
        }
    }
    */
}
