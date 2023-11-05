namespace Entities;

public enum PlayerStates
{
    Alive,
    Dead,
    Stunned
}

public class Player : Entity
{
    float critMultiplier = 1;
    public float critToAdd = 0;
    public int defeatedEnemies = 0;
    public float gold;

    public PlayerStates playerState = PlayerStates.Alive;
    public List<ItemBase> inventory = new();

    public Player(float hp = 100, float dmg = 10, float gold = 0)
    {
        damage = dmg;
        health = hp;
        this.gold = gold;
    }

    public float Attack(Enemy enemyToAttack)
    {
        if (playerState == PlayerStates.Dead) throw new ArgumentException("You're dead stupid. You can't attack.");

        critMultiplier = 1;

        if (playerState == PlayerStates.Stunned)
        {
            playerState = PlayerStates.Alive;
            Console.WriteLine("Player is stunned and cannot attack. You can attack in (1) turn.");
            Console.WriteLine("Press enter to continue.");
            Console.ReadLine();
            return 0;
        }

        Random random = new();
        double randChance = random.NextDouble();
        Console.WriteLine($"{randChance:P}");

        float dmg = damage;

        // calculate missed hit
        if (randChance > .98) {
            Console.WriteLine("Missed!");
            Console.WriteLine("Press enter to continue.");
            Console.ReadLine();
            return 0;
        }

        // calculate critical hit
        if (randChance > .85 && randChance < .98){
            Console.WriteLine("CRITICAL HIT!");
            critMultiplier = 2.5f;
            critMultiplier += critToAdd;

            critToAdd = 0;
        }

        Console.WriteLine($"{dmg * critMultiplier} damage dealt to the enemy.");
        Console.WriteLine("Press enter to continue.");
        Console.ReadLine();
        return enemyToAttack.health -= dmg * critMultiplier;
    }

    public void ViewInventory()
    {
        if (playerState == PlayerStates.Stunned)
        {
            Console.WriteLine("Player is stunned. You cannot access items. Select \"attack\" to continue.");
            Console.WriteLine("Press enter to continue.");
            Console.ReadLine();
        }

        if (inventory.Count < 1)
        {
            Console.WriteLine("You have no items available.\nPress enter to continue.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Item\tName\t\tQuantity\tStats");
        
        for (int i = 0; i < inventory.Count; i++)
        {
            Console.Write($"{i + 1}. \t");
            Console.Write(inventory[i].name);
            Console.Write($"\t{inventory[i].Quantity}\t");
            Console.WriteLine();
        }

        Console.WriteLine("\nType the item number you would like to access (or press enter to exit)");
        string? readInput = Console.ReadLine();

        if (readInput != null)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                bool itemExists = false;

                if ((i + 1).ToString() == readInput)
                {
                    itemExists = true;
                    Console.Clear();
                    Console.WriteLine($"Item selected: {inventory[i].name}");
                    Console.WriteLine("What would you like to do with this item?");
                    Console.WriteLine("1. Use\n2. Inspect");
                    readInput = Console.ReadLine();

                    switch (readInput)
                    {
                        case "1":
                            inventory[i].Use(this);
                            break;

                        case "2":
                            inventory[i].ItemInfo();
                            break;
                    }
                }

                if (itemExists) break;
            }
        }

    }

    public void VerifyInventory()
    {
        for (int i = 0; i < inventory.Count; i++)
            if (inventory[i].Quantity < 1) inventory.Remove(inventory[i]);   
    }

    public override void DisplayInfo()
    {
        Console.Clear();
        Console.WriteLine("PLAYER");
        Console.WriteLine("------");
        Console.WriteLine($"Health: {health}");
        Console.WriteLine($"Damage: {damage}");
        Console.WriteLine($"Items in inventory: {inventory.Count}");
        Console.WriteLine($"Enemies defeated: {defeatedEnemies}");
        Console.WriteLine("\nPress enter to continue.");
        Console.ReadLine();
    }
}