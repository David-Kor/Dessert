using UnityEngine;

public class Inventory
{
    public char ingredient;    //재료 기호 -> NULL값('\0')이면 비어있음
    public int remainingPeriod;   //유통기한까지 남은 시간(단위 : 일)
    public int count;  //남은 개수
    public int horIndex;
    public int verIndex;
    public Sprite image;
    public const int MAX_COUNT = 99;

    public Inventory()
    {
        ingredient = '\0';
        remainingPeriod = 0;
        count = 0;
    }
    public Inventory(char _ingredient, int _remainingPeriod, int _count, int _horIndex, int _verIndex)
    {
        ingredient = _ingredient;
        remainingPeriod = _remainingPeriod;
        count = _count;
        horIndex = _horIndex;
        verIndex = _verIndex;
        image = GameObject.Find("MenuDataBase").GetComponent<MenuData>().getIngrImage[ingredient];
    }

    public void UpdateImageByIngr()
    {
        image = GameObject.Find("MenuDataBase").GetComponent<MenuData>().getIngrImage[ingredient];
    }
}
