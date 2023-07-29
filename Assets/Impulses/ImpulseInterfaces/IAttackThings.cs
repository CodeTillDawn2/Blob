using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackThings : IImpulse
{
    ImpulseVariable AttackThings { get; }

}
