using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedObject : MonoBehaviour {
    private Order myOrder;

	// Use this for initialization
	void Start () {
        myOrder = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetOrder(Order _order)
    {
        myOrder = _order;
    }

    public Order GetOrder() { return myOrder; }
}
