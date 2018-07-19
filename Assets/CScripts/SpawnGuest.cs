using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGuest : MonoBehaviour {
    public GameObject originGuest;   //생성할 손님
    public GameObject tableGroup;   //테이블들을 묶은 오브젝트
    public GameObject field;

    public float spawnMinDelaySec;
    public float spawnMaxDelaySec;
    public bool doSpawn;

    private List<GameObject> emptyTables;
    private GameObject targetTable;
    private TableStateManager[] stateOfTables;

    private float timer;
    private float spawnDelay;

    private enum STATE { CLEAR = 0, USING, DIRTY };

    // Use this for initialization
    void Start () {
        stateOfTables = tableGroup.GetComponentsInChildren<TableStateManager>();
        emptyTables = new List<GameObject>();
        targetTable = null;
        doSpawn = false;
        timer = 0;
        spawnDelay = Random.RandomRange(spawnMinDelaySec, spawnMaxDelaySec + 1);
	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;
        if (timer >= spawnDelay)
        {   //지정한 Delay만큼의 간격으로 손님 생성
            timer = 0;
            doSpawn = true;
            spawnDelay = Random.RandomRange(spawnMinDelaySec, spawnMaxDelaySec + 1);
        }

        if (doSpawn)
        {
            int i, j;

            for (i = 0; i < stateOfTables.Length; i++)
            {   //빈 테이블들을 찾아서 목록을 저장
                if (stateOfTables[i].stateOfTable == (int)STATE.CLEAR) { emptyTables.Add(stateOfTables[i].gameObject); }
            }
            if (emptyTables.Count > 0)
            {   //빈 테이블 중에서 랜덤으로 선택
                targetTable = emptyTables[Random.RandomRange(0, emptyTables.Count)];
            }
            else
            {   //빈 테이블이 없으면 취소
                doSpawn = false;
                return;
            }


            targetTable.GetComponent<TableStateManager>().stateOfTable = (int)TableStateManager.STATE.RESERVED;   //대상 테이블 상태를 예약 됨으로 변경
            GameObject[] targetGrounds = targetTable.GetComponent<AccessPositionOfObject>().GetAllGround();
            
            if (targetGrounds != null)
            {
                GameObject[] newGuest = new GameObject[2];
                List<int> reserve = new List<int>();

                //손님 오브젝트 생성
                newGuest[0] = Instantiate(originGuest, (Vector2)transform.position+Vector2.up, transform.rotation,GameObject.FindGameObjectWithTag("Hall").transform);
                newGuest[1] = Instantiate(originGuest, (Vector2)transform.position+Vector2.down, transform.rotation, GameObject.FindGameObjectWithTag("Hall").transform);

                newGuest[0].GetComponent<GuestAction>().SetMyGroup(newGuest[1]);
                newGuest[1].GetComponent<GuestAction>().SetMyGroup(newGuest[0]);

                if (!Camera.main.name.Equals("Hall Camera"))
                {   //홀카메라가 메인카메라가 아니면 스프라이트를 렌더링하지 않음
                    newGuest[0].GetComponentInChildren<SpriteRenderer>().enabled = false;
                    newGuest[1].GetComponentInChildren<SpriteRenderer>().enabled = false;
                }

                for (i = 0; i < newGuest.Length; i++)
                {   //오브젝트 정보 초기화 및 이동 명령
                    newGuest[i].GetComponent<GuestAction>().SetFieldObject(field);
                    newGuest[i].GetComponent<GuestAction>().currentState = "Move";
                    newGuest[i].GetComponent<GuestAction>().SetMyTable(targetTable);

                    for (j = 0; j < targetGrounds.Length; j++)
                    {
                        if (targetGrounds[j].GetComponent<GroundNode>().IsPassable() && reserve.IndexOf(j) < 0)
                        {
                            newGuest[i].GetComponent<GuestAction>().MoveThisGround(targetGrounds[j]);
                            reserve.Add(j);
                            break;
                        }
                    }

                }

            }

            doSpawn = false;

        }

        emptyTables.Clear();

	}

}
