using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starcraft
{
    class Terran
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

        public Terran(string type)
        {
            if (type == "Q")
            {
                this.Health = 50;
                this.Damage = 30;
                this.Range = 1;
                this.CraftingTime = 2;
                this.PopUnit = 1;
                this.Display = "Q ";
            }
            else if (type == "W")
            {
                this.Health = 20;
                this.Damage = 10;
                this.Range = 5;
                this.CraftingTime = 3;
                this.PopUnit = 2;
                this.Display = "W ";
            }
            else if (type == "E")
            {
                this.Health = 70;
                this.Damage = 20;
                this.Range = 2;
                this.CraftingTime = 3;
                this.PopUnit = 4;
                this.Display = "E ";
            }
        }

        public Terran(string type, int x, int y)
        {
            this.X = x;
            this.Y = y;
            if (type == "Q")
            {
                this.Health = 50;
                this.Damage = 30;
                this.Range = 1;
                this.CraftingTime = 0;
                this.PopUnit = 1;
                this.Display = "Q ";
            }
            else if (type == "W")
            {
                this.Health = 20;
                this.Damage = 10;
                this.Range = 5;
                this.CraftingTime = 0;
                this.PopUnit = 2;
                this.Display = "W ";
            }
            else if (type == "E")
            {
                this.Health = 70;
                this.Damage = 20;
                this.Range = 2;
                this.CraftingTime = 0;
                this.PopUnit = 4;
                this.Display = "E ";
            }
            else if (type == "H")
            {
                this.Health = 50;
                this.Display = "H ";
            }
        }

        public int AttackOrMove(Zerg[] zergs, string[,] map, int x, int y)
        {
            if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1))
            {
                if (map[x, y] != "x " && map[x, y] != "Q " && map[x, y] != "W " && map[x, y] != "E " && map[x, y] != "H ")
                {
                    if (this.Range >= this.X - x + this.Y - y)
                    {
                        bool found = false;
                        int i = 0;
                        while (i < zergs.Length && !found)
                        {
                            if (zergs[i] != null && zergs[i].X == x && zergs[i].Y == y)
                            {
                                zergs[i].Health -= this.Damage;
                                found = true;
                                if (zergs[i].Health <= 0)
                                {
                                    if (map[x, y] == "C ")
                                    {
                                        return 1;
                                    }

                                    map[x, y] = "x ";
                                    zergs[i] = null;
                                }
                            }

                            i++;
                        }

                        return 0;
                    }
                }
                else if (this.Move(map, x, y))
                {
                    return 0;
                }
            }

            return -1;
        }

        public bool Move(string[,] map, int x, int y)
        {
            if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1))
            {
                if (((x - this.X == 1 || x - this.X == -1) && y == this.Y) || (y - this.Y == 1 || y - this.Y == -1) && x == this.X)
                {
                    if (map[x, y] != "Q " && map[x, y] != "W " && map[x, y] != "E " && map[x, y] != "H ")
                    {
                        map[this.X, this.Y] = "x ";
                        this.X = x;
                        this.Y = y;
                        map[x, y] = this.Display;
                        return true;
                    }
                }
            }

            return false;
        }

        public int CraftingTimeReduce()
        {
            this.CraftingTime--;
            return this.CraftingTime;
        }
    }
}
