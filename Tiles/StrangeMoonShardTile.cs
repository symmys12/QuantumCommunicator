using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuantumCommunicator.Items;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace QuantumCommunicator.Tiles
{

    public class StrangeMoonShardTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileLighted[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileFrame[Type] = 0;
            Main.tileNoFail[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;

            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();

            AddMapEntry(new Color(200, 200, 200), name);
        }

        // This method allows you to determine how much light this block emits
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
			r = 0.93f;
			g = 0.11f;
			b = 0.12f;
        }

        public override bool CanPlace(int i, int j)
        {
            // Check for solid tile below or wall behind.
            bool isSolidBelow = j + 1 < Main.maxTilesY && Main.tile[i, j + 1].HasTile && Main.tileSolid[Main.tile[i, j + 1].TileType];
            bool hasWallBehind = Main.tile[i, j].WallType > 0;
            bool isOverlappingTile = Main.tile[i, j].TileType > 0;
            Main.NewText("overlapping: " + Main.tile[i, j].TileType);
            // Allow placement if there's a solid tile below or a wall behind, but not on top of another solid tile/entity.
            if ((isSolidBelow || hasWallBehind) && !isOverlappingTile)
            {
                Main.NewText("CanPlace");
                return true;
            }
            Main.NewText("Cannot Place");
            return false; // The tile below is not solid, prevent placement.
        }
        private const int animationFrameWidth = 34; // the width of the tile in pixels
		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset) {
			// Tweak the frame drawn by x position so tiles next to each other are off-sync and look much more interesting
			int uniqueAnimationFrame = Main.tileFrame[Type] + i;
			if (i % 2 == 0)
				uniqueAnimationFrame += 3;
			if (i % 3 == 0)
				uniqueAnimationFrame += 3;
			if (i % 4 == 0)
				uniqueAnimationFrame += 3;
			uniqueAnimationFrame %= 6;

			// frameYOffset = modTile.animationFrameHeight * Main.tileFrame [type] will already be set before this hook is called
			// But we have a horizontal animated texture, so we use frameXOffset instead of frameYOffset
			frameXOffset = uniqueAnimationFrame * animationFrameWidth;
		}
        public override void AnimateTile(ref int frame, ref int frameCounter) {
			// Or, more compactly:
			if (++frameCounter >= 9) {
				frameCounter = 0;
				frame = ++frame % 6;
			}
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}