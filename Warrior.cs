using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Warrior : Player
    {
        public static string _description = "Warrior has high strength. He can use all weapons to fight with the monster except the magic monster. He can use magic potion to fight with magic monster 1 time.";
        public override bool Add(Item item)
        {
            if (item.Type == ItemType.Grenade)
            {
                return false;
            }
            else
            {
                return _inventory.Add(item);
            }

        }
        public override int Attack()
        {
            Console.WriteLine("Warrior attacks with sword");
            return PrimaryWeapon != null ? PrimaryWeapon.Use() : 0;
        }

        public override void TakeDamage(int points)
        {
            int delta = points;
            if(HasShield) delta -= _inventory.GetFirstItemByType(ItemType.Shield).Use();
            if (delta > 0) base.UpdateHealth(delta/(-1));
        }

        public static HeroData DefaultProperty = new HeroData()
        {
            valid = true,
            strength = 50,
            agility = 50,
            intelligence = 50,
            type = PlayerType.Warrior
        };
        public Warrior(HeroData data) : base(new Location(0, 0), data)
        {

        }
    }
}
