using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject  barrelSpawner;
    public float lowerBound = 0f;
    public float upperBound = 0f;

    private void Start()
    {
        Spawn();
    }
    
    private void Spawn()
    {
        Instantiate(barrelSpawner,transform.position, Quaternion.identity);
        Invoke(nameof(Spawn),Random.Range(lowerBound + 1f, upperBound + 3f));
    }
}