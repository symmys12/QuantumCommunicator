using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuantumCommunicator.Helper;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace QuantumCommunicator.Interface
{
    public class AddNewTeleportLocationUI : UIState
    {

        internal DragableUIPanel AddNewTeleportPanel;
        private CustomUITextBox uiText;
        public static bool visible = false;
        private float width = 300f;
        private float height = 155f;
        public float oldScale;
        private const float maxTeleportLocations = 20;
        private int MaxTextLength = 100;

        public override void OnInitialize()
        {
            AddNewTeleportPanel = new DragableUIPanel(10f, 40f, width, false);
            AddNewTeleportPanel.Left.Set(580f, 0f);
            AddNewTeleportPanel.Top.Set(100f, 0f);
            AddNewTeleportPanel.Width.Set(width, 0f);
            AddNewTeleportPanel.Height.Set(height, 0f);
            AddNewTeleportPanel.BackgroundColor = new Color(30, 43, 46);
            AddNewTeleportPanel.BorderColor = new Color(110, 156, 167);
           
            UIDivider divider = new UIDivider(2, Color.DimGray);
            divider.Left.Set(0, 0f);
            divider.Top.Set(40f, 0f);
            divider.Width.Set(0, 1f);
            AddNewTeleportPanel.Append(divider);

            uiText = new CustomUITextBox("");
            uiText.Width.Set(width, 0f);
            uiText.Height.Set(30, 0f);
            uiText.SetTextMaxLength(MaxTextLength);
            AddNewTeleportPanel.AddToContent(uiText);

			Asset<Texture2D> buttonDeleteTexture = ModContent.Request<Texture2D>("QuantumCommunicator/UI/Images/close");
			UIImageButton closeButton = new UIImageButton(buttonDeleteTexture);
			closeButton.Left.Set(width - 60, 0f);
			closeButton.Top.Set(0, 0f);
			closeButton.Width.Set(32, 0f);
			closeButton.Height.Set(32, 0f);
			closeButton.OnLeftClick += CloseButton;
			AddNewTeleportPanel.Append(closeButton);

            Asset<Texture2D> buttonTexture = ModContent.Request<Texture2D>("QuantumCommunicator/UI/Images/save");
            UIImageButton saveButton = new UIImageButton(buttonTexture);
            saveButton.Left.Set(0, 0.45f);
            saveButton.Top.Set(0, 0f);
            saveButton.Width.Set(32, 0f);
            saveButton.Height.Set(32, 0f);
            saveButton.OnLeftClick += (evt, listeningElement) =>
            {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                if(maxTeleportLocations <= WorldSaver.GetSavedCoordinates().Count){
                    Main.NewText("You have reached the maximum number of locations.");
                    return;
                }
                string text = CustomUITextBox.text;
                if(text == "") return;
                List<NamedCoordinate> savedCoordinates = WorldSaver.GetSavedCoordinates();
                if(savedCoordinates.Exists(x => x.Name == text)){
                    Main.NewText("Location already exists");
                    return;
                }
                Vector2 location = new Vector2(Main.LocalPlayer.position.X, Main.LocalPlayer.position.Y);
                WorldSaver.AddCoordinate(text, location);
                CustomUITextBox.text = "";
                CustomUITextBox.Unfocus();
                QuantumCommunicatorMenuSystem.quantumCommunicatorMenuUI.UpdateUI(); 
                AddNewTeleportLocationMenuSystem.addNewTeleportLocationMenuSystem.HideMyUI();
            };

            AddNewTeleportPanel.AddToContent(saveButton);
            
            Append(AddNewTeleportPanel);
        }
        public void ClearUIText(){
            uiText.SetText("");
        }
        private void CloseButton(UIMouseEvent evt, UIElement listeningElement)
		{
            SoundEngine.PlaySound(SoundID.MenuClose);
            CustomUITextBox.text = "";
            CustomUITextBox.Unfocus();
            AddNewTeleportLocationMenuSystem.addNewTeleportLocationMenuSystem.HideMyUI();
            AddNewTeleportLocationMenuSystem.teleportLocationUI.Unfocus();
		}
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
			if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}
            if (oldScale != Main.inventoryScale)
            {
                oldScale = Main.inventoryScale;
                Recalculate();
            }
        }

        public void UpdatePos(Vector3 pos){
            AddNewTeleportPanel.Left.Set(pos.X + pos.Y + 10f, 0);
            AddNewTeleportPanel.Top.Set(pos.Z, 0f);
        }
        public void Unfocus()
        {
            CustomUITextBox.text = "";
            CustomUITextBox.focused = false;
            Main.blockInput = false; // Optional: Unblock game input when not focused
        }
    }
}