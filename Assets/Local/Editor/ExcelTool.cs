using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LocalCode.Common;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEditor;
using UnityEngine;

public static class ExcelTool
{
    private const string table_class_path = "Assets/Remote/HotUpdateCode/Game/GameTable/";
    private const string table_asset_path = "Assets/Remote/GameAssets/GameTable/{0}.txt";
    [MenuItem("HUITool/ExportExecel")]
    private static void ExportExcel()
    {
        var excel_path = Path.GetFullPath(Application.dataPath.Replace("Assets", "Excel"));
        var excel_list = Directory.GetFiles(excel_path, "*.xlsx");
        EditorUtility.ClearProgressBar();
        for (var i = 0; i < excel_list.Length; i++)
        {
            var excel = excel_list[i];
            EditorUtility.DisplayProgressBar("Excel转换", $"正在转换 {Path.GetFileNameWithoutExtension(excel)}",
                (float)i / excel_list.Length);
            ExcelToJson(excel);
        } 
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }
    
    private static void ExcelToJson(string excel_Path)
    {
        var table_path= Path.GetFullPath(Path.GetDirectoryName(table_asset_path));
        var file_name = Path.GetFileNameWithoutExtension(excel_Path);
        file_name = char.ToUpper(file_name[0]) + file_name.Substring(1);
        var json_path = Path.Combine(table_path, $"{file_name}.txt");
        var data = ReadExcel(excel_Path);
        var json = "";
        if (data.Count == 1)
        {
            json = JsonConvert.SerializeObject(data[0], Formatting.Indented);
        }
        else
        {
            json = JsonConvert.SerializeObject(data,Formatting.Indented);
        }
        if (!Directory.Exists(table_path))
        {
            Directory.CreateDirectory(table_path);
        }
        json = CryptoUtil.Encrypt(json);
        
        using (var writer = new StreamWriter(json_path, false))
        {
            writer.Write(json);
        }
    }
    
    private static List<Dictionary<string, object>> ReadExcel(string excel_path)
    {
        var result = new List<Dictionary<string, object>>();
        using (var stream = File.OpenRead(excel_path))
        {
            var workbook = new XSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(0);
            if (sheet.GetRow(0).GetCell(0).ToString() == "id")
            {
                result.AddRange(ReadCommonExcelToJsonAndClass(sheet,excel_path));
            }
            else
            {
                result.Add(ReadConfigExcelToJsonAndClass(sheet,excel_path));
            }
        }
        return result;
    }
    private static List<Dictionary<string,object>> ReadCommonExcelToJsonAndClass(ISheet sheet, string excel_path)
    {
        var result = new List<Dictionary<string, object>>();
        var headers = GetRowCells(sheet, 0);
        var types = GetRowCells(sheet, 1);
        ExportClass(excel_path,headers,types);
        for (var i = 2; i <= sheet.LastRowNum; i++)
        {
            var row = sheet.GetRow(i);
            if (row == null) continue;
            var dict = new Dictionary<string, object>();
            for (int j = 0; j < headers.Count; j++)
            {
                dict[headers[j]] = GetData(types[j], row.GetCell(j)?.ToString() ?? string.Empty);
            }
            result.Add(dict);
        }
        return result;
    }

    private static Dictionary<string, object> ReadConfigExcelToJsonAndClass(ISheet sheet, string excel_path)
    {
        var dictionary = new Dictionary<string, object>();
        var names = new List<string>();
        var types = new List<string>();
        for (int i = 0; i <= sheet.LastRowNum; i++)
        {
            var row = sheet.GetRow(i);
            var name = row.GetCell(0)?.ToString();
            names.Add(name);
            var type = row.GetCell(1)?.ToString();
            types.Add(type);
            var value = row.GetCell(2)?.ToString();
            dictionary[name] = GetData(type, value);
        }
        ExportClass(excel_path, names, types, true);

        return dictionary;
    }

    private static List<string> GetRowCells(ISheet sheet, int rowIndex)
    {
        var row = sheet.GetRow(rowIndex);
        if (row == null) return null;
        return row.Cells.Select(cell => cell.ToString()).ToList();
    }
    
    
  
    public static void ExportClass(string excel_path,List<string> name, List<string> types ,bool config = false)
    {   
        var table_name = Path.GetFileNameWithoutExtension(excel_path);
        var class_path = Path.Combine(Path.GetFullPath(table_class_path), $"{table_name}Value.cs");
        
        var count = Mathf.Min(name.Count, types.Count);
        var str = new StringBuilder();
        str.Append("using HUIFramework.Common;\n");
        str.Append("\n");
        str.Append("namespace Table\n");
        str.Append("{\n");
        if (config == false)
        {
            str.Append("   public class " + table_name +"Value" + " : TableValue\n");
        }
        else
        {
            str.Append("   public class " + table_name +"Value" + "\n");
        }
        str.Append("   {\n");
        for (var i = 0; i < count; i++)
        {
            if(name[i]== "id")
            {
                continue;
            }
            str.Append("       public " + types[i] + " " + name[i] + "; \n");
        }
        str.Append("   }\n");
        str.Append("}\n");
        
        if (!Directory.Exists(table_class_path))
        {
            Directory.CreateDirectory(table_class_path);
        }
        // if (File.Exists(class_path))
        // {
        //     File.Delete(class_path);
        // }
        using (var writer = new StreamWriter(class_path, false))
        {
            writer.Write(str.ToString());
        }
    }
    
    private static object GetData(string type, string data)
    {
        if (string.IsNullOrEmpty(data)) return null;
        switch (type.ToLower())
        {
            case "int":
                if (int.TryParse(data, out var i)) return i;
                break;
            case "single":
                if (float.TryParse(data, out var f)) return f;
                break;
            case "double":
                if (double.TryParse(data, out var d)) return d;
                break;
            case "bool":
                if (bool.TryParse(data, out var b)) return b;
                break;
            case "string":
                return data;
            case "int[]":
                var intArray = data.Split(',');
                var intList = new List<int>();
                foreach (var item in intArray)
                {
                    if (int.TryParse(item, out var intValue))
                    {
                        intList.Add(intValue);
                    }
                }
                return intList.ToArray();
            case "string[]":
                var stringArray = data.Split(',');
                var stringList = new List<string>();
                foreach (var item in stringArray)
                {
                    stringList.Add(item.Trim());
                }
                return stringList.ToArray();
            case "float[]":
                var floatArray = data.Split(',');
                var floatList = new List<float>();
                foreach (var item in floatArray)
                {
                    if (float.TryParse(item, out var floatValue))
                    {
                        floatList.Add(floatValue);
                    }
                }
                return floatList.ToArray();
            default:
                EditorUtility.ClearProgressBar();
                return data;
        }
        return data;
    }
}
