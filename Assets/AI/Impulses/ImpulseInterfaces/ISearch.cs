using System.Collections.Generic;

public interface ISearch : IImpulse
{
    public List<ImpulseVariable> SearchImpulses { get; }
}
