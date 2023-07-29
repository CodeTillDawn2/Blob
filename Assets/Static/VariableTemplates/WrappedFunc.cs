using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrappedFunc 
{
    public Func<float, List<ImpulseStep>> func;
    public float time;

    public WrappedFunc(Func<float, List<ImpulseStep>> func, float time)
    {
        this.func = func;
        this.time = time;
    }
}
