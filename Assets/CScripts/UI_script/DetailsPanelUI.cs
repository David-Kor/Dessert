using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DetailsPanelUI : MonoBehaviour
{
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* Kitchen UI > CookUI > PageUI + PageButton > OrderPanelUI > [ DetailPanelUI ] > SelectPlayerPanelUI */

    //Child(0) : The Required Time Text Panel
    //Child(1~7) : The Required Ingredients Text Panel
    //Child(8) : Select Player Button
    //Child(9) : Commit

    public MenuData.Menu menu;

    public void DetailsPanelWrite(int _mid)     //DetailPanelUI에서 출력할 내용 설정 (Menu 클래스 정보)
    {
        menu = MenuData.ConvertMenuIDToMenu(_mid);

        transform.GetChild(0).GetComponentInChildren<Text>().text = menu.time.ToString() + "s";

        for (int i = 0; i < menu.ingredients.Count; i++)    //ingredients(재료)는 최대 7개 -> i = 0~6
        {
            transform.GetChild(i + 1).GetComponentInChildren<Text>().text = MenuData.ConvertIngredientSignToName(menu.ingredients[i].sign)    //메뉴 이름
                + " x " + menu.ingredients[i].require.ToString();   //필요 개수
        }
    }

    public void ResetDetailsPanelText()
    {
        for (int i = 0; i < 8; i++)
        {
            transform.GetChild(i).GetComponentInChildren<Text>().text = "";
        }
    }

}
