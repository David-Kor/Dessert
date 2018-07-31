using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookPageUI : MonoBehaviour
{
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* Kitchen UI > CookUI > [ PageUI ] + PageButton > OrderPanelUI > DetailPanelUI > SelectPlayerPanelUI */
    public bool CheckAllOrderPanelInactive()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)    //하나라도 활성화 되어있으면 false반환
            {
                return false;
            }
        }

        return true;
    }

}
