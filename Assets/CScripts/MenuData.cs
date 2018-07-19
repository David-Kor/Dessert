using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}

public class MenuData : MonoBehaviour
{
    public enum SEASON { SPRING, SUMMER, FALL, WINTER }
    public enum TOOL { A, B, C }    //SAMPLE

    public struct Ingredients
    {
        public string name;
        public int stock;
        public int expirationDate;
    }

    public struct Menu
    {
        public string name;     //메뉴명
        public float time;  //제작시간
        public SEASON season;   //판매 계절
        public List<TOOL> tool;   //사용 도구        
        public List<Ingredients> ingredients;   //필요한 재료(이름,재고, 유통기한)
        public bool enabled;    //메뉴 활성화 여부
    }

    public LinkedList<Menu> menuList;

    public TextAsset recipeCSV;

    void Start()
    {
        List<Dictionary<string, object>> data = CSVReader.Read(recipeCSV.name);
        menuList = new LinkedList<Menu>();
        Menu newMenu = new Menu();
        {
            newMenu.name = "TMP";
            newMenu.time = 10.0f;
            newMenu.season = SEASON.SPRING;
            newMenu.tool = new List<TOOL>();
            newMenu.ingredients = new List<Ingredients>();
            newMenu.enabled = true;
        }   //SAMPLE
        menuList.AddLast(newMenu);
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