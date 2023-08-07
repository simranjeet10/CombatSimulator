using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Magician : Player
    {
        public static string _description = "Magician can use any weapon. But his strength and energy is low. He cannot fight for a long time. He need to buy strenth potion to increase the energy.";

        public override bool Add(Item item)
        { 
                return _inventory.Add(item);
        }
        public override int Attack()
        {
            return PrimaryWeapon != null ? PrimaryWeapon.Use() : 0;
        }

        public override void TakeDamage(int points)
        {
            int delta = points;
            if (HasShield) delta -= _inventory.GetFirstItemByType(ItemType.Shield).Use();
            if (delta > 0) base.UpdateHealth(delta / (-1));
        }
        public static HeroData DefaultProperty = new HeroData()
        {
            valid = true,
            strength = 40,
            agility = 50,
            intelligence = 30,
            type = PlayerType.Magician
        };
        public Magician(HeroData data) : base(new Location(0, 0), data) { }


    }
}
