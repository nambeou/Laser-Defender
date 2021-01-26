using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        if (FindObjectOfType<Player>() != null) {
            GetComponent<TextMeshProUGUI>().text = FindObjectOfType<Player>().GetHealth().ToString();
        }
    }

}
