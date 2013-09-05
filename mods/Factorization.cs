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
					
		public static string BARREL_ID = "factory_barrel";
		
		public static void Register()
		{			
			Substrate.TileEntityFactory.Register(FactoryBarrel.TypeId,typeof(FactoryBarrel));
			
			// Creating a BlockInfo (or BlockInfoEx) will also automatically register the
			// block ID, lighting, and opacity data with internal tables in the BlockInfo
			BlockInfoEx Factory_Barrel = (BlockInfoEx) new BlockInfoEx(BlockTypeM.FACTORY_BARREL, "factory_barrel").SetOpacity(0);
			
			// You can redefine already-registered blocks at any time by creating a new
			// BlockInfo object with the given ID.
			Factory_Barrel.SetTileEntity(BARREL_ID);
		}
	}
	
	public class FactoryBarrel : TileEntity
	{
		
		
		public static readonly SchemaNodeCompound BarrelSchema = TileEntity.Schema.MergeInto(new SchemaNodeCompound("")
		{
		new SchemaNodeScaler("upgrade", TagType.TAG_INT),
		new SchemaNodeString("id", TypeId),
		new SchemaNodeScaler("facing", TagType.TAG_BYTE),
		new SchemaNodeScaler("draw_active_byte",TagType.TAG_BYTE),
		new SchemaNodeScaler("ver", TagType.TAG_STRING),
		new SchemaNodeScaler("item_count",TagType.TAG_INT),
		new SchemaNodeCompound ("item_type", new SchemaNodeCompound("item_type") {
		                        	new SchemaNodeScaler("Count", TagType.TAG_BYTE),
		                        	new SchemaNodeScaler("Damage", TagType.TAG_SHORT),
		                        	new SchemaNodeScaler("id", TagType.TAG_SHORT),
		                        })
		});

		
		public static string TypeId 
        {
            get { return "factory_barrel"; }
        }
		
		private int _CAPACITY = 192;
		
		private Item _items;
		
		protected FactoryBarrel (string id)
			: base(id)
		{
			_items = new Item();
		}

		public FactoryBarrel ()
			: this(TypeId)
		{
		}

		public FactoryBarrel (TileEntity te)
			: base(te)
		{
			FactoryBarrel tec = te as FactoryBarrel;
			if (tec != null) {
				
			}
			if (tec != null) {
				TagNodeCompound block;
				TagNode block1;
				TagNode blockid;
				TagNode blockdamage;
				TagNode blockcount;
				
				tec.Source.TryGetValue("item_type", out block1);
				
				block = block1.ToTagCompound();
				
				block.TryGetValue("id", out blockid);
				block.TryGetValue("Damage", out blockdamage);
				block.TryGetValue("Count", out blockcount);
				
				_items = new Item(blockid.ToTagInt().Data);
				_items.Damage = blockdamage.ToTagInt().Data;
				_items.Count = blockcount.ToTagInt().Data;
				
				//_items = tec._items.Copy();
			}
			else {
				_items = new Item();
			}
		}
		
		#region ICopyable<TileEntity> Members

		public override TileEntity Copy ()
		{
			return new FactoryBarrel(this);
		}

		#endregion


		#region IItemContainer Members

		public Item Item
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

            TagNodeCompound items = ctree["item_type"].ToTagCompound();
            _items = new Item().LoadTree(items);

            return this;
        }

        public override TagNode BuildTree ()
        {
            TagNodeCompound tree = base.BuildTree() as TagNodeCompound;
            tree["item_type"] = _items.BuildTree();

            return tree;
        }

        public override bool ValidateTree (TagNode tree)
        {
            return new NbtVerifier(tree, BarrelSchema).Verify();
        }

        #endregion
	}
	
}