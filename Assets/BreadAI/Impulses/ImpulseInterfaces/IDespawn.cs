using System.Collections.Generic;

public interface IDespawn : IImpulse
{
    List<ImpulseVariable> DespawnImpulses { get; }

}
