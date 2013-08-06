/*
 * Created by SharpDevelop.
 * User: Joshua
 * Date: 7/16/2013
 * Time: 1:39 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;

namespace BlockReplace
{
	/// <summary>
	/// Description of FileParser.
	/// </summary>
	public class FileParser
	{
		
		Dictionary<string,string> LoadedList  = new Dictionary<string,string>();
		
		public FileParser()
		{
			
		}
		
		public Dictionary<int[],int[]> GetReplacementList(string path)
		{
			string[] list = {};
			Dictionary<int[],int[]> ReplaceList = new Dictionary<int[], int[]>();
			
			try
			{
				//string path = @"replace.dat";
				list = File.ReadAllLines(path);
			}
			catch (Exception e)
			{
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
			}
			
			foreach (String line in list)
			{
				if (!line.Equals(""))
				{
					if (!line.Contains("#"))
					{
						char[] delimiterChars = {'>'};

						String[] temp = line.Split(delimiterChars);
						
						LoadedList.Add(temp[0],temp[1]);
					}
				}
			}
			
			//Ok, now lets break this down into a useable format.
			//This has absolutely no error checking
			foreach (KeyValuePair<string, string> s in LoadedList)
			{

				char[] metadata = {':'};
				string[] _strValue = {"0","0"};
				string[] _strKey = {"0","0"};
				
				if (s.Value.Contains(":"))
				{
					_strValue = s.Value.Split(metadata);
				}
				else
				{
					_strValue.SetValue(s.Value,0);
					_strValue.SetValue("0",1);
				}
				
				if (s.Key.Contains(":"))
				{
					_strKey = s.Key.Split(metadata);
				}
				else
				{
					_strKey.SetValue(s.Key,0);
					_strKey.SetValue("0",1);
				}

				int[] _intValue = {Convert.ToInt32(_strValue[0]),Convert.ToInt32(_strValue[1])};
				int[] _intKey = {Convert.ToInt32(_strKey[0]),Convert.ToInt32(_strKey[1])};
				ReplaceList.Add(_intKey,_intValue);

				
				//Console.WriteLine("Block {0} converted to Block {1}", s.Key, s.Value);

			}
			return ReplaceList;
		}
	}
}
