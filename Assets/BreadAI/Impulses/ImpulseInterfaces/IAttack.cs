using System.Collections.Generic;

public interface IAttack : IImpulse
{
    List<ImpulseVariable> AttackImpulses { get; }

}
