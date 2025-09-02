using IL.Terraria.DataStructures;
using Microsoft.Xna.Framework;
using On.Terraria.DataStructures;
using Terraria;
using Terraria.DataStructures;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;
using Main = Terraria.Main;

namespace SitemAtHome
{
    [ApiVersion(2, 1)]
    public class SitemAtHome : TerrariaPlugin
    {
        public override string Name => "Sitem At home";
        public override Version Version => new Version(0, 8, 4);
        public override string Author => "Geolindrag";
        public override string Description => "Mimics Sitem from Dark gaming";

        public SitemAtHome(Main game) : base(game) { }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(
                new Command(
                    Permissions.item, sitem, "sitem"
                )
                {
                    HelpText = "/sitem \"WEAPON NAME\" -Parameter -Value -Parameter -Value ...\nValid parameters :\n-d set damage (integer)\t-k set knockback (decimal)\n-ua Animation time (integer)\t-ut Usage time (integer)\n-s Projectile to shoot (Proj ID)\t-ss Projectile speed (integer)\n-sc Weapon scale (Decimal)\t-amt Amount of the item to give (int)\n-a Makes an item to be considered as ammo (AmmoID)\t-uam Ammo type consummed when shot (AmmoID)\n-na Enabling it makes that an ammo type can't go into the ammo slots (0/1)"
                }
            );
        }

        private void sitem(CommandArgs args)
        {
            bool failure = false;
            int amt = 1;
            if (args.Parameters.Count <= 1) //checks to make sure the cmd isn't messed up
            {
                args.Player.SendErrorMessage("No parameters set, Check /help sitem");
                return;
            }
            else if ((args.Parameters.Count - 1) % 2 != 0)
            {
                args.Player.SendErrorMessage("You are missing a value");
                return;
            }
            TSPlayer plr = new TSPlayer(args.Player.Index);
            List<Item> itmList = TShock.Utils.GetItemByIdOrName(args.Parameters[0].ToString());
            if (itmList.Count == 0)//If several/No items match
            {
                args.Player.SendErrorMessage("Item does not exist");
                return;
            }
            else if (itmList.Count > 1)
            {
                string matches = "";
                foreach (var item in itmList)
                {
                    matches += "(" + item.netID + ") " + item.Name + " | ";
                }
                args.Player.SendMessage("Multiple items matched\n"+matches, new Color(255, 255, 0));
                return;
            }
            Item itm = TShock.Utils.GetItemByIdOrName(args.Parameters[0].ToString()).First<Item>(); //actually set the item into a template

            for (int i = 1; i < args.Parameters.Count; i += 2)
            {
                if (!int.TryParse(args.Parameters[i + 1], out int result) & !float.TryParse(args.Parameters[i + 1], out float result2)) {//check that the values provided are actually values
                    args.Player.SendErrorMessage("Invalid value for parameter {0}(n° {1}), recieved {2}", args.Parameters[i],(i+1)/2, args.Parameters[i + 1]);
                    failure = true;//We dont return inmediatly, but set a flag, so all misstakes are made known
                }
                switch (args.Parameters[i])
                {
                    case "-d":
                        itm.damage = result;
                        break;
                    case "-k":
                        itm.knockBack = result2;
                        break;
                    case "-kb":
                        itm.knockBack = result2;
                        break;
                    case "-ua":
                        itm.useAnimation = result;
                        break;
                    case "-ut":
                        itm.useTime = result;
                        break;
                    case "-s":
                        itm.shoot = result;
                        break;
                    case "-ss":
                        itm.shootSpeed = result;
                        break;
                    case "-sc":
                        itm.scale = result2;
                        break;
                    case "-a":
                        itm.ammo = result;
                        break;
                    case "-uam":
                        itm.useAmmo = result;
                        break;
                    case "-na":
                        itm.notAmmo = result == 1 ? true : false;
                        break;
                    case "-amt":
                        amt = result;
                        break;
                    default:
                        args.Player.SendErrorMessage("Parameter n° {0} is invalid, recieved {1}", (i+1)/2, args.Parameters[i]);
                         failure = true;
                        break;
                }
            }
            if (failure == true) return; //after we made known all what was bad, return
            //Create an item into the game and get its internal ID
            int iIndex = Item.NewItem(Projectile.GetNoneSource(), new Vector2(plr.X, plr.Y), new Vector2(itm.height, itm.width), itm.type, amt, false, 0, true, false);
            Item modItm = Main.item[iIndex]; //Paste that item we just made into an object so we can mod it
            modItm.playerIndexTheItemIsReservedFor = plr.Index;
            //and start filling it with our modded template
            modItm.color = itm.color;
            modItm.damage = itm.damage;
            modItm.knockBack = itm.knockBack;
            modItm.useAnimation = itm.useAnimation;
            modItm.useTime = itm.useTime;
            modItm.shoot = itm.shoot;
            modItm.shootSpeed = itm.shootSpeed;
            modItm.width = itm.width;
            modItm.height = itm.height;
            modItm.scale = itm.scale;
            modItm.ammo = itm.ammo;
            modItm.useAmmo = itm.useAmmo;
            modItm.notAmmo = itm.notAmmo;
            Main.item[iIndex] = modItm; //And to finish it, write our item back at the game
            TSPlayer.All.SendData(PacketTypes.UpdateItemDrop, string.Empty, iIndex);//Make other clients know that it exist our drop
            TSPlayer.All.SendData(PacketTypes.ItemOwner, string.Empty, iIndex);//Make it only pickable by who used the cmd
            TSPlayer.All.SendData(PacketTypes.TweakItem, string.Empty, iIndex,255,63);//Make the clients know that we did some stuff to the item
        }
    }

}
