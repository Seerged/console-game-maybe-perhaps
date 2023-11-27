namespace Entities;

public abstract class Entity
{
    public float Health { get; protected set; } = 0f;
    public float Damage { get; init; }  = 0f;

    public abstract void DisplayInfo();
    public abstract float TakeDamage(float damage);
}