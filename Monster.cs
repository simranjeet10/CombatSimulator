using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    // Main class
    public class Monster
    {
        protected const int maxHealth = 100;
        protected int _health = maxHealth;
        protected bool _isAlive = true;
        protected int _rewardStrength;
        protected int _rewardAgility;
        protected int _rewardXP;
        // Monster's properties
        protected int _strength;
        protected int _agility;
        protected string? _name;
        protected MonsterType _type;
        protected Random r = Helper.rand;

        public Monster()
        {

        }

        #region Monster's properties
        public bool IsAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }

        public int Health { 
            get { return _health; } 
            set 
            { 
                if (value > 100) _health = 100;
                else if (value < 0) { _health = 0; _isAlive = false; } 
                else _health = value;
            }
        }

        protected int Agility
        {
            get { return _agility; }
            set { var result = value < 0 ? _agility = 0 : _agility = value; }
        }
        protected int Strength
        {
            get { return _strength; }
            set { var result = value < 0 ? _strength = 0 : _strength = value; }
        }

        public virtual string? Name { get { return _name; } }
        #endregion

        #region Methods
        public virtual int? Attack() { return 1; }

        public virtual void Attack(Player p) { return; }

        public virtual int SpecialAttack() { return 1; }
        public virtual void SpecialAttack(Player p) { return; }
        public virtual void Reward(Player p) { }
        public virtual void TakeDamage(int damagePt) 
        {
            _health -=damagePt;
        }
        public virtual void TakeDamage(Player p, int damagePt)
        {
            _health -= damagePt;
        }

        #endregion

    }
}
