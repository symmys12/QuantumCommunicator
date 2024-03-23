using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace QuantumCommunicator.Helper
{
	public class TeleportHandler : Entity
	{
		
		public static void HandleTeleport(Player player, Vector2 coords, int teleportType = 0, bool forceHandle = false, int whoAmI = 0)
		{
			bool syncData = forceHandle || Main.netMode == NetmodeID.SinglePlayer;
			if (syncData)
			{
                RunTeleport(player, coords, syncData, false);
			}
			else
			{
				SyncTeleport(teleportType);
			}
		}
		
		private static void SyncTeleport(int teleportType = 0)
		{
			var netMessage = QuantumCommunicator.instance.GetPacket();
			netMessage.Write((byte)QuantumCommunicator.QuantumCommunicatorMessage.TeleportPlayer);
			netMessage.Write(teleportType);
			netMessage.Send();
		}

		private static void RunTeleport(Player player, Vector2 pos, bool syncData = false, bool convertFromTiles = false)
		{
			bool postImmune = player.immune;
			int postImmunteTime = player.immuneTime;

			if (convertFromTiles)
				pos = new Vector2(pos.X * 16 + 8 - player.width / 2, pos.Y * 16 - player.height);

			LeaveDust(player);

			//Kill hooks
			player.grappling[0] = -1;
			player.grapCount = 0;
			for (int index = 0; index < 1000; ++index)
			{
				if (Main.projectile[index].active && Main.projectile[index].owner == player.whoAmI && Main.projectile[index].aiStyle == 7)
					Main.projectile[index].Kill();
			}

			player.Teleport(pos, 2, 0);
			player.velocity = Vector2.Zero;
			player.immune = postImmune;
			player.immuneTime = postImmunteTime;

			LeaveDust(player);

			if (Main.netMode != NetmodeID.Server)
				return;

			if (syncData)
			{
				RemoteClient.CheckSection(player.whoAmI, player.position, 1);
				NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, (float)player.whoAmI, pos.X, pos.Y, 3, 0, 0);
			}
		}

		private static void LeaveDust(Player player)
		{
			//Leave dust
			for (int index = 0; index < 70; ++index)
				Main.dust[Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
			Main.TeleportEffect(player.getRect(), 1);
			Main.TeleportEffect(player.getRect(), 3);
		}
	}
}