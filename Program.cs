using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Xml.Serialization;


namespace Game
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Start >>> \n");
            HeroData data = new HeroData();
            data.name = "Peter";
            data.valid = true;
            data.strength = 45;
            data.agility = 45;
            data.intelligence = 70;
            data.type = PlayerType.Wizard;
            //Player p = new GeneratePlayer().StartGame();
            Player p = new Wizard( data);
            Monster m = new Medusa();
            p.Add(new Sword());
            p.Add(new Grenade());
            p.Add(new HealingPotion());
            p.Add(new StrengthPotion());
            p.Add(new Shield());
            p.Add(new Helmet());
            p.Add(new PoisonGrenade());
            p.Add(new MagicGrenade());

            CombatSimulation.SoloCombat(p, m);

            data.name = "Wizard";
            Player p1 = new Wizard(data);
            data.name = "Warrier";
            Player p2 = new Warrior(data);
            data.name = "Magician";
            Player p3 = new Magician(data);
            Monster m1 = new Medusa();
            Monster m2 = new Abaddon();
            Monster m3 = new Nomar();
            Monster m4 = new SwordSnatcher();
            Monster m5 = new Zombie();
            List<Player> ps = new List<Player> { p1, p2, p3 };
            List<Monster> ms = new List<Monster> { m1, m2, m3 , m4, m5};
            CombatSimulation.GroupCombat(ps, ms, false);
            CombatSimulation.SoloCombat(p,m);



        }

        #region: Test code
        private static void FinalTest(Player p1)
        {
            ArmourMerchant aMerchant1 = new ArmourMerchant();
            aMerchant1.Interact(p1);
            PotionMerchant pMerchant1 = new PotionMerchant();
            pMerchant1.Interact(p1);
            WeaponMerchant wMerchant1 = new WeaponMerchant();
            wMerchant1.Interact(p1);
        }

        private static void InventoryTest()
        {
            Inventory inv = new Inventory(10, 50, 50);
            Map m = new Map();
            Sword s = new Sword();
            Map m2 = new Map();
            Sword s2 = new Sword();
            inv.Add(m);
            inv.Add(s);
            inv.Add(m2);
            inv.Add(s2);
            bool x = inv.DoesItemExist("map");
            bool y = inv.DoesItemExist("sword");
            Console.WriteLine(x);
            Console.WriteLine(y);
            inv.ShowCapacity();
            inv.DisplayAll();
            inv.Remove(3);
            inv.DisplayAll();
 
            inv.Remove(3);
            inv.DisplayAll();
            inv.SortByValue("price");
            inv.DisplayAll();
            Console.WriteLine(inv.CountItem(ItemType.Map));
        }

        private static void TestJson()
        {
            HeroData data = new HeroData();
            
            data.intelligence = 90;
            data.strength = 80;
            
            //data.itemType = new List<ItemType> { ItemType.Shield, ItemType.Helmet };
            string json = JsonConvert.SerializeObject(data); // myObj is the struct you want to serialize
            File.WriteAllText("Foo.json", json); //Write the text to Foo.json
            string jsonString = File.ReadAllText("Foo.json");

            
            Console.WriteLine(jsonString);       // ==> print string
              

            Dictionary<string, object> mydict = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
            foreach (var kvp in mydict)          // ===> convert string to dict
            {
                Console.WriteLine(kvp.Key);
                Console.WriteLine(kvp.Value);
            }

            // ===> convert string to struct
            HeroData dataFromJsonString = JsonConvert.DeserializeObject<HeroData>(jsonString);
            Console.Write(dataFromJsonString.strength);


        }

        private static void TestHero()
        {
            Player p1 = new GeneratePlayer().StartGame();

            FinalTest(p1);
            p1.EquipWeapon();

            //int damageGiven = p1.Attack(new Random);

            //Console.WriteLine($"{damageGiven} is the damage given");

            p1.TakeDamage(20);
            p1.TakeDamage(20);
            p1.ShowCapacity();

            p1.UsePotion();
            p1.ShowCapacity();
        }
        #endregion
    }
}

