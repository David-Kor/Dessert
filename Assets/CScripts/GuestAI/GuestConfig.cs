using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestConfig {
    public GameObject table;    //당장은 쓰이지 않음
    public List<char> favorite;

    public GuestConfig(GameObject _table)
    {
        table = _table;
        favorite = new List<char>();

        char sign;
        for (int i = 0; i < 3; i++)
        {   //임의의 선호하는 재료 추가

            sign = (char)Random.Range('d', 'o');    //랜덤 범위 : c, e~n (연속된 문자 d를 c대신 사용)
            if (sign == 'd') { sign = 'c'; }    //d를 c로 다시 변환

            for (int j = 0; j < favorite.Count; j++)
            {    //중복을 허용하지 않음
                if (sign == favorite[j])
                {
                    favorite.RemoveAt(j);
                    j--;
                    i--;
                }
            }

            favorite.Add(sign);

        }
    }
}
