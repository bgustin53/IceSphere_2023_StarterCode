using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Objects to Spawn")]
    [SerializeField] private GameObject iceSphere;
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject powerUp;

    [Header("Wave Fields")]
    [SerializeField] private int initialWave;
    [SerializeField] private int increaseEachWave;
    [SerializeField] private int maximumWave;

    [Header("Portal Fields")]
    [SerializeField] private int portalFirstAppearance;
    [SerializeField] private float portalByWaveProbability;
    [SerializeField] private float portalByWaveDuration;

    [Header("PowerUp Fields")]
    [SerializeField] private int powerUpFirstAppearance;
    [SerializeField] private float powerUpByWaveProbability;
    [SerializeField] private float powerUpByWaveDuration;

    [Header("Objects")]
    [SerializeField] private GameObject island;
    [SerializeField] private PlayerController player;

    private Vector3 islandSize;
    private int waveNumber;
    private bool portalActive;
    private bool powerUpActive;

    // Start is called before the first frame update
    void Start()
    {
        islandSize = island.GetComponent<MeshCollider>().bounds.size;
        waveNumber = initialWave;
        if(GameManager.Instance.debugTransport)
        {
            portalByWaveDuration = 99;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((waveNumber > portalFirstAppearance || GameManager.Instance.debugSpawnPortal) && !portalActive)
        {
            SetObjectActive(portal, portalByWaveProbability);
        }

        if ((waveNumber > powerUpFirstAppearance || GameManager.Instance.debugSpawnPowerUp)
             && !powerUpActive)
        {
            SetObjectActive(powerUp, powerUpByWaveProbability);
        }

        if (FindObjectsOfType<IceSphereController>().Length == 0 &&
           GameObject.Find("Player") != null)
        {
            SpawnIceWave();
        }
    }

    private void SpawnIceWave()
    {
        for(int i = 0; i <= increaseEachWave * (waveNumber - 1) ; i++)
        {
            Instantiate(iceSphere, SetRandomPosition(0), iceSphere.transform.rotation);
        }

        if(waveNumber < maximumWave)
        {
            waveNumber++;
        }
    }

    private void SetObjectActive(GameObject obj, float byWaveProbability)
    {
        if(Random.value < waveNumber * byWaveProbability * Time.deltaTime ||
           GameManager.Instance.debugSpawnPortal || GameManager.Instance.debugSpawnPowerUp)
        {
            obj.transform.position = SetRandomPosition(obj.transform.position.y);
            StartCoroutine(CountdownTimer(obj.tag));
        }
    }

    private Vector3 SetRandomPosition(float posY)
    {
        float posX = Random.Range(-islandSize.x / 2.75f, islandSize.x / 2.75f);
        float posZ = Random.Range(-islandSize.z / 2.75f, islandSize.z / 2.75f);
        return new Vector3(posX, posY, posZ);
    }

    IEnumerator CountdownTimer(string objectTag)
    {
        float byWaveDuration = 0;

        switch (objectTag)
        {
            case "Portal":
                portal.SetActive(true);
                portalActive = true;
                byWaveDuration = portalByWaveDuration;
                break;
            case "PowerUp":
                powerUp.SetActive(true);
                powerUpActive = true;
                byWaveDuration = powerUpByWaveDuration;
                break;
        }

        yield return new WaitForSeconds(waveNumber * byWaveDuration);

        switch (objectTag)
        {
            case "Portal":
                portal.SetActive(false);
                portalActive = false;
                break;
            case "PowerUp":
                powerUp.SetActive(false);
                powerUpActive = false;
                break;
        }
    }
}
