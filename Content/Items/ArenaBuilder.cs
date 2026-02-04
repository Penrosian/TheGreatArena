using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;

namespace TheGreatArena.Content.Items
{
    public class ArenaBuilder : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 10;
            Item.height = 32;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 1;
            Item.useTime = 120;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = Item.buyPrice(platinum: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Dynamite, 500)
                .AddIngredient(ItemID.WoodPlatform, 2000)
                .AddTile(TileID.MythrilAnvil)
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

        internal static void HandleBuilding(int targetY, int width)
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

        internal static void ClearRow(int y, int width)
        {
            for (int x = 0; x < width; x++)
            {
                Point16 tl = Terraria.ObjectData.TileObjectData.TopLeft(x, y);
                Chest.DestroyChestDirect(tl.X, tl.Y, Chest.FindChest(tl.X, tl.Y));
                WorldGen.KillTile(x, y, noItem: true);
                WorldGen.ConvertWall(x, y, WallID.None);
            }
        }

        internal static void PlaceRow(int y, int width)
        {
            for (int x = 0; x < width; x++)
            {
                WorldGen.PlaceTile(x, y, TileID.Platforms, style: 43);
            }
        }

        private void SendPacket(int targetY, int width)
        {
            const int packetType = 0;
            ModPacket packet = Mod.GetPacket();
            packet.Write(packetType);
            packet.Write(targetY);
            packet.Write(width);
            packet.Send();
        }
    }
}