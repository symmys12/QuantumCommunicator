using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuantumCommunicator.Interface;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace QuantumCommunicator.Helper
{
	// This DragableUIPanel class inherits from UIPanel. 
	// Inheriting is a great tool for UI design. By inheriting, we get the background drawing for free from UIPanel
	// We've added some code to allow the panel to be dragged around. 
	// We've also added some code to ensure that the panel will bounce back into bounds if it is dragged outside or the screen resizes.
	// UIPanel does not prevent the player from using items when the mouse is clicked, so we've added that as well.
	internal class DragableUIPanel : UIPanel
	{
		// Stores the offset from the top left of the UIPanel while dragging.
		private Vector2 offset;
		public bool dragging;
		public float _contentHeight = 0f;
		public float _contentWidth = 0f;
		private float _currHeight = 0f;
		private float _padding = 0f; // Set padding between elements
		public UIScrollbar ScrollBar;
		public Vector3 pos = new Vector3(0, 0, 0);

		public DragableUIPanel(float padding, float contentHeight, float contentWidth, bool scrollBar)
		{
			_padding = padding;
			_contentHeight = contentHeight;
			_currHeight = contentHeight;
			_contentWidth = contentWidth;
			OverflowHidden = true; // Important to clip content outside the viewable area
		}

        public void AddToContent(UIElement element)
		{
			// Adjust element's top position based on current content height and padding
			element.Top.Set(_currHeight + _padding, 0f); // Add padding above each element
			_currHeight += element.Height.Pixels + _padding; // Increase total content height by element's height and padding

			Append(element); // Add the UIElement to the panel
		}
		public override void Recalculate()
		{
			base.Recalculate();
		}
		public override void LeftMouseDown(UIMouseEvent evt)
		{
			base.LeftMouseDown(evt);
			DragStart(evt);
		}
		public override void LeftMouseUp(UIMouseEvent evt)
		{
			base.LeftMouseUp(evt);
			DragEnd(evt);
		}

		private void DragStart(UIMouseEvent evt)
		{
			if(evt.Target is UIImageButton || evt.Target is UITextBox || evt.Target is UIText || evt.Target is UIScrollbar || evt.Target is ChildUIPanel || evt.Target is UIRow){
				return;
			}
			offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
			dragging = true;
		}

		private void DragEnd(UIMouseEvent evt)
		{
			if(evt.Target is UIImageButton || evt.Target is UITextBox || evt.Target is UIText || evt.Target is UIScrollbar || evt.Target is ChildUIPanel || evt.Target is UIRow){
				return;
			}
			Vector2 end = evt.MousePosition;
			dragging = false;

			Left.Set(end.X - offset.X, 0f);
			Top.Set(end.Y - offset.Y, 0f);

			pos = new Vector3(Left.Pixels, Width.Pixels, Top.Pixels);
			Recalculate();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime); // don't remove.
			// Checking ContainsPoint and then setting mouseInterface to true is very common. This causes clicks on this UIElement to not cause the player to use current items. 
			if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}

			if (dragging)
			{
				Left.Set(Main.mouseX - offset.X, 0f); // Main.MouseScreen.X and Main.mouseX are the same.
				Top.Set(Main.mouseY - offset.Y, 0f);
				Recalculate();
			}

			// Here we check if the DragableUIPanel is outside the Parent UIElement rectangle. 
			// (In our example, the parent would be ExampleUI, a UIState. This means that we are checking that the DragableUIPanel is outside the whole screen)
			// By doing this and some simple math, we can snap the panel back on screen if the user resizes his window or otherwise changes resolution.
			var parentSpace = Parent.GetDimensions().ToRectangle();
			if (!GetDimensions().ToRectangle().Intersects(parentSpace))
			{
				Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
				Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
				// Recalculate forces the UI system to do the positioning math again.
				Recalculate();
			}
		}
    }
}