using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject {
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] float enemyMoveSpeed = 3f;
    [SerializeField] int numberOfEnemies = 5;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }
    public GameObject GetPathPrefab() { return pathPrefab; }
    public List<Transform> GetWaypoints() { 
        List<Transform> res = new List<Transform>();
        foreach (Transform tf in pathPrefab.transform) {
            res.Add(tf);
        }
        return res;    
    }
    public float GetTimeBetweenSpawns() { return timeBetweenSpawns; }
    public float GetSpawnRandomFactor() { return spawnRandomFactor; }
    public float GetEnemyMoveSpeed() { return enemyMoveSpeed; }
    public int GetNumberOfEnemies() { return numberOfEnemies; }

}
