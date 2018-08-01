using System.Collections.Generic;
using UnityEngine;

public class StorageUI : MonoBehaviour
{
    public GameObject storage;
    public GameObject refrigeratorPageUI;
    public List<GameObject> pages;
    public int numberOfPages = 3;

    void Start()
    {
        pages = new List<GameObject>();

        for (int i = 0; i < numberOfPages; i++)
        {
            pages.Add(Instantiate(refrigeratorPageUI, transform));
        }

    }

    public void SetViewRefrPageUI(int _index)   //현재 보이는 페이지를 변경
    {
        for (int i = 0; i < numberOfPages; i++)
        {
            if (_index == i) { pages[i].SetActive(true); }
            else { pages[i].SetActive(false); }
        }

        GetComponentInChildren<RefrPageButtonUI>().currentIndex = _index;   //버튼UI의 currenIndex변경
        GetComponentInChildren<RefrPageButtonUI>().CheckIndexRange();   //currenIndex 범위 체크 (버튼 비활성화 조건 확인)

        //해당 냉장고의 마커가 활성화 되어있지 않으면
        if (storage.transform.GetChild(_index).GetChild(1).GetComponent<SpriteRenderer>().enabled == false)
        {
            storage.transform.GetChild(_index).GetComponentInChildren<ObjectEventManager>().doEvent = true;
        }
    }

}
