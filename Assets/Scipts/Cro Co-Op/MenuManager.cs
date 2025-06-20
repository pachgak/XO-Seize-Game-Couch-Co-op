using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : NetworkBehaviour
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

    public void ReSetBoardButton()
    {
        ReSetBoardButtonServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void ReSetBoardButtonServerRpc()
    {
        ReSetBoardButtonClientRpc();
    }

    [Rpc(SendTo.Everyone)]
    private void ReSetBoardButtonClientRpc()
    {
        //reBoardBnt.SetActive(false);
        //changTrunBnt.SetActive(true);
        GameManager.instance.ResetBoard();
    }

    public void ChangTrunButton()
    {
        ChangTrunButtonServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void ChangTrunButtonServerRpc()
    {
        ChangTrunButtonClientRpc();
    }

    [Rpc(SendTo.Everyone)]
    private void ChangTrunButtonClientRpc()
    {
        //reBoardBnt.SetActive(false);
        //changTrunBnt.SetActive(true);
        GameManager.instance.ChangPlayerFrist();
    }

    public void CleanScoreButton()
    {
        CleanScoreButtonServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void CleanScoreButtonServerRpc()
    {
        CleanScoreButtonClientRpc();
    }

    [Rpc(SendTo.Everyone)]
    private void CleanScoreButtonClientRpc()
    {
        //reBoardBnt.SetActive(false);
        //changTrunBnt.SetActive(true);
        ScoreManager.instance.ResetScore();
    }

    public void SetCustomPoint()
    {
        if (!string.IsNullOrEmpty(cuntomPointInput.text))
        {
            SetCustomPointServerRpc(int.Parse(cuntomPointInput.text));
        }
    }

    [Rpc(SendTo.Server)]
    private void SetCustomPointServerRpc(int cutomPoint)
    {
        SetCustomPointClientRpc(cutomPoint);
    }

    [Rpc(SendTo.Everyone)]
    private void SetCustomPointClientRpc(int cutomPoint)
    {
        GameManager.instance.pointBase = cutomPoint;
        GameManager.instance.ResetBoard();

        cuntomPointInput.text = null;
    }

}
