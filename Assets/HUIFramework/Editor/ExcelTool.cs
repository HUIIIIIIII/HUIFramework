using System.Collections.Generic;
using System.IO;
using System.Text;
using HUIFramework.Common;
using log4net.Core;
using Newtonsoft.Json;
using NPOI.XSSF.UserModel;
using UnityEditor;
using UnityEngine;

public static class ExcelTool
{
    private const string table_class_path = "Assets/HotUpdate/Table/";
    private const string table_asset_path = "Assets/Game/Table/{0}.txt";
    
    [MenuItem("HUITool/ExcelTool")]
    public static void Export()
    {
        var excel_path = Path.GetFullPath(Application.dataPath.Replace("Assets", "Excel"));
        var excel_list = Directory.GetFiles(excel_path, "*.xlsx");
        ExportExcel(excel_list);
    }
    
    private static void ExportExcel(string[] excel_list)
    {
        EditorUtility.ClearProgressBar();
        for (var i = 0; i < excel_list.Length; i++)
        {
            var excel = excel_list[i];
            EditorUtility.DisplayProgressBar("Excel转换", $"正在转换 {Path.GetFileNameWithoutExtension(excel)}",
                (float)i / excel_list.Length);
            ExcelToJson(excel);
        } 
        EditorUtility.ClearProgressBar();
    }
    
   
    private static void ExcelToJson(string file)
    {
        var table_path= Path.GetFullPath(Path.GetDirectoryName(table_asset_path));
        var file_name = Path.GetFileNameWithoutExtension(file);
        file_name = char.ToUpper(file_name[0]) + file_name.Substring(1);
        var file_path = Path.Combine(table_path, $"{file_name}.txt");
        var json = JsonConvert.SerializeObject(ReadExcel(file), Formatting.Indented);
        if (!Directory.Exists(table_path))
        {
            Directory.CreateDirectory(table_path);
        }
        if (File.Exists(file_path))
        {
            File.Delete(file_path);
        }
        var content = CryptoUtil.Encrypt(json);
        using (var writer = new StreamWriter(file_path, false))
        {
            writer.Write(content);
        }
    }
    
    private static List<Dictionary<string, object>> ReadExcel(string file_path)
    {
        var result = new List<Dictionary<string, object>>();
        using (var stream = File.OpenRead(file_path))
        {
            var workbook = new XSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(0);
            var header = new List<string>();
            var types = new List<string>();
            var rowEnum = sheet.GetRowEnumerator();
            
            // data_name
            if (rowEnum.MoveNext())
            {
                var row = (NPOI.SS.UserModel.IRow)rowEnum.Current;
                foreach (var cell in row.Cells)
                    header.Add(cell.ToString());
            }
            // data type
            if (rowEnum.MoveNext())
            {
                var row = (NPOI.SS.UserModel.IRow)rowEnum.Current;
                foreach (var cell in row.Cells)
                {
                    types.Add(cell.ToString());
                }
            }
            ExportClass(file_path, header, types);
            while (rowEnum.MoveNext())
            {
                var row = (NPOI.SS.UserModel.IRow)rowEnum.Current;
                var dict = new Dictionary<string, object>();
                for (int i = 0; i < header.Count; i++)
                {
                    dict[header[i]] = GetData(types[i], row.GetCell(i)?.ToString() ?? string.Empty);
                }
                result.Add(dict);
            }
        }
        return result;
    }

    public static void ExportClass(string file_path,List<string> name, List<string> types)
    {   
        var table_name = Path.GetFileNameWithoutExtension(file_path);
        table_name = char.ToUpper(table_name[0]) + table_name.Substring(1);
        
        var class_file_path = Path.Combine(Path.GetFullPath(table_class_path), $"{table_name}Value.cs");
        
        var count = Mathf.Min(name.Count, types.Count);
        var str = new StringBuilder();
        str.Append("using HUIFramework.Common;\n");
        str.Append("\n");
        str.Append("namespace Table\n");
        str.Append("{\n");
        str.Append("   public class " + table_name +"Value" + " : TableValue\n");
        str.Append("   {\n");
        for (var i = 0; i < count; i++)
        {
            if(name[i]== "id")
            {
                continue;
            }
            str.Append("       public " + types[i] + " " + name[i] + " { get; }\n");
        }
        str.Append("   }\n");
        str.Append("}\n");
        
        if (!Directory.Exists(table_class_path))
        {
            Directory.CreateDirectory(table_class_path);
        }
        if (File.Exists(class_file_path))
        {
            File.Delete(class_file_path);
        }
        using (var writer = new StreamWriter(class_file_path, false))
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
                throw new LogException("value type is not supported: " + type);
        }
        return data;
    }
}
