/*
 * Created by SharpDevelop.
 * User: Joshua
 * Date: 8/14/2013
 * Time: 10:16 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using Substrate;
using Substrate.Core;
using Substrate.Nbt;

namespace Mod.Factorization
{
	public class Factorization
	{
		
		// Convenience class -- like the BlockType class, it's not required that you define this
		static class BlockTypeM
		{
			public static int FACTORY_BARREL = 1657;
		}
		
		public static void Register()
		{
			
			//Substrate.TileEntityFactory.Register(TileEntityIronChest.TypeId,typeof(TileEntityIronChest));
			
			// Creating a BlockInfo (or BlockInfoEx) will also automatically register the
			// block ID, lighting, and opacity data with internal tables in the BlockInfo
			BlockInfoEx Factory_Barrel = (BlockInfoEx) new BlockInfoEx(BlockTypeM.FACTORY_BARREL, "Factory_Barrel").SetOpacity(0);
			
			// You can redefine already-registered blocks at any time by creating a new
			// BlockInfo object with the given ID.
			Factory_Barrel.SetTileEntity("factory_barrel");
		}
	}
			
//   	abstract public class FactoryBarrel : TileEntity, IItemContainer
//    {
//   		public static readonly SchemaNodeCompound ChestSchema = TileEntity.Schema.MergeInto(new SchemaNodeCompound("")
//   		{
//   			new SchemaNodeString("id", TypeId),
//   			new SchemaNodeScaler("facing", TagType.TAG_INT),
//   			new SchemaNodeList("Items", TagType.TAG_COMPOUND, ItemCollection.Schema),
//   		});
//
//   		
//   		public static string TypeId;
//   		
//   		private int _CAPACITY = 108;
//   		
//   		private ItemCollection _items;
//   		
//        protected TileEntityIChest (string id)
//            : base(id)
//        {
//            _items = new ItemCollection(_CAPACITY);
//        }
//
//        public TileEntityIChest ()
//            : this(TypeId)
//        {
//        }
//
//        public TileEntityIChest (TileEntity te)
//            : base(te)
//        {    	       	
//            TileEntityIChest tec = te as TileEntityIChest;
//            if (tec != null) {
//                _items = tec._items.Copy();
//            }
//            else {
//                _items = new ItemCollection(_CAPACITY);
//            }
//        }
//        
//                #region ICopyable<TileEntity> Members
//
//        public override TileEntity Copy ()
//        {
//            return new TileEntityIronChest(this);
//        }
//
//        #endregion
//
//
//        #region IItemContainer Members
//
//        public ItemCollection Items
//        {
//            get { return _items; }
//        }
//
//        #endregion
//
//
//        #region INBTObject<TileEntity> Members
//
//        public override TileEntity LoadTree (TagNode tree)
//        {
//            TagNodeCompound ctree = tree as TagNodeCompound;
//            if (ctree == null || base.LoadTree(tree) == null) {
//                return null;
//            }
//
//            TagNodeList items = ctree["Items"].ToTagList();
//            _items = new ItemCollection(_CAPACITY).LoadTree(items);
//
//            return this;
//        }
//
//        public override TagNode BuildTree ()
//        {
//            TagNodeCompound tree = base.BuildTree() as TagNodeCompound;
//            tree["Items"] = _items.BuildTree();
//
//            return tree;
//        }
//
//        public override bool ValidateTree (TagNode tree)
//        {
//            return new NbtVerifier(tree, ChestSchema).Verify();
//        }
//
//        #endregion
//   	}
}