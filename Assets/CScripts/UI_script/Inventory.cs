using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public char ingredient;    //재료 기호 -> NULL값('\0')이면 비어있음
    public int remainingPeriod;   //유통기한까지 남은 시간(단위 : 일)
    public int count;  //남은 개수
    public int horIndex;
    public int verIndex;
}
