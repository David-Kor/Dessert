﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    public GameObject[] playableCharacterObjects; //이하 플레이어 오브젝트라 명함

    private Vector2 clickPos;
    private GameObject selectedPlayer;
    private GameObject selectedGround;
    private GameObject selectedOtherObject;
    private bool isClickPlayer;
    private bool isClickUI;

    //초기화
    void Start()
    {
        selectedPlayer = null;
        selectedGround = null;
        selectedOtherObject = null;
        isClickPlayer = false;
        isClickUI = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectedGround = null;
            selectedOtherObject = null;
            clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //메인 카메라 화면 기준 좌표

            //플레이어 오브젝트 선택(클릭)
            CastRayOnMouseClick();

            if (!isClickUI) //UI를 클릭한 것이 아닌 경우
            {
                //플레이어 오브젝트의 이동
                if (selectedPlayer != null && selectedGround != null && !isClickPlayer)
                {
                    selectedPlayer.GetComponent<MyCharacterMove>().MoveThisGround(selectedGround);
                }

                if (selectedOtherObject != null)
                {   //플레이어를 제외한 종합 오브젝트 함수
                    selectedOtherObject.GetComponent<ObjectEventManager>().OnClickThisObject(selectedPlayer);
                }
            }

        }

        if (Input.GetButtonDown("Cancel") && selectedPlayer != null)
        {  //ESC키를 누르거나 화면이 전환되면 선택 해제
            isClickPlayer = false;
            selectedPlayer.transform.GetChild(1).gameObject.SetActive(false);
            selectedPlayer = null;
            selectedGround = null;
            selectedOtherObject = null;
        }

    }

    void CastRayOnMouseClick() // 유닛 히트처리 부분.  레이를 쏴서 처리합니다. 
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(clickPos, Vector2.zero, 0f);

        if (hit.Length > 0)
        { //히트되었다면 여기서 실행

            for (int i = 0; i < hit.Length; i++)
            {
                //플레이어가 선택된 경우
                if (hit[i].collider.gameObject.CompareTag("Player"))
                {
                    if (hit[i].transform.parent.gameObject != selectedPlayer) { isClickPlayer = true; }  //이전에 선택한 캐릭터와 다른 캐릭터를 클릭한 경우
                    else { isClickPlayer = false; }  //이전 선택한 캐릭터와 동일한 캐릭터를 클릭한 경우

                    selectedPlayer = hit[i].collider.gameObject.transform.parent.gameObject;
                    selectedPlayer.transform.GetChild(1).gameObject.SetActive(true);

                    for (int j = 0; j < playableCharacterObjects.Length; j++)
                    {
                        if (playableCharacterObjects[j] != selectedPlayer)
                        {
                            playableCharacterObjects[j].transform.GetChild(1).gameObject.SetActive(false);
                        }
                    }

                    //break;
                }
                else
                {
                    if (hit[i].collider.gameObject.CompareTag("SpriteInObject")) { selectedOtherObject = hit[i].collider.gameObject; }   //특정 오브젝트인 경우

                    if (hit[i].collider.gameObject.CompareTag("Ground"))
                    {
                        selectedGround = hit[i].collider.gameObject;
                        if (isClickPlayer)
                        {   //플레이어를 클릭한 이후의 클릭부터 이동 가능 -> 클릭하자마자 이동하는 것을 방지
                            isClickPlayer = false;
                            selectedGround = null;
                        }
                    }

                    if (hit[i].collider.CompareTag("UI")) { isClickUI = true; } //UI 클릭 시

                }

            }  //for문 끝

        }


    }   //hit처리 완료

    public void ResetSelectPlayer()
    {
        if (selectedPlayer != null) { selectedPlayer.transform.GetChild(1).gameObject.SetActive(false); }
        selectedPlayer = null;
        isClickPlayer = false;
        isClickUI = false;
    }

    public GameObject GetSelectedPlayer() { return selectedPlayer; }

}