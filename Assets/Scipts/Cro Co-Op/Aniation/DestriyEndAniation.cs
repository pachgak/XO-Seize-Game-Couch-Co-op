using UnityEngine;

public class DestriyEndAniation : MonoBehaviour
{
    public AudioClip effectSound;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Destroyer()
    {
        Destroy(gameObject);
    }

    public void PlaySound()
    {
        GetComponent<AudioSource>().PlayOneShot(effectSound);
    }

}
