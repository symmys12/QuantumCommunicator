using Terraria.UI;

public class UIRow : UIElement
	{
		public UIRow()
		{
			// Set the size of the container. Adjust these values based on your needs.
			Width.Set(0, 1f); // Full width of the parent
			Height.Set(32, 0f); // Height sufficient to contain a button and text
		}
	}