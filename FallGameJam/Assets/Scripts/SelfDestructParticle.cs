using UnityEngine;

public class SelfDestructParticle : MonoBehaviour
{   
    [SerializeField] private float selfDestructTime; 
    void Update()
    {
        selfDestructTime -= Time.deltaTime;
        if(selfDestructTime < 0)
            Destroy(gameObject);
    }
}
