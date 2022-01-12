using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starcraft
{
    class ZergInventory
    {
        Zerg[] allUnits;
        Zerg[] inProcessUnits;
        int maxPop;
        int maxNumber;

        // az összes egység (ami a pályán van + ami a gyártósoron)
        public Zerg[] AllUnits
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

        // gyártósoron lévő összes egység
        public Zerg[] InProcessUnits
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

        // max ekkora populációval rendelkezhet a játékos
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

        // max ennyi egység lehet a mapon + gyártósoron
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

        public ZergInventory()
        {
            // a tömb akkora, hogy biztosan elférjen benne minden éppen gyártósoron lévő egység
            this.inProcessUnits = new Zerg[20];
            this.MaxNumber = 8;
            this.MaxPop = 20;
        }

        // kiszámolja a játékos populációjának aktuális értékét
        public int ZergPopulation(Zerg[] units)
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

        // kiszámolja, hány egységgel rendelkezik a játékos
        public int ZergNumberOfAllUnits()
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

        // gyártósorra kerül egy egység
        public void AddUnitToAllUnits(Zerg unit)
        {
            int i = this.SearchFreePlaceInAllUnits();
            int j = this.SearchFreePlaceInInProcessUnits();
            this.AllUnits[i] = unit;
            this.InProcessUnits[j] = unit;
        }

        // szabad helyet keres a tömbben
        private int SearchFreePlaceInAllUnits()
        {
            int i = 0;
            while (AllUnits[i] != null)
            {
                i++;
            }

            return i;
        }

        // szabad helyet keres a tömbben
        private int SearchFreePlaceInInProcessUnits()
        {
            int i = 0;
            while (InProcessUnits[i] != null)
            {
                i++;
            }

            return i;
        }

        // lekerül a gyártósorról, és kikerül a pályára az egység
        private void AddUnitToMap(string[,] map, int index)
        {
            bool found = false;
            Random rnd = new Random();
            // addig rakosgatjuk az egységet, amíg üres mezőt nem találunk neki
            do
            {
                int x = rnd.Next(0, map.GetLength(0));
                int y = rnd.Next(0, 2);
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

        // kitöröljük a gyártósor tömbjéből az egységet
        private void DeleteFromInProcessUnits(int index)
        {
            this.InProcessUnits[index] = null;
        }

        // az összes gyártósoron lévő egységet megkeressük a tömbben (a tömbben lehetnek hézagok)
        public void SearchAllInProcessUnits(string[,] map)
        {
            for (int i = 0; i < this.InProcessUnits.Length; i++)
            {
                if (this.InProcessUnits[i] != null)
                {
                    this.InProcessUnits[i].CraftingTimeReduce();
                    // ha az aktuális egység elkészült, akkor levesszük a gyártósorról
                    if (this.InProcessUnits[i].CraftingTime == 0)
                    {
                        AddUnitToMap(map, i);
                    }
                }
            }
        }
    }
}
