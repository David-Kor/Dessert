using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayLightUI : MonoBehaviour {
    public float secPerDay;   //minutePerDay의 값만큼 하루의 시간이 결정
    public float moveHeight;
    private Vector3 highest;

    /*Lowest Y Position : -1.12
      Highest Y Position : 1.12*/
	void Start () {
        highest = new Vector3(transform.position.x, transform.position.y + moveHeight, -10);
    }
	
	
	void Update () {
        if (transform.position.y <= highest.y)
        {
            transform.position = transform.position + (Vector3.up * 2 * Time.deltaTime) / secPerDay;
        }
	}
}
