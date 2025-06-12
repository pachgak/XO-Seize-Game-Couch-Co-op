using UnityEngine;
using UnityEngine.UI;

public class ItemDrop : MonoBehaviour
{
    public float initialSpreadForce = 300f;
    public float spreadDuration = 0.3f;
    public float delyDuration = 0.3f;

    public float attractingDuration = 0.3f;

    private event System.Action ActionHit;

    private RectTransform rectTransform;
    private Vector3 targetTo;
    public PlayerBase playerOwner;

    private Vector2 spreadTargetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // กำหนดตำแหน่งเป้าหมายในการกระจายแบบสุ่ม
        Vector2 randomSpreadDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Vector2 spreadTargetPosition2d = rectTransform.anchoredPosition + randomSpreadDirection * Random.Range(initialSpreadForce/2, initialSpreadForce);
        spreadTargetPosition = new Vector3(spreadTargetPosition2d.x, spreadTargetPosition2d.y, 0f);

        Spread();
        Invoke(nameof(MoveTO), delyDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(Slot bySlot ,Vector3 to, System.Action hitAction)
    {
        playerOwner = bySlot.owner;
        ActionHit += hitAction;
        targetTo = to;
        ItemIconManager.instance.SpawnItemIcon(bySlot.owner.typeIs, 1, this.transform);

        //if (bySlot.owner.typeIs == GameManager.PlayerType.X)
        //{
        //    GetComponent<Image>().sprite = ItemIconManager.instance.iconX;
        //}
        //if (bySlot.owner.typeIs == GameManager.PlayerType.O)
        //{
        //    GetComponent<Image>().sprite = ItemIconManager.instance.iconO;
        //}
    }

    private void Spread()
    { 

        //this.gameObject.LeanMove(spreadTargetPosition , spreadDuration);
        LeanTween.move(rectTransform, spreadTargetPosition, spreadDuration).setEase(LeanTweenType.easeOutCubic);
    }
    //targetToPlayer.pointText.transform.position
    private void MoveTO()
    {
        LeanTween.move(gameObject, targetTo, attractingDuration).setEase(LeanTweenType.easeInCubic).setOnComplete(() => 
        {
            //PointManager.instance.AddPlayerPoint(targetToPlayer,1);
            ActionHit?.Invoke();
            ItemDropManager.instance.checkPoint();
            Destroy(gameObject);
        });
    }

}
