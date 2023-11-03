using Entities;

Player player = new Player(200);
//Item item = new Item();

Random random = new();

int total = 0;
int target = 50;

bool isPlaying = false;
bool hasWon = false;
bool defeatedEnemy = false;

string? readResult;

Console.Clear();

Console.WriteLine("Turn Based Console Game");
Console.WriteLine("How to play");
Console.WriteLine("Play? [y/n]");

readResult = Console.ReadLine();

if (readResult != null)
{
  if (readResult.ToLower() == "y") isPlaying = true;
}

while (isPlaying && !hasWon)
{
  Console.Clear();
  DiceRoll();

  Console.WriteLine("A NEW ENEMY APPEARS...\n");
  Thread.Sleep(1500);

  SelectOptions(GenerateRandEnemy(), player);
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
  } else if (total > target) 
  {
    roll = random.Next(1, 2);
    total -= roll;
    Console.WriteLine($"Your total is above {target} so rolls will be deducted until you hit the target.");
  }

  Console.WriteLine($"{(roll > 0 ? $"Rolled a {roll}! Your current total is {total}" : "Rolled a 0. How unfortunate.")}");

  if (total == target)
  {
    hasWon = true;
    Console.WriteLine("Congratulations!");
    return;
  }

  Console.WriteLine("Press any key to continue");
  Console.ReadKey();
  Console.Clear();
}

void SelectOptions(Enemy enemy, Player player)
{
  do
  {
    Console.WriteLine("Your Turn");
    Console.WriteLine($"Current Total: {total}");
    Console.WriteLine("-------------------");
    Console.WriteLine($"Enemy Health: {Math.Round(enemy.health, 2)}");
    Console.WriteLine($"Your Health: {Math.Round(player.health, 2)}");
    Console.WriteLine("-------------------");
    Console.WriteLine("What will you do?");
    Console.WriteLine("1: Attack");
    Console.WriteLine("2: Items");
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
            Console.WriteLine("Enemy defeated!");
            Console.WriteLine("With another enemy defeated, the dice can be rolled.");
            Thread.Sleep(1500);
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

Enemy GenerateRandEnemy()
{
  defeatedEnemy = false;
  return new Enemy(random.Next(25, 50), (float) Math.Round(random.NextSingle() + 1, 2) * 5);
}