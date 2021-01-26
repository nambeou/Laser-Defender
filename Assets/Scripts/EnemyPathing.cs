using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    int currentWaypointIndex = 0;
    List<Transform> waypoints;
    // Start is called before the first frame update
    void Start()
    {
        this.waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[currentWaypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig wave) {
        this.waveConfig = wave;
    }

    private void Move () {
        float additionalFactor = 1 + (float)(FindObjectOfType<Level>().GetCurrentLevel()*0.05);
        if (currentWaypointIndex <= waypoints.Count - 1) {
            var targetPos = waypoints[currentWaypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetEnemyMoveSpeed()* additionalFactor * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, movementThisFrame);
            if (transform.position.Equals(targetPos)) {
                currentWaypointIndex ++ ;
            }
        } else {
            Destroy(gameObject);
        }
    }
}
