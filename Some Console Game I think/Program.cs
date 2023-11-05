using Entities;

/*
  TO DO
  - Add random events (finding an item, or getting obliterated by the sun lol)
  - Give player a weapon system (Somewhat similar to item system, only in the sense of it deriving from ItemBase)
  - When weapon system is implemented, make it at certain point enemies become stronger
*/

Player player = new Player(150);

Random random = new();

int total = 0;
int target = 50;

bool isPlaying = false;
bool hasWon = false;
bool defeatedEnemy;

string? readResult;

Console.Clear();

Console.WriteLine("Turn Based Console Game\n");
Console.WriteLine($"How to play:\nyou must roll a die until you reach the target, which is {target}. But to roll the die, you must first defeat an enemy.");
Console.WriteLine("Each time you roll the die, a new enemy appears. Along the way, you may encounter some special events.");
Console.WriteLine("\nPlay? [y / n]");

readResult = Console.ReadLine();

if (readResult != null)
{
  if (readResult.ToLower() == "y") isPlaying = true;
}

while (isPlaying && !hasWon)
{
  Console.Clear();
  DiceRoll();

  if (!hasWon)
  {
    Console.WriteLine("A NEW ENEMY APPEARS...\n");
    Thread.Sleep(1500);

    SelectOptions(GenerateRandEnemy(25, 50));
    TryShop();
  }
}

void DiceRoll()
{
  Console.Clear();
  Console.WriteLine("The dice is rolled...\n");

  Thread.Sleep(random.Next(1000, 1500));

  int roll = 0;

  if (total < target) 
  {
    roll = random.Next(0, 7);
    total += roll;
  } 
  else if (total > target) 
  {
    roll = random.Next(1, 3);
    total -= roll;
    Console.WriteLine($"Your total is above {target} so rolls will be deducted until you hit the target.");
  }

  Console.WriteLine($"{(roll > 0 ? $"Rolled a {roll}! Your current total is {total}" : "Rolled a 0. How unfortunate.")}");

  if (total == target)
  {
    hasWon = true;
    Console.WriteLine("Congratulations! You've successfully survived!");
    Console.WriteLine("Now give yourself a pat on the back. Or maybe play again.");
    Console.ReadLine();
    return;
  }

  Console.WriteLine("Press any key to continue");
  Console.ReadKey();
  Console.Clear();
}

void SelectOptions(Enemy enemy)
{
  do
  {
    Console.Clear();
    Console.WriteLine("Your Turn");
    Console.WriteLine($"Current Total: {total}");
    Console.WriteLine("-------------------");
    Console.WriteLine($"Enemy Health: {Math.Round(enemy.health, 2)}");
    Console.WriteLine($"Your Health: {Math.Round(player.health, 2)}");
    Console.WriteLine("-------------------");
    Console.WriteLine("What will you do?");
    Console.WriteLine("1: Attack");
    Console.WriteLine("2: Access Inventory");
    Console.WriteLine("3: View Stats");

    readResult = Console.ReadLine();

    Console.Clear();

    if (readResult != null)
    {
      switch (readResult.ToLower())
      {
        case "1":
          player.Attack(enemy);

          if (enemy.health <= 0)
          {
            defeatedEnemy = true;
            player.defeatedEnemies++;

            float goldEarned = player.defeatedEnemies > 7 ? random.Next(70, 110) : random.Next(50, 70);
            player.gold += goldEarned;

            Console.WriteLine($"Enemy defeated! You earned {goldEarned} gold from that fight.");
            Console.WriteLine("With another enemy defeated, the dice can be rolled.\n");
            Console.WriteLine("Press enter to continue.");

            Console.ReadLine();
            return;
          }
          
          Console.WriteLine("Waiting for enemy...");
          Thread.Sleep(random.Next(1000, 2500));

          enemy.Attack(player);
          break;

        case "2":
          player.ViewInventory();
          player.VerifyInventory();
          break;

        case "3":
          Console.WriteLine("Which stats do you want to check?");
          Console.WriteLine("1. Your Stats\n2. Enemy Stats");

          readResult = Console.ReadLine();

          switch (readResult)
          {
            case "1":
              player.DisplayInfo();
              break;

            case "2":
              enemy.DisplayInfo();
              break;
          }
          break;

        case "exit":
          isPlaying = false;
          break;
      }
    }

    if (player.health <= 0)
    {
      Console.WriteLine("you ded...\nGAME OVER");

      player.playerState = PlayerStates.Dead;
      isPlaying = false;

      Console.ReadKey();
    }

  } while(!defeatedEnemy && player.health <= 0 == false && isPlaying);
}

void TryShop()
{
  if (random.NextSingle() < .7f) return;

  Console.WriteLine("During your travels, you've stumbled upon a shop!");
  Console.WriteLine("Would you like to enter the shop? [y / n]");

  do
  {
    readResult = Console.ReadLine();
    if (readResult != null) readResult = readResult.ToLower();

  } while (readResult != "y" && readResult != "n");

  if (readResult == "n") return;

  ItemBase[] itemsForSale = GenerateItems();

  do
  {
    Console.Clear();

    Console.WriteLine("Take a look at the shopkeepers items...\n");
    Console.WriteLine("Item\tName\t\tQuantity\tCost");
    for (int i = 0; i < itemsForSale.Length; i++)
    {
      Console.WriteLine($"{i + 1}. \t{itemsForSale[i].name}\t{itemsForSale[i].Quantity}\t\t{itemsForSale[i].shopPrice} gold");
    }

    Console.WriteLine($"\nYou have: {player.gold} gold available.");
    Console.WriteLine("Select an item you would like to inspect. (or type exit to leave the shop).");

    readResult = Console.ReadLine();

    if (readResult != null)
    {
      if (readResult.ToLower() == "exit") return;

      for (int i = 0; i < itemsForSale.Length; i++)
      {
        bool itemExists = false;

        if ((i + 1).ToString() == readResult)
        {
          itemExists = true;
          if (itemsForSale[i].Quantity < 1)
          {
            Console.Clear();
            Console.WriteLine("We're out of this item.");

            Console.WriteLine("Press enter to continue.");
            Console.ReadLine();
            break;
          }

          Console.Clear();
          Console.WriteLine($"Item selected: {itemsForSale[i].name}");
          Console.WriteLine("What would you like to do with this item?");
          Console.WriteLine("1. Buy\n2. Inspect");
          readResult = Console.ReadLine();

          switch (readResult)
          {
            case "1":
              if (itemsForSale[i].shopPrice > player.gold)
                Console.WriteLine($"You do not have enough gold to purchase this item! You need: {itemsForSale[i].shopPrice - player.gold} gold to purchase this item.");

              else
              {
                if (player.inventory.Contains(itemsForSale[i]))
                {
                  for (int j = 0; j < player.inventory.Count; j++)
                  {
                    if (itemsForSale[i] == player.inventory[j])
                      player.inventory[j].Quantity++;
                  }
                }
                else
                {
                  ItemBase item = itemsForSale[i];
                  player.inventory.Add((ItemBase) item.Clone());
                }

                itemsForSale[i].Quantity--;

                player.gold -= itemsForSale[i].shopPrice;

                Console.WriteLine($"Purchased {itemsForSale[i].name} for {itemsForSale[i].shopPrice} gold!");
                Console.WriteLine($"You have {player.gold} gold remaining.");
              }

              Console.WriteLine("Press enter to continue.");
              Console.ReadLine();
              break;

            case "2":
              itemsForSale[i].ItemInfo();
              break;
          }
        }

        if (itemExists) break;
      }
    }  
  } while (true);
}

Enemy GenerateRandEnemy(int minHealth, int maxHealth)
{
  defeatedEnemy = false;
  return new Enemy(random.Next(minHealth, maxHealth), (float) Math.Round(random.NextSingle() + 1, 2) * 5);
}

ItemBase[] GenerateItems()
{
  Items items = new();

  List<ItemBase> itemsAvailable = new()
  {
    { items.ReturnItem(CriticalPotions.Critical) },
    { items.ReturnItem(HealthPotions.Health) },
    { items.ReturnItem(HealthPotions.SuperHealth) }
  };

  List<ItemBase> itemsForSale = new();

  // generate 2 items
  for (int i = 0; i <= 1; i++)
  {
    int randIndex = random.Next(0, itemsAvailable.Count);
    
    itemsForSale.Add(itemsAvailable[randIndex]);
    itemsAvailable.Remove(itemsAvailable[randIndex]);

    itemsForSale[i].Quantity = random.Next(1, 4);
  }

  return itemsForSale.ToArray();
}