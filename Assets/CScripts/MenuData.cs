using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SEASON = EnumCollection.SEASON;   //EnumCollection에 있는 enum형들을 통해 일관성 있게 사용 가능
using PLATE = EnumCollection.PLATE;
using TOOL = EnumCollection.TOOL;

public class MenuData : MonoBehaviour
{
    public List<Dictionary<string, string>> recipes;
    
    public class Ingredients
    {
        public char sign;
        public int require;
    }

    public class Menu
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

    public static List<Menu> menuList;

    public List<Menu> enableMenus;

    /*KEY_LIST : ID	이름	종류	계절	가격	제작 시간	설비	I1	Q1	I2	Q2	I3	Q3	I4	Q4	I5	Q5	I6	Q6	I7	Q7*/
    private string[] recipeKey;

    private static List<Dictionary<string, string>> ingredList;

    private const int MAX_COUNT_INGREDIENT = 7;

    void Start()
    {
        menuList = new List<Menu>();
        enableMenus = new List<Menu>();
        ingredList = new List<Dictionary<string, string>>();
        ParsingRecipe();    //모든 레시피가 저장된 recipe.csv파일로부터 정보를 읽어들임
        ParsingIngredient();    //모든 재료가 저장된 Init_Ingredient.csv에서 정보를 읽음
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
                        sign = recipes[i]["I" + j.ToString()].ToCharArray(0, 1)[0],  //Str[0] To Char
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

            menuList.Add(newMenu);
        }   //메뉴 작성 완료
        
    }

    void ParsingRecipe()
    {
        FileStream f = new FileStream(Application.dataPath + "/Resources/recipe.csv", FileMode.Open);   //레시피 파일 불러오기
        StreamReader sr = new StreamReader(f);
        string line = sr.ReadLine();    //한 줄 읽기
        string[] value;
        recipes = new List<Dictionary<string, string>>();

        recipeKey = line.Split(',');  //첫번째 줄이 Key들이므로 쉼표로 문자열을 구분하여 저장

        while (line != null)
        {
            line = sr.ReadLine();
            if (line == null) { break; }

            value = line.Split(',');
            recipes.Add(new Dictionary<string, string>());  //레시피 한 줄 추가(count+1)
            for (int i = 0; i < recipeKey.Length; i++)  //Key의 종류 수만큼 반복
            {
                recipes[recipes.Count - 1].Add(recipeKey[i], value[i]);
            }
        }

        sr.Close();
    }   //외부 레시피 파일 정보 불러오기

    void ParsingIngredient()
    {
        FileStream f = new FileStream(Application.dataPath + "/Resources/Init_Ingredient.csv", FileMode.Open);   //재료 파일 불러오기
        StreamReader sr = new StreamReader(f);
        string line = sr.ReadLine();    //한 줄 읽기
        string[] key;
        string[] value;

        key = line.Split(',');  //첫번째 줄이 Key들이므로 쉼표로 문자열을 구분하여 저장
        while (line != null)
        {
            line = sr.ReadLine();
            if (line == null) break;

            value = line.Split(',');
            ingredList.Add(new Dictionary<string, string>());
            for (int i = 0; i < key.Length; i++)
            {
                ingredList[ingredList.Count - 1].Add(key[i], value[i]);
            }
        }

    }   //외부 재료 파일 정보 불러오기

    void WriteParsingInfo()
    {   //작성 완료된 메뉴들을 파일로 작성하여 테스트
        FileStream f = new FileStream(Application.dataPath + "/Resources/TEST_MENU_LIST.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(f, System.Text.Encoding.UTF8);

        for (int i = 0; i < recipeKey.Length; i++)
        {
            sw.Write(recipeKey[i] + "\t");
        }
        sw.WriteLine();
        /*KEY_LIST : ID	이름	종류	계절	가격	제작 시간	설비	I1	Q1	I2	Q2	I3	Q3	I4	Q4	I5	Q5	I6	Q6	I7	Q7*/

        for (int i = 0; i < menuList.Count; i++)
        {
            sw.Write(menuList[i].id + "\t" + menuList[i].name + "\t" + menuList[i].plate + "\t" + menuList[i].season + "\t" + menuList[i].price + "\t" + menuList[i].time + "\t");
            for (int j = 0; j < menuList[i].tool.Count; j++)
            {
                sw.Write(menuList[i].tool[j]);
            }

            for (int j = 0; j < menuList[i].ingredients.Count; j++)
            {
                sw.Write("\t" + menuList[i].ingredients[j].sign + " : " + menuList[i].ingredients[j].require);
            }
            sw.WriteLine("\t" + menuList[i].enabled);
        }
        sw.Close();
    }   //가공이 완료된 메뉴 리스트 내보내기(테스트 및 확인 용)

    public void ActivationAllMenu(GameObject dayCounter)
    {   //(외부 클래스에서 호출되는 함수) 메뉴별 활성화 상태 갱신

        if (menuList.Count <= 0 || dayCounter == null) { return; }

        for(int i=0; i<menuList.Count; i++)
        {
            //현재 계절에 맞는 지 검사(제철 검사)
            if (menuList[i].season == dayCounter.GetComponent<DayCounter>().currentSeason
                || menuList[i].season == SEASON.EVERYDAY)
            {
                menuList[i].enabled = true;
                enableMenus.Add(menuList[i]);
            }
            else
            {
                menuList[i].enabled = false;
            }

        }

        WriteParsingInfo();
    }   //메뉴별 활성화 상태 갱신

    public List<int> GetIDsContainsIngredients(List<char> ingredList, int maxIndex)   //해당 재료를 포함하는 Menu를 탐색
    {
        List<int> menu = new List<int>();

        for (int i = 0; i < menuList.Count; i++)
        {
            //해당 재료를 포함하는 Menu를 탐색하여 리스트에 추가
            for (int j = 0; j < menuList[i].ingredients.Count; j++)
            {
                if (ingredList.Contains(menuList[i].ingredients[j].sign))
                {
                    menu.Add(menuList[i].id);
                    if (menu.Count <= maxIndex) { return menu; }
                    break;
                }
            }

        }

        return menu;
    }

    public static string ConvertMenuIDToName(int _id)   //메뉴의 ID를 이름으로 변환(존재하지 않는 ID면 null반환)
    {
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].id == _id)
            {
                return menuList[i].name;
            }
        }
        return null;
    }

    public static string ConvertIngredientSignToName(char _sign)    //재료의 기호(sign)를 이름으로 변환(존재하지 않으면 null반환)
    {
        for (int i = 0; i < ingredList.Count; i++)
        {
            if (ingredList[i]["Pref"].ToCharArray(0, 1)[0] == _sign)
            {
                return ingredList[i]["Name"];
            }
        }
        return null;
    }

    public Menu GetMenu(int _id)    //id로 메뉴 검색 후 반환, 실패 시 null 반환
    {
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].id == _id) { return menuList[i]; }
        }

        return null;
    }
}