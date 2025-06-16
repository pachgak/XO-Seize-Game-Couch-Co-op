using System.Collections;
using UnityEngine;

public class testCounddown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FullProcess()); 
    }

    IEnumerator FullProcess()
    {
        // ������� Coroutine ����Ѻ�Ѻ�����ѧ
        yield return StartCoroutine(CountdownLoop());

        // ����� Coroutine �Ѻ�����ѧ������� ����͹ҹ 5 �Թҷ�
        Debug.Log("Countdown finished. Waiting 5 seconds...");
        yield return new WaitForSeconds(5f);

        // ��ѧ�ҡ�� 5 �Թҷ� ����ʴ���ͤ��� "End"
        Debug.Log("End");
    }

    IEnumerator CountdownLoop()
    {
        for (int i = 10; i >= 1; i--)
        {
            Debug.Log("Countdown: " + i);
            yield return new WaitForSeconds(1f); // �� 1 �Թҷ� ��͹�ʴ�����Ţ�Ѵ�
        }
        Debug.Log("Countdown complete!");
    }
}