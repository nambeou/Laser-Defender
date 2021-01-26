using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScoreDisplayer : MonoBehaviour
{
    GameSession session;
    [SerializeField] TextMeshProUGUI scoreValue;

    // Start is called before the first frame update
    void Start()
    {
        session = FindObjectOfType<GameSession>();
        scoreValue.text = session.GetPoint().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreValue.text = session.GetPoint().ToString();
    }
}
