using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionOpenAndClose : MonoBehaviour
{
    public Vector2 activePos;
    public float slideSpeed = 10.0f;
    private Vector2 originPos;
    private bool isOpen;

    // Use this for initialization
    void Start()
    {
        originPos = transform.position;
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenOrCloseOptionUI()
    {
        if (!isOpen)
        {
            isOpen = true;
            iTween.MoveTo(gameObject, iTween.Hash("x", activePos.x, "speed", slideSpeed,"easetype", iTween.EaseType.easeInOutQuad));
        }
        else
        {
            isOpen = false;
            iTween.MoveTo(gameObject, iTween.Hash("x", originPos.x, "speed", slideSpeed, "easetype", iTween.EaseType.easeInOutQuad));
        }
    }
}
