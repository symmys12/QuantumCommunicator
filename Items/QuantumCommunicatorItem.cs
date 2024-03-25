
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Humanizer;
using System.Linq;

namespace QuantumCommunicator.Items
{
	public class QuantumCommunicatorItem : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.PipBoy.hjson file.
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(20, 6));
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.value = 5000000;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Master;

			Item.holdStyle = ItemHoldStyleID.None;
		}

		public override void UpdateInfoAccessory(Player player)
		{
			// Here you add the effects that the Cell Phone provides
			player.accWatch = 3;
			player.accDepthMeter = 1;
			player.accCompass = 1;
			player.accFishFinder = true;
			player.accWeatherRadio = true;
			player.accCalendar = true;
			player.accThirdEye = true;
			player.accJarOfSouls = true;
			player.accCritterGuide = true;
			player.accStopwatch = true;
			player.accOreFinder = true;
			player.accDreamCatcher = true;
		}

		public override void AddRecipes()
		{
			RecipeGroup recipeGroup = new RecipeGroup(() => Language.GetTextValue("Shellphone"), new int[]
            {
                ItemID.Shellphone,
				ItemID.ShellphoneDummy,
				ItemID.ShellphoneOcean,
				ItemID.ShellphoneHell,
				ItemID.ShellphoneSpawn
			});
			RecipeGroup.RegisterGroup("Shellphone", recipeGroup);
			
			Recipe recipe = CreateRecipe();

			recipe.AddIngredient<StrangeMoonShard>();
			recipe.AddIngredient(ItemID.RodOfHarmony);
			recipe.AddRecipeGroup("Shellphone");
			recipe.AddIngredient(ItemID.TeleportationPotion, 30);
			recipe.AddIngredient(ItemID.Teleporter, 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
		{
			player.ItemCheck_ManageRightClickFeatures();
			if (player.whoAmI != Main.myPlayer)
			{
				return true;
			}
			if(player.altFunctionUse == 2){
				player.Teleport(Main.MouseWorld, TeleportationStyleID.TeleportationPotion);
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item6, player.position);
				return true;
			}
			Vector2 spawnPosition = new Vector2(player.SpawnX * 16, player.SpawnY * 16);
			if (player.SpawnX == -1 && player.SpawnY == -1)
			{
				// If spawn point is not set, use the world's default spawn location
				spawnPosition = new Vector2(Main.spawnTileX * 16, Main.spawnTileY * 16);
			}
			spawnPosition.Y -= 48;
			player.Teleport(spawnPosition, TeleportationStyleID.TeleportationPotion);
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item6, player.position);
			return true;
		}


        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.player[Main.myPlayer];
			int indexOfFirst = 0;
			if(!Main.keyState.IsKeyDown(Keys.LeftShift)){
				// If the player is not holding shift, show the default tooltip
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, $"Left click to teleport home, right click (or press '{QuantumCommunicator.OpenMenuKey.GetAssignedKeys().FirstOrDefault()}') to open teleportation menu"));
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, $"Right clicking with the item in hand (or by pressing '{QuantumCommunicator.RodOfHarmonyKey.GetAssignedKeys().FirstOrDefault()}') will teleport you to the cursor's location"));
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "holdShift", "[c/d18c38:* Hold LEFT SHIFT to show most of the players stats *]"));
				return;
			} else {
				int genericCrit = (int)player.GetCritChance(DamageClass.Generic);
				string meleeDamage = (int)(player.GetDamage(DamageClass.Melee).Additive * 100 - 100) + "%" + " / " + (player.GetCritChance(DamageClass.Melee) + genericCrit) + "%";
				string playerAttackSpeed = (int)(player.GetAttackSpeed(DamageClass.Melee) * 100) + "%";
				string rangedDamage = (int)(player.GetDamage(DamageClass.Ranged).Additive * 100 - 100) + "%" + " / " +(player.GetCritChance(DamageClass.Ranged) + genericCrit) + "%";
				string magicDamage = (int)(player.GetDamage(DamageClass.Magic).Additive * 100 - 100) + "%" + " / " + (player.GetCritChance(DamageClass.Magic) + genericCrit) + "%";
				string throwingDamage = (int)(player.GetDamage(DamageClass.Throwing).Additive * 100 - 100) + "%" + " / " + (player.GetCritChance(DamageClass.Throwing) + genericCrit) + "%";
				string summonDamage = (int)(player.GetDamage(DamageClass.Summon).Additive * 100 - 100) + "%";
				string maxMinions = player.maxMinions + " / " + player.maxTurrets;
				string endurance = (player.endurance * 100) + "%";
				string moveSpeed = (player.moveSpeed * 100) + "%";
				string maxLife = (player.statLifeMax2 - player.statLifeMax).ToString();
				string manaReduction = (100 - (player.manaCost * 100)) + "%";
				string lifeRegen = player.lifeRegen.ToString();
		
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, $"Left click to teleport home, right click (or press '{QuantumCommunicator.OpenMenuKey.GetAssignedKeys().FirstOrDefault()}') to open teleportation menu"){OverrideColor = Color.Beige});
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, $"Right clicking with the item in hand (or by pressing '{QuantumCommunicator.RodOfHarmonyKey.GetAssignedKeys().FirstOrDefault()}') will teleport you to the cursor's location"){OverrideColor = Color.Beige});
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, "[c/243029:———————————————————————————————]"));
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, "Melee damage/critical strike chance boosts are " + meleeDamage){OverrideColor = Color.Red});
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, "Melee swing time is " + playerAttackSpeed){OverrideColor = Color.LimeGreen});
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, "Ranged damage/critical strike chance boosts are " + rangedDamage){OverrideColor = Color.SkyBlue});
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, "Magic damage/critical strike chance boosts are " + magicDamage){OverrideColor = Color.Orange});
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, "Thrown damage/critical strike chance boosts are " + throwingDamage){OverrideColor = Color.MediumVioletRed});
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, "Summoner damage boost is " + summonDamage){OverrideColor = Color.Gray});
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, "Max amounts of minions/sentries are " + maxMinions){OverrideColor = Color.Green});
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, "Damage Reduction boost is " + endurance){OverrideColor = Color.Yellow});
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, "Movement speed boost is " + moveSpeed){OverrideColor = Color.Blue});
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, "Max life boost is " + maxLife){OverrideColor = Color.DeepSkyBlue});
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, "Mana usage reduction is " + manaReduction){OverrideColor = Color.Magenta});
				tooltips.Insert(++indexOfFirst, new TooltipLine(Mod, "Tooltip"+indexOfFirst, "Life regeneration is " + lifeRegen){OverrideColor = Color.PaleVioletRed});
			}
		}
	}
}