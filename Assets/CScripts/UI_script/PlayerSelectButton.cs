using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectButton : MonoBehaviour {
    public void OnClickThisButton()
    {
        GetComponentInParent<CookPlayerSelectButtonUI>().SelectThisPlayer(gameObject);
    }
}
