using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterAnimation : MonoBehaviour
{
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite sideSprite;
    public Sprite takeOrderSprite;
    public Sprite[] scrubSprite;

    public bool isServing;
    public bool isScrubbing;
    public bool isTakeOrder;
    public bool isWorking;
    public float animationFramePerSec;

    private Vector2 scrubbingLocalPos;
    private Vector2 noScrubbingLocalPos;
    private Vector2 direct;
    private Vector2 prePosition;
    private float timer;
    private int currentScrubSprite;
    private MyCharacterMove characterMove;

    // Use this for initialization
    void Start()
    {
        prePosition = transform.position;
        characterMove = GetComponentInParent<MyCharacterMove>();
        currentScrubSprite = 0;
        timer = 0f;
        scrubbingLocalPos = Vector2.up * 0.3f;
        noScrubbingLocalPos = Vector2.up * 0.75f;
        isTakeOrder = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isScrubbing)
        {   //테이블 청소 모션의 프레임 카운터
            timer += Time.deltaTime * animationFramePerSec;
        }
        if (characterMove != null)
        {
            if (characterMove.GetIsMoving())
            {
                if (isScrubbing) { isScrubbing = false; }
                if (isTakeOrder) { isTakeOrder = false; }
                MoveAnimation();
            }
            else
            {
                StopAnimation();
            }
        }

        if (isScrubbing) { transform.localPosition = scrubbingLocalPos; }
        else { transform.localPosition = noScrubbingLocalPos; }
    }

    void MoveAnimation()
    {   //이동 모션
        SetChildActiveServing(isServing);
        SetChildActiveScrubbing(isScrubbing);

        if (prePosition != (Vector2)transform.position)
        {

            if (Mathf.Abs(prePosition.x - transform.position.x) >= Mathf.Abs(prePosition.y - transform.position.y))
            {   //가로축으로 더 많이 이동할 시
                GetComponent<SpriteRenderer>().sprite = sideSprite;

                if (prePosition.x < transform.position.x)
                {   //오른쪽 이동
                    transform.localScale = Vector2.left + Vector2.up;    //좌우 반전
                    direct = Vector2.right;
                }
                else
                {   //왼쪽 이동
                    transform.localScale = Vector2.right + Vector2.up;   //원상복귀
                    direct = Vector2.left;
                }

            }
            else
            {   //세로축으로 더 많이 이동할 시
                if (prePosition.y < transform.position.y)
                {   //위로 이동
                    GetComponent<SpriteRenderer>().sprite = backSprite;
                    transform.localScale = Vector2.right + Vector2.up;   //원상복귀
                    direct = Vector2.up;
                }
                else
                {   //아래 이동
                    GetComponent<SpriteRenderer>().sprite = frontSprite;
                    transform.localScale = Vector2.right + Vector2.up;   //원상복귀
                    direct = Vector2.down;
                }
            }
        }

        if (isServing) { ServingAnimation(); }

        prePosition = transform.position;

    }

    void StopAnimation()
    {   //정지 모션
        SetChildActiveServing(isServing);
        SetChildActiveScrubbing(isScrubbing);

        direct = Vector2.down;
        transform.localScale = Vector2.one;   //원상복귀

        //서빙
        if (isServing) { ServingAnimation(); }
        //테이블 청소
        if (isScrubbing) { ScrubbingAnimation(); }

        //주문 받기
        if (isTakeOrder) { GetComponent<SpriteRenderer>().sprite = takeOrderSprite; }
        //작업 진행(주방)
        else if (isWorking) { GetComponent<SpriteRenderer>().sprite = backSprite; }
        //평상시
        else { GetComponent<SpriteRenderer>().sprite = frontSprite; }

    }

    void ServingAnimation()
    {
        //바라보는 방향에 따라 위치를 변경
        if (direct == Vector2.down)
        {   /*hand: 0.3, -0.6
              dish: 0.3, -0.5
              */
            transform.GetChild(0).localPosition = Vector2.right * 0.3f + Vector2.down * 0.6f;
            transform.GetChild(1).localPosition = Vector2.right * 0.3f + Vector2.down * 0.5f;
        }
        else if (direct == Vector2.left)
        {   /*hand: -0.35, -0.6
              dish: -0.35, -0.5
              */
            transform.GetChild(0).localPosition = Vector2.left * 0.35f + Vector2.down * 0.6f;
            transform.GetChild(1).localPosition = Vector2.left * 0.35f + Vector2.down * 0.5f;
        }
        else if (direct == Vector2.right)
        {
            transform.GetChild(0).localPosition = Vector2.left * 0.35f + Vector2.down * 0.6f;
            transform.GetChild(1).localPosition = Vector2.left * 0.35f + Vector2.down * 0.5f;
        }
        else
        {

            transform.GetChild(0).localPosition = Vector2.right * 0.3f + Vector2.down * 0.6f;
            transform.GetChild(1).localPosition = Vector2.right * 0.3f + Vector2.down * 0.5f;
        }

        for (int i = 0; i < 2; i++)
        {
            if (direct == Vector2.up || direct == Vector2.right) { transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + i - 2; }
            if (direct == Vector2.down || direct == Vector2.left) { transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + i+1; }
        }

    }

    void ScrubbingAnimation()
    {   //테이블 청소
        if (currentScrubSprite >= scrubSprite.Length) { currentScrubSprite = 0; }

        if (timer >= 1.0f)
        {
            transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = scrubSprite[currentScrubSprite++];
            timer = 0f;
        }
    }

    void SetChildActiveServing(bool _setActive)
    {
        transform.GetChild(0).gameObject.SetActive(_setActive);
        transform.GetChild(1).gameObject.SetActive(_setActive);
    }

    void SetChildActiveScrubbing(bool _setActive)
    {
        transform.GetChild(2).gameObject.SetActive(_setActive);
    }
}
