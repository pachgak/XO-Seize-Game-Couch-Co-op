using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject reBoardBnt;
    public GameObject changTrunBnt;

    public TMP_InputField cuntomPointInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        reBoardBnt.SetActive(false); 
        changTrunBnt.SetActive(true);
        GameManager.OnSetSlot += ChangerReBoardAndChangTrun;
        GameManager.OnResetBoard += ReBoardUI;
    }

    private void OnDestroy()
    {
        GameManager.OnSetSlot -= ChangerReBoardAndChangTrun;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangerReBoardAndChangTrun(Slot slot)
    {
        if (changTrunBnt.activeSelf)
        {
            reBoardBnt.SetActive(true);
            changTrunBnt.SetActive(false);
        }
    }

    public void ReBoardUI()
    {
        reBoardBnt.SetActive(false);
        changTrunBnt.SetActive(true);
        //GameManager.instance.ResetBoard();
    }

    public void SetCustomPoint()
    {
        if (!string.IsNullOrEmpty(cuntomPointInput.text))
        {
            GameManager.instance.pointBase = int.Parse(cuntomPointInput.text);
            ReBoardUI();

            cuntomPointInput.text = null;
        }
    }

}
