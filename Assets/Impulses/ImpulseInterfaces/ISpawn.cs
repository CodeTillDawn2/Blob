using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawn : IImpulse
{
    ImpulseVariable Spawn { get; }

}
