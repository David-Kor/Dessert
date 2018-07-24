using UnityEngine;
using UnityEngine.UI;

public class ShowD_DayUI : MonoBehaviour
{
    GameObject dayCounter;
    bool update;
    void Start()
    {
        update = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (update)     //모든 Start함수들이 완료된 이후 한 번만 실행
        {
            dayCounter = GameObject.Find("Day Counter");
            if (dayCounter.GetComponent<DayCounter>().remainDays == 0)
            {
                GetComponent<Text>().text = "D - Day";
            }
            else
            {
                GetComponent<Text>().text = "D - " + dayCounter.GetComponent<DayCounter>().remainDays.ToString();
            }

            update = false;
        }

    }
}
