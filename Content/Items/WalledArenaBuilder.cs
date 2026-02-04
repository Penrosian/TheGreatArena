using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheGreatArena.Content.Items
{
    public class WalledArenaBuilder : ArenaBuilder
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ArenaBuilder>()
                .AddIngredient(ItemID.GrayBrickWall, 4000)
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
                }
            }
        }

        new internal static void ClearRow(int y, int width)
        {
            for (int x = 0; x < width; x++)
            {
                Point16 tl = Terraria.ObjectData.TileObjectData.TopLeft(x, y);
                Chest.DestroyChestDirect(tl.X, tl.Y, Chest.FindChest(tl.X, tl.Y));
                WorldGen.KillTile(x, y, noItem: true);
                WorldGen.ConvertWall(x, y, WallID.GrayBrick);
            }
        }

        private void SendPacket(int targetY, int width)
        {
            const int packetType = 2;
            ModPacket packet = Mod.GetPacket();
            packet.Write(packetType);
            packet.Write(targetY);
            packet.Write(width);
            packet.Send();
        }
    }
}