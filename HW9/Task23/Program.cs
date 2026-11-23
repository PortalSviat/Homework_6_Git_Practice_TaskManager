using System;

public interface IDamageCalculator
{
    int CalculateDamage(int baseDamage, int targetArmor);
}

public class PhysicalDamageCalculator : IDamageCalculator
{
    public int CalculateDamage(int baseDamage, int targetArmor)
    {
        int damage = baseDamage - targetArmor / 2;
        return Math.Max(1, damage);
    }
}

public class MagicDamageCalculator : IDamageCalculator
{
    public int CalculateDamage(int baseDamage, int targetArmor)
    {
        int damage = Math.Max(1, baseDamage - targetArmor / 4);

        if (targetArmor == 0)
        {
            damage *= 2;
        }

        return damage;
    }
}

public class FireDamageCalculator : IDamageCalculator
{
    public int CalculateDamage(int baseDamage, int targetArmor)
    {
        int damage = Math.Max(1, baseDamage - targetArmor / 10);

        if (baseDamage > 50)
        {
            damage += 10;
        }

        return damage;
    }
}

public class Unit
{
    public int HP;
    public int Armor;
    public string Name;

    public Unit(string name, int hp, int armor)
    {
        Name = name;
        HP = hp;
        Armor = armor;
    }
}

public class DamageSystem
{
    private readonly IDamageCalculator calculator;

    public DamageSystem(IDamageCalculator calculator)
    {
        this.calculator = calculator;
    }

    public void ApplyDamage(Unit target, int baseDamage)
    {
        int damage = calculator.CalculateDamage(baseDamage, target.Armor);
        target.HP = Math.Max(0, target.HP - damage);
        Console.WriteLine($"Юнит {target.Name} получил {damage} урона. Осталось HP: {target.HP}");
    }
}

public class Program
{
    public static void Main()
    {
        Unit target = new Unit("Гоблин", 300, 40);
        DamageSystem physical = new DamageSystem(new PhysicalDamageCalculator());
        DamageSystem magic = new DamageSystem(new MagicDamageCalculator());
        DamageSystem fire = new DamageSystem(new FireDamageCalculator());

        Console.WriteLine("Физический урон, базовый урон 60, броня 40:");
        physical.ApplyDamage(target, 60);

        Console.WriteLine("Магический урон, базовый урон 60, броня 40:");
        magic.ApplyDamage(target, 60);

        Console.WriteLine("Огненный урон, базовый урон 60, броня 40:");
        fire.ApplyDamage(target, 60);

        // Это всё тот же объект. Меняем броню, чтобы проверить удвоение магии.
        target.Armor = 0;
        Console.WriteLine("Магический урон, базовый урон 20, броня 0:");
        magic.ApplyDamage(target, 20);
    }

    // Чтобы добавить ледяной урон, нужно создать IceDamageCalculator,
    // реализующий IDamageCalculator, и описать в CalculateDamage его формулу.
    // Затем передать new IceDamageCalculator() в конструктор DamageSystem.
    // DamageSystem менять не нужно: он уже работает с любым IDamageCalculator.
}
