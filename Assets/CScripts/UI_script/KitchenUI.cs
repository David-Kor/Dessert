using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TITLE = EnumCollection.TITLE;

public class KitchenUI : MonoBehaviour
{
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* [ Kitchen UI ] > CookUI > PageUI + PageButton > OrderPanelUI > DetailPanelUI > SelectPlayerPanelUI */

    private GameObject titlePanel;
    private GameObject[] uiPanel;

    void Start()
    {
        titlePanel = transform.GetChild(0).gameObject;
        uiPanel = new GameObject[5];

        int index = (int)TITLE.COOK;
        uiPanel[index] = transform.GetChild(index + 1).gameObject;

        index = (int)TITLE.PROCURE;
        uiPanel[index] = transform.GetChild(index + 1).gameObject;

        index = (int)TITLE.STOCK;
        uiPanel[index] = transform.GetChild(index + 1).gameObject;

        index = (int)TITLE.WASH;
        uiPanel[index] = transform.GetChild(index + 1).gameObject;

        index = (int)TITLE.NONE;
        uiPanel[index] = null;
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
                titlePanel.GetComponentInChildren<Text>().text = "요리 하기";
                uiPanel[index].SetActive(true);
                break;
            case TITLE.PROCURE:
                index = (int)TITLE.PROCURE;
                titlePanel.GetComponentInChildren<Text>().text = "재료 조달";
                break;
            case TITLE.STOCK:
                index = (int)TITLE.STOCK;
                titlePanel.GetComponentInChildren<Text>().text = "재고 관리";
                break;
            case TITLE.WASH:
                index = (int)TITLE.WASH;
                titlePanel.GetComponentInChildren<Text>().text = "설거지";
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
