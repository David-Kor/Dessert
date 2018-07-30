using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookPlayerSelectButtonUI : MonoBehaviour
{
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* Kitchen UI > CookUI > PageUI + PageButton > OrderPanelUI > DetailPanelUI > [ SelectPlayerPanelUI ] */

    private GameObject[] players;
    private GameObject cookingStand;

    void Start()
    {
        players = Camera.main.GetComponent<EventListener>().playableCharacterObjects;
        cookingStand = GameObject.Find("CookingStand");
    }

    public void SelectThisPlayer(GameObject _selectButton)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject == _selectButton)
            {
                cookingStand.GetComponentInChildren<CookingStand>().OrderCookingToPlayer(players[i], GetComponentInParent<DetailsPanelUI>().menu);
            }
        }
    }

}
