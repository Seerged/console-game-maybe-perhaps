using Entities;

Player player = new(150);

Random random = new();

int total = 0;
int target = 50;

bool isPlaying = false;
bool hasWon = false;
bool defeatedEnemy = false;

bool debug = false;

string? readResult;

Console.Clear();

if (!debug)
{
  Console.WriteLine("Turn Based Console Game\n");
  Console.WriteLine($"How to play:\nyou must roll a die until you reach the target, which is {target}.\nBut to roll the die, you must first defeat an enemy.");
  Console.WriteLine("Each time you roll the die, a new enemy appears.\nAlong the way, you may encounter some interesting scenarios.");
  Console.WriteLine("\nI will warn you though. This game is super rng...");
  Console.WriteLine("\nPlay? [y / n]");

  readResult = Console.ReadLine();

  if (readResult != null)
  {
    if (readResult.ToLower() == "y") isPlaying = true;
  }
}
else
{
  isPlaying = true;
}

GameplayLoop();

void GameplayLoop()
{
  while (isPlaying && !hasWon)
  {
    Console.Clear();
    DiceRoll();

    if (!hasWon)
    {
      Console.WriteLine("A NEW ENEMY APPEARS...\n");
      Thread.Sleep(1500);

      if (player.defeatedEnemies <= 7)
      {
        Fight(GenerateRandomEnemy(25, 50, 5));
      }
      else if (player.defeatedEnemies > 7)
      {
        Fight(GenerateRandomEnemy(35, 65, 6));
      }

      RandomEvent();
    }
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

void Fight(Enemy enemy)
{
  do
  {
    Console.Clear();
    Console.WriteLine("Your Turn");
    Console.WriteLine($"Current Total: {total}");
    Console.WriteLine("-------------------");
    Console.WriteLine($"{enemy.Name} Health: {Math.Round(enemy.Health, 2)}");
    Console.WriteLine($"Your Health: {Math.Round(player.Health, 2)}");
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

          if (enemy.Health <= 0)
          {
            player.defeatedEnemies++;

            if (player.defeatedEnemies == 7)
            {
              Console.WriteLine("The enemies are only going to get more difficult from now on.\n");
            }

            float goldEarned = player.defeatedEnemies > 7 ? random.Next(80, 130) : random.Next(50, 71);
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

    if (player.Health <= 0)
    {
      Console.WriteLine("you ded...\nGAME OVER");

      player.playerState = PlayerStates.Dead;
      isPlaying = false;

      Console.ReadKey();
    }

  } while(!defeatedEnemy && player.Health <= 0 == false && isPlaying);
}

void ShopEvent()
{
  Console.Clear();
  Console.WriteLine("During your travels, you've stumbled upon a shop!");
  Console.WriteLine("Would you like to enter the shop? [y / n]");

  do
  {
    readResult = Console.ReadLine();
    if (readResult != null) readResult = readResult.ToLower();

  } while (readResult != "y" && readResult != "n");

  if (readResult == "n") return;

  ItemBase[] itemsForSale = GenerateItems(2, 4);

  do
  {
    Console.Clear();

    Console.WriteLine("Take a look at the shopkeepers items...\n");
    Console.WriteLine($"Item{"Name", 20}{"Quantity", 13}{"Cost", 14}");

    for (int i = 0; i < itemsForSale.Length; i++)
    {
      Console.WriteLine($"{i + 1}.{itemsForSale[i].Name, 22}{itemsForSale[i].Quantity, 13}{$"{itemsForSale[i].ShopPrice} gold", 14}");
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
          Console.WriteLine($"Item selected: {itemsForSale[i].Name}");
          Console.WriteLine("What would you like to do with this item?");
          Console.WriteLine("1. Buy\n2. Inspect");
          readResult = Console.ReadLine();

          switch (readResult)
          {
            case "1":
              if (itemsForSale[i].ShopPrice > player.gold)
                Console.WriteLine($"You do not have enough gold to purchase this item! You need: {itemsForSale[i].ShopPrice - player.gold} gold to purchase this item.");

              else
              { 
                bool hasItem = false;

                for (int j = 0; j < player.inventory.Count; j++)
                {
                  if (itemsForSale[i].Name == player.inventory[j].Name)
                  { 
                    hasItem = true;
                    player.inventory[j].Quantity++;
                  }
                }
                
                if (!hasItem)
                {
                  ItemBase item = itemsForSale[i];
                  
                  player.inventory.Add((ItemBase) item.Clone());
                  player.inventory[^1].Quantity = 1;
                }

                itemsForSale[i].Quantity--;

                player.gold -= itemsForSale[i].ShopPrice;

                Console.WriteLine($"Purchased {itemsForSale[i].Name} for {itemsForSale[i].ShopPrice} gold!");
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

void ChestEvent()
{
  Console.Clear();
  Console.WriteLine("You've found a chest...");
  Console.WriteLine("Would you like to open it? It may have something good, or something bad, who knows... [y / n]");

  do
  {
    readResult = Console.ReadLine();
    if (readResult != null) readResult = readResult.ToLower();
  } while (readResult != "y" && readResult != "n");

  if (readResult == "n") return;

  Console.Clear();

  float randomChance = random.NextSingle();

  if (randomChance > .8f) // item
  {
    ItemBase item = GenerateRandomItem();
    Console.WriteLine("You got... an item!\n");
    Console.WriteLine($"And that item is called {item.Name}.");

    bool foundItem = false;

    for (int i = 0; i < player.inventory.Count; i++)
    {
      if (player.inventory[i].Name == item.Name)
      {
        player.inventory[i].Quantity++;
        foundItem = true;

        Console.WriteLine($"+1 {item.Name}.");
      }
    }

    if (!foundItem)
    {
      Console.WriteLine($"{item.Name} has been added to your inventory.");
      player.inventory.Add(item);
    }
  }
  else if (randomChance > .65f) // gold
  {
    int goldEarned = random.Next(150, 400);

    Console.WriteLine("GOLD!");
    Console.WriteLine($"Found {goldEarned} gold in the chest.");

    player.gold += goldEarned;
  }
  else if (randomChance > .6f) // bomb
  {
    Console.WriteLine("It appears its a bomb.");

    int healthLost = random.Next(10, 26);

    Console.WriteLine("... and it blew up in your face.");
    Console.WriteLine($"Lost {healthLost} health.");
    player.TakeDamage(healthLost);

    if (player.Health <= 0)
    {
      isPlaying = false;
      Console.WriteLine("... and you died as a result\n. . . that sucks.");
    }
  }
  else if (randomChance <= .05f) // fight mimic
  {
    Console.WriteLine("It was a MIMIC!");
    Thread.Sleep(2000);
    Fight(new Enemy(75, 10, "Mimic"));

    Console.WriteLine("Well that was... surprising.");
    Console.WriteLine("It dropped a bit more gold than usual AND added some to your roll total, somehow.\n");

    int goldEarned = random.Next(100, 300);
    player.gold += goldEarned;
    Console.WriteLine($"Earned an extra {goldEarned} gold.");

    total += 2;
    Console.WriteLine("2 Has been added to your total.");

    if (total == target)
    {
      Console.WriteLine("And wow... you won.");
      Console.WriteLine("Good job.");
    }
  }
  else // nothing
  {
    Console.WriteLine("Well your choice didn't seem to affect you much.");
    Console.WriteLine("Because you earned nothing.");
    Console.WriteLine("Enter to continue >:)");
    Console.ReadLine();
    return;
  }

  Console.WriteLine("\nPress enter to continue.");
  Console.ReadLine();
}

void WizardEvent()
{
  Console.Clear();
  Console.WriteLine("A Wizard approaches you!");
  Console.WriteLine("He wants to try his new luck spell on you.");
  Console.WriteLine("Do you allow him to perform the spell? It could be good or bad... [y / n]");

  do
  {
    readResult = Console.ReadLine();
    if (readResult != null) readResult = readResult.ToLower();
  } while (readResult != "y" && readResult != "n");

  if (readResult == "n") return;

  Console.Clear();

  if (random.NextSingle() > .6f) // good event
  {
    float randomChance = random.NextSingle();

    if (randomChance > .7f)
    {
      int healAmount = random.Next(50, 70);
      Console.WriteLine("He has healed you greatly!");
      Console.WriteLine($"The wizard has healed you {healAmount} health.");

      player.Heal(healAmount);
    }
    else if (randomChance > .5f)
    {
      int critIncrease = random.Next(1, 2);

      Console.WriteLine("His spell seems to have empowered your next critical hit.");
      Console.WriteLine($"+{critIncrease} crit multiplier on your next hit.");

      player.critToAdd += critIncrease;
    }
    else
    {
      Console.WriteLine("The spell worked!");
      int goldEarned = random.Next(50, 125);

      Console.WriteLine("Admittedly it isn't much gold, but still, its gold.");
      Console.WriteLine($"The spell gave you {goldEarned} gold.");

      player.gold += goldEarned;
    }
  }
  else // bad event
  {
    int damageTaken = random.Next(10, 21);

    Console.WriteLine("The Wizards' spell failed...");
    Console.WriteLine("And it set you alight...");

    Console.WriteLine($"You took {damageTaken} damage from the fire as a result.");

    player.TakeDamage(damageTaken);

    if (player.Health <= 0)
    {
      isPlaying = false;
      Console.WriteLine("... and you died as a result\n. . . that sucks.");
    }
  }

  Console.WriteLine("Press enter to continue.");
  Console.ReadLine();
}

void FoodEvent()
{
  Console.Clear();
  Console.WriteLine("So, you found a cake... in the middle of the forest.");
  Console.WriteLine("It's literally sitting on a treestump in a clearing.");
  Console.WriteLine("I wouldn't eat it, but if you want to... [y / n]");

  do
  {
    readResult = Console.ReadLine();
    if (readResult != null) readResult = readResult.ToLower();
  } while (readResult != "y" && readResult != "n");

  if (readResult == "n") return;

  Console.Clear();

  Console.WriteLine("You eat the cake...\n");

  float randomChance = random.NextSingle();

  if (randomChance > .65f)
  {
    Console.WriteLine("... and it was safe to eat... Somehow.");
    Console.WriteLine("It healed you 30 health.");

    player.Heal(30);
  }
  else
  {
    Console.WriteLine("... and it wasn't safe to eat... you were poisoned, idiot.");
    Console.WriteLine("It dealt you 20 health.");

    player.TakeDamage(20);

    if (player.Health <= 0)
    {
      isPlaying = false;
      Console.WriteLine("... and you died as a result\n. . . that sucks.");
    }
  }

  Console.WriteLine("Press enter to continue.");
  Console.ReadLine();
}

void RandomEvent()
{
  if (hasWon || !isPlaying) return;

  float randomChance = random.NextSingle();

  if (randomChance > .7f) ShopEvent();
  else if (randomChance > .56f) ChestEvent();
  else if (randomChance > .4f) WizardEvent();
  else if (randomChance > .35f) FoodEvent();
}

Enemy GenerateRandomEnemy(int minHealth, int maxHealth, int damageMulti)
{
  defeatedEnemy = false;
  return new Enemy(random.Next(minHealth, maxHealth), (float) Math.Round(random.NextSingle() + 1, 2) * damageMulti);
}

ItemBase[] GenerateItems(int limit, int maxQuantity)
{
  WeaponsDictionary weapons = new();
  Items items = new();

  List<ItemBase> itemsAvailable = new()
  {
    { items.ReturnItem(CritPotions.CriticalMultiplier) },
    { items.ReturnItem(HealthPotions.Health) },
    { items.ReturnItem(HealthPotions.SuperHealth) },
    { weapons.ReturnWeapon(WeaponKeys.ShortSword) },
    { weapons.ReturnWeapon(WeaponKeys.WarAxe) }
  };

  if (limit > itemsAvailable.Count) throw new ArgumentOutOfRangeException($"Limit entered is greater than items available in list. Passed {limit} as argument. {nameof(itemsAvailable)} has {itemsAvailable.Count} items available.");

  List<ItemBase> itemsForSale = new();

  for (int i = 0; i < limit; i++)
  {
    int randIndex = random.Next(0, itemsAvailable.Count);
    
    itemsForSale.Add(itemsAvailable[randIndex]);
    itemsAvailable.Remove(itemsAvailable[randIndex]);

    itemsForSale[i].Quantity = random.Next(1, maxQuantity);
  }

  return itemsForSale.ToArray();
}

ItemBase GenerateRandomItem()
{
  WeaponsDictionary weapons = new();
  Items items = new();

  ItemBase[] itemsAvailable = 
  {
    items.ReturnItem(CritPotions.CriticalMultiplier),
    items.ReturnItem(HealthPotions.Health),
    items.ReturnItem(HealthPotions.SuperHealth),
    weapons.ReturnWeapon(WeaponKeys.ShortSword),
    weapons.ReturnWeapon(WeaponKeys.WarAxe) 
  };

  return itemsAvailable[random.Next(0, itemsAvailable.Length)];
}