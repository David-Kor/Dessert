using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    private string[] invenKey;  //당장은 쓰이지 않는 더미(18-08-02 기준)
    private List<string[]> parser;

    private static List<ParsedInventory> parsedInventory;
    public struct ParsedInventory {
        public int refrIndex;
        public char ingredient;
        public int remainPeriod;
        public int count;
        public int horIndex;
        public int verIndex;
    }

    void Awake()
    {
        parser = new List<string[]>();
        parsedInventory = new List<ParsedInventory>();
        ParsingSaveFile();
        ParsingInventory();
    }

    /*문서 -> 리스트<문자열 배열> 파싱*/
    private void ParsingSaveFile()  //세이브 파일을 불러들여 모든 행을 ','로 나눈 문자열들의 배열로 파싱
    {
        FileStream f = new FileStream(Application.dataPath + "/Save/Save.csv", FileMode.Open);   //인벤토리 파일 불러오기
        StreamReader sr = new StreamReader(f);
        string line = "ReadLine";

        while (line != null)
        {
            line = sr.ReadLine();   //내용을 한 줄씩 읽어들임
            if (line == null) { break; }

            parser.Add(line.Split(','));
        }   //작성 완료

        sr.Close();
    }


    /* Title : Inventory
     * Key List : Refrigerator, Ingredient, RemainPeriod, Count, HorizonalIndex, VerticalIndex
     *           [냉장고 인덱스, 재료(기호), 남은 유통기한, 개수, 가로 인덱스(칸), 세로 인덱스(행)]
     */
    void ParsingInventory()
    {
        int startIndex = -1;    //Inventory정보의 시작 행(Key값)
        int endIndex = -1;     //Inventory정보의 마지막 행

        //Inventory의 정보를 갖고 있는 행의 범위를 탐색
        for (int i = 0; i < parser.Count; i++)
        {
            if (parser[i][0][0] == '#') //한 행의 첫 문자열의 시작이 '#'이면 실행
            {
                //startIndex가 지정되어있지 않으면
                if (startIndex < 0) { startIndex = i + 1; } //i행은 제목
                //startIndex가 지정되어 있으면
                else { endIndex = i - 1; }  //i행은 다른 정보의 제목
            }
        }
        //범위를 탐색할 수 없으면 취소 (세이브 파일이 손상됐거나 잘못된 형식)
        if (startIndex + endIndex < 0) { return; }

        ParsedInventory bufferInventory;
        string[] value;
        invenKey = parser[startIndex];

        for (int i = startIndex + 1; i <= endIndex; i++)
        {
            value = parser[i];
            //파싱된 정보에서 인벤토리 정보를 parsedInventory에 추가
            bufferInventory = new ParsedInventory
            {
                refrIndex = System.Convert.ToInt32(value[0]),
                ingredient = value[1][0],   //첫 문자
                remainPeriod = System.Convert.ToInt32(value[2]),
                count = System.Convert.ToInt32(value[3]),
                horIndex = System.Convert.ToInt32(value[4]),
                verIndex = System.Convert.ToInt32(value[5])
            };

            parsedInventory.Add(bufferInventory);
        }
    }

    public static List<Inventory> LoadInventory(int _refrIndex)
    {
        List<Inventory> rtnInventories = new List<Inventory>();

        for (int i = 0; i < parsedInventory.Count; i++)
        {
            //냉장고 인덱스 비교
            if (parsedInventory[i].refrIndex == _refrIndex)
            {
                rtnInventories.Add(new Inventory
                {
                    ingredient = parsedInventory[i].ingredient,
                    remainingPeriod = parsedInventory[i].remainPeriod,
                    count = parsedInventory[i].count,
                    horIndex = parsedInventory[i].horIndex,
                    verIndex = parsedInventory[i].verIndex
                });
            }
        }
        return rtnInventories;
    }
}
