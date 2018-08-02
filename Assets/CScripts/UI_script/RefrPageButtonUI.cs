using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefrPageButtonUI : MonoBehaviour {
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* Kitchen UI > StorageUI > RefrigeratorPageUI + [ RefrPageButtonUI ] > InventoryPanelUI > InventorySpaceUI > IngredientUI */

    //Child(0) : TEXT
    //Child(1) : Left Button
    //Child(2) : Right Button


    public int currentIndex = 0;
    private int maxIndex = 3;
    private GameObject textUI;
    private GameObject leftButton;
    private GameObject rightButton;

    void Awake()
    {
        textUI = transform.GetChild(0).gameObject;
        leftButton = transform.GetChild(1).gameObject;
        rightButton = transform.GetChild(2).gameObject;

        maxIndex = GetComponentInParent<StorageUI>().numberOfPages;
        currentIndex = 0;
        leftButton.GetComponent<Button>().interactable = false;  //초기 페이지는 1페이지이므로 왼쪽 버튼 비활성화
        textUI.GetComponent<Text>().text = (currentIndex + 1) + " / " + maxIndex;
    }

    //오른쪽 버튼 클릭시 오른쪽 페이지를 활성화시킴
    public void ShiftRightActivePageOnClick()
    {
        leftButton.GetComponent<Button>().interactable = true;
        GetComponentInParent<StorageUI>().SetViewRefrPageUI(currentIndex + 1);
    }

    //왼쪽 버튼 클릭시 왼쪽 페이지를 활성화시킴
    public void ShiftLeftActivePageOnClick()
    {
        rightButton.GetComponent<Button>().interactable = true;
        GetComponentInParent<StorageUI>().SetViewRefrPageUI(currentIndex - 1);
    }

    public void SetCurrentPageIndex(int _index) { currentIndex = _index; }

    public void CheckIndexRange()   //currentIndex의 범위를 확인하여 버튼 활성화 여부 결정
    {
        //첫 페이지 도달 시 왼쪽 버튼을 비활성화
        if (currentIndex + 1 <= 1)
        {
            leftButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            leftButton.GetComponent<Button>().interactable = true;
        }

        //마지막 페이지 도달 시 오른쪽 버튼을 비활성화
        if (currentIndex + 1 >= maxIndex)
        {
            rightButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            rightButton.GetComponent<Button>().interactable = true;
        }

        textUI.GetComponent<Text>().text = (currentIndex + 1) + " / " + maxIndex;
    }

}
