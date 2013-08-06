/*
 * Created by SharpDevelop.
 * User: Joshua
 * Date: 8/2/2013
 * Time: 8:26 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using Substrate;
using Substrate.Core;
using Substrate.Nbt;

namespace BlockReplace
{
	   // Convenience class -- like the BlockType class, it's not required that you define this
    static class BlockTypeM
    {
        public static int IRON_CHEST = 181;
    }
	
	public class TileEntityIronChest : TileEntity, IItemContainer
    {
        public static readonly SchemaNodeCompound ChestSchema = TileEntity.Schema.MergeInto(new SchemaNodeCompound("")
        {
            new SchemaNodeString("id", TypeId),
            new SchemaNodeScaler("facing", TagType.TAG_INT),
            new SchemaNodeList("Items", TagType.TAG_COMPOUND, ItemCollection.Schema),
        });

        public static string TypeId 
        {
            get { return "IRON"; }
        }

        private const int _CAPACITY = 54;

        private ItemCollection _items;

        protected TileEntityIronChest (string id)
            : base(id)
        {
            _items = new ItemCollection(_CAPACITY);
        }

        public TileEntityIronChest ()
            : this(TypeId)
        {
        }

        public TileEntityIronChest (TileEntity te)
            : base(te)
        {
            TileEntityIronChest tec = te as TileEntityIronChest;
            if (tec != null) {
                _items = tec._items.Copy();
            }
            else {
                _items = new ItemCollection(_CAPACITY);
            }
        }

        #region ICopyable<TileEntity> Members

        public override TileEntity Copy ()
        {
            return new TileEntityIronChest(this);
        }

        #endregion


        #region IItemContainer Members

        public ItemCollection Items
        {
            get { return _items; }
        }

        #endregion


        #region INBTObject<TileEntity> Members

        public override TileEntity LoadTree (TagNode tree)
        {
            TagNodeCompound ctree = tree as TagNodeCompound;
            if (ctree == null || base.LoadTree(tree) == null) {
                return null;
            }

            TagNodeList items = ctree["Items"].ToTagList();
            _items = new ItemCollection(_CAPACITY).LoadTree(items);

            return this;
        }

        public override TagNode BuildTree ()
        {
            TagNodeCompound tree = base.BuildTree() as TagNodeCompound;
            tree["Items"] = _items.BuildTree();

            return tree;
        }

        public override bool ValidateTree (TagNode tree)
        {
            return new NbtVerifier(tree, ChestSchema).Verify();
        }

        #endregion
    }
	
	public class TileEntityCopperChest : TileEntityIronChest
	{
		new public static string TypeId
		{
			get { return "COPPER"; }
		}
		
		private const int _CAPACITY = 45;
		
	}
	
	public class TileEntitySilverChest : TileEntityIronChest
	{
		new public static string TypeId
		{
			get { return "SILVER"; }
		}
		
		private const int _CAPACITY = 72;
		
	}
	
	public class TileEntityGoldChest : TileEntityIronChest
	{
		new public static string TypeId
		{
			get { return "GOLD"; }
		}
		
		private const int _CAPACITY = 81;
		
	}
	
	public class TileEntityCrystalChest : TileEntityIronChest
	{
		new public static string TypeId
		{
			get { return "CRYSTAL"; }
		}
		
		private const int _CAPACITY = 108;
		
	}
	
	public class TileEntityDiamondChest : TileEntityIronChest
	{
		new public static string TypeId
		{
			get { return "DIAMOND"; }
		}
		
		private const int _CAPACITY = 108;
		
	}
	
}