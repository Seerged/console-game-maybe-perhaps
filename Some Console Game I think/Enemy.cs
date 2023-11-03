namespace Entities;

public class Enemy : Entity
{
    public Enemy(float hp, float dmg)
    {
        health = hp;
        damage = dmg;
    }

    public float Attack(Player player)
    {
        Random random = new();
        double randChance = random.NextDouble();
        Console.WriteLine($"{randChance:P}");
        float critMultiplier = 1;

        float dmg = damage;

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

        Console.WriteLine($"You were attacked for {dmg * critMultiplier} damage!");
        Console.WriteLine("Press enter to continue.");
        Console.ReadLine();
        return player.health -= dmg * critMultiplier;
    }

    public override void DisplayInfo()
    {
        Console.Clear();
        Console.WriteLine("ENEMY");
        Console.WriteLine("------");
        Console.WriteLine($"Health: {health}");
        Console.WriteLine($"Damage: {damage}");
        Console.WriteLine("Press enter to continue.");
        Console.ReadLine();
    }
}