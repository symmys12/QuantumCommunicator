using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using QuantumCommunicator.Helper;
using QuantumCommunicator.Items;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace QuantumCommunicator.Interface
{
    public class AddNewTeleportLocationMenuSystem : ModSystem
    {
        internal static AddNewTeleportLocationUI teleportLocationUI;
        public static AddNewTeleportLocationMenuSystem addNewTeleportLocationMenuSystem;
        private static UserInterface userInterface;
        public void ShowMyUI()
        {
            AddNewTeleportLocationUI.visible = true;
            userInterface?.SetState(teleportLocationUI);
        }

        public void HideMyUI()
        {
            teleportLocationUI.Unfocus();
            teleportLocationUI.ClearUIText();
            AddNewTeleportLocationUI.visible = false;
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
            teleportLocationUI = new AddNewTeleportLocationUI();
            userInterface = new UserInterface();
            addNewTeleportLocationMenuSystem = new AddNewTeleportLocationMenuSystem();
            teleportLocationUI.Initialize();
            userInterface.SetState(teleportLocationUI);
            addNewTeleportLocationMenuSystem.HideMyUI();
        }
        public override void OnWorldUnload()
        {
            teleportLocationUI.Unfocus();
            teleportLocationUI.Remove();
            userInterface.SetState(null);
            addNewTeleportLocationMenuSystem = null;
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1) {
			    layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("Quantum Communicator: Add Teleport Location UI", DrawAddLocationUI, InterfaceScaleType.UI));
            }
		}
        private bool DrawAddLocationUI() {
			if (!Main.gameMenu && userInterface.CurrentState != null)
			{
				userInterface.Draw(Main.spriteBatch, new GameTime());
			}
			return true;
		}
    }
}