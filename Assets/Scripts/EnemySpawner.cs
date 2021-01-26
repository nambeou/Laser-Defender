using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    int waveConfigIndex = 0;
    [SerializeField] bool looping = false;
    Level level;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAllWaves());
        level = FindObjectOfType<Level>();
    }

    private IEnumerator SpawnAllWaves() {
        do {
            for (int index = waveConfigIndex; index < waveConfigs.Count; index ++) {
                yield return StartCoroutine(SpawnEnemiesInWave(waveConfigs[index]));
            }
            level.LevelUp();
        } while (looping);
    }

    private IEnumerator SpawnEnemiesInWave(WaveConfig wave) {
        for (int index = 0; index < wave.GetNumberOfEnemies(); index ++) {
            GameObject enemy = Instantiate(
                wave.GetEnemyPrefab(),
                wave.GetWaypoints()[0].transform.position,
                Quaternion.identity
            );
            enemy.GetComponent<EnemyPathing>().SetWaveConfig(wave);
            yield return new WaitForSeconds(wave.GetTimeBetweenSpawns());
        }
    }


}
