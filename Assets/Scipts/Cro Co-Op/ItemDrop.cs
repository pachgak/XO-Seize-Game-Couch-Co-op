using UnityEngine;
using UnityEngine.UI;

public class ItemDrop : MonoBehaviour
{
    public float initialSpreadForce = 300f;
    public float spreadDuration = 0.3f;
    public float delyDuration = 0.3f;

    public float attractingDuration = 0.3f;

    private RectTransform rectTransform;
    private PlayerBase targetToPlayer;
    

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

    public void SetOwner(Slot bySlot)
    {
        targetToPlayer = bySlot.owner;
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

    private void MoveTO()
    {
        LeanTween.move(gameObject, targetToPlayer.pointText.transform.position, attractingDuration).setEase(LeanTweenType.easeInCubic).setOnComplete(() => 
        { 
            PointManager.instance.AddPlayerPoint(targetToPlayer,1);
            Destroy(gameObject);
        });
    }

}
