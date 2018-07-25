using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterMove : MonoBehaviour
{
    //필드 전체
    public GameObject hallField;
    public GameObject kitchenField;
    public GameObject currentField;
    //이동 속도
    public float moveSpeed = 5.0f;
    //목적지 좌표
    protected Vector2 targetPosition;
    protected GameObject targetGround;
    //현재 좌표
    protected Vector2 thisPosition;
    //true면 움직이기 시작
    protected bool isMoving;
    //경로상의 노드들을 저장한 리스트, [0]번지는 목적지이다.
    protected List<GroundNode> path;
    //path를 순서대로 받아오는 인덱스 계수기
    protected int counter;

    void Start()
    {
        isMoving = false;
        thisPosition = this.transform.position;
        counter = -1;
    }



    void Update()
    {

        if (isMoving && path != null)
        {   //경로가 존재하며 이동 명령을 받은 경우

            if (counter >= 0)
            {   //목적지 도달까지

                thisPosition = transform.position;
                float dist = Mathf.Pow(path[counter].GetPosition().x - thisPosition.x, 2) + Mathf.Pow(path[counter].GetPosition().y - thisPosition.y, 2);

                if (dist > 0.01f)
                {   //이동
                    iTween.MoveTo(gameObject, iTween.Hash("x", path[counter].GetPosition().x, "y", path[counter].GetPosition().y, "speed", moveSpeed, "easetype", iTween.EaseType.linear));
                }
                else
                {   //한칸 이동 완료 시
                    counter--;
                }
            }
            else
            {   //목적지에 도달 했을 시
                isMoving = false;
                transform.position = targetGround.transform.position;
                path.Clear();
            }
        }
        if (path == null)
        {
            counter = -1;
            isMoving = false;
        }

    }



    public bool MoveThisGround(GameObject _targetGround)   //이동 명령을 할 때 호출됨
    {
        if (_targetGround == null || !_targetGround.CompareTag("Ground"))   //대상이 없거나 땅이 아니면 움직이지 않음
        {
            path.Clear();
            counter = -1;
            return false;
        }

        targetGround = _targetGround;

        //출발좌표, 도착좌표를 받음
        targetPosition.x = _targetGround.transform.position.x;
        targetPosition.y = _targetGround.transform.position.y;
        thisPosition = RayCastGround(transform.position).transform.position;

        FieldNavigation _navi = currentField.GetComponent<Field>().navigator;
        path = _navi.FindPath(RayCastGround(transform.position).GetComponent<GroundNode>(), _targetGround.GetComponent<GroundNode>());

        if (path != null)
        {
            counter = path.Count - 1;
            isMoving = true;
            return true;
        }
        return false;
    }

    protected GameObject RayCastGround(Vector2 _rayPos)     //레이캐스트로 해당 좌표에 있는 Ground 오브젝트 탐색
    {

        RaycastHit2D[] hit = Physics2D.RaycastAll(_rayPos, Vector2.zero);

        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.gameObject.tag == "Ground") { return hit[i].collider.gameObject; }
            }
        }

        return null;
    }

    public bool GetIsMoving() { return isMoving; }

    public GameObject GetTargetGround() { return targetGround; }

    public void CheckField()    //현재 필드(주방/홀)를 확인
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.zero);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject.name.Equals("Hall")) { currentField = hallField; }
            if (hit[i].collider.gameObject.name.Equals("Kitchen")) { currentField = kitchenField; }
        }
    }

}
