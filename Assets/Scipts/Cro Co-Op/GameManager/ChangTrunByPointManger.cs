using UnityEngine;

public class ChangTrunByPointManger : MonoBehaviour
{
    public static ChangTrunByPointManger instance;
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
    public bool isChecker = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PointManager.OnPointXChange += checkPoint;
        PointManager.OnPointOChange += checkPoint;
    }

    private void OnDestroy()
    {
                PointManager.OnPointXChange -= checkPoint;
        PointManager.OnPointOChange -= checkPoint;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TrigerChangTrunByPoint(int addPointRetrun)
    {

        if (addPointRetrun == 0)
        {
            GameManager.instance.ChangeTurn(true);
        }
        else
        {
            pointRetrun = GameManager.instance.playerBaseNext.point + addPointRetrun;
            isChecker = true;
        }
    }

    public void checkPoint(int pointCheck)
    {
        if (!isChecker) return;
        if (pointRetrun == pointCheck)
        {
            GameManager.instance.ChangeTurn(true);
            isChecker = false;
        }
    }
}
