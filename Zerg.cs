using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starcraft
{
    class Zerg
    {
        private int health;
        private int damage;
        private int range;
        private int populationUnit;
        private int craftingTime;
        private int x;
        private int y;
        private string display;

        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }

        public int Damage
        {
            get
            {
                return damage;
            }
            set
            {
                damage = value;
            }
        }

        public int Range
        {
            get
            {
                return range;
            }
            set
            {
                range = value;
            }
        }

        public int PopUnit
        {
            get
            {
                return populationUnit;
            }
            set
            {
                populationUnit = value;
            }
        }

        public int CraftingTime
        {
            get
            {
                return craftingTime;
            }
            set
            {
                craftingTime = value;
            }

        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public string Display
        {
            get
            {
                return display;
            }
            set
            {
                display = value;
            }
        }

        // konstruktor a játékmenet folyamán hozzáadott egységekhez
        public Zerg(string type)
        {
            if (type == "A")
            {
                this.Health = 50;
                this.Damage = 30;
                this.Range = 1;
                this.CraftingTime = 2;
                this.PopUnit = 1;
                this.Display = "A ";
            }
            else if (type == "S")
            {
                this.Health = 20;
                this.Damage = 10;
                this.Range = 5;
                this.CraftingTime = 3;
                this.PopUnit = 2;
                this.Display = "S ";
            }
            else if(type == "D")
            {
                this.Health = 70;
                this.Damage = 20;
                this.Range = 2;
                this.CraftingTime = 3;
                this.PopUnit = 4;
                this.Display = "D ";
            }
        }

        // konstruktor kezdő egységek megadásához
        public Zerg(string type, int x, int y)
        {
            this.X = x;
            this.Y = y;
            if (type == "A")
            {
                this.Health = 50;
                this.Damage = 30;
                this.Range = 1;
                this.CraftingTime = 0;
                this.PopUnit = 1;
                this.Display = "A ";
            }
            else if (type == "S")
            {
                this.Health = 20;
                this.Damage = 10;
                this.Range = 5;
                this.CraftingTime = 0;
                this.PopUnit = 2;
                this.Display = "S ";
            }
            else if (type == "D")
            {
                this.Health = 70;
                this.Damage = 20;
                this.Range = 2;
                this.CraftingTime = 0;
                this.PopUnit = 4;
                this.Display = "D ";
            }
            else if (type == "C")
            {
                this.Health = 50;
                this.Display = "C ";
            }
        }

        public int AttackOrMove(Terran[] terrans, string[,] map, int x, int y)
        {
            // ha a megadott koordináta a pályán belül található
            if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1))
            {
                // ha a megadott koordinátán nem üres terület és nem saját egység szerepel
                if (map[x, y] != "x " && map[x, y] != "A " && map[x, y] != "S " && map[x, y] != "D " && map[x, y] != "C ")
                {
                    // ha az adott egység hatótávolsága elég nagy
                    if (this.Range >= this.X - x + this.Y - y)
                    {
                        bool found = false;
                        int i = 0;
                        while (i < terrans.Length && !found)
                        {
                            // megkeressük a tömbben az adott koordinátához tartozó egységet, amit sebezni fogunk
                            if (terrans[i] != null && terrans[i].X == x && terrans[i].Y == y)
                            {
                                terrans[i].Health -= this.Damage;
                                found = true;
                                // ha az egység meghal...
                                if (terrans[i].Health <= 0)
                                {
                                    // ha az egység maga a központ, vége a játéknak (success = 0)
                                    if (map[x, y] == "H ")
                                    {
                                        // success = 1
                                        return 1;
                                    }

                                    // ...akkor üres terület keletkezik a helyén
                                    map[x, y] = "x ";
                                    terrans[i] = null;
                                }
                            }

                            i++;
                        }

                        // még nincs vége a játéknak (success = 0)
                        return 0;
                    }
                }
                // ha a megadott koordináta üres terület vagy saját egység
                else if (this.Move(map, x, y))
                {
                    return 0;
                }
            }

            // ebben az esetben nem sikerült a manőver, így nem lépett az aktuális játékos
            return -1;
        }

        public bool Move(string[,] map, int x, int y)
        {
            // ha a megadott koordináta az egység mellett van (átlósan nem tud mozogni)
            if (((x - this.X == 1 || x - this.X == -1) && y == this.Y) || (y - this.Y == 1 || y - this.Y == -1) && x == this.X)
            {
                // ha a megadott koordinátán nincs baráti egység
                if (map[x, y] != "A " && map[x, y] != "S " && map[x, y] != "D " && map[x, y] != "C ")
                {
                    // az egység korábbi helye üres mezővé alakul a mapon
                    map[this.X, this.Y] = "x ";
                    this.X = x;
                    this.Y = y;
                    // az egység aktuális helyét beállítjuk a mapon
                    map[x, y] = this.Display;
                    return true;
                }
            }

            // ebben az esetben nem sikerült a manőver
            return false;
        }

        // minden körben levonunk az összes éppen gyártósoron lévő egység kraftolási idejéből
        public int CraftingTimeReduce()
        {
            this.CraftingTime--;
            return this.CraftingTime;
        }
    }
}
