using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageButtonUI : MonoBehaviour
{
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* Kitchen UI > CookUI > PageUI + [ PageButton ] > OrderPanelUI > DetailPanelUI */

    public GameObject myPage;

    public void ActiveMyPageOnClick() { GetComponentInParent<CookingUI>().SetCurrentPage(myPage); }
}
