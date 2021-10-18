using UnityEngine;
using MilkShake;

public class CameraShaker : MonoBehaviour
{
    private Shaker MyShaker;
    [SerializeField] private ShakePreset ShakePreset;

    void Start()
    {
    	MyShaker = GetComponent<Shaker>();
    }

    public void shakeCam() {
        MyShaker.Shake(ShakePreset);
    }
}
