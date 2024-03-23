using System.Collections.Generic;
using System.Linq;
using log4net.Repository.Hierarchy;
using QuantumCommunicator.Items;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace QuantumCommunicator.Helper 

{
    public class ItemManipulationSystem : ModSystem
    {

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Floating Island Houses"));
            if (genIndex != -1)
            {
                int indexToInsert = genIndex + 1;
                tasks.Insert(indexToInsert, new PassLegacy("QuantumCommunicator: Item Manipulation", delegate (GenerationProgress progress, GameConfiguration configuration)
                {
                    progress.Message = "Placing Moon Shard in Chest ...";

                    // Iterate over all chests to find a Sky Island chest
                    for (int i = 0; i < Main.chest.Length - 1; i++)
                    {
                        Chest chest = Main.chest[i];
                        if (chest != null)
                        {
                            // Check if the chest is in the sky layer (using world surface as a rough estimate)
                            // and not already containing items to differentiate from pre-generated special chests
                            // This is a basic check and might need adjustments depending on your needs
                            if (chest != null && chest.y < Main.spawnTileY)
                            {
                                Tile tile = Main.tile[chest.x, chest.y];
                                int chestStyle = tile.TileFrameX / 36;
                                Mod.Logger.Debug("Checking chest " + i + " at position " + chest.x + ", " + chest.y + " with style " + chestStyle);
                                if(chestStyle != 13){
                                    continue;
                                }

                                // Assuming MyItemID is the ID of your custom item
                                int myItemID = ModContent.ItemType<StrangeMoonShard>(); // Replace MyCustomItem with your actual item class
                                int emptySlotIndex = -1;
                                for(int itemSlot = 0; itemSlot < chest.item.Length; itemSlot++)
                                {
                                    if(chest.item[itemSlot] == null || chest.item[itemSlot].type == ItemID.None)
                                    {
                                        emptySlotIndex = itemSlot;
                                        break; // Exit the loop once the item has been placed
                                    }
                                }
                                if(emptySlotIndex != -1)
                                {
                                    // Place the item in the chest
                                    chest.item[emptySlotIndex].SetDefaults(myItemID);
                                    chest.item[emptySlotIndex].stack = 1; // Set the amount of the item
                                    Mod.Logger.Debug("Placed Moon Shard in chest " + i + " at slot " + emptySlotIndex + " at position " + chest.x + ", " + chest.y);
                                    break; // Exit the loop once the item has been placed
                                }
                            }
                        }
                    }
                }));
            }
            base.ModifyWorldGenTasks(tasks, ref totalWeight);
        }

        private bool IsSkywareChest(Chest chest){
            Tile tile = Main.tile[chest.x, chest.y];
            int chestStyle = tile != null ? tile.TileFrameX / 36 : 0;
            if(chestStyle != 13){
                return false;
            }
            return true;
        }
    }
}