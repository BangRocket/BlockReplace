/*
 * Created by SharpDevelop.
 * User: Joshua Heidorn
 * Date: 8/2/2013
 * Time: 8:26 PM
 * 
 */
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Substrate;
using Substrate.Core;
using Substrate.Nbt;

namespace Mod.IronChest
{
	public class IronChest
	{
		public static void Register()
		{
			Substrate.TileEntityFactory.Register(TileEntityIronChest.TypeId,typeof(TileEntityIronChest));
			Substrate.TileEntityFactory.Register(TileEntityCopperChest.TypeId,typeof(TileEntityCopperChest));
			Substrate.TileEntityFactory.Register(TileEntityGoldChest.TypeId,typeof(TileEntityGoldChest));
			Substrate.TileEntityFactory.Register(TileEntitySilverChest.TypeId,typeof(TileEntitySilverChest));
			Substrate.TileEntityFactory.Register(TileEntityDiamondChest.TypeId,typeof(TileEntityDiamondChest));
			Substrate.TileEntityFactory.Register(TileEntityCrystalChest.TypeId,typeof(TileEntityCrystalChest));
			
			// Creating a BlockInfo (or BlockInfoEx) will also automatically register the
			// block ID, lighting, and opacity data with internal tables in the BlockInfo
			BlockInfoEx IronChest = (BlockInfoEx) new BlockInfoEx(BlockTypeM.IRON_CHEST, "IronChest").SetOpacity(0);
			
			// You can redefine already-registered blocks at any time by creating a new
			// BlockInfo object with the given ID.
			IronChest.SetTileEntity("IRON");
		}
		
		public static void setTileEntity(string s)
		{
			BlockInfoEx IronChest = (BlockInfoEx) new BlockInfoEx(BlockTypeM.IRON_CHEST, "IronChest").SetOpacity(0);
			IronChest.SetTileEntity(s);
		}
		
		public static TileEntityIChest getChestType(TileEntity te, int blockdata)
		{
			switch (blockdata)
			{
					
				case 0:
					TileEntityIronChest _itec = te as TileEntityIronChest;
					return _itec;
				case 1:
					TileEntityGoldChest _gtec = te as TileEntityGoldChest;
					return _gtec;
				case 2:
					TileEntityDiamondChest _dtec = te as TileEntityDiamondChest;
					return _dtec;
				case 3:
					TileEntityCopperChest _cptec = te as TileEntityCopperChest;
					return _cptec;
				case 4:
					TileEntitySilverChest _stec = te as TileEntitySilverChest;
					return _stec;
				case 5:
					TileEntityCrystalChest _ctec = te as TileEntityCrystalChest;
					return _ctec;
				default:
					TileEntityIronChest tec = te as TileEntityIronChest;
					return tec;
			}
		}
		
		public string getType(int data)
		{
			switch (data)
			{
				case 0:
					return "IRON";
				case 1:
					return "GOLD";
				case 2: 
					return "DIAMOND";
				case 3:
					return "COPPER";
				case 4:
					return "SILVER";
				case 5:
					return "CRYSTAL";
				default:
					return "IRON";
			}
				
		}
		
	}
	
	// Convenience class -- like the BlockType class, it's not required that you define this
    static class BlockTypeM
    {
        public static int IRON_CHEST = 181;
    }
	
   	abstract public class TileEntityIChest : TileEntity, IItemContainer
    {
   		public static readonly SchemaNodeCompound ChestSchema;
   		
   		public static string TypeId;
   		
   		private int _CAPACITY = 108;
   		
   		private ItemCollection _items;
   		
        protected TileEntityIChest (string id)
            : base(id)
        {
            _items = new ItemCollection(_CAPACITY);
        }

        public TileEntityIChest ()
            : this(TypeId)
        {
        }

        public TileEntityIChest (TileEntity te)
            : base(te)
        {    	       	
            TileEntityIChest tec = te as TileEntityIChest;
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
    
   	public class TileEntityIronChest : TileEntityIChest
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

        private int _CAPACITY = 54;

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