using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


// when it is player's. player can choose to use item/change weapon to add more power.
// 

namespace Game
{
    static public class CombatSimulation
    {
        /// <summary>
        /// Player fight with monster 1:1 until either dies or runaway.
        /// </summary>
        /// <param name="p"> hero object </param>
        /// <param name="m"> monster object </param>
        static public void SoloCombat(Player p, Monster m)
        {
            bool _runAway = false;
            Console.WriteLine("------------------------ Combat Starts!------------------------");
            while (p.IsAlive && m.IsAlive && !_runAway)
            {
                int turn = Helper.rand.Next(0, 2);     //0= player, 1= monster
                if (turn == 0) HeroPlay(p, m, _runAway);
                else MonsterPlay(p, m);
                ConsoleHelper.WriteLine($"Hero's HP --> |__{p.Health}__||__{m.Health}__| <-- Monster's HP ", ConsoleColor.DarkYellow);
            }

            if (_runAway) Console.WriteLine($"Hero runs away from the monster");
            else if (p.IsAlive) m.Reward(p);
            else ConsoleHelper.WriteLine("RIP our brave hero... You loses!", ConsoleColor.Magenta);
        }




        /// <summary>
        /// Group combat will choose 1 from each side. If one is dead or run away, one will be removed from the list. 
        /// Then player has to choose the next round from each side.
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="ms"></param>
        /// <param name="_runAway"></param>
        static public void GroupCombat(List<Player> ps, List<Monster> ms, bool _runAway)
        {
            while (ps.Count!= 0 && ms.Count !=0)
            {
                
                // choose hero
                Console.WriteLine("Please choose the hero >>> ");
                for (int i = 0; i < ps.Count(); i++) Console.WriteLine($"\t{i + 1}. {ps[i].Name}");
                int p_choice = Helper.GetChoice();
                if (p_choice > ps.Count())
                {
                    Console.WriteLine($"Incorrect choice. So the first hero will play. ");
                    p_choice = ps.Count();
                }
                Player p = ps[p_choice - 1];

                // choose monster
                Console.WriteLine("Please choose the monster >>> ");
                for (int i = 0; i < ms.Count(); i++) Console.WriteLine($"\t{i + 1}. {ms[i].Name}");
                int m_choice = Helper.GetChoice();
                if (m_choice > ms.Count())
                {
                    Console.WriteLine($"Incorrect choice. So the first monster will play. ");
                    m_choice = ms.Count();
                }
                Monster m = ms[p_choice - 1];
                

                SoloCombat(p, m);

                if (p.IsAlive & !_runAway) ms.Remove(m);
                else if (m.IsAlive) ps.Remove(p);

                Console.Write("Do you want to continue the next combat? (y/n) >>> ");
                if (Console.ReadLine() == "n") break;
            }    
        }



        /// <summary>
        /// When it is hero's turn. Hero can choose to change the main weapon, or use potions to upgrade strength and health.
        /// He can fight with the main weapon.
        /// Or he can choose to runaway from the combat.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="m"></param>
        /// <param name="_runAway"></param>
        static public void HeroPlay(Player p, Monster m, bool _runAway)
        {
            ConsoleHelper.WriteLine("What woule you like to do?\n\t1. Select items\n\t2. Fight\n\t3. Run away", ConsoleColor.Blue);
            string? choice = Console.ReadLine();
            switch (choice) 
            {
                case "1": HeroEquip(p); if(p.PrimaryWeapon is not null) p.Attack(m); break;
                case "2": p.Attack(m); break;
                case "3": _runAway = true; break;
                default:
                    ConsoleHelper.WriteLine("Invalid option. So you have to fight.");
                    p.Attack(m);
                    break;
            }
        }


        /// <summary>
        /// Monster's turn
        /// </summary>
        /// <param name="p"></param>
        /// <param name="m"></param>
        static public void MonsterPlay(Player p, Monster m)
        {
            ConsoleHelper.Write("Monster's turn:\t", ConsoleColor.DarkRed);
            m.Attack(p);
        }

        /// <summary>
        /// From Hero's turn method, if hero choose to change weapon or use potion, this method is implemented.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        static public bool HeroEquip(Player p)
        {
            while (true)
            {
                p.DisplayAll();
                if( p.ItemCount() == 0) return false;
                Console.Write("Which items would you like to use? >>> ");
                int choice = Helper.GetChoice();
                Item item = p.GetItemByIndex(choice - 1);
                if (item.ItemCategory == ItemCategory.Weapon) p.PrimaryWeapon = item;
                else if (item.ItemCategory == ItemCategory.Armour) p.PrimaryWeapon = item;
                else if (item.Type == ItemType.Strength) { p.Strength += item.Use(); p.RemoveAt(choice - 1); }
                else if (item.Type == ItemType.Healing) {p.Health += item.Use(); p.RemoveAt(choice - 1); }

                Console.Write("Do you want to select more items? (y/n) >>> ");
                if (Console.ReadLine() == "n") break;
            }
            if (p.PrimaryWeapon is not null) return true;
            return false;
        }



    }


}
