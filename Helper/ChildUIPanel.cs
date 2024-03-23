using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace QuantumCommunicator.Helper
{
    public class ChildUIPanel : UIPanel
    {

        public float _contentWidth = 0f;
        public float _viewHeight = 0f;
        private float _totalContentHeight = 0f;
        private float _padding = 0f; // Set padding between elements
        private const float maxTeleportLocations = 20;
        public UIScrollbar ScrollBar;
        private readonly List<UIElement> _contentElements = new List<UIElement>();

        public ChildUIPanel(float padding, float contentHeight, float contentWidth, bool scrollBar)
        {
            _padding = padding;
            _viewHeight = contentHeight;
            _contentWidth = contentWidth;
            OverflowHidden = true; // Important to clip content outside the viewable area
            if (scrollBar)
            {
                ScrollBar = new UIScrollbar();
                ScrollBar.SetView(100f, 1000f);
                ScrollBar.OnScrollWheel += Scrollbar_OnScrollWheel;
                OnScrollWheel += Scrollbar_OnScrollWheel;
                Append(ScrollBar);
            }
        }
        private void Scrollbar_OnScrollWheel(UIScrollWheelEvent evt, UIElement listeningElement)
        {
            int dir = Math.Sign(evt.ScrollWheelValue);
            float scrollIncrement = 32f; // Change this value as needed

            // Calculate the new position, but don't update the scrollbar yet
            float newPosition = ScrollBar.ViewPosition + (dir * -scrollIncrement); // Negate direction to match content movement

            // Clamp the new position to prevent scrolling out of bounds
            newPosition = MathHelper.Clamp(newPosition, 0f, Math.Max(0f, _totalContentHeight - _viewHeight + 40f));

            // If there's a change (i.e., we're not at the boundaries), update the scrollbar and adjust content
            if (newPosition != ScrollBar.ViewPosition)
            {
                ScrollBar.ViewPosition = newPosition;
                
                // Calculate the offset to apply to content elements based on the scrollbar position
                float contentOffset = -newPosition;
                foreach (var e in _contentElements)
                {
                    e.Top.Pixels = contentOffset;
                    contentOffset += e.Height.Pixels; // Increment offset for the next element based on each element's height
                    e.Recalculate();
                }
            }
        }
        public void AddToContent(UIElement element)
        {
            // Adjust element's top position based on current content height and padding
            if(_totalContentHeight == 0){
                element.Top.Set(_totalContentHeight, 0f); // Add padding above each element
            }
            else{
                element.Top.Set(_totalContentHeight + _padding, 0f); // Add padding above each element
            }
            _totalContentHeight += element.Height.Pixels; // Increase total content height by element's height and padding

            _contentElements.Add(element); // Add element to content list for easy management
            Append(element); // Add the UIElement to the panel

            UpdateScrollbar();
        }
        public void AddLocationToPanel(string text, UIImageButton button, MouseEvent onClickAction)
        {
            UIRow row = new UIRow();
            string tempText = text;
            if(text.Length > 12){
                tempText = string.Concat(text.AsSpan(0, 10), "...");
            }
            UITextWithTooltip uiText = new UITextWithTooltip(tempText, text);
            // Create and configure the text element
            uiText.Left.Set(40, 0f); // Position at the start of the row
            uiText.Top.Set(8 + _padding, 0f); // Add a little padding to the top

            // Create and configure the button
            button.Left.Set(0, 0f); // Position after the text, with some spacing
            // button.Top.Set(2 + _padding, 0f); // Add a little padding to the top
            button.Width.Set(32, 0f); // Set the width of the button
            button.Height.Set(32, 0f); // Set the height of the button
            button.OnLeftClick += onClickAction; // Assign the click event handler

            Asset<Texture2D> deleteButton = ModContent.Request<Texture2D>("QuantumCommunicator/UI/Images/remove");
            UIImageButton delete = new UIImageButton(deleteButton);
            delete.Left.Set(_contentWidth - 80, 0f);
            delete.Top.Set(5, 0f);
            delete.Width.Set(26, 0f);
            delete.Height.Set(26, 0f);
            delete.OnLeftClick += (evt, element) => {RemoveChildAndAdjust(row); WorldSaver.RemoveCoordinate(uiText.Text);};

            // Add the text and button to the row
            row.Append(uiText);
            row.Append(button);
            row.Append(delete);
            AddToContent(row);
        }
        public void RemoveChildAndAdjust(UIElement element)
		{
			_contentElements.Remove(element); // Remove the element from the content list
			RemoveChild(element); // Remove the UIElement from the panel
			_totalContentHeight = 0;
			foreach(var e in _contentElements){
				e.Top.Set(_totalContentHeight + _padding, 0f); // Add padding above each element
				_totalContentHeight += e.Height.Pixels; // Increase total content height by element's height and padding
			}
			UpdateScrollbar();
		}
		public void ClearContent()
		{
			foreach(var element in _contentElements)
			{
				RemoveChild(element); // Remove the UIElement from the panel
			}
            _totalContentHeight = 0;
			_contentElements.Clear(); // Clear the content list
			UpdateScrollbar();
		}
		private void UpdateScrollbar()
		{
			// Assuming ScrollBar has been initialized and added to the panel
			if (ScrollBar != null)
			{
				ScrollBar.SetView(_viewHeight/maxTeleportLocations, _totalContentHeight); // Adjust view based on total content height
				ScrollBar.Height.Set(GetInnerDimensions().Height, 0f); // Ensure scrollbar height matches panel height
				ScrollBar.Left.Set(-10f, 1f); // Position scrollbar at the right edge
				
			}
		}
		public override void Recalculate()
		{
			base.Recalculate();
			UpdateScrollbar();
		}

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}
            foreach (UIRow textElement in _contentElements.Cast<UIRow>())
            {
                foreach(UIElement element in textElement.Children){
                    if (element is UITextWithTooltip)
                    {
                        UITextWithTooltip text = (UITextWithTooltip)element;
                        if (text.IsHovered)
                        {
                            Main.instance.MouseText(text.TooltipText);
                            break; // Assume only one tooltip can be active at a time
                        }
                    }
                }
            }
        }
    }

    public class UITextWithTooltip : UIText {
        public bool IsHovered { get; private set; }
        public string TooltipText { get; private set; }

        public UITextWithTooltip(string text, string tooltip) : base(text)
        {
            TooltipText = tooltip;
            IsHovered = false;
        }
        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            IsHovered = true; // set hovered state
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
            IsHovered = false; // clear hovered state
        }
    }
}