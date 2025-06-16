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
        // เริ่มต้น Coroutine สำหรับนับถอยหลัง
        yield return StartCoroutine(CountdownLoop());

        // เมื่อ Coroutine นับถอยหลังเสร็จสิ้น ให้รอนาน 5 วินาที
        Debug.Log("Countdown finished. Waiting 5 seconds...");
        yield return new WaitForSeconds(5f);

        // หลังจากรอ 5 วินาที ให้แสดงข้อความ "End"
        Debug.Log("End");
    }

    IEnumerator CountdownLoop()
    {
        for (int i = 10; i >= 1; i--)
        {
            Debug.Log("Countdown: " + i);
            yield return new WaitForSeconds(1f); // รอ 1 วินาที ก่อนแสดงตัวเลขถัดไป
        }
        Debug.Log("Countdown complete!");
    }
}