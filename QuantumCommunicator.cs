using Terraria.ModLoader;
using log4net;
using Terraria;


namespace QuantumCommunicator
{
	public class QuantumCommunicator : Mod
	{
		internal static QuantumCommunicator instance;

        public static new ILog Logger { get; } = LogManager.GetLogger(typeof(QuantumCommunicator));
        
		public override void Load()
		{
			Main.blockInput = false;
			instance = this;
		}
        public override void Unload()
		{
			instance = null;
		}
		public enum QuantumCommunicatorMessage : byte
        {
            TeleportPlayer
        }

	}
}