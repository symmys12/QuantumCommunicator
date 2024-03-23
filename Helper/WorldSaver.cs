using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using QuantumCommunicator.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace QuantumCommunicator.Helper
{
    public class WorldSaver : ModSystem
    {
        private static List<NamedCoordinate> savedCoordinates = new List<NamedCoordinate>();
        private bool shardDropped = false;
        string tagName;
        string tagForShard;
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
            if(shardDropped){
                tag.Add(tagForShard, shardDropped);
                Mod.Logger.Info("Shard dropped and saved");
            }
        }
        public override void OnWorldLoad()
        {
            base.OnWorldLoad();
            tagName = Main.worldID+"_SavedNamedCoordinates";
            tagForShard = Main.worldID+"_ShardDropped";
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

            if(!tag.ContainsKey(tagForShard)){
                IEntitySource source = new EntitySource_WorldEvent("ShardDroppedEvent");
                int dropX = Main.spawnTileX;
                int dropY = 50; // 600 pixels above the player

                // Ensure the dropY is within world bounds (not above the top of the world)

                int itemIndex = Item.NewItem(source, new Vector2(dropX, dropY), ModContent.ItemType<StrangeMoonShard>());
                if (itemIndex >= 0 && itemIndex < Main.item.Length)
                {
                    Item item = Main.item[itemIndex];

                    // Apply an initial velocity to simulate falling
                    item.velocity.Y = Main.rand.Next(10, 20) / 10f; // Downward velocity

                    // If this action is happening on a server, sync the item spawn to all clients
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, itemIndex, 1f);
                    }
                    // Now we set the flag to indicate the item has been dropped.
                    Mod.Logger.Info("Shard dropped " + item.position.X + " " + item.position.Y);
                    shardDropped = true;
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