using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
    public int sizeOfFieldWidth;
    public int sizeOfFieldHeight;
    public GameObject originOfChild;
    public string fieldName;
    public FieldNavigation navigator;
    private GameObject[,] childGround;
	
	void Start () {
        //childGround 생성
        int i, j;
        Vector2 vec = transform.position;
        childGround = new GameObject[sizeOfFieldWidth, sizeOfFieldHeight];
        navigator = gameObject.AddComponent<FieldNavigation>();
        navigator.InitFieldNavigation(this);

        for (i = 0; i < sizeOfFieldWidth; i++)
        {

            for (j = 0; j < sizeOfFieldHeight; j++)
            {
                childGround[i, j] = Instantiate(originOfChild, vec, this.transform.rotation, this.gameObject.transform);
                childGround[i, j].name = fieldName;
                childGround[i, j].GetComponent<GroundUnit>().setIndex(i, j);
                navigator.AddNode(childGround[i, j].AddComponent<GroundNode>(), i, j);
                vec.y++;
            }
            vec.y = transform.position.y;
            vec.x++;

        }

        AccessPositionOfObject[] init = GameObject.FindGameObjectWithTag("Hall").GetComponentsInChildren<AccessPositionOfObject>();
        for (i = 0; i < init.Length; i++)
        {
            init[i].InitAccessGround();
        }

    }

    // Update is called once per frame
    void Update() {

    }

    public GameObject[,] GetGrounds() { return childGround; }

}
