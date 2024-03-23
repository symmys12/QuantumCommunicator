using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using QuantumCommunicator.Items;

namespace QuantumCommunicator.Interface {

	public class QuantumCommunicatorMenuSystem : ModSystem {
		internal static QuantumCommunicatorMenuUI quantumCommunicatorMenuUI;
		public static UserInterface userInterface;
		public static QuantumCommunicatorMenuSystem quantumCommunicatorMenuSystem;
        public void ShowMyUI() {
            QuantumCommunicatorMenuUI.visible = true;
		    userInterface?.SetState(quantumCommunicatorMenuUI);
		}

		public void HideMyUI() {
            QuantumCommunicatorMenuUI.visible = false;
			userInterface?.SetState(null);
		}
		public override void UpdateUI(GameTime gameTime) {
			// Here we call .Update on our custom UI and propagate it to its state and underlying elements
			if (userInterface != null) {
							// it will only draw if the player is not on the main menu
				if (!Main.gameMenu && userInterface.CurrentState != null)
				{
					userInterface?.Update(gameTime);
				}
			}
		}

        public override void OnWorldLoad() {
            quantumCommunicatorMenuUI = new QuantumCommunicatorMenuUI();
            userInterface = new UserInterface();
            quantumCommunicatorMenuSystem = new QuantumCommunicatorMenuSystem();
            quantumCommunicatorMenuUI.Initialize();
            userInterface.SetState(quantumCommunicatorMenuUI);
            quantumCommunicatorMenuSystem.HideMyUI();
        }

        public override void Load()
        {
            On_ItemSlot.RightClick_ItemArray_int_int += On_ItemSlotRightClick;
        }

        public override void Unload()
        {
            On_ItemSlot.RightClick_ItemArray_int_int -= On_ItemSlotRightClick;
        }
        public override void OnWorldUnload()
        {
            quantumCommunicatorMenuUI.Remove();
            userInterface.SetState(null);
            quantumCommunicatorMenuSystem = null;
        }
        // Adding a custom layer to the vanilla layer list that will call .Draw on your interface if it has a state
        // Setting the InterfaceScaleType to UI for appropriate UI scaling
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1) {
			    layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("Quantum Communicator: Teleport UI", DrawTeleportUI, InterfaceScaleType.UI));
            }
		}
		private bool DrawTeleportUI() {
			if (!Main.gameMenu && userInterface.CurrentState != null)
			{
				userInterface.Draw(Main.spriteBatch, new GameTime());
			}
			return true;
		}
        internal static void On_ItemSlotRightClick(On_ItemSlot.orig_RightClick_ItemArray_int_int orig, Item[] inv, int context, int slot)
        {
			orig(inv, context, slot);
			if (!Main.mouseRight)
				return;
            if (context == 0 && Main.mouseRightRelease){
				if (inv[slot].type == ModContent.ItemType<QuantumCommunicatorItem>()){
					quantumCommunicatorMenuUI.UpdateUI();
					if(QuantumCommunicatorMenuUI.visible){
						quantumCommunicatorMenuSystem.HideMyUI();
					} else {
						quantumCommunicatorMenuSystem.ShowMyUI();
					}
					if(AddNewTeleportLocationUI.visible){
                        AddNewTeleportLocationMenuSystem.addNewTeleportLocationMenuSystem.HideMyUI();
                    }
				}
			}
        }
	}
}