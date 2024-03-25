using QuantumCommunicator.Interface;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuantumCommunicator
{
    public class QuantumCommunicatorPlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            if (!Main.playerInventory)
            {
                // If the inventory is open and the panel is not visible, show the panel
                QuantumCommunicatorMenuSystem.quantumCommunicatorMenuSystem.HideMyUI();
                AddNewTeleportLocationMenuSystem.addNewTeleportLocationMenuSystem.HideMyUI();
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if(PlayerHasItem(ModContent.ItemType<Items.QuantumCommunicatorItem>())){
                if(QuantumCommunicator.OpenMenuKey.JustPressed)
                {
                    QuantumCommunicatorMenuSystem.quantumCommunicatorMenuSystem.ToggleUI();
                }
                if(QuantumCommunicator.RodOfHarmonyKey.JustPressed)
                {
                    Player player = Main.player[Main.myPlayer];
                    player.Teleport(Main.MouseWorld, TeleportationStyleID.TeleportationPotion);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item6, player.position);
                }
            }
        }
        private bool PlayerHasItem(int itemType)
        {
            for (int i = 0; i < Player.inventory.Length; i++)
            {
                if (Player.inventory[i].type == itemType && Player.inventory[i].stack > 0)
                {
                    return true; // Item found in inventory
                }
            }
            return false; // Item not found
        }
    }
}