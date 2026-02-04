using System.IO;
using Terraria.ModLoader;
using TheGreatArena.Content.Items;

namespace TheGreatArena
{
	public class TheGreatArena : Mod
	{
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			int packetType = reader.ReadInt32();
			int targetY = reader.ReadInt32();
			int width = reader.ReadInt32();
			switch (packetType)
			{
				case 0:
					ArenaBuilder.HandleBuilding(targetY, width);
					break;
				case 1:
					HammeredArenaBuilder.HandleBuilding(targetY, width);
					break;
				case 2:
					WalledArenaBuilder.HandleBuilding(targetY, width);
					break;
				case 3:
					FullArenaBuilder.HandleBuilding(targetY, width);
					break;
			}
		}
	}
}
