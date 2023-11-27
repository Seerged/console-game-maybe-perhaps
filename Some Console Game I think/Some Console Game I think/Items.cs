namespace Entities;

public enum HealthPotions
{
    Health,
    SuperHealth
}

public enum CritPotions
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
                Name = "Health Potion",
                Description = "Heals you. It really doesn't do much else.",
                Quantity = 1,
                Healing = 20,
                ShopPrice = 100
            }
        },
        {
            HealthPotions.SuperHealth, new()
            {
                Name = "S Health Potion",
                Description = "Short for Super Health Potion. Basically a beefed up version of the health potion.",
                Quantity = 1,
                Healing = 50,
                ShopPrice = 214
            }
        }
    };

    public readonly Dictionary<CritPotions, CritPotion> criticalPotions = new()
    {
        {
            CritPotions.Critical, new()
            {
                Name = "Crit Potion",
                Description = "Increases crit damage multiplier.",
                Quantity = 1,
                CritMultiToAdd = .5f,
                ShopPrice = 163
            }
        }
    };

    public HealingItem ReturnItem(HealthPotions key) => healingPotions[key];
    public CritPotion ReturnItem(CritPotions key) => criticalPotions[key];
}

public abstract class ItemBase : ICloneable
{
    public string Name { get; init; } = "No Name";
    public string Description { get; init; } = "No Description";
    public float ShopPrice { get; init; }
    private int _quantity;
    public int Quantity
    {
        get => _quantity;

        set
        {
            if (value < 0) 
            {
                Console.WriteLine("WARNING: Quantity decremented below 0. Check logic in code");
                Console.ReadLine();
                return;
            }

            _quantity = value;
        }
    }

    public abstract object Clone();
    public abstract void ItemInfo();
    public abstract string WriteItemTooltip();

    public virtual void Use(Player player) => throw new NotImplementedException();
}

public class HealingItem : ItemBase
{
    public float Healing { get; init; } = 0f;

    public override void ItemInfo()
    {
        Console.Clear();
        Console.WriteLine($"{Name}\n{Description}\nHealing: {Healing}");
        Console.WriteLine("Press enter to continue.");
        Console.ReadLine();
    }

    public override void Use(Player player)
    {
        if (Quantity == 0) return;

        player.Heal(Healing);
        Quantity--;

        Console.Clear();
        Console.WriteLine($"Healed for {Healing} hp.");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    public override string WriteItemTooltip() => $"+{Healing} healing";

    public override object Clone()
    {
        return new HealingItem
        {
            Name = Name,
            Description = Description,
            ShopPrice = ShopPrice,
            Quantity = Quantity,
            Healing = Healing
        };
    }
}

public class CritPotion : ItemBase
{
    public float CritMultiToAdd { get; init; }

    public override void ItemInfo()
    {
        Console.Clear();
        Console.WriteLine($"{Name}\n{Description}\nCritical Damage Increase: {CritMultiToAdd}");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    public override void Use(Player player)
    {
        if (Quantity == 0) return;

        Quantity--;
        player.critToAdd += .5f;

        Console.Clear();
        Console.WriteLine($"Increased crit damage by {CritMultiToAdd} next time you get a critical hit.");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    public override string WriteItemTooltip() => $"{CritMultiToAdd} Crit multiplier";

    public override object Clone()
    {
        return new CritPotion()
        {
            Name = Name,
            Description = Description,
            ShopPrice = ShopPrice,
            Quantity = Quantity,
            CritMultiToAdd = CritMultiToAdd
        };
    }
}