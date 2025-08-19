public interface IStatusEffect
{ 
    void Apply(Unit unit);
    void Remove(Unit unit);
    void Tick(float deltaTime);
    float Duration { get; }
}

