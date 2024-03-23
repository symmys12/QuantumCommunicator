using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace QuantumCommunicator.Helper
{
    public static class TextHelper
    {
        public static List<string> WrapText(string text, float maxWidth)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            List<string> lines = new List<string>();
            string[] words = text.Split(' ');
            
            string currentLine = "";
            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(currentLine + word);
                
                if (size.X > maxWidth && currentLine.Length > 0)
                {
                    lines.Add(currentLine);
                    currentLine = "";
                }
                
                if (currentLine.Length > 0)
                    currentLine += " ";
                currentLine += word;
            }
            
            if (currentLine.Length > 0)
                lines.Add(currentLine);
            
            return lines;
        }
    }
}