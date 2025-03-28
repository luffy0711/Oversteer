using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpawner : MonoBehaviour
{

    public List<GameObject> trackPrefabs; // List of track prefabs
    private Transform currentEndPoint;     // Current endpoint where the next track will spawn
    public float spawnDelay = 2f;         // Delay between spawns

    void Start()
    {
        // Start the spawning process
        StartCoroutine(SpawnTrack());
        currentEndPoint = GameObject.FindGameObjectWithTag("EndPoint").transform;
    }

    IEnumerator SpawnTrack()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            // Pick a random track
            GameObject newTrack = Instantiate(
                trackPrefabs[Random.Range(0, trackPrefabs.Count)],
                currentEndPoint.position,
                Quaternion.identity
            );

            // Find the new endpoint from the spawned track
            currentEndPoint = newTrack.transform.Find("EndPoint");

            // Safety check in case the EndPoint is missing
            if (currentEndPoint == null)
            {
                Debug.LogError("No EndPoint found on the spawned track!");
                break;
            }
        }
    }
}