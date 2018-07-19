using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEventManager : MonoBehaviour {
    public bool doEvent;    //이벤트 플래그 변수
    public GameObject currentPlayer;

    void Start()
    {
        doEvent = false;
    }

    public void OnClickThisObject(GameObject _player)
    {   //클릭시 플래그 발생
        currentPlayer = _player;
        doEvent = true;
    }

}
