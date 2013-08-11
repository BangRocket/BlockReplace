/*
 * Created by SharpDevelop.
 * User: Joshua Heidorn
 * Date: 8/10/2013
 * Time: 9:11 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace BlockReplace
{
	/// <summary>
	/// Description of ModHandler.
	/// </summary>
	public class ModHandler
	{
		List<string> _modList = new List<string>();
		
		public void CallMethod()
		{
			
		}
		
		public void CallMethod(string name)
		{
			_modList.Add(name);
		}
		
		public List<string> ModList
		{
			get {return _modList;}
		}
	}
}
