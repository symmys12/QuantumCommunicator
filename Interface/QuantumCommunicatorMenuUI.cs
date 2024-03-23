using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ID;
using ReLogic.Content;
using Terraria.Audio;
using QuantumCommunicator.Helper;
using System.Collections.Generic;

namespace QuantumCommunicator.Interface
{
	internal class QuantumCommunicatorMenuUI : UIState
	{
        public DragableUIPanel QuantumComPanel;
        public ChildUIPanel childPanel;
		public static bool visible = false;
        private float width = 300f;
        private float height = 300f;
        private float topMargin = 38f;
        public float oldScale;
        public override void OnInitialize(){
            QuantumComPanel = new DragableUIPanel(2f, 40f, width, true);
            QuantumComPanel.PaddingRight = 10f;
			QuantumComPanel.Left.Set(250f, 0f);
			QuantumComPanel.Top.Set(400f, 0f);
			QuantumComPanel.Width.Set(width, 0f);
			QuantumComPanel.Height.Set(height, 0f);
            QuantumComPanel.BackgroundColor = new Color(30, 43, 46);
            QuantumComPanel.BorderColor = new Color(110, 156, 167);
            QuantumComPanel.pos = new Vector3(QuantumComPanel.Left.Pixels, QuantumComPanel.Width.Pixels, QuantumComPanel.Top.Pixels);

            UIDivider divider = new UIDivider(2, Color.DimGray);
            divider.Left.Set(0, 0f);
            divider.Top.Set(topMargin, 0f);
            divider.Width.Set(0, 1f);
            QuantumComPanel.Append(divider);

            //close button
			Asset<Texture2D> closeDeleteTexture = ModContent.Request<Texture2D>("QuantumCommunicator/UI/Images/close");
			UIImageButton closeButton = new UIImageButton(closeDeleteTexture);
			closeButton.Left.Set(width - 60, 0f);
			closeButton.Top.Set(0, 0f);
			closeButton.Width.Set(32, 0f);
			closeButton.Height.Set(32, 0f);
			closeButton.OnLeftClick += CloseButton;
			QuantumComPanel.Append(closeButton);

            //add location button 
            Asset<Texture2D> buttonAddTexture = ModContent.Request<Texture2D>("QuantumCommunicator/UI/Images/add-new");
            UIImageButton addButton = new UIImageButton(buttonAddTexture);
            addButton.Left.Set(0, 0f);
            addButton.Top.Set(0, 0f);
            addButton.Width.Set(32, 0f);
            addButton.Height.Set(32, 0f);
            addButton.OnLeftClick += AddTeleportLocation;
            QuantumComPanel.Append(addButton);
            Append(QuantumComPanel);

            childPanel = new ChildUIPanel(1f, height - 90, width - 20, true);
			childPanel.Width.Set(width - 20, 0f);
			childPanel.Height.Set(height-80, 0f);
			childPanel.Left.Set(0, 0f);
			childPanel.Top.Set(topMargin + 10, 0f);
            childPanel.BackgroundColor = new Color(30, 43, 46);
            childPanel.BorderColor = new Color(110, 156, 167);
            childPanel.OverflowHidden = true;
        }
		public void UpdateUI()
		{

            Player player = Main.player[Main.myPlayer];
            childPanel.ClearContent();
			List<NamedCoordinate> savedCoordinates = WorldSaver.GetSavedCoordinates();
            Asset<Texture2D> buttonTexture = ModContent.Request<Texture2D>("QuantumCommunicator/UI/Images/gogo");

			foreach(var coord in savedCoordinates)
			{
                UIImageButton button = new UIImageButton(buttonTexture);
                childPanel.AddLocationToPanel(coord.Name, button, (evt, args) => TeleportHandler.HandleTeleport(player, coord.Coordinates));
			}
            QuantumComPanel.Append(childPanel);
		}
        private void AddTeleportLocation(UIMouseEvent evt, UIElement listeningElement){
            SoundEngine.PlaySound(SoundID.MenuOpen);
            AddNewTeleportLocationMenuSystem.teleportLocationUI.UpdatePos(QuantumComPanel.pos);
            AddNewTeleportLocationMenuSystem.addNewTeleportLocationMenuSystem.ShowMyUI();
        }
		
        private void CloseButton(UIMouseEvent evt, UIElement listeningElement)
		{
            SoundEngine.PlaySound(SoundID.MenuClose);
			QuantumCommunicatorMenuSystem.quantumCommunicatorMenuSystem.HideMyUI();
            AddNewTeleportLocationMenuSystem.addNewTeleportLocationMenuSystem.HideMyUI();
            AddNewTeleportLocationMenuSystem.teleportLocationUI.Unfocus();
		}
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(oldScale != Main.inventoryScale){
                oldScale = Main.inventoryScale;
                Recalculate();
            }
        }
    }
    
}