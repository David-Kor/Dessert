﻿using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public int orderID;
    public GameObject table;
    public List<int> menuIDList;
    public bool isComplete;    //UI에 적용 여부
}