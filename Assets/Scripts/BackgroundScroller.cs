using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float backgroundSCrollingSpeed = 0.5f;
    Material material;
    Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        offset = new Vector2(0, backgroundSCrollingSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        this.material.mainTextureOffset += offset * Time.deltaTime;
    }
}
