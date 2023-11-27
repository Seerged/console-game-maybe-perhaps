namespace Entities;

public class Enemy : Entity
{
    public string Name { get; init; }

    public Enemy(float hp, float dmg, string enemyName = "Enemy")
    {
        Health = hp;
        Damage = dmg;
        Name = enemyName;
    }

    public float Attack(Player player)
    {
        Random random = new();
        double randChance = random.NextDouble();
        Console.WriteLine($"{randChance:P}");
        float critMultiplier = 1;

        float dmg = Damage;

        if (randChance > .89 && randChance < .9)
        {
            Console.WriteLine("STUNNING HIT!");
            Console.WriteLine("Player is stunned!");
            critMultiplier = 1.5f;
            player.playerState = PlayerStates.Stunned;
        }

        // calculate missed hit
        if (randChance > .98){
            Console.WriteLine("Missed!");
            Console.WriteLine("Press enter to continue.");
            Console.ReadLine();
            return 0;
        }

        // calculate critical hit
        if (randChance > .90 && randChance < .98){
            Console.WriteLine("CRITICAL HIT!");
            critMultiplier = 2.5f;      
        }

        Console.WriteLine($"You were attacked for {Math.Round(dmg * critMultiplier, 2)} damage!");
        Console.WriteLine("Press enter to continue.");
        Console.ReadLine();
        return player.TakeDamage(dmg * critMultiplier);
    }

    public override float TakeDamage(float damage) => Health -= damage;

    public override void DisplayInfo()
    {
        Console.Clear();
        Console.WriteLine(Name);
        Console.WriteLine("------");
        Console.WriteLine($"Health: {Health}");
        Console.WriteLine($"Damage: {Math.Round(Damage, 2)}");
        Console.WriteLine("\nPress enter to continue.");
        Console.ReadLine();
    }
}