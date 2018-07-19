using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayLightUI : MonoBehaviour {
    public float minutePerDay;   //minutePerDay의 값만큼 하루의 시간이 결정

    private Vector3 lowest;
    private Vector3 highest;

    /*Lowest Y Position : -1.12
      Highest Y Position : 1.12*/
	void Start () {
        lowest = new Vector3(-45f, -1.12f, -10);
        highest = new Vector3(-45f, 1.12f, -10);
    }
	
	
	void Update () {
        if (transform.position.y <= highest.y)
        {
            transform.position = transform.position + (Vector3.up * 2 * Time.deltaTime) / minutePerDay / 60.0f;
        }
	}
}
