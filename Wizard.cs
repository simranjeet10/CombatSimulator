using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Wizard : Player
    {
        public static string _description = "Wizard use magic to fight with the monster. He can fight with magic monster effectively. Wizard can acquire the magic sword to fight with monster. ";
        public override bool Add(Item item)
        {
            if (item.Type == ItemType.Sword)
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
            Random r = Helper.rand;
            int damagePt;
            if (PrimaryWeapon != null) damagePt = PrimaryWeapon.Use();
            else damagePt = 0; 
            Console.WriteLine("Wizard attacks with magic spell!");
            return Convert.ToInt32(damagePt*r.NextDouble());         
        }
        
        public override void TakeDamage(int points)
        {
            int delta = points;
            Random r = Helper.rand; 
            if (HasShield) delta -= Convert.ToInt32( _inventory.GetFirstItemByType(ItemType.Shield).Use()*r.NextDouble());
            if (delta > 0) base.UpdateHealth(delta / (-1));
        }
        public static HeroData DefaultProperty = new HeroData()
        {
            valid = true,
            strength = 45,
            agility = 45,
            intelligence = 70,
            type = PlayerType.Wizard
        };

        public Wizard(HeroData data) : base(new Location(0, 0), data) { }
    }
}
