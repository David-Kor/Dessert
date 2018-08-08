using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionOpenAndClose : MonoBehaviour
{
    //Child(0) : Button
    //Child(1) : Panel

    public Vector2 activePos;
    public float slideSpeed = 10.0f;
    public Sprite openButtonImage;
    public Sprite closeButtonImage;

    private Vector2 originPos;
    private bool isOpen;
    private bool onClick;

    void Start()
    {
        originPos = transform.position;
        activePos = Camera.main.ScreenToWorldPoint(activePos);
        isOpen = false;
        onClick = false;
    }

    void Update()
    {
        if (onClick)
        {
            if (isOpen
                && Mathf.Abs(activePos.x - transform.position.x) < 0.1f)
            {
                transform.GetChild(0).GetComponent<Image>().sprite = closeButtonImage;
                onClick = false;
            }
            else if (!isOpen
                && Mathf.Abs(originPos.x - transform.position.x) < 0.1f)
            {
                transform.GetChild(0).GetComponent<Image>().sprite = openButtonImage;
                onClick = false;
            }
        }
    }

    public void OpenOrCloseOptionUI()
    {
        onClick = true;

        if (!isOpen)
        {
            isOpen = true;
            iTween.MoveTo(gameObject, iTween.Hash("x", activePos.x, "speed", slideSpeed, "easetype", iTween.EaseType.easeInOutQuad));
        }
        else
        {
            isOpen = false;
            iTween.MoveTo(gameObject, iTween.Hash("x", originPos.x, "speed", slideSpeed, "easetype", iTween.EaseType.easeInOutQuad));
        }
    }
}
