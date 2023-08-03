using System.Collections.Generic;

public interface ISpawn : IImpulse
{
    List<ImpulseVariable> SpawnImpulses { get; }

}
