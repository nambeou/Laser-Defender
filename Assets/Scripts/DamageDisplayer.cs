using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageDisplayer : MonoBehaviour
{
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
            GetComponent<TextMeshProUGUI>().text = FindObjectOfType<Player>().GetComponent<DamageDealer>().GetDamage().ToString();
        }
    }
}
