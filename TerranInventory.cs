using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starcraft
{
    class TerranInventory
    {
        Terran[] allUnits;
        Terran[] inProcessUnits;
        int maxPop;
        int maxNumber;

        public Terran[] AllUnits
        {
            get
            {
                return allUnits;
            }
            set
            {
                allUnits = value;
            }
        }

        public Terran[] InProcessUnits
        {
            get
            {
                return inProcessUnits;
            }
            set
            {
                inProcessUnits = value;
            }
        }

        public int MaxPop
        {
            get
            {
                return maxPop;
            }
            set
            {
                maxPop = value;
            }
        }

        public int MaxNumber
        {
            get
            {
                return maxNumber;
            }
            set
            {
                maxNumber = value;
            }
        }

        public TerranInventory()
        {
            this.inProcessUnits = new Terran[20];
            this.MaxNumber = 8;
            this.MaxPop = 20;
        }

        public int TerranPopulation(Terran[] units)
        {
            int pop = 0;
            for (int i = 0; i < units.Length; i++)
            {
                if (units[i] != null)
                {
                    pop += units[i].PopUnit;
                }
            }

            return pop;
        }

        public int TerranNumberOfAllUnits()
        {
            int count = 0;
            for (int i = 0; i < this.allUnits.Length; i++)
            {
                if (this.allUnits[i] != null)
                {
                    count++;
                }
            }

            return count;
        }

        public void AddUnitToAllUnits(Terran unit)
        {
            int i = this.SearchFreePlaceInAllUnits();
            int j = this.SearchFreePlaceInInProcessUnits();
            this.AllUnits[i] = unit;
            this.InProcessUnits[j] = unit;
        }

        private int SearchFreePlaceInAllUnits()
        {
            int i = 0;
            while (AllUnits[i] != null)
            {
                i++;
            }

            return i;
        }

        private int SearchFreePlaceInInProcessUnits()
        {
            int i = 0;
            while (InProcessUnits[i] != null)
            {
                i++;
            }

            return i;
        }

        private void AddUnitToMap(string[,] map, int index)
        {
            bool found = false;
            Random rnd = new Random();
            do
            {
                int x = rnd.Next(0, map.GetLength(0));
                int y = rnd.Next(map.GetLength(1) - 2, map.GetLength(1));
                if (map[x, y] == "x ")
                {
                    this.inProcessUnits[index].X = x;
                    this.inProcessUnits[index].Y = y;
                    map[x, y] = this.inProcessUnits[index].Display;
                    this.DeleteFromInProcessUnits(index);
                    found = true;
                }
            } while (!found);
        }

        private void DeleteFromInProcessUnits(int index)
        {
            this.InProcessUnits[index] = null;
        }

        public void SearchAllInProcessUnits(string[,] map)
        {
            for (int i = 0; i < this.InProcessUnits.Length; i++)
            {
                if (this.InProcessUnits[i] != null)
                {
                    this.InProcessUnits[i].CraftingTimeReduce();
                    if (this.InProcessUnits[i].CraftingTime == 0)
                    {
                        AddUnitToMap(map, i);
                    }
                }
            }
        }
    }
}
