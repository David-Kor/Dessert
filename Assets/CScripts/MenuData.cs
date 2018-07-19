using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;


public class MenuData : MonoBehaviour
{
    public List<Dictionary<string, string>> recipes;
    public enum SEASON { SPRING, SUMMER, FALL, WINTER, EVERYDAY }
    public enum TOOL { A, B, C, D }    //SAMPLE
    public enum PLATE { DISH, CUP }
    public struct Ingredients
    {
        public string name;
        public int require;
    }
    public struct Menu
    {
        public int id;  //식별자
        public string name;     //메뉴명
        public int price;   //가격
        public int time;  //제작시간
        public PLATE plate; //담을 용기
        public SEASON season;   //판매 계절
        public List<TOOL> tool;   //사용 도구        
        public List<Ingredients> ingredients;   //필요한 재료(이름,재고, 유통기한)
        public bool enabled;    //메뉴 활성화 여부
    }

    public LinkedList<Menu> menuList;

    /*KEY_LIST : ID	이름	종류	계절	가격	제작 시간	설비	I1	Q1	I2	Q2	I3	Q3	I4	Q4	I5	Q5	I6	Q6	I7	Q7*/
    private string[] key;
    const int MAX_COUNT_INGREDIENT = 7;

    void Start()
    {
        ParsingRecipe();    //모든 레시피가 저장된 recipe.csv파일로부터 정보를 읽어들임
        menuList = new LinkedList<Menu>();
        Menu newMenu;
        for (int i = 0; i < recipes.Count; i++)
        {
            //읽어들인 정보로 새 메뉴 작성
            newMenu = new Menu
            {
                id = System.Convert.ToInt32(recipes[i]["ID"]),
                name = recipes[i]["이름"],
                price = System.Convert.ToInt32(recipes[i]["가격"]),
                time = System.Convert.ToInt32(recipes[i]["제작 시간"]),
                tool = new List<TOOL>(),
                ingredients = new List<Ingredients>(),
                enabled = true
            };

            switch (recipes[i]["종류"])
            {
                case "디쉬":
                    newMenu.plate = PLATE.DISH;
                    break;
                case "컵":
                    newMenu.plate = PLATE.CUP;
                    break;
            }

            switch (recipes[i]["계절"])
            {
                case "사계":
                    newMenu.season = SEASON.EVERYDAY;
                    break;
                case "봄":
                    newMenu.season = SEASON.SPRING;
                    break;
                case "여름":
                    newMenu.season = SEASON.SUMMER;
                    break;
                case "가을":
                    newMenu.season = SEASON.FALL;
                    break;
                case "겨울":
                    newMenu.season = SEASON.WINTER;
                    break;
            }

            for (int j = 1; j <= MAX_COUNT_INGREDIENT; j++)
            {   //필요한 재료 리스트 추가
                if (recipes[i]["I" + j.ToString()] != "null")
                {
                    newMenu.ingredients.Add(new Ingredients
                    {
                        name = recipes[i]["I" + j.ToString()],
                        require = System.Convert.ToInt32(recipes[i]["Q" + j.ToString()])
                    });
                }
            }

            for (int j = 0; j < recipes[i]["설비"].Length; j++)
            {   //필요한 설비 리스트 추가
                switch (recipes[i]["설비"][j])
                {
                    case 'A':
                        newMenu.tool.Add(TOOL.A);
                        break;
                    case 'B':
                        newMenu.tool.Add(TOOL.B);
                        break;
                    case 'C':
                        newMenu.tool.Add(TOOL.C);
                        break;
                    case 'D':
                        newMenu.tool.Add(TOOL.D);
                        break;
                }
            }

            menuList.AddLast(newMenu);
        }   //메뉴 작성 완료
        
    }

    void ParsingRecipe()
    {
        FileStream f = new FileStream(Application.dataPath + "/Resources/recipe.csv", FileMode.Open);
        StreamReader sr = new StreamReader(f);
        string line = sr.ReadLine();
        string[] value;
        recipes = new List<Dictionary<string, string>>();

        key = line.Split(',');

        while (line != null)
        {
            line = sr.ReadLine();
            if (line == null) { break; }

            value = line.Split(',');
            recipes.Add(new Dictionary<string, string>());
            for (int i = 0; i < key.Length; i++)
            {
                recipes[recipes.Count - 1].Add(key[i], value[i]);
            }
        }

        sr.Close();
    }

    void WriteParsingInfo()
    {   //작성 완료된 메뉴들을 파일로 작성하여 테스트
        FileStream f = new FileStream(Application.dataPath + "/Resources/TEST_MENU_LIST.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(f, System.Text.Encoding.UTF8);
        LinkedListNode<Menu> node = menuList.First;
        for (int i = 0; i < key.Length; i++)
        {
            sw.Write(key[i] + "\t");
        }
        sw.WriteLine();
        /*KEY_LIST : ID	이름	종류	계절	가격	제작 시간	설비	I1	Q1	I2	Q2	I3	Q3	I4	Q4	I5	Q5	I6	Q6	I7	Q7*/

        for (int i = 0; i < menuList.Count; i++)
        {
            sw.Write(node.Value.id + "\t" + node.Value.name + "\t" + node.Value.plate + "\t" + node.Value.season + "\t" + node.Value.price + "\t" + node.Value.time + "\t");
            for (int j = 0; j < node.Value.tool.Count; j++)
            {
                sw.Write(node.Value.tool[j]);
            }

            for (int j = 0; j < node.Value.ingredients.Count; j++)
            {
                sw.Write("\t" + node.Value.ingredients[j].name + " : " + node.Value.ingredients[j].require);
            }
            sw.WriteLine();
            node = node.Next;
        }
        sw.Close();
    }
}
/*
 * ID,1,
   이름
   계절
   가격
   제작
   설비
   재료
   재료
   재료
   재료
   재료
   재료
   재료
*/