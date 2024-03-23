using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.UI;

namespace QuantumCommunicator.Helper
{
    public class UIDivider : UIElement
    {
        private Color _color;
        private float _height;
        private float _width = 0.7f;

        public UIDivider(float height = 2, Color? color = null)
        {
            _height = height;
            _color = color ?? Color.Black; // Default color is Gray
            this.Height.Set(_height, 0f);
            this.Width.Set(0, _width); // Take the full width of the parent container
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle dimensions = GetDimensions();
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)_height), _color);
        }
    }
}