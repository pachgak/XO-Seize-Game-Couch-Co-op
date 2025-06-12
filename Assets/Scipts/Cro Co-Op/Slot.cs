using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    
    public GameObject imageXO;
    public TMP_Text countXO;
    public GameObject danger;
    public GameObject protect;

    public GameObject itemDropPrefab;
    public GameObject exprotionPrefab;

    private AudioSource audioSource;
    public AudioClip setEmptySound;
    public AudioClip explosionSound;

    [Header("System")]
    public PlayerBase owner;
    public int point;
    public bool isProtect = false;
    public int lifeTrunProtect = 0;
    [Header("Slot_Id")]
    public int rowSlot;
    public int colSlot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        audioSource = GetComponent<AudioSource>();
        ResetSlot();

        //Add Event
        BulletManager.OnBulletHit += SlotSetCheckItMe;
        GameManager.OnTrueChange += ProtectCountDownTrun;
    }

    private void OnDestroy()
    {
        BulletManager.OnBulletHit -= SlotSetCheckItMe;
        GameManager.OnTrueChange -= ProtectCountDownTrun;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.ClickSlot(this);
    }

    public void ResetSlot()
    {
        owner = null;
        if(point > 0) RetrueToOwner();
        point = 0;
        countXO.text = $"{point}";
        //countXO.gameObject.SetActive(false);
        danger.SetActive(false);
        protect.SetActive(false);

        for (int i = 0; i < imageXO.transform.childCount; i++)
        {
            Destroy(imageXO.transform.GetChild(i).gameObject);
        }
    }

    public void RetrueToOwner()
    {
        //PointManager.instance.AddPlayerPoint(owner, point); 
        for (int i = 0; i < point; i++)
        {
            GameObject clone = Instantiate(itemDropPrefab, EffectManager.instance.effectGameParant);
            clone.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;
            clone.GetComponent<ItemDrop>().SetUp(this, this.owner.pointText.transform.position,() => 
                {
                    PointManager.instance.AddPlayerPoint(clone.GetComponent<ItemDrop>().playerOwner, 1); 
                });
        }

        for (int i = 0; i < imageXO.transform.childCount; i++)
        {
            Destroy(imageXO.transform.GetChild(i).gameObject);
        }

    }

    public void RetrueToScore()
    {
        //PointManager.instance.AddPlayerPoint(owner, point); 
        for (int i = 0; i < point; i++)
        {
            GameObject clone = Instantiate(itemDropPrefab, EffectManager.instance.effectGameParant);
            clone.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;
            clone.GetComponent<ItemDrop>().SetUp(this, this.owner.WinText.transform.position, () =>
            {
            });
        }

        ResetSlot();
    }

    public void DestroySlot()
    {
        GameObject clone = Instantiate(exprotionPrefab, EffectManager.instance.effectGameParant);
        clone.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;

        ResetSlot();
    }

    public void SlotSetCheckItMe(Slot slot)
    {
        if (slot == this)
        {
            
            if (owner == null)
            {
                audioSource.PlayOneShot(setEmptySound);
                SetOwner(slot);
                GameManager.instance.ChangeTurn(true);
            }
            else if (owner != GameManager.instance.playerBaseTrun)
            {
                audioSource.PlayOneShot(explosionSound);
                SetNewOwner(slot);
                ChangTrunByPointManger.instance.TrigerChangTrunByPoint(point-1); 
            }
            else
            {

            }
        }
    }

    private void SetOwner(Slot slot)
    {
        owner = GameManager.instance.playerBaseTrun;
        point++;
        countXO.text = $"{point}";
        //PointManager.instance.RemovePlayerPoint(owner, point);
        ItemIconManager.instance.SpawnItemIcon(owner.typeIs, point, imageXO.transform);
        AddProtect(2);

        GameManager.instance.CheckWin(slot);

    }

    private void SetNewOwner(Slot slot)
    {
        ExporToOwner();

        SetOwner(slot);

        PointManager.instance.RemovePlayerPoint(owner, point);

    }

    private void ExporToOwner()
    {
        GameObject clone = Instantiate(exprotionPrefab, EffectManager.instance.effectGameParant);
        clone.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;

        RetrueToOwner();
    }

    private void AddProtect(int lifeTrun)
    {
        lifeTrunProtect = lifeTrun;
        protect.SetActive(true);
        isProtect = true;
    }

    public void ProtectCountDownTrun() 
    {
        if(lifeTrunProtect > 0) lifeTrunProtect--;
        if (isProtect && lifeTrunProtect == 0)
        {
            isProtect = false;
            protect.SetActive(false);
        }
    }

    public void ShowDanger(bool set)
    {
        danger.SetActive(set);
    }
}
