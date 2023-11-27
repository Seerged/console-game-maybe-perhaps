namespace Entities;

public enum WeaponKeys
{
    RustedBlade,
    ShortSword,
    WarAxe
}


public class WeaponsDictionary
{
    public readonly Dictionary<WeaponKeys, Weapon> weapons = new()
    {
        { 
            WeaponKeys.RustedBlade,  new()
            {
                Name = "Rusted Blade",
                Description = "I'm not even sure how it still works. It's very worn down.",
                Quantity = 1,
                Damage = 10,
                CritMultiplier = .5f,
                CritChance = .13f,
                MissChance = .02f
            }
        },

        {
            WeaponKeys.ShortSword, new()
            {
                Name = "Silver Shortsword",
                Description = "A fine weapon. Still has its edge.",
                Quantity = 1,
                Damage = 15,
                CritMultiplier = 1.5f,
                CritChance = .15f,
                MissChance = .02f,
                ShopPrice = 300f
            }
        },

        {
            WeaponKeys.WarAxe, new()
            {
                Name = "War Axe",
                Description = "Looks pretty badass, but its heavy... and slow. At least it does a lot of damage.",
                Quantity = 1,
                Damage = 25,
                CritMultiplier = 2f,
                CritChance = .05f,
                MissChance = .06f,
                ShopPrice = 500f
            }
        }
    };

    public Weapon ReturnWeapon(WeaponKeys weaponKey) => weapons[weaponKey];
}

public abstract class WeaponBase : ItemBase
{
    public float Damage { get; init; }
    public float CritMultiplier { get; init; }
    public float CritChance { get; init; }
    public float MissChance { get; init; }
}

public class Weapon : WeaponBase
{
    public override void ItemInfo()
    {
        Console.Clear();
        Console.WriteLine($"{Name}\nDescription: {Description}\nDamage: {Damage}\nCrit Multiplier: {CritMultiplier}x");
        Console.WriteLine($"Crit Chance: {CritChance:P}\nMiss Chance: {MissChance:P}");
        Console.WriteLine("Press enter to continue.");
        Console.ReadLine();
    }

    public override void Use(Player player)
    {
        if (Quantity < 1) return;

        if (player.weapon != null)
        {
            bool foundWeapon = false;

            for (int i = 0; i < player.inventory.Count; i++)
            {
                if (player.inventory[i].Name == player.weapon.Name)
                {
                    player.inventory[i].Quantity++;
                    foundWeapon = true;
                    break;
                }
            }
            
            if (!foundWeapon) player.inventory.Add(player.weapon);

            Console.WriteLine($"Unequipped {player.weapon.Name}");
        }

        player.weapon = (Weapon) Clone();

        for (int i = 0; i < player.inventory.Count; i++)
        {
            if (player.inventory[i].Name == Name)
            {
                player.inventory[i].Quantity--;
                break;
            }
        }

        Console.WriteLine($"Equipped {player.weapon.Name}");
        Console.WriteLine("Press enter to continue.");
        Console.ReadLine();
    }

    public override string WriteItemTooltip() => $"{Damage} damage";

    public override object Clone()
    {
        return new Weapon()
        {
            Name = Name,
            Description = Description,
            ShopPrice = ShopPrice,
            Quantity = Quantity,
            Damage = Damage,
            CritMultiplier = CritMultiplier,
            CritChance = CritChance,
            MissChance = MissChance
        };
    }
}