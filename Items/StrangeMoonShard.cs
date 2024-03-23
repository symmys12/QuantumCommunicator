using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuantumCommunicator.Items
{
	public class StrangeMoonShard : ModItem
	{
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.PipBoy.hjson file.
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(15, 8));
		}
		public override void SetDefaults()
		{
			Item.width = 25;
			Item.height = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 0;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.rare = ItemRarityID.Gray;
			Item.createTile = ModContent.TileType<Tiles.StrangeMoonShardTile>();
		}
	}
}