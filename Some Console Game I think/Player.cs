namespace Entities;

public enum PlayerStates
{
    Alive,
    Dead,
    Stunned
}

public class Player
{
    public float health = 100f;
    public float damage = 5f;
    public PlayerStates playerState = PlayerStates.Alive;
    float critMultiplier = 1;
    public float critToAdd = 0;

    public List<ItemBase> inventory = new()
    {
        Items.ReturnItem(HealthPotions.Health),
        Items.ReturnItem(CriticalPotions.Critical)
    };

    public Player(float hp = 100, float dmg = 10)
    {
        damage = dmg;
        health = hp;
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
}