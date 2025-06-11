using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class itemShoot : MonoBehaviour
{
    public float speed = 300;

    private Slot targetSlot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shooting(float time)
    {

        LeanTween.move(this.gameObject, targetSlot.transform.position, time).setOnComplete(() => 
        { 
            BulletManager.instance.BulletHitTarget(targetSlot); 
            Destroy(this.gameObject);
        });
    }

    public void SetUpAndShoot(Slot Slot)
    {
        targetSlot = Slot;
        float time = Vector3.Distance(this.transform.position, targetSlot.transform.position) / speed;
        Shooting(time);

        ItemIconManager.instance.SpawnItemIcon(GameManager.instance.playerTrun, Slot.point + 1, this.transform);
    }
}
