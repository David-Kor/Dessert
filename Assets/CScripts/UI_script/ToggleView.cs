using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleView : MonoBehaviour {
    public Camera hallCamera;
    public Camera kitchenCamera;
    private GameObject hallMasterObject;
    private GameObject kitchenMasterObject;

    void Start()
    {
        hallMasterObject = GameObject.FindGameObjectWithTag("Hall");
        kitchenMasterObject = GameObject.FindGameObjectWithTag("Kitchen");
    }

    public void ToggleViewHallToKitchen()
    {
        int i;
        //카메라 전환
        kitchenCamera.enabled = true;
        kitchenCamera.depth = -1;
        kitchenCamera.GetComponent<EventListener>().enabled = true;
        hallCamera.enabled = false;
        hallCamera.depth = -2;
        hallCamera.GetComponent<EventListener>().enabled = false;
        //플레이어 선택 해제
        kitchenCamera.GetComponent<EventListener>().ResetSelectPlayer();
        hallCamera.GetComponent<EventListener>().ResetSelectPlayer();
        //플레이어들의 현재 필드를 체크하여 갱신
        for (i = 0; i < Camera.main.GetComponent<EventListener>().playableCharacterObjects.Length; i++)
        {
            Camera.main.GetComponent<EventListener>().playableCharacterObjects[i].GetComponent<MyCharacterMove>().CheckField();
        }
        //Hall의 모든 스프라이트 해제(리소스 최소화)
        SpriteRenderer[] spriteInHall = hallMasterObject.GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer[] spriteInKitchen = kitchenMasterObject.GetComponentsInChildren<SpriteRenderer>();

        for (i = 0; i < spriteInHall.Length; i++)
        {
            if (spriteInHall[i].gameObject.CompareTag("Marker")) { continue; }
            spriteInHall[i].enabled = false;
        }
        for (i = 0; i < spriteInKitchen.Length; i++)
        {
            if (spriteInKitchen[i].gameObject.CompareTag("Marker")) { continue; }
            spriteInKitchen[i].enabled = true;
        }

    }

    public void ToggleViewKitchenToHall()
    {
        int i;
        //카메라 전환
        hallCamera.enabled = true;
        hallCamera.depth = -1;
        hallCamera.GetComponent<EventListener>().enabled = true;
        kitchenCamera.enabled = false;
        kitchenCamera.depth = -2;
        kitchenCamera.GetComponent<EventListener>().enabled = false;
        //플레이어 선택 해제
        kitchenCamera.GetComponent<EventListener>().ResetSelectPlayer();
        hallCamera.GetComponent<EventListener>().ResetSelectPlayer();
        //플레이어들의 현재 필드를 체크하여 갱신
        for (i = 0; i < Camera.main.GetComponent<EventListener>().playableCharacterObjects.Length; i++)
        {
            Camera.main.GetComponent<EventListener>().playableCharacterObjects[i].GetComponent<MyCharacterMove>().CheckField();
        }
        //Kitchen의 모든 스프라이트 해제(리소스 최소화)
        SpriteRenderer[] spriteInHall = hallMasterObject.GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer[] spriteInKitchen = kitchenMasterObject.GetComponentsInChildren<SpriteRenderer>();

        for (i = 0; i < spriteInHall.Length; i++)
        {
            if (spriteInHall[i].gameObject.CompareTag("Marker")) { continue; }
            spriteInHall[i].enabled = true;
        }
        for (i = 0; i < spriteInKitchen.Length; i++)
        {
            if (spriteInKitchen[i].gameObject.CompareTag("Marker")) { continue; }
            spriteInKitchen[i].enabled = false;
        }

    }

}
