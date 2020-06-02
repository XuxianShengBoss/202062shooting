using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Excel;
using System.Data;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

public class ExcelReader 
{
	public enum TableType
	{
		Enum,
		Data,
		STR
	}

	public static Dictionary<string, string> tableEnums = new Dictionary<string, string>();
	public static Dictionary<string, string> tableData = new Dictionary<string, string>();
	public static List<string[]> tableStrs = new List<string[]>();

	public static void Clear(){
		tableEnums.Clear ();
		tableData.Clear ();
		tableStrs.Clear ();
	}

	public static void ParseExcel(string excelFilePath)
	{
		LoadExcel (excelFilePath);
	}

	private static void LoadExcel(string excelFilePath)
	{
		DataSet dataSet = LoadExcelAsDataSet(excelFilePath);

		for (int i = 0; i < dataSet.Tables.Count; i++) {
			
			DataTable table = dataSet.Tables [i];


			string type = table.Rows [0] [0].ToString();
			string name = table.Rows [0] [1].ToString();


			Debug.Log("type:"+type+"\t\tname:"+name+"\t\t sheet:"+table.TableName);

			if (type == TableType.Enum.ToString()) {
				tableEnums.Add (name, ParseEnumTable (table));
			} if (type == TableType.Data.ToString()) {
				tableData.Add (name, ParseDataTable (table));
			} if (type == TableType.STR.ToString()) {
				tableEnums.Add (name, ParseEnumTable (table));
				tableStrs.Add (ParseStrTable(table, "CN"));
                tableStrs.Add(ParseStrTable(table, "EN"));
            }
		}


	}

	private static DataSet LoadExcelAsDataSet(string excelFilePath){
		FileStream stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		DataSet dataSet = excelReader.AsDataSet ();

//		int columns = dataSet.Tables[0].Columns.Count;
//		int rows = dataSet.Tables[0].Rows.Count;
//
//		for(int i = 0;  i< rows; i++)
//		{
//			for(int j =0; j < columns; j++)
//			{
//				string  nvalue  = dataSet.Tables[0].Rows[i][j].ToString();
//				Debug.Log(nvalue);
//			}
//		}	
		return dataSet;
	}

	//pares enum sheet, return as .cs file content
	private static string ParseEnumTable(DataTable table){
		int columns = table.Columns.Count;
		int rows = table.Rows.Count;
		string enumName = table.Rows [0] [1].ToString();

		int idCol = -1;
		int valueCol = -1;
		int descCol = -1;

		for (int i = 0; i < columns; i++) {
			string name = table.Rows [2] [i].ToString ();
			if (name == "id") {
				idCol = i;
			} else if (name == "value") {
				valueCol = i;
			} else if (name == "desc") {
				descCol = i;
			}	
		}

		//create enum file content
		string s = "public enum " + enumName + "\n{\n";
		for (int row = 3; row < rows; row++) {
			string id = table.Rows [row] [idCol].ToString();
			if(!string.IsNullOrEmpty(id)){
				//add id
				s += "\t" + id;
				//add value
				if (valueCol != -1) {
					string value = table.Rows [row] [valueCol].ToString();
					if (!string.IsNullOrEmpty (value)) {
						s += " = " + value;
					}
				}
				s += ",";
				//add desc
				if (descCol != -1) {
					string desc = table.Rows [row] [descCol].ToString();
					if (!string.IsNullOrEmpty (desc)) {
						s += " //" + desc;
					}
				}
				s += "\n";
			}
		}
		s += "}";
		//Debug.LogError (s);
		return s;
	}

	//Parse data sheet, return as json
	private static string[] ParseStrTable(DataTable table, string language){
		int columns = table.Columns.Count;
		int rows = table.Rows.Count;

		int valueCol = -1;
		int lanCol = -1;

		for (int i = 0; i < columns; i++) {
			string k = table.Rows [2] [i].ToString ();
			if (!string.IsNullOrEmpty (k)) {
				if (k == language) {
					lanCol = i;
				} else if (k == "value") {
					valueCol = i;
				}
			}
		}

		int maxValue = 0;
		for (int i = 3; i < rows; i++) {
			string v = table.Rows [i] [valueCol].ToString();
			if (!string.IsNullOrEmpty (v)) {
				int value = 0;
				if (int.TryParse (v, out value)) {
					if (value > maxValue) {
						maxValue = value;
					}
				}
			}
		}


		string[] strList = new string[maxValue + 1];

		for (int i = 3; i < rows; i++) {
			string v = table.Rows [i] [valueCol].ToString();
			if (!string.IsNullOrEmpty (v)) {
				int value = 0;
				if (int.TryParse (v, out value)) {
					string str = table.Rows [i] [lanCol].ToString();
					strList [value] = str;
				}
			}
		}

		//Debug.LogError (s);

		return strList;
	}


	//Parse data sheet, return as json
	private static string ParseDataTable(DataTable table){
		int columns = table.Columns.Count;
		int rows = table.Rows.Count;
		string className = table.Rows [0] [1].ToString();

		//list invalid columns
		List<int> keys = new List<int>();
		for (int i = 0; i < columns; i++) {
			string k = table.Rows [2] [i].ToString ();
			if (!string.IsNullOrEmpty (k)) {
				keys.Add (i);
			}
		}


		int keyCount = keys.Count;
		string s = "[";
		//读取数据
		for (int i = 3; i < rows; i++) {
			//check values
			bool hasValue = false;
			for (int j = 0; j < keyCount; j++) {
				string v = table.Rows [i] [keys[j]].ToString();
				if (!string.IsNullOrEmpty (v)) {
					hasValue = true;
					break;
				}
			}

			//add date
			if (hasValue) {
				s += "{";
				for (int j = 0; j < keyCount; j++) {
					//读取第1行数据作为表头字段
					string k = table.Rows [2] [keys [j]].ToString ();
					//Key-Value对应
					string v = table.Rows [i] [keys [j]].ToString ();

					s += "\"" + k + "\":\"" + v + "\",";
				}
				s += "},";
			}
		}
		s += "]";

		//Debug.LogError (s);

		return s;
	}

}