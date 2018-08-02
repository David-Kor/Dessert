using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TITLE = EnumCollection.TITLE;

public class KitchenUI : MonoBehaviour
{
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* [ Kitchen UI ] > CookUI + StorageUI + DishingUI + BuyUI */

    private GameObject titlePanel;
    private GameObject[] uiPanel;

    void Start()
    {
        titlePanel = transform.GetChild(0).gameObject;
        uiPanel = new GameObject[5];
        uiPanel[(int)TITLE.COOK] = transform.GetChild((int)TITLE.COOK + 1).gameObject;
        uiPanel[(int)TITLE.BUY] = transform.GetChild((int)TITLE.BUY + 1).gameObject;
        uiPanel[(int)TITLE.STORAGE] = transform.GetChild((int)TITLE.STORAGE + 1).gameObject;
        uiPanel[(int)TITLE.DISHING] = transform.GetChild((int)TITLE.DISHING + 1).gameObject;
        uiPanel[(int)TITLE.NONE] = null;

        for (int i = 0; i < uiPanel.Length - 1; i++)
        {
            uiPanel[i].SetActive(true); //Start 또는 Awake 함수를 한 번씩 실행하여 초기화
            uiPanel[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeViewUI(TITLE _title)
    {
        int index = 0;
        switch (_title)
        {
            case TITLE.COOK:
                index = (int)TITLE.COOK;
                titlePanel.GetComponentInChildren<Text>().text = "Cooking";
                uiPanel[index].SetActive(true);
                break;
            case TITLE.BUY:
                index = (int)TITLE.BUY;
                titlePanel.GetComponentInChildren<Text>().text = "Ingr. Buy";
                break;
            case TITLE.STORAGE:
                index = (int)TITLE.STORAGE;
                titlePanel.GetComponentInChildren<Text>().text = "Refrigerator";
                uiPanel[index].SetActive(true);
                break;
            case TITLE.DISHING:
                index = (int)TITLE.DISHING;
                titlePanel.GetComponentInChildren<Text>().text = "Dishing";
                break;
            case TITLE.NONE:
                index = (int)TITLE.NONE;
                titlePanel.GetComponentInChildren<Text>().text = "";
                CookOrderPanelUI[] orderPanels = GetComponentsInChildren<CookOrderPanelUI>();
                for (int i = 0; i < orderPanels.Length; i++)
                {
                    orderPanels[i].ResetDetails();
                }
                break;
        }

        UIViewResetWithOut(uiPanel[index]);

    }

    void UIViewResetWithOut(GameObject _ui)
    {
        for (int i = 0; i < uiPanel.Length - 1; i++)
        {
            if (uiPanel[i] == _ui) { continue; }
            uiPanel[i].SetActive(false);
        }
    }

}
