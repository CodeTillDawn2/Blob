using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IImpulse
{
    void SetImpulse(ImpulseVariable impulseToChange, params WrappedFunc[] funcs);
}
