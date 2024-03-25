using System.Collections.Generic;
using Microsoft.Xna.Framework;
using QuantumCommunicator.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace QuantumCommunicator.Helper
{
    public class WorldSaver : ModSystem
    {
        private static List<NamedCoordinate> savedCoordinates = new List<NamedCoordinate>();
        string tagName;
        public static void AddCoordinate(string name, Vector2 coord)
        {
            savedCoordinates.Add(new NamedCoordinate(name, coord));
        }
        public override void SaveWorldData(TagCompound tag)
        {
            List<TagCompound> coordinateTags = new List<TagCompound>();
            foreach (NamedCoordinate namedCoord in savedCoordinates)
            {
                coordinateTags.Add(new TagCompound {
                    {"Name", namedCoord.Name},
                    {"X", namedCoord.Coordinates.X},
                    {"Y", namedCoord.Coordinates.Y}
                });
            }
            if(tagName != null) tag.Add(tagName, coordinateTags);
        }
        public override void OnWorldLoad()
        {
            base.OnWorldLoad();
            Player player = Main.player[Main.myPlayer];
            tagName = Main.worldID+"_"+player.whoAmI+"_SavedNamedCoordinates";
        }
        public override void LoadWorldData(TagCompound tag)
        {
            savedCoordinates.Clear();
            if (tag.ContainsKey(tagName))
            {
                IList<TagCompound> coordinateTags = tag.GetList<TagCompound>(tagName);
                foreach (TagCompound coordinateTag in coordinateTags)
                {
                    string name = coordinateTag.GetString("Name");
                    float x = coordinateTag.GetFloat("X");
                    float y = coordinateTag.GetFloat("Y");
                    savedCoordinates.Add(new NamedCoordinate(name, new Vector2(x, y)));
                }
            }

        }

        public static void RemoveCoordinate(string name)
        {
            savedCoordinates.RemoveAll(x => x.Name == name);
        }

        public static List<NamedCoordinate> GetSavedCoordinates()
        {
            return savedCoordinates;
        }
    }
}