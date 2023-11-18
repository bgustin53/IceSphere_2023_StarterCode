using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSphereController : MonoBehaviour
{
    [SerializeField] private float startDelay;
    [SerializeField] private float reductionEachRepeat;
    [SerializeField] private float minimumVolume;

    private Rigidbody iceRB;
    private ParticleSystem iceVFX;

    // Start is called before the first frame update
    void Start()
    {
        
        if(GameManager.Instance.debugSpawnWaves)
        {
            reductionEachRepeat = .5f;
        }

        iceRB = GetComponent<Rigidbody>();
        iceVFX = GetComponent<ParticleSystem>();
        RandomizeSizeAndMass();
        InvokeRepeating("Melt", startDelay, 0.4f);
    }

    // Update is called once per frame
    private void RandomizeSizeAndMass()
    {
        float randomValue = Random.value * 0.5f + 0.5f;
        iceRB.mass *= randomValue;
        transform.localScale *= randomValue;
    }

    private void Dissolution()
    {
        float volume = 4f / 3f * Mathf.PI * Mathf.Pow(transform.localScale.x, 2);
        if(volume < 0.8 && FindObjectsOfType<IceSphereController>().Length > 1)
        {
            iceVFX.Stop();
        }

        if(volume < minimumVolume)
        {
            Destroy(gameObject);
        }
    }

    private void Melt()
    {
        iceRB.mass *= reductionEachRepeat;
        transform.localScale *= reductionEachRepeat;
        Dissolution();
    }
}
