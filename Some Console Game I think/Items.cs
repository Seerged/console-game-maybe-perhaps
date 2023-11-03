namespace Entities;

public enum HealthPotions
{
    Health
}

public enum CriticalPotions
{
    Critical
}

public static class Items
{
    public static readonly Dictionary<HealthPotions, HealingItem> healingPotions = new()
    {
        {HealthPotions.Health, new()
            {
                name = "Health Potion",
                description = "Heals you. It really doesn't do much else.",
                Quantity = 0,
                healing = 20
            }
        }
    };

    public static readonly Dictionary<CriticalPotions, CriticalItem> criticalPotions = new()
    {
        {CriticalPotions.Critical, new()
            {
                name = "Crit Potion",
                description = "Increases crit damage multiplier.",
                Quantity = 0,
                critMultiToAdd = .5f
            }
        }
    };

    public static HealingItem ReturnItem(HealthPotions key) => healingPotions[key];
    public static CriticalItem ReturnItem(CriticalPotions key) => criticalPotions[key];
}

public abstract class ItemBase
{
    public string name = "No Name";
    public string description = "No Description";
    private int _quantity;
    public int Quantity
    {
        get => _quantity;

        set
        {
            if (value > 999) throw new ArgumentOutOfRangeException("Quantity cannot be over 1000. How did you get that many anyways?");

            if (value < 0) return;


            _quantity = value;
        }
    }
    
    public abstract void Info();
    
    public virtual void Use(Player player) {}
}

public class HealingItem : ItemBase
{
    public float healing = 0f;

    public override void Info()
    {
        Console.Clear();
        Console.WriteLine($"{name}\n{description}\nHealing: {healing}");
        Console.WriteLine("Press enter to continue.");
        Console.ReadLine();
    }

    public override void Use(Player player)
    {
        if (Quantity == 0) return;

        player.health += healing;
        Quantity--;

        Console.Clear();
        Console.WriteLine($"Healed for {healing} hp.");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }
}

public class CriticalItem : ItemBase
{
    public float critMultiToAdd = 0;

    public override void Info()
    {
        Console.Clear();
        Console.WriteLine($"{name}\n{description}\nCritical Damage Increase: {critMultiToAdd}");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    public override void Use(Player player)
    {
        if (Quantity == 0) return;

        Quantity--;
        player.critToAdd += .5f;

        Console.Clear();
        Console.WriteLine($"Increased crit damage by {critMultiToAdd} next time you get a critical hit.");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }
}