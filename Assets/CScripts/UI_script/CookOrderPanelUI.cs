using UnityEngine.UI;
using UnityEngine;

public class CookOrderPanelUI : MonoBehaviour
{
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* Kitchen UI > CookUI > PageUI + PageButton > [ OrderPanelUI ] > DetailPanelUI */

    //Child(2) -> DetailPanel

    public void OrderPanelTextWrite(string _tid, string _name, int _mid)    //OrderPanelUI 및 DetailPanelUI에서 출력할 내용 설정
    {
        //OrderPanelUI에서 출력할 Order 객체 정보 작성
        transform.GetChild(0).GetComponentInChildren<Text>().text = _tid;
        transform.GetChild(1).GetComponentInChildren<Text>().text = _name;

        //DetailPanelUI에서 출력할 Menu 객체 정보 전달
        bool detailActive = transform.GetChild(2).gameObject.activeSelf;
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(2).GetComponentInChildren<DetailsPanelUI>().DetailsPanelWrite(_mid);
        transform.GetChild(2).gameObject.SetActive(detailActive);   //원래 비활성화 상태였으면 다시 비활성화시킴
    }

    public void ResetDetails()   //OrderPanel 자신이 가진 DetailPanel을 비활성화
    {
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public void ResetAllDetails()   //현재 페이지의 전체 DetailPanel을 비활성화
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetChild(2).gameObject.SetActive(false);
        }
    }

    public void ViewDetailsOnClick()    //OrderPanel 클릭 시 DetailPanel 활성화
    {
        bool active = !transform.GetChild(2).gameObject.activeSelf;
        ResetAllDetails();
        transform.GetChild(2).gameObject.SetActive(active);
    }

}
