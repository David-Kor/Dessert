using UnityEngine;

public class SpriteOrderLayer : MonoBehaviour
{
    private Vector2 prePosition;

    void Start()
    {
        prePosition = transform.position;
    }

    void Update()
    {
        //위치가 바뀔 때마다 sortingOrder값을 변경 (y에 반비례)
        if (prePosition != (Vector2)transform.position) { GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.parent.position.y * 10) * -1; }
    }


}
