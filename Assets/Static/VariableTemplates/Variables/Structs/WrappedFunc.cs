using System;
using System.Collections.Generic;

public class WrappedFunc
{
    public Func<float, List<ImpulseStep>> func;
    public float time;
    public bool waitUntilFinished;

    public WrappedFunc(Func<float, List<ImpulseStep>> func, float time, bool waitUntilFinished)
    {
        this.func = func;
        this.time = time;
        this.waitUntilFinished = waitUntilFinished;
    }
}
