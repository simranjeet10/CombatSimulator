using Newtonsoft.Json.Serialization;
using NPOI.OpenXmlFormats.Dml.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Game
{
    public class Medusa : Monster
    {
        private int _snakes;   // Medusa can use snake to fight 10 times.
        private float _swordMultiply = 1.5f;

        /// <summary>
        /// Constructor with default properties
        /// </summary>
        /// <param name="name"></param>
        /// <param name="strength"></param>
        /// <param name="agility"></param>
        /// <param name="rewardXP"></param>
        /// <param name="rewardAg"></param>
        /// <param name="rewardSt"></param>
        public Medusa(string name = "Medusa", int strength = 100, int agility = 90, int rewardXP = 20, int rewardAg = 12, int rewardSt = 15, int snakes=10)
        {
            _type = MonsterType.Medusa;
            _name = name;
            _strength = strength;
            _agility = agility;
            _rewardXP = rewardXP;
            _rewardAgility = rewardAg;
            _rewardStrength = rewardSt;
            _snakes = snakes;
        }

        /// <summary>
        /// The attack method. It will random which attack to use.
        /// </summary>
        /// <param name="p"></param>
        public override void Attack(Player p)
        {
            int chosenAttack = r.Next(1, 8);
            switch (chosenAttack)
            { 
                case 1: Attack1(p); break;
                case 2: Attack2(p); break;
                case 3: Attack3(p); break;
                case 4: Attack1(p); break;
                case 5: Attack2(p); break;
                case 6: Attack3(p); break;
                case 7: SpecialAttack(p); break;  // special attack probability is 1/7
            } 
        }

        /// <summary>
        /// Attack1 is based on strength and health of Medusa. The weaker, the less effective.
        /// </summary>
        /// <param name="p"></param>
        private void Attack1(Player p) 
        {
            int damagePts = r.Next(10,(_strength+_health)/5);
            Console.WriteLine(value: $"Medusa throw the {Math.Round(r.NextDouble()*10,2)} tons rocks at you! ");
            p.TakeDamage(damagePts);
        }

        /// <summary>
        /// Attack2 is based on the number of snakes she has. So by default, she has 10 times to attack.
        /// Game can create an advanced Medusa by giving more snakes.
        /// </summary>
        /// <param name="p"></param>
        private void Attack2(Player p) 
        {
            
            int damagePts = _snakes * Convert.ToInt32(_agility/100);
            _snakes--;
            Console.WriteLine(value: $"Medusa splashes snake poison at you! ");
            p.TakeDamage(damagePts);
        }

        /// <summary>
        /// Attack 3 damage is based on Medusa's strength vs. Player's strength
        /// </summary>
        /// <param name="p"></param>
        private void Attack3(Player p)
        {
            int damagePts = Math.Min(Math.Abs(this.Strength - p.Strength),20);
            Console.WriteLine(value: $"Medusa curses at you! ");
            p.TakeDamage(damagePts);
        }

        /// <summary>
        /// Special Attack can cause player to death if one do not have a shield. If hero have shield, it will damage only 1/3 of health.
        /// </summary>
        /// <param name="p"></param>
        public override void SpecialAttack(Player p) 
        {
            Console.WriteLine(value: $"Medusa is gazing you. Lift your shield to protect yourself!");
            int damagePts = p.MaxHealth;
            if (p.HasShield) p.TakeDamage(damagePts/3);    //Without shield, the player dies.
            else { Console.WriteLine("Without shield, you turn into stone."); p.TakeDamage(damagePts); }
        }

        /// <summary>
        /// The method print out if Medusa dies from the battle. Winner will get some values and items.
        /// </summary>
        /// <param name="p"></param>
        public override void Reward(Player p) 
        {
            ConsoleHelper.WriteLine($"Congratulation! You defeat {this.Name} . You wins!", ConsoleColor.Green);
            ConsoleHelper.WriteLine($"You acquire two poison grenades and  gain strength +{_rewardStrength}, agility +{_rewardAgility}, and XP +{_rewardXP};", ConsoleColor.Green);
            p.Strength += _rewardStrength;
            p.Agility += _rewardAgility;
            p.XP += _rewardXP;
            p.Add(new MagicGrenade());
            p.Add(new MagicGrenade());
        }

        /// <summary>
        /// Take damage from Hero's attack. If hero use sword, the damage is x1.5
        /// </summary>
        /// <param name="points"></param>
        public override void TakeDamage(Player p, int points) 
        {
            if (p.HasSword) points =Convert.ToInt32( points* _swordMultiply);
            _health -= points;
            this.Agility -= r.Next(1,10);
            this.Strength -= r.Next(1, 10);
        }
    }
}
