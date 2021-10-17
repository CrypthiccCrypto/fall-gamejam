using UnityEngine;

public class ShobHead : MonoBehaviour
{
    AudioSource audiosource;
    
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }
    public void shootAudio() {
        audiosource.Play();
    }
}
