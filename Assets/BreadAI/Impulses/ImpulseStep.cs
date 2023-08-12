using System;
using System.Collections.Generic;

public class ImpulseStep
{
    public string impulseStepNameDebug = "";
    public Func<bool> OnlyDoIf;
    public Func<List<ImpulseStep>> StepAction;

    /// <summary>
    /// An impulse step is a single action which may be part of a larger chain of actions (an impulse)
    /// </summary>
    /// <param name="StepAction"></param>
    /// <param name="OnlyDoIf"></param>
    public ImpulseStep(Func<List<ImpulseStep>> StepAction, Func<bool> OnlyDoIf = null)
    {
        this.StepAction = StepAction;
        this.OnlyDoIf = OnlyDoIf;
    }
}
