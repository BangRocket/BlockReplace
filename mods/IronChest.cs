/*
 * Created by SharpDevelop.
 * User: Joshua Heidorn
 * Date: 8/2/2013
 * Time: 8:26 PM
 * 
 */
using System;
using System.Collections.Generic;

using Substrate;
using Substrate.Core;
using Substrate.Nbt;

namespace Mod.IronChest
{
	public class IronChest
	{
		
		// Convenience class -- like the BlockType class, it's not required that you define this
		static class BlockTypeM
		{
			public static int IRON_CHEST = 181;
		}
		
		private static List<string> _chestTypes = new List<string>{"IRON", "COPPER", "CRYSTAL", "DIAMOND", "GOLD", "SILVER"};
		
		public static List<string> ChestTypes
		{
			get {return _chestTypes;}
		}
		
		private static Stack<TileEntity> _chestList = new Stack<TileEntity>();
		
		public static Stack<TileEntity> ChestList
		{
			get {return _chestList;}
		}
				
		public const int IRON = 0;
		public const int GOLD = 1;
		public const int DIAMOND = 2;
		public const int COPPER = 3;
		public const int SILVER = 4;
		public const int CRYSTAL = 5;
		
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
			//IronChest.SetTileEntity("IRON");
		}
		
		public static void SetTileEntity(string s)
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
		
		public static TileEntityIChest getChestType(TileEntity te)
		{
			switch (te.ID)
			{
				case "IRON":
					TileEntityIronChest _itec = te as TileEntityIronChest;
					return _itec;
				case "GOLD":
					TileEntityGoldChest _gtec = te as TileEntityGoldChest;
					return _gtec;
				case "DIAMOND":
					TileEntityDiamondChest _dtec = te as TileEntityDiamondChest;
					return _dtec;
				case "COPPER":
					TileEntityCopperChest _cptec = te as TileEntityCopperChest;
					return _cptec;
				case "SILVER":
					TileEntitySilverChest _stec = te as TileEntitySilverChest;
					return _stec;
				case "CRYSTAL":
					TileEntityCrystalChest _ctec = te as TileEntityCrystalChest;
					return _ctec;
				default:
					TileEntityIronChest tec = te as TileEntityIronChest;
					return tec;
			}
		}
	}
			
   	abstract public class TileEntityIChest : TileEntity, IItemContainer
    {
   		public static readonly SchemaNodeCompound ChestSchema = TileEntity.Schema.MergeInto(new SchemaNodeCompound("")
   		{
   			new SchemaNodeString("id", TypeId),
   			new SchemaNodeScaler("facing", TagType.TAG_INT),
   			new SchemaNodeList("Items", TagType.TAG_COMPOUND, ItemCollection.Schema),
   		});

   		
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

        new public static string TypeId 
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
   	}
   	
   	public class TileEntityCopperChest : TileEntityIChest
   	{
   		new public static string TypeId
   		{
   			get { return "COPPER"; }
   		}
   		
   		private const int _CAPACITY = 45;
   		
   		private ItemCollection _items;
   		
   		protected TileEntityCopperChest (string id)
   			: base(id)
   		{
   			_items = new ItemCollection(_CAPACITY);
   		}

   		public TileEntityCopperChest ()
   			: this(TypeId)
   		{
   		}
   		
   		public TileEntityCopperChest (TileEntity te)
   			: base(te)
   		{
   			TileEntityCopperChest tec = te as TileEntityCopperChest;
   			if (tec != null) {
   				_items = tec._items.Copy();
   			}
   			else {
   				_items = new ItemCollection(_CAPACITY);
   			}
   		}
   		
   	}
   	
   	public class TileEntitySilverChest : TileEntityIChest
   	{
   		new public static string TypeId
   		{
   			get { return "SILVER"; }
   		}
   		
   		private const int _CAPACITY = 72;
   		
   		private ItemCollection _items;
   		
   		protected TileEntitySilverChest (string id)
   			: base(id)
   		{
   			_items = new ItemCollection(_CAPACITY);
   		}

   		public TileEntitySilverChest ()
   			: this(TypeId)
   		{
   		}
   		
   		public TileEntitySilverChest (TileEntity te)
   			: base(te)
   		{
   			TileEntitySilverChest tec = te as TileEntitySilverChest;
   			if (tec != null) {
   				_items = tec._items.Copy();
   			}
   			else {
   				_items = new ItemCollection(_CAPACITY);
   			}
   		}
   		
   	}
   	
   	public class TileEntityGoldChest : TileEntityIChest
   	{
   		new public static string TypeId
   		{
   			get { return "GOLD"; }
   		}
   		
   		private const int _CAPACITY = 81;
   		
   		private ItemCollection _items;
   		
   		protected TileEntityGoldChest (string id)
   			: base(id)
   		{
   			_items = new ItemCollection(_CAPACITY);
   		}

   		public TileEntityGoldChest ()
   			: this(TypeId)
   		{
   		}
   		
   		public TileEntityGoldChest (TileEntity te)
   			: base(te)
   		{
   			TileEntityGoldChest tec = te as TileEntityGoldChest;
   			if (tec != null) {
   				_items = tec._items.Copy();
   			}
   			else {
   				_items = new ItemCollection(_CAPACITY);
   			}
   		}
   		
   	}
   	
   	public class TileEntityCrystalChest : TileEntityIChest
   	{
   		new public static string TypeId
   		{
   			get { return "CRYSTAL"; }
   		}
   		
   		private const int _CAPACITY = 108;
   		
   		private ItemCollection _items;
   		
   		protected TileEntityCrystalChest (string id)
   			: base(id)
   		{
   			_items = new ItemCollection(_CAPACITY);
   		}

   		public TileEntityCrystalChest ()
   			: this(TypeId)
   		{
   		}
   		
   		public TileEntityCrystalChest (TileEntity te)
   			: base(te)
   		{
   			TileEntityCrystalChest tec = te as TileEntityCrystalChest;
   			if (tec != null) {
   				_items = tec._items.Copy();
   			}
   			else {
   				_items = new ItemCollection(_CAPACITY);
   			}
   		}
   		
   	}
   	
   	public class TileEntityDiamondChest : TileEntityIChest
   	{
   		new public static string TypeId
   		{
   			get { return "DIAMOND"; }
   		}
   		
   		private const int _CAPACITY = 108;
   		
   		private ItemCollection _items;
   		
   		protected TileEntityDiamondChest (string id)
   			: base(id)
   		{
   			_items = new ItemCollection(_CAPACITY);
   		}

   		public TileEntityDiamondChest ()
   			: this(TypeId)
   		{
   		}
   		
   		public TileEntityDiamondChest (TileEntity te)
   			: base(te)
   		{
   			TileEntityDiamondChest tec = te as TileEntityDiamondChest;
   			if (tec != null) {
   				_items = tec._items.Copy();
   			}
   			else {
   				_items = new ItemCollection(_CAPACITY);
   			}
   		}
   		
   	}
	
}