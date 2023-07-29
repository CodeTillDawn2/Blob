using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRetarget : IImpulse
{
    public ImpulseVariable Retarget { get; }
}
