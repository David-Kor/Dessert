using UnityEngine;
using UnityEngine.UI;

public class TotalCashValue : MonoBehaviour
{
    private float prevTotal;
    // Use this for initialization
    void Start()
    {
        prevTotal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (prevTotal != PayAtCounter.totalIncome)
        {
            prevTotal = PayAtCounter.totalIncome;
            if (prevTotal < 1000)
            {
                GetComponent<Text>().text = prevTotal.ToString();
            }
            else
            {
                string total = string.Format("{0}", prevTotal.ToString("#,##"));
                GetComponent<Text>().text = total;
            }
        }
    }

}
