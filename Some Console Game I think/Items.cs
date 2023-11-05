using System.Runtime.InteropServices;

namespace Entities;

public enum HealthPotions
{
    Health,
    SuperHealth
}

public enum CriticalPotions
{
    Critical
}

public class Items
{
    public readonly Dictionary<HealthPotions, HealingItem> healingPotions = new()
    {
        {
            HealthPotions.Health, new()
            {
                name = "Health Potion",
                description = "Heals you. It really doesn't do much else.",
                Quantity = 1,
                healing = 20,
                shopPrice = 100
            }
        },
        {
            HealthPotions.SuperHealth, new()
            {
                name = "SHealth Potion",
                description = "Basically a beefed up version of the health potion.",
                Quantity = 1,
                healing = 50,
                shopPrice = 214
            }
        }
    };

    public readonly Dictionary<CriticalPotions, CriticalItem> criticalPotions = new()
    {
        {
            CriticalPotions.Critical, new()
            {
                name = "Crit Potion",
                description = "Increases crit damage multiplier.",
                Quantity = 1,
                critMultiToAdd = .5f,
                shopPrice = 163
            }
        }
    };

    public HealingItem ReturnItem(HealthPotions key) => healingPotions[key];
    public CriticalItem ReturnItem(CriticalPotions key) => criticalPotions[key];
}

public abstract class ItemBase : ICloneable
{
    public string name = "No Name";
    public string description = "No Description";
    public float shopPrice;
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

    public abstract object Clone();

    public abstract void ItemInfo();
    
    public virtual void Use(Player player) => throw new NotImplementedException();
}

public class HealingItem : ItemBase
{
    public float healing = 0f;

    public override void ItemInfo()
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

    public override object Clone()
    {
        return new HealingItem
        {
            name = name,
            description = description,
            shopPrice = shopPrice,
            Quantity = Quantity,
            healing = healing
        };
    }
}

public class CriticalItem : ItemBase
{
    public float critMultiToAdd;

    public override void ItemInfo()
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

    public override object Clone()
    {
        return new CriticalItem()
        {
            name = name,
            description = description,
            shopPrice = shopPrice,
            Quantity = Quantity,
            critMultiToAdd = critMultiToAdd
        };
    }
}