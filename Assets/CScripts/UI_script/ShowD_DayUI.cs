using UnityEngine;
using UnityEngine.UI;

public class ShowD_DayUI : MonoBehaviour {
    GameObject dayCounter;

    void Start()
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
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
