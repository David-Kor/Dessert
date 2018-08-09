using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTablePointer : MonoBehaviour
{
    public Vector2 targetTablePosition;

    private GameObject marker;
    private PlayerCharacterAnimation playerState;
    private SpriteRenderer pointerMarker;
    private bool preState;
    private Vector2 targeting;
    private float angle;

    void Start()
    {
        marker = transform.parent.GetChild(1).gameObject;
        playerState = transform.parent.GetComponentInChildren<PlayerCharacterAnimation>();
        pointerMarker = GetComponentInChildren<SpriteRenderer>();
        preState = false;
        pointerMarker.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //상태 변화시 1번만 실행
        if (preState != (playerState.isServing && marker.activeSelf))
        {
            preState = playerState.isServing && marker.activeSelf;
            //플레이어가 선택된 상태이고 서빙중인 경우에만 포인터 활성화
            if (preState) { pointerMarker.enabled = true; }
            else { pointerMarker.enabled = false; }
        }

        //조건을 만족할 시에 목표 테이블을 바라보게 함
        if (preState)
        {
            targeting.x = targetTablePosition.x - transform.position.x;
            targeting.y = targetTablePosition.y - transform.position.y;
            angle = Mathf.Rad2Deg * Mathf.Atan2(targeting.y, targeting.x) - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }
}
