using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDespawn : IImpulse
{
    ImpulseVariable Despawn { get; }

}
