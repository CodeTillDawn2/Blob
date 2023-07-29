using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseStep
{
    public Func<float, List<ImpulseStep>> StepAction; //Should return true when finished
    public float ExecutionTime; //Should return true when finished

    public ImpulseStep(Func<float,List<ImpulseStep>> StepAction, float ExecutionTime)
    {
        this.StepAction = StepAction;
        this.ExecutionTime = ExecutionTime;
    }
}
