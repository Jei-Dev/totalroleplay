using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace totalRoleplay
{
	[Serializable]
	public class Configuration : IPluginConfiguration
	{
		public bool showTextNotify { get; set; } = true;

		private static int VersionLatest = 0;
		public int Version { get; set; } = VersionLatest;

		public bool BooleanProperty { get; set; } = true;

		// the below exist just to make saving less cumbersome
		[NonSerialized]
		private DalamudPluginInterface pluginInterface;

		public void Initialize(DalamudPluginInterface pluginInterface)
		{
			this.pluginInterface = pluginInterface;

			var needsResave = (Version != VersionLatest);
			if (needsResave) { Save(); }
		}

		public void Save()
		{
			pluginInterface.SavePluginConfig(this);
		}
	}
}
