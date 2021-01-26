using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    [SerializeField] int point = 0;
    Level level;
    private void Awake()
    {
        SetUpSingleTon();
        this.level = FindObjectOfType<Level>();
    }

    private void SetUpSingleTon()
    {
        if(FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddScore(int score) {
        this.point += score*(1 + FindObjectOfType<Level>().GetCurrentLevel()/10);
    }

    public int GetPoint() {
        return this.point;
    }

    public void ResetGameSession() {
        Destroy(gameObject);
    }
}
