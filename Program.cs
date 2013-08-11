using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

using System.Collections;
using System.Collections.Generic;

using Substrate;
using Substrate.Core;
using Substrate.TileEntities;
using Substrate.Nbt;

using Mod.IronChest;

// This example replaces all instances of one block ID with another in a world.
// Substrate will handle all of the lower-level headaches that can pop up, such
// as maintaining correct lighting or replacing TileEntity records for blocks
// that need them.

// For a more advanced Block Replace example, see replace.cs in NBToolkit.

namespace BlockReplace
{
	class Program
	{
		static void Main (string[] args)
		{
			if (args.Length != 3) {
				Console.WriteLine("Usage: BlockReplace <r/t> <world> <replacefile>");
				return;
			}

			string mode = args[0];
			string input = args[1];
			string replacefile = args[2];

			IronChest.Register();
			TileEntityCleaner TEC = new TileEntityCleaner();

			List<string> TEList = new List<string>();
			List<int> BlockList = new List<int>();

			// Open our world
			MystWorld world = MystWorld.Open(input);
			
			string root = world.Path;
			
			// Check for Mystcraft and/or Non-Vanilla ages
			
			// Data structure to hold names of subfolders to be examined for files.
			Stack dirs = new Stack(20);
			List<int> mystAges = new List<int>();
			List<int> extraDims = new List<int>();

			
			if (!System.IO.Directory.Exists(root))
			{
				throw new ArgumentException();
			}
			dirs.Push(root);

			while (dirs.Count > 0)
			{
				string currentDir = dirs.Pop().ToString();
				string[] subDirs;
				try
				{
					subDirs = System.IO.Directory.GetDirectories(currentDir);
				}

				catch (UnauthorizedAccessException e)
				{
					Console.WriteLine(e.Message);
					continue;
				}
				catch (System.IO.DirectoryNotFoundException e)
				{
					Console.WriteLine(e.Message);
					continue;
				}

				foreach (string str in subDirs)
				{
					//Console.WriteLine(str);
					//if (str.Contains("DIM"))
					//{
					if (str.Contains("DIM_MYST"))
					{
						string resultString = Regex.Match(str, @"\d+").Value;
						mystAges.Add(Int32.Parse(resultString));
					}
					//else
					//{
					//	string resultString = Regex.Match(str, @"\d+").Value;
					//	extraDims.Add(Int32.Parse(resultString));
					//}
					//}
					
				}
			}
			
			//Console.WriteLine("Myst: {0} Other: {1}",mystAges.Count,extraDims.Count);
			
			//lets use a stack to track all of the dimension chunkmanagers
			Stack dimChunkManagers = new Stack(mystAges.Count);

			FileParser fp = new FileParser();
			Dictionary<int[], int[]> ReplaceList = fp.GetReplacementList(replacefile);

			Dictionary<string, string> RList = new Dictionary<string, string>();

			Console.WriteLine("Replacement pairs loaded from replace.dat:");

			//this is a waste of effort, fix it later.
			foreach (KeyValuePair<int[], int[]> butt in ReplaceList)
			{
				string oldBlock = butt.Key.GetValue(0).ToString() + ":" + butt.Key.GetValue(1).ToString();
				string newBlock = butt.Value.GetValue(0).ToString() + ":" + butt.Value.GetValue(1).ToString();
				RList.Add(oldBlock, newBlock);
			}

			foreach (KeyValuePair<int[], int[]> pair in ReplaceList)
			{
				string outk;
				string outv;

				if (pair.Key.GetValue(1).ToString() == "0")
				{
					outk = pair.Key.GetValue(0).ToString();
				}
				else
				{
					outk = pair.Key.GetValue(0) + ":" + pair.Key.GetValue(1);
				}

				if (pair.Value.GetValue(1).ToString() == "0")
				{
					outv = pair.Value.GetValue(0).ToString();
				}
				else
				{
					outv = pair.Value.GetValue(0) + ":" + pair.Value.GetValue(1);
				}

				Console.WriteLine("Block {0}\t will be converted to\t Block {1}", outk, outv);
			}

			Stack dimStack = new Stack();
			int currentDim = 0;

			foreach (int dim in mystAges)
			{
				//Console.WriteLine(dim.ToString());
				//if (dim == 4){
				dimChunkManagers.Push(world.GetMystChunkManager(dim));
				dimStack.Push(dim);
				//}
			}

			//				//leave this go until you can fix the -1 issue and write proper handling for it.
			//				foreach (int dim in extraDims)
			//				{
			//					dimChunkManagers.Push(OpenMystDimension(dim, world));
			//				}

			//add Vanilla ChunkManagers to the list too
			dimChunkManagers.Push(world.GetChunkManager(Dimension.NETHER));
			dimStack.Push(Dimension.NETHER);
			dimChunkManagers.Push(world.GetChunkManager(Dimension.THE_END));
			dimStack.Push(Dimension.THE_END);
			dimChunkManagers.Push(world.GetChunkManager(Dimension.DEFAULT));
			dimStack.Push(Dimension.DEFAULT);

			if (mode == "r")
			{

				Console.WriteLine();
				Console.WriteLine("Dimensions/Ages to be processed: {0}", dimChunkManagers.Count);
				
				int dimCount = dimChunkManagers.Count + 3;
				
				Console.WriteLine("Press any key to continue...");
				Console.ReadLine();
				Stopwatch mainCounter = new Stopwatch();
				mainCounter.Start();
				
				foreach (RegionChunkManager cm in dimChunkManagers)
				{
					int chunkCount = 0;
					
					currentDim = Convert.ToInt32(dimStack.Pop());
					
					foreach (ChunkRef chunk in cm)
					{
						chunkCount++;
					}

					FasterProcessChunk(cm,currentDim,RList);		
					currentDim++;
				}
				
				mainCounter.Stop();
				Console.WriteLine("Time Elapsed: {0} hours, {1} minutes, {2} seconds", mainCounter.Elapsed.Hours.ToString(),mainCounter.Elapsed.Minutes.ToString(),mainCounter.Elapsed.Seconds.ToString());
			}
			
			if (mode == "t")
			{
				Console.WriteLine("Press any key to continue...");
				Console.ReadLine();
				foreach (RegionChunkManager cm in dimChunkManagers)
				{
					foreach (ChunkRef chunk in cm)
					{
						// You could hardcode your dimensions, but maybe some day they
						// won't always be 16.  Also the CLR is a bit stupid and has
						// trouble optimizing repeated calls to Chunk.Blocks.xx, so we
						// cache them in locals
						int xdim = chunk.Blocks.XDim;
						int ydim = chunk.Blocks.YDim;
						int zdim = chunk.Blocks.ZDim;

						// x, z, y is the most efficient order to scan blocks (not that
						// you should care about internal detail)
						for (int x = 0; x < xdim; x++)
						{
							for (int z = 0; z < zdim; z++)
							{
								for (int y = 0; y < ydim; y++)
								{

									TileEntity TE = chunk.Blocks.GetTileEntity(x, y, z);
									//Console.WriteLine(chunk.Blocks.GetBlock(x,y,z).Data.ToString());

									if (TE != null)
									{
										if (TEC.getDeleteList().Contains(TE.ID))
										{
											using (System.IO.StreamWriter coverFile = File.AppendText(@"covers.txt"))
											{
												coverFile.WriteLine("-----------------------------");
												foreach (KeyValuePair<string,TagNode> anus in TE.Source)
												{
													if (anus.Value.GetTagType() == TagType.TAG_BYTE_ARRAY)
													{
														byte[] b = anus.Value.ToTagByteArray();
														coverFile.WriteLine("{0}:{1},{2}",anus.Key,b.GetValue(0).ToString(),b.GetValue(1).ToString());
													}
													else
													{
														coverFile.WriteLine("{0}:{1}", anus.Key,anus.Value);
													}
												}
											}
										}

										
										if (!TEList.Contains(TE.ID))
										{
											TEList.Add(TE.ID);
											Console.WriteLine("Tile Entity {0} found at {1},{2},{3}", TE.ID, x, y, z);
										}
									}

								}
							}
						}

						// Save the chunk
						cm.Save();

						Console.WriteLine("Processed Chunk {0},{1}", chunk.X, chunk.Z);
					}
				}
				TEList.Sort();

				foreach (string s in TEList)
				{
					Console.WriteLine("Tile Entity: {0}", s);
					
				}
				
				using (System.IO.StreamWriter file = new StreamWriter(@"output.txt"))
				{
					foreach (string line in TEList)
					{
						file.WriteLine(line);
					}
				}
			}
			Console.ReadLine();
		}
		
		static void FasterProcessChunk(RegionChunkManager rcm, int dimnumber, Dictionary<string,string> ReplaceList)
		{
			int currentchunk = 0;
			int chunktotal = 0;
			
			foreach (ChunkRef chunk in rcm)
			{
				chunktotal++;
			}
			
			foreach (ChunkRef chunk in rcm)
			{
				Stopwatch sw = new Stopwatch();
				sw.Start();

				currentchunk++;
				char[] metadata = {':'};
				
				string blockOutput = "";
				string tileOutput = "";
				
				// You could hardcode your dimensions, but maybe some day they
				// won't always be 16.  Also the CLR is a bit stupid and has
				// trouble optimizing repeated calls to Chunk.Blocks.xx, so we
				// cache them in locals
				int xdim = chunk.Blocks.XDim;
				int ydim = chunk.Blocks.YDim;
				int zdim = chunk.Blocks.ZDim;


				// x, z, y is the most efficient order to scan blocks (not that
				// you should care about internal detail)
				for (int x = 0; x < xdim; x++)
				{
					for (int z = 0; z < zdim; z++)
					{
						for (int y = 0; y < ydim; y++)
						{
							
							int currentID = chunk.Blocks.GetID(x,y,z);
							int currentData = chunk.Blocks.GetData(x,y,z);
							TileEntity TE = chunk.Blocks.GetTileEntity(x, y, z);
							
							TileEntityCleaner TC = new TileEntityCleaner();
							
							if (TE != null)
							{
								
								if (TC.getDeleteList().Contains(TE.ID))
								{
									chunk.Blocks.ClearTileEntity(x,y,z);
									Console.WriteLine("Tile Entity {0} found, cleared at {1},{2},{3}", TE.ID, x, y, z);
								}
								
								//if (TE != null)
								//	Console.WriteLine(TE.ID.ToString());
								
								if (IronChest.ChestTypes.Contains(TE.ID))
								{
									//IronChest.ChestList.Push(TE);
									var temp = IronChest.getChestType(TE);
									
									for (int i = 0; i <= temp.Items.Capacity; i++)
									{
										Item item = temp.Items[i];
										if (item != null)
										{
											string _tmpChestItem = item.ID.ToString() + ":" + item.Damage.ToString();
											//Console.WriteLine(_tmpChestItem);
											
											if (ReplaceList.TryGetValue(_tmpChestItem,out tileOutput))
											{
												string[] _strKey = tileOutput.Split(metadata);
												
												temp.Items[i].ID = Convert.ToInt32(_strKey.GetValue(0));
												temp.Items[i].Damage = Convert.ToInt32(_strKey.GetValue(1));
											}
											
										}
										
									}
									
									switch (currentData)
									{
										case 0:
											IronChest.SetTileEntity("IRON");
											break;
										case 1:
											IronChest.SetTileEntity("GOLD");
											break;
										case 2:
											IronChest.SetTileEntity("DIAMOND");
											break;
										case 3:
											IronChest.SetTileEntity("COPPER");
											break;
										case 4:
											IronChest.SetTileEntity("SILVER");
											break;
										case 5:
											IronChest.SetTileEntity("CRYSTAL");
											break;
										default:
											IronChest.SetTileEntity("IRON");
											break;
									}
									
									chunk.Blocks.SetTileEntity(x,y,z,temp);
								}
								
								if (TE.ID == "Chest")
								{
									TileEntityChest temp = new TileEntityChest(TE);
									
									for (int i = 0; i <= temp.Items.Capacity; i++)
									{
										Item item = temp.Items[i];
										if (item != null)
										{
											string _tmpChestItem = item.ID.ToString() + ":" + item.Damage.ToString();
											//Console.WriteLine(_tmpChestItem);
											
											if (ReplaceList.TryGetValue(_tmpChestItem,out tileOutput))
											{
												string[] _strKey = tileOutput.Split(metadata);
												
												temp.Items[i].ID = Convert.ToInt32(_strKey.GetValue(0));
												temp.Items[i].Damage = Convert.ToInt32(_strKey.GetValue(1));
											}
											
										}
										
									}
									chunk.Blocks.SetTileEntity(x,y,z, temp);
								}
							}
							
							string currentBlock = currentID.ToString() + ":" + currentData.ToString();
							
							if (ReplaceList.TryGetValue(currentBlock, out blockOutput))
							{
								string[] _strKey = blockOutput.Split(metadata);
								
								chunk.Blocks.SetID(x,y,z,(Convert.ToInt32(_strKey.GetValue(0))));
								chunk.Blocks.SetData(x,y,z,(Convert.ToInt32(_strKey.GetValue(1))));

//								Console.WriteLine("Block changed from ID {0}:{1} to {2}:{3} at {4},{5},{6}",
//								                  currentID,
//								                  currentData,
//								                  _strKey.GetValue(0),
//								                  _strKey.GetValue(1),
//								                  x, y, z);
								
							}
							
						}
					}
				}
				//ProcessModBlocks();
				
				// Save the chunk
				rcm.Save();
				sw.Stop();
				
				switch (dimnumber)
				{
					case Dimension.DEFAULT:
						Console.WriteLine("Processed Chunk {0},{1}\t\t({2} of {3} in Overworld (DIM{4})", chunk.X, chunk.Z, currentchunk, chunktotal, Dimension.DEFAULT);
						break;
					case Dimension.NETHER:
						Console.WriteLine("Processed Chunk {0},{1}\t\t({2} of {3} in Nether (DIM{4})", chunk.X, chunk.Z, currentchunk, chunktotal, Dimension.NETHER);
						break;
					case Dimension.THE_END:
						Console.WriteLine("Processed Chunk {0},{1}\t\t({2} of {3} in The End (DIM{4})", chunk.X, chunk.Z, currentchunk, chunktotal, Dimension.THE_END);
						break;
					default:
						Console.WriteLine("Processed Chunk {0},{1}\t\t({2} of {3} in Mystcraft Age {4}) in {5}.{6}", chunk.X, chunk.Z, currentchunk, chunktotal, dimnumber, sw.Elapsed.Seconds.ToString(),sw.Elapsed.Milliseconds.ToString());
						break;
				}
				
				//Console.WriteLine(sw.ElapsedMilliseconds.ToString());
				//Console.ReadLine();
				
			}
			
			switch (dimnumber)
			{
				case Dimension.DEFAULT:
					Console.WriteLine("Overworld (DIM{0}) completed.",Dimension.DEFAULT);
					break;
				case Dimension.NETHER:
					Console.WriteLine("Nether (DIM{0}) completed.",Dimension.DEFAULT);
					break;
				case Dimension.THE_END:
					Console.WriteLine("The End (DIM{0}) completed.",Dimension.THE_END);
					break;
				default:
					Console.WriteLine("Mystcraft Age {0} (DIM{0}) completed.", dimnumber);
					break;
			}
		}
	}
}
