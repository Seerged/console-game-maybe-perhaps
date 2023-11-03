namespace Entities;

public abstract class Entity
{
    public float health = 0f;
    public float damage = 0f;

    public abstract void DisplayInfo();
}