/*
 * Created by SharpDevelop.
 * User: Joshua
 * Date: 8/6/2013
 * Time: 7:23 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace BlockReplace
{
	/// <summary>
	/// Description of TileEntityCleaner.
	/// </summary>
	public class TileEntityCleaner
	{	
		public static List<string> DeleteList = new List<string>();
		
		public TileEntityCleaner()
		{
			DeleteList.Add("RPGrate");
			DeleteList.Add("RPPipe");
			DeleteList.Add("RPPump");
			DeleteList.Add("Water Strainer");
			DeleteList.Add("RPFilter");
			DeleteList.Add("RPSorter");
			DeleteList.Add("RPGRSTube");
			DeleteList.Add("RPMulti");
			DeleteList.Add("RPWind");

			DeleteList.Add("Redwire");
			DeleteList.Add("Bluewire");
			DeleteList.Add("Covers");
			DeleteList.Add("RPTube");
			DeleteList.Add("RPRSTube");
			DeleteList.Add("RPSolar");
			DeleteList.Add("RPBatBox");
			DeleteList.Add("RPDeploy");
			DeleteList.Add("RPTranspose");
		}
		
	}
}
