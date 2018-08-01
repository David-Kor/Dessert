using UnityEngine;
using TITLE = EnumCollection.TITLE;

public class ObjectEventManager : MonoBehaviour {
    public bool doEvent;    //이벤트 플래그 변수
    public bool doCancel;   //클릭에 의한 선택 취소
    public GameObject currentPlayer;

    void Start()
    {
        doEvent = false;
        doCancel = false;
    }

    public void OnClickThisObject(GameObject _player)
    {   //오브젝트 클릭시 플래그 발생
        currentPlayer = _player;
        doEvent = true;
    }

    public void CancelSelectObject() { doCancel = true; }

    public static void CancelAllSelectObject()  //모든 오브젝트의 선택 해제
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");
        ObjectEventManager eventManager = null;

        for (int i = 0; i < objects.Length; i++)
        {
            eventManager = objects[i].GetComponentInChildren<ObjectEventManager>();
            if (eventManager != null)
            {
                eventManager.CancelSelectObject();
            }
        }

        GameObject.Find("KitchenUI").GetComponent<KitchenUI>().ChangeViewUI(TITLE.NONE);

    }

    public static void CancelAllSelectWithOutObject(GameObject obj)   //지정 오브젝트를 제외한 모든 오브젝트 선택 해제
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");
        ObjectEventManager eventManager = null;

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] == obj) { continue; }    //예외지정 오브젝트는 스킵함

            eventManager = objects[i].GetComponentInChildren<ObjectEventManager>();
            if (eventManager != null)
            {
                eventManager.CancelSelectObject();
            }
        }

        GameObject.Find("KitchenUI").GetComponent<KitchenUI>().ChangeViewUI(TITLE.NONE);

    }

}
