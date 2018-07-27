using UnityEngine;
using SEASON = EnumCollection.SEASON;

public class DayCounter : MonoBehaviour {
    public int remainDays;   //D-Counter
    public int pastDays;
    public int dayPerSeason;    //[One] Season == [dayPerSeason] Day
    public SEASON currentSeason;
    public GameObject menuDB;

    private const int DAY_OF_END = 365; //Maximum Day
    private bool dailyInit;


	void Start () {
        //Synchronization
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
        remainDays = DAY_OF_END - pastDays;
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
