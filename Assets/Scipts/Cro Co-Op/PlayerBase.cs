using TMPro;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public int winCount = 0;
    public int point = 0;
    public GameObject giveUpUI;
    public TMP_Text pointText;
    public TMP_Text WinText;
    public GameObject playerFirstIcon;
    [Header("System")]
    public GameManager.PlayerType typeIs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (this == GameManager.instance.playerX)
        {
            ScoreManager.OnScoreXChange += ShowWin;
            PointManager.OnPointXChange += ShowPoint;
        }
        if (this == GameManager.instance.playerO)
        {
            ScoreManager.OnScoreOChange += ShowWin;
            PointManager.OnPointOChange += ShowPoint;
        }
        GameManager.OnChangPlayerFrist += ShowFirstIcon;
    }
    private void OnDestroy()
    {
        if (this == GameManager.instance.playerX)
        {
            ScoreManager.OnScoreXChange -= ShowWin;
            PointManager.OnPointXChange -= ShowPoint;
        }
        if (this == GameManager.instance.playerO)
        {
            ScoreManager.OnScoreOChange -= ShowWin;
            PointManager.OnPointOChange -= ShowPoint;
        }
        GameManager.OnChangPlayerFrist -= ShowFirstIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowWin(int score)
    {
        GameObject clone = Instantiate(ScoreManager.instance.effectWinChangePrefab, EffectManager.instance.effectTopGameUIParant);
        clone.GetComponent<RectTransform>().position = WinText.GetComponent<RectTransform>().position;

        WinText.text = $"{score}";
        PopAniNumber(WinText.gameObject);
    }

    private void ShowPoint(int point)
    {
        pointText.text = $"{point}";
        PopAniNumber(pointText.gameObject);
    }

    private void ShowFirstIcon(PlayerBase player)
    {
        LeanTween.cancel(playerFirstIcon.gameObject);
        if (player == this)
        {    
            playerFirstIcon.SetActive(true);
            LeanTween.scale(playerFirstIcon.gameObject, Vector3.one, 1.5f).setEase(LeanTweenType.easeOutElastic);
        }
        else LeanTween.scale(playerFirstIcon.gameObject, Vector3.zero, 0.5f).setOnComplete(() => { playerFirstIcon.SetActive(false); }); ;
        }

    private void PopAniShowIcon(GameObject icon)
    {
        icon.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        LeanTween.scale(icon.gameObject, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeOutElastic);
    }

    private void PopAniNumber(GameObject numberText)
    {
        numberText.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        LeanTween.scale(numberText.gameObject, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeOutElastic);
    }
}
