using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderLayer : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.parent.position.y * 10) * -1;
    }


}
