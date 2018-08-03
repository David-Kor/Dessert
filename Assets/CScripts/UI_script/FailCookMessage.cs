using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailCookMessage : MonoBehaviour {
    public float speed = 1.0f;          //진동 속도
    public float amount = 1.0f;        //진폭
    public float timeToShake = 0.5f; //진동 시간
    public float timeToLive = 1.5f;    //출력 시간

    private Vector2 shakePos;
    private Vector2 originPos;
    private float timer;
	// Use this for initialization
	void Start () {
        timer = 0;
        originPos = transform.position;
        shakePos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        //진동
        if (timer < timeToShake)
        {
            shakePos.x = originPos.x + Mathf.Sin(Time.time * speed) * amount;
            transform.position = shakePos;
        }
        //원래 위치로
        else { transform.position = originPos; }

        if (timer >= timeToLive)
        {
            Destroy(gameObject);
        }
	}
}
