using Terraria.ModLoader;
using log4net;
using Terraria;


namespace QuantumCommunicator
{
	public class QuantumCommunicator : Mod
	{
		internal static QuantumCommunicator instance;

		internal static ModKeybind OpenMenuKey;
		internal static ModKeybind RodOfHarmonyKey;

        public static new ILog Logger { get; } = LogManager.GetLogger(typeof(QuantumCommunicator));
        
		public override void Load()
		{
			Main.blockInput = false;
			instance = this;

			OpenMenuKey = KeybindLoader.RegisterKeybind(this, "Open Quantum Communicator Teleport Menu", "O");
			RodOfHarmonyKey = KeybindLoader.RegisterKeybind(this, "Use Rod of Harmony", "R");
		}
        public override void Unload()
		{
			Main.blockInput = false;
			instance = null;

			OpenMenuKey = null;
			RodOfHarmonyKey = null;
		}
		public enum QuantumCommunicatorMessage : byte
        {
            TeleportPlayer
        }

	}
}