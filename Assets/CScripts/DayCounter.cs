using UnityEngine;
using SEASON = EnumCollection.SEASON;

public class DayCounter : MonoBehaviour {
    public int remainDays;   //D-Counter
    public int pastDays;
    public int dayPerSeason;    //[One] Season = [dayPerSeason] Day
    public SEASON currentSeason;
    public GameObject menuDB;

    private float secPerDay;
    private const int DAY_OF_END = 365; //Maximum Day
    private bool dailyInit;


	void Start () {
        //Synchronization
        secPerDay = GetComponentInChildren<DayLightUI>().secPerDay;
        int season = pastDays / dayPerSeason;

        season %= 4;    //0~3 : 총 4계절
        switch ((SEASON)season)
        {
            case SEASON.SPRING:
                currentSeason = SEASON.SPRING;
                break;
            case SEASON.SUMMER:
                currentSeason = SEASON.SUMMER;
                break;
            case SEASON.FALL:
                currentSeason = SEASON.FALL;
                break;
            case SEASON.WINTER:
                currentSeason = SEASON.WINTER;
                break;
        }

        dailyInit = true;

	}
	
	// Update is called once per frame
	void Update () {
        if (dailyInit)
        {
            dailyInit = false;
            menuDB.GetComponent<MenuData>().ActivationAllMenu(gameObject);
        }
	}



}
