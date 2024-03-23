using QuantumCommunicator.Helper;
using QuantumCommunicator.Interface;
using QuantumCommunicator.Items;
using Terraria;
using Terraria.GameContent.ItemDropRules;
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
    }
}