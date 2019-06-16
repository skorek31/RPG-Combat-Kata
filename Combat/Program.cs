using System;
using System.Collections.Generic;


namespace Combat
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    public class Faction
    {
        private int id;
        private string name;

        public Faction(int id)
        {
            this.id = id;
            SetFactionName(id);
        }

        public void SetFactionName(int id)
        {
            if (id == 0)
            {
                this.name = "The Harpers";
            }
            else if (id == 1)
            {
                this.name = "The Order Of The Gauntlet";
            }
            else if (id == 2)
            {
                this.name = "The Emerald Enclave";
            }
            else
            {
                this.name = "The Lord's Alliance";
            }
        }

        public string GetFactionName()
        {
            return this.name;
        }

        public int GetFactionID()
        {
            return this.id;
        }
    }

    public interface ICharacter
    {
        int GetHealth();
        int GetLevel();
        bool IsAlive();
    }

    public class Warrior : ICharacter
    {
        private int health = 1000;
        private int level = 1;
        private bool alive = true;
        private string characterClass;
        private int range;
        private int x;
        private int y;
        private List<Faction> warriorFactions = new List<Faction>();
        

        public Warrior(string characterClass, int level, int x, int y)
        {
            this.characterClass = characterClass;
            this.x = x;
            this.y = y;
            this.level = level;

            if (characterClass == "Melee")
            {
                this.range = 2;
            }
            else if (characterClass == "Ranged")
            {
                this.range = 20;
            }
        }

        public int GetHealth()
        {
            return health;
        }
        
        public int GetLevel()
        {
            return level;
        }

        public bool IsAlive()
        {
            return alive;
        }

        public List<Faction> GetFactionsList()
        {
            return warriorFactions;
        }

        public void JoinFaction(int factionID)
        {
            this.warriorFactions.Add(new Faction(factionID));
        }

        public void LeaveFaction(int factionID)
        {
            /// <summary>
            /// Finds and removes faction with given ID
            /// </summary>
            /// <param name="factionID">ID number of faction to remove.</param>

            foreach (Faction item in warriorFactions)
            {
                if (item.GetFactionID() == factionID)
                {
                    this.warriorFactions.Remove(item);
                }
            }
        }

        public bool IsAlly(Warrior target)
        {
            /// <summary>
            /// Method check if objects belongs to the same faction
            /// </summary>
            /// <param name="target">Object to check.</param>
            /// <returns>
            /// True if objects belongs to the same faction, otherwise false.
            /// </returns>
            
            List<Faction> targetFactions = target.GetFactionsList();

            foreach (Faction warriorFaction in this.GetFactionsList())
            {
                foreach (Faction targetFaction in targetFactions)
                {
                    if (warriorFaction.GetFactionID() == targetFaction.GetFactionID())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        public int Attack(Warrior target)
        {
            /// <summary>
            /// Method handles warrior attack
            /// </summary>
            /// <param name="target">Attack target.</param>
            /// <returns>
            /// dmg value - if target was attacked successfully
            /// -1 - if character tried to attack itself or is out of range
            /// </returns>

            if (target != this && this.InRange(target))
            {
                Random randomDamage = new Random();
                int damageValue = -1;
                if (target.GetLevel() >= this.level + 5)
                {
                    damageValue = (int)(randomDamage.Next(50, 101) * 1.5);
                    target.health -= damageValue;
                }
                else if (this.level >= target.GetLevel() + 5)
                {
                    damageValue = (int)(randomDamage.Next(50, 101) * 0.5);
                    target.health -= damageValue;
                }
                else
                {
                    damageValue = randomDamage.Next(50, 101);
                    target.health -= damageValue;
                }
                
                if (target.health <= 0)
                {
                    target.health = 0;
                    target.alive = false;
                    damageValue = 0;
                }
                return damageValue;
            }
            else
            {
                return -1;
            }
        }
        
        public int Heal(Warrior target)
        {
            /// <summary>
            /// Method handles warrior healing his ally.
            /// In order to make successful heal target needs to be:
            /// 1. Alive
            /// 2. Different than a Character performing healing action.
            /// 3. In the same faction as a Character performing action.
            /// </summary>
            /// <param name="target">Heal target. Must be in the same faction.</param>
            /// <returns>
            /// If heal was successful:
            /// Health value change (e.g. 0 if target HP = max before heal)
            /// -1 - heal unsuccessful
            /// </returns>

            Random randomValue = new Random();
            
            if (target.IsAlive() && target != this && this.IsAlly(target))
            {
                int healValue = randomValue.Next(50, 101);
                health += healValue;
                if (health > 1000)
                {
                    health = 1000;
                    return 0;
                }
                return healValue;
            }
            return -1;
        }

        public List<int> GetCoordinates()
        {
            var coordinates = new List<int>();
            coordinates.Add(this.x);
            coordinates.Add(this.y);
            return coordinates;
        }

        public bool InRange(Warrior target)
        {
            var attackerCoordinates = this.GetCoordinates();
            var targetCoordinates = target.GetCoordinates();

            int attackerX = attackerCoordinates[0];
            int attackerY = attackerCoordinates[1];

            int targetX = targetCoordinates[0];
            int targetY = targetCoordinates[1];

            double distance = Math.Sqrt(Math.Pow((targetY - attackerY), 2) + Math.Pow((targetX - attackerX), 2));
            
            if (distance <= this.range)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
