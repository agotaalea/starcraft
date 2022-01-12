using System;

namespace Starcraft
{
    class Program
    {
        public static Random rnd = new Random();

        static void Main(string[] args)
        {
            // kezdő egységek és a map beállítása
            string[,] map = new string[5, 5];
            ZergInventory zergInventory = new ZergInventory();
            TerranInventory terranInventory = new TerranInventory();
            Zerg[] zergs = new Zerg[10];
            zergs[0] = new Zerg("A", (map.GetLength(0) - 1) / 2, 1);
            zergs[1] = new Zerg("S", (map.GetLength(0) - 1) / 2 - 2, 0);
            zergs[2] = new Zerg("D", (map.GetLength(0) - 1) / 2 + 2, 0);
            zergs[3] = new Zerg("C", (map.GetLength(0) - 1) / 2, 0);
            Terran[] terrans = new Terran[10];
            terrans[0] = new Terran("Q", (map.GetLength(0) - 1) / 2, map.GetLength(1) - 2);
            terrans[1] = new Terran("W", (map.GetLength(0) - 1) / 2 + 2, map.GetLength(1) - 1);
            terrans[2] = new Terran("E", (map.GetLength(0) - 1) / 2 - 2, map.GetLength(1) - 1);
            terrans[3] = new Terran("H", (map.GetLength(0) - 1) / 2, map.GetLength(1) - 1);
            zergInventory.AllUnits = zergs;
            terranInventory.AllUnits = terrans;
            for (int i = 0; i < 4; i++)
            {
                map[zergs[i].X, zergs[i].Y] = zergs[i].Display;
                map[terrans[i].X, terrans[i].Y] = terrans[i].Display;
            }

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == null)
                    {
                        map[i, j] = "x ";
                    }
                }
            }

            bool zergRound = true;
            bool end = false;
            // addig tart a játék, amíg meg nem hal az egyik csapat központja
            while (!end)
            {
                // ha még nem halt meg valamelyik csapatnak a központja, akkor jön a következő játékos
                if (!end)
                {
                    zergInventory.SearchAllInProcessUnits(map);
                    MapDrawing(map, zergRound, zergInventory, terranInventory, zergs, terrans);
                    end = ZergTurn(map, zergInventory, terranInventory, zergs, terrans);
                    zergRound = !zergRound;
                }

                if (!end)
                {
                    terranInventory.SearchAllInProcessUnits(map);
                    MapDrawing(map, zergRound, zergInventory, terranInventory, zergs, terrans);
                    end = TerranTurn(map, zergInventory, terranInventory, terrans, zergs);
                    zergRound = !zergRound;
                }
            }

            Console.ReadLine();
        }

        public static bool ZergTurn(string[,] map, ZergInventory zergInventory, TerranInventory terranInventory, Zerg[] zergs, Terran[] terrans)
        {
            Console.WriteLine();
            int steps = 3;
            while (steps > 0)
            {
                string create = Console.ReadLine();
                // egységet generál
                if (create.ToUpper() == "A" || create.ToUpper() == "S" || create.ToUpper() == "D" )
                {
                    if (zergInventory.ZergNumberOfAllUnits() < zergInventory.MaxNumber && zergInventory.ZergPopulation(zergs) < zergInventory.MaxPop)
                    {
                        Zerg temporary = new Zerg(create.ToUpper());
                        // ha az új egységgel együtt sem éri el a csapat a max populációt, akkor létrejön az egység (a mapon még nem látszódik)
                        if (zergInventory.ZergPopulation(zergs) + temporary.PopUnit <= zergInventory.MaxPop)
                        {
                            Zerg newUnit = new Zerg(create.ToUpper());
                            zergInventory.AddUnitToAllUnits(newUnit);
                            steps--;
                        }
                        else
                        {
                            temporary = null;
                        }
                    }
                }
                // ha x,y az első bemenet, és az jó formátumban van megadva, akkor az azt jelenti, hogy egy karaktert akarunk kiválasztani
                else if (CorrectInput(create))
                {
                    string select = create;
                    int x = int.Parse(select.Split(",")[0]);
                    int y = int.Parse(select.Split(",")[1]);
                    int index = SelectZerg(map, zergs, x, y);
                    // ha jó koordinátákat adtunk meg, akkor kiválasztjuk az egységet
                    if (index != -1)
                    {
                        // megadjuk a célkoordinátákat is
                        string destination = Console.ReadLine();
                        int destX = int.Parse(destination.Split(",")[0]);
                        int destY = int.Parse(destination.Split(",")[1]);
                        int success = zergs[index].AttackOrMove(terrans, map, destX, destY);
                        // success = 0 esetben még nem halt meg a központ
                        if (success == 0)
                        {
                            steps--;
                        }
                        // success = 1 esetében meghalt, vége a játéknak
                        else if (success == 1)
                        {
                            steps = 0;
                            GameEnd("Zergs");
                            return true;
                        }
                    }
                }

                // minden lépés után újrarajzoljuk a mapot
                MapDrawing(map, true, zergInventory, terranInventory, zergs, terrans);
            }

            return false;
        }

        public static bool CorrectInput(string input)
        {
            bool isCoordinate = true;
            int commaQuantity = 0;
            int i = 0;
            // addig vizsgáljuk a bemenetet, amíg a string végére nem érünk, vagy ha előbb kiderül a bemenetről, hogy nem megfelelő
            while (i < input.Length && isCoordinate && commaQuantity < 2)
            {
                // ha a bemenet adott karaktere nem szám és nem is vessző, vagy ha a bemenet utolsó karaktere vessző, vagy ha a bemenet első karaktere vessző, akkor biztosan rossz a bemenet
                if ((!char.IsDigit(input[i]) && input[i] != ',') || (i == input.Length - 1 && input[i] == ',') || (i == 0 && input[i] == ','))
                {
                    isCoordinate = false;
                }

                // ha találtunk egy vesszőt, akkor növeljük a változó értékét. Ennek a végén pontosan 1-es értéket kell felvennie ahhoz, hogy a bemenet helyes legyen.
                if (input[i] == ',')
                {
                    commaQuantity++;
                }

                i++;
            }

            // ha több vessző is van a stringben, akkor rossz a bemenet
            if (commaQuantity != 1)
            {
                isCoordinate = false;
            }

            return isCoordinate;
        }

        public static int SelectZerg(string[,] map, Zerg[] zergs, int x, int y)
        {
            int i = 0;
            int index = -1;
            while (i < zergs.Length && index == -1)
            {
                // a megadott x és y alapján megnézzük, van-e azon a ponton Zerg egység, és hogy az az egység lejött-e már a gyártósorról
                if (zergs[i] != null && zergs[i].X == x && zergs[i].Y == y && zergs[i].CraftingTime == 0)
                {
                    // ha a központ van a megadott helyen, akkor a központot nem jelölhetjük ki, nem tudunk vele mozogni vagy támadni
                    if (zergs[i].Display == "C ")
                    {
                        return index;
                    }

                    index = i;
                }
                else
                {
                    i++;
                }
            }

            return index;
        }

        public static bool TerranTurn(string[,] map, ZergInventory zergInventory, TerranInventory terranInventory, Terran[] terrans, Zerg[] zergs)
        {
            Console.WriteLine();
            int steps = 3;
            while (steps > 0)
            {
                string create = Console.ReadLine();
                if (create.ToUpper() == "Q" || create.ToUpper() == "W" || create.ToUpper() == "E")
                {
                    if (terranInventory.TerranNumberOfAllUnits() < terranInventory.MaxNumber && terranInventory.TerranPopulation(terrans) < terranInventory.MaxPop)
                    {
                        Terran temporary = new Terran(create.ToUpper());
                        if (terranInventory.TerranPopulation(terrans) + temporary.PopUnit <= terranInventory.MaxPop)
                        {
                            Terran newUnit = new Terran(create.ToUpper());
                            terranInventory.AddUnitToAllUnits(newUnit);
                            steps--;
                        }
                        else
                        {
                            temporary = null;
                        }
                    }
                }
                else if (CorrectInput(create))
                {
                    string select = create;
                    int x = int.Parse(select.Split(",")[0]);
                    int y = int.Parse(select.Split(",")[1]);
                    int index = SelectTerran(map, terrans, x, y);
                    if (index != -1)
                    {
                        string destination = Console.ReadLine();
                        int destX = int.Parse(destination.Split(",")[0]);
                        int destY = int.Parse(destination.Split(",")[1]);
                        int success = terrans[index].AttackOrMove(zergs, map, destX, destY);
                        if (success == 0)
                        {
                            steps--;
                        }
                        else if (success == 1)
                        {
                            steps = 0;
                            GameEnd("Terrans");
                            return true;
                        }
                    }
                }

                MapDrawing(map, false, zergInventory, terranInventory, zergs, terrans);
            }

            return false;
        }

        public static int SelectTerran(string[,] map, Terran[] terrans, int x, int y)
        {
            int i = 0;
            int index = -1;
            while (i < terrans.Length && index == -1)
            {
                if (terrans[i] != null && terrans[i].X == x && terrans[i].Y == y && terrans[i].CraftingTime == 0)
                {
                    if (terrans[i].Display == "H ")
                    {
                        return index;
                    }

                    index = i;
                }
                else
                {
                    i++;
                }
            }

            return index;
        }

        public static void MapDrawing(string[,] map, bool isZergRound, ZergInventory zergInventory, TerranInventory terranInventory, Zerg[] zergs, Terran[] terrans)
        {
            Console.Clear();
            // fejléc kiírása
            Header(isZergRound, zergInventory, terranInventory, zergs, terrans);
            // egységek és üres területek kirajzolása
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    // üres területek
                    if (map[i, j] == "x ")
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(map[i, j]);
                    }
                    // zerg egységek
                    else if (map[i, j] == "A " || map[i, j] == "S " || map[i, j] == "D " || map[i, j] == "C ")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(map[i, j]);
                    }
                    // terran egységek
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(map[i, j]);
                    }
                }

                Console.WriteLine();
            }

            // lábléc kiírása
            Footer(isZergRound);
        }

        public static void Header(bool isZergRound, ZergInventory zergInventory, TerranInventory terranInventory, Zerg[] zergs, Terran[] terrans)
        {
            // kiírja a soron következő játékos aktuális populációjának értékét
            if (isZergRound)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Zerg turn! - Population: {0}/{1}", zergInventory.ZergPopulation(zergs), 20);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Terran turn! - Population: {0}/{1}", terranInventory.TerranPopulation(terrans), 20);
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        public static void Footer(bool isZergRound)
        {
            // lehetséges parancsok megjelenítése
            Console.WriteLine();
            Console.WriteLine();
            if (isZergRound)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("row,column -> row,column - Select then move or attack");
                Console.WriteLine("A - AddMarine");
                Console.WriteLine("S - AddMarouder");
                Console.WriteLine("D - AddFirebat");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("row,column -> row,column - Select then move or attack");
                Console.WriteLine("Q - AddMarine");
                Console.WriteLine("W - AddMarouder");
                Console.WriteLine("E - AddFirebat");
            }
        }

        public static void GameEnd(string winner)
        {
            // ha vége a játéknak, törlődik a konzol, és kiírjuk a nyertes csapatának nevét
            Console.Clear();
            if (winner == "zerg")
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine("And the winner is the {0}!", winner.ToUpper());
        }
    }
}
