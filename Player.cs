using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using Game;
using NPOI.HSSF.Record;

namespace Game
{
    public class Player
    {
        #region : Declare attributes 

        // Inventory properties
        private const int MaxItem = 10;
        private const float MaxVolume = 100;
        private const float MaxWeight = 100;
        protected Inventory _inventory = new Inventory(MaxItem, MaxVolume, MaxWeight);
        private bool _hasMap = true;
        private bool _hasSword = false;
        private bool _hasShield = false;

        public Inventory Inventory { get => _inventory; }

        // Health & lives
        private const int _maxHealth = 100;
        private int _health = _maxHealth;
        private bool _isAlive = true;
        private float _cash = 100;
        private object map;
        private HeroData _data;
        private Item _primaryWeapon;
        private int _xp = 0;
        #endregion 

        #region setup properties
        public HeroData HeroProfile { get; set; }
        public int Strength { get { return _data.strength; } set { _data.strength = value; } }
        public int Agility { get { return _data.agility; } set { _data.agility = value; } }
        public int Intelligence { get { return _data.intelligence; } set { _data.intelligence = value; } }
        public Item PrimaryWeapon { get; set; }
        public bool HasMap { get => _inventory.DoesItemExist("map"); }
        public bool HasSword { get => _inventory.DoesItemExist("sword"); }
        public bool HasShield { get => _inventory.DoesItemExist("shield"); }
        public string Name { get { return _data.name; }  }
        public PlayerType Type { get => HeroProfile.type; }
        public int XP { get { return _xp; } set { _xp = value; } }

        public int Health
        {
            get { return _health; }
            set{
                if (value > 100) _health = 100;
                else if (value < 0) { _health = 0; _isAlive = false; }
                else _health = value; }
        }
        public int MaxHealth { get { return _maxHealth; } }
  
        // ensure cash cannot be set negative
        public float Cash
        {
            get { return _cash; }
            set {
                if (value < 0)  Console.WriteLine("Cash cannot be negative");
                else _cash = value;
            }
        }

        public bool IsAlive { get => _health > 0; }

        // Represents the distance the player can sense danger.
        // Diagonal adjacent squares have a range of 2 from the player.
        public int SenseRange { get; } = 1;

        public Location Location { get; set; }

        public string CauseOfDeath { get; private set; }

        public void Kill(string cause)
        {
            _health = 0;
            CauseOfDeath = cause;
        }
        #endregion


        #region: Class Constructor

        public Player(Location location) { }
        public Player() { }
        public Player(Location start, HeroData data)
        {
            Location = start;
            _data = data;
        }

        #endregion

        #region: ShowStatus/ ShowCapacity/ ShowConfig

        public void DisplayAll()
        {
            _inventory.DisplayAll();
        }

        public void ShowStatus()
        {
            _inventory.DisplayAll();
            string message = HasSword ? "You have a sword." : "You donot have a sword";
            Console.WriteLine(message);

            string messageForMap = HasMap ? "You have a map." : "You donot have a map";
            Console.WriteLine(messageForMap);

            if (PrimaryWeapon != null)
            {
                Console.WriteLine($"Your primary weapon is: {PrimaryWeapon.ToString()}");
            }
        }

       
        public void ShowCapacity()
        {
            Console.WriteLine("------------------------------- Your Inventory -------------------------------");
            Console.WriteLine($"You have: ${Cash}");
            _inventory.ShowCapacity();
            ShowConfig(HeroProfile);
        }
        public void ShowConfig(HeroData data)
        {
            ConsoleHelper.WriteLine($"Hero's name : {data.name.ToUpper()} ");
            ConsoleHelper.WriteLine($"Hero's type : {data.type} ");
            ConsoleHelper.WriteLine($"Hero's strength : {data.strength} ");
            ConsoleHelper.WriteLine($"Hero's intelligence : {data.intelligence} ");
            ConsoleHelper.WriteLine($"Hero's agility : {data.agility} ");
            ConsoleHelper.WriteLine($"Hero's health: {Health}");

        }
        #endregion


        #region inventory related
        public virtual bool Add(Item item)  
        {
            return _inventory.Add(item);
        }
        public virtual void RemoveAt(int index)
        {
            _inventory.RemoveAt(index);
        }

        public Item GetItemByIndex(int index)
        {
            return _inventory.GetItemByIndex(index);
        }
        public List<Item> Weapons(Inventory _inventory)
        {
            return Weapons(_inventory);
        }

        private List<Item> GetWeapons()
        {
            return _inventory.GetItemsByCategory(ItemCategory.Weapon);
        }
        public int ItemCount() { return _inventory.Count(); }
        #endregion

        /**************************************************************************************************
             * You do not need to alter anything below here but you are free to do
             * For example - while under the effects of a potion of invulnerability, the player cannot die
         *************************************************************************************************/






        #region combat
        public virtual int Attack()
        {
            return 0;
        }

       
        public virtual void Attack(Monster m)
        {
            if (PrimaryWeapon is null) {Console.WriteLine("You do not equip any weapon yet."); return; }
            m.TakeDamage(PrimaryWeapon.Use());
        }

        public virtual void TakeDamage(int points) { this.Health -= points; }

        public void UpdateHealth(int delta)
        {
            Health += delta;
        }

        #endregion


        #region use items

        public void EquipWeapon()
        {
            List<Item> weapons = GetWeapons();
            _inventory.DisplayAll(weapons);

            Console.Write("Choose the number of the weapon to equip it: ");

            int choice = Helper.GetChoice();
            Item item = _inventory.GetItemByIndex(choice - 1);
            PrimaryWeapon = item;
        }

        public void UsePotion()
        {
            List<Item> potions = _inventory.GetItemsByCategory(ItemCategory.Potion);

            if (potions.Count <= 0)
            {
                Console.WriteLine("You don't have any potions.");
                return;
            }

            _inventory.DisplayAll(potions);
            Console.Write("Choose the number of the potion to use it: ");

            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice > 0 && choice <= potions.Count)
            {
                int pts = potions[choice - 1].Use();
                UpdateHealth(pts);
            }
        }

        public void UsePotion(Item item)
        {
            int pts = item.Use();
            UpdateHealth(pts);
        }
        #endregion
    }
    // Represents a location in the 2D game world, based on its row and column.
    public record Location(int Row, int Column);
}

