using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalTabBuutton : MonoBehaviour {

    public static GameObject currentSelect = null;

    private RectTransform parentTransform;
    private RectTransform myTransfrom;
    private Vector2 originSize;

    void Start()
    {
        parentTransform = transform.parent.GetComponent<RectTransform>();
        myTransfrom = GetComponent<RectTransform>();
        originSize = parentTransform.sizeDelta;
    }
    
    void Update()
    {
        if (currentSelect != gameObject
            && parentTransform.sizeDelta != originSize)
        {
            parentTransform.sizeDelta = originSize;
        }
    }

    public void SelectThisTab()
    {
        currentSelect = gameObject;
        parentTransform.sizeDelta = myTransfrom.sizeDelta;
        Debug.Log(parentTransform.sizeDelta);
        Debug.Log(transform.parent.GetComponent<RectTransform>().sizeDelta);
    }
}
