using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookPlayerSelectButtonUI : MonoBehaviour
{
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* Kitchen UI > CookUI > PageUI + PageButton > OrderPanelUI > DetailPanelUI > [ SelectPlayerPanelUI ] */

    //Child(0~3) : SelectButton
    private GameObject[] players;
    private GameObject cookingStand;
    private GameObject selectedPlayer;
    private GameObject kitchenField;

    private ColorBlock selectedColorBlock;
    private ColorBlock unselectedColorBlock;

    void Start()
    {
        players = Camera.main.GetComponent<EventListener>().playableCharacterObjects;
        cookingStand = GameObject.Find("CookingStand");
        kitchenField = GameObject.Find("Field_Kitchen");
        selectedColorBlock = GetComponentInChildren<Button>().colors;
        unselectedColorBlock = GetComponentInChildren<Button>().colors;
        selectedColorBlock.normalColor = new Color32(255, 255, 255, 255);
        unselectedColorBlock.normalColor = new Color32(128, 128, 128, 128);
    }

    void Update()
    {
        //주방에 있는 플레이어의 버튼만 활성화
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<MyCharacterMove>().currentField == kitchenField)
            {
                transform.GetChild(i).GetComponent<Button>().interactable = true;
            }
            else
            {
                transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }
    }

    public void SelectThisPlayer(GameObject _selectButton)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject == _selectButton)
            {
                transform.GetChild(i).GetComponent<Button>().colors = selectedColorBlock;
                selectedPlayer = players[i];
            }
            else
            {
                transform.GetChild(i).GetComponent<Button>().colors = unselectedColorBlock;
            }
        }
    }

    public void SubmitSelectPlayer()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Button>().colors = selectedColorBlock;
        }
            if (selectedPlayer != null)
        {
            int state;
            state = cookingStand.GetComponentInChildren<CookingStand>().
                OrderCookingToPlayer(selectedPlayer, GetComponentInParent<DetailsPanelUI>().menu);

            if (state == -1)  //요리 명령 전달 실패 시 (재료 부족)
            {
                GetComponentInParent<CookingUI>().ViewFailCookUI("재료가 부족합니다.");
            }
            else if (state == 0)
            {
                GetComponentInParent<CookingUI>().ViewFailCookUI("조리대가 이미 사용중입니다.");
            }
            else if (state == 1)
            {   //요리 명령을 내린 플레이어가 클릭에 의해 선택된 플레이어라면 선택을 취소함.
                Camera.main.GetComponent<EventListener>().ResetSelectPlayer();
                selectedPlayer.GetComponentInChildren<PlayerCharacterAnimation>().isWorking = true;
            }
            selectedPlayer = null;
        }
    }

}
