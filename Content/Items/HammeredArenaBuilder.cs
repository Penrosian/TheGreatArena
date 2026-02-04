using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheGreatArena.Content.Items
{
    public class HammeredArenaBuilder : ArenaBuilder
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ArenaBuilder>()
                .AddIngredient(ItemID.WoodenHammer)
                .Register();
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && !player.noBuilding)
            {
                int targetY = Player.tileTargetY;
                int width = Main.maxTilesX;
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    HandleBuilding(targetY, width);
                }
                else
                {
                    SendPacket(targetY, width);
                }
            }
            return true;
        }

        new internal static void HandleBuilding(int targetY, int width)
        {
            for (int y = 0; y <= targetY; y++)
            {
                ClearRow(y, width);
                if (y % 100 == 0 && y != 0)
                {
                    PlaceRow(y, width);
                    HammerRow(y, width);
                }
            }
        }

        private static void HammerRow(int y, int width)
        {
            for (int x = 0; x < width; x++)
            {
                WorldGen.SlopeTile(x, y, 1);
            }
        }

        private void SendPacket(int targetY, int width)
        {
            const int packetType = 1;
            ModPacket packet = Mod.GetPacket();
            packet.Write(packetType);
            packet.Write(targetY);
            packet.Write(width);
            packet.Send();
        }
    }
}