using System;
using UnityEngine;
using UnityEngine.Events;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public static event Action<Slot> OnBulletHit;
    public static event Action OnTest;

    public GameObject bulletPrefab;
    public Transform bulletParant;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.OnSetSlot += ShootItem;
        
    }

    private void OnDestroy()
    {
        GameManager.OnSetSlot -= ShootItem;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootItem(Slot targetSlot)
    {
        GameObject clone = Instantiate(bulletPrefab, bulletParant);
        clone.GetComponent<RectTransform>().position = GameManager.instance.playerBaseTrun.pointText.GetComponent<RectTransform>().position;
        clone.GetComponent<itemShoot>().SetUpAndShoot(targetSlot);
    }

    public void BulletHitTarget(Slot targetSlot)
    {
        OnBulletHit?.Invoke(targetSlot);
    }
}
