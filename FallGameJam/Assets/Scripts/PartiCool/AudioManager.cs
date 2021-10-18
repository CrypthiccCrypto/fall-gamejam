using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioClip[] audioClipArray;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // 0 = bullet shot
    }

    public void playAudio(int idx, float volume) {
        audioSource.PlayOneShot(audioClipArray[idx], volume);
    }
}
