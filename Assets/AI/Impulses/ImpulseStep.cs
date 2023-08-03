using System;
using System.Collections.Generic;

public class ImpulseStep
{
    public string impulseStepNameDebug = "";
    public Func<bool> OnlyDoIf;
    public Func<bool> WaitUntilTrue;
    public Func<float, List<ImpulseStep>> StepAction;
    public float ExecutionTime;

    /// <summary>
    /// An impulse step is a single action which may be part of a larger chain of actions (an impulse)
    /// </summary>
    /// <param name="StepAction"></param>
    /// <param name="OnlyDoIf"></param>
    /// <param name="WaitUntilTrue"></param>
    public ImpulseStep(Func<float, List<ImpulseStep>> StepAction, Func<bool> OnlyDoIf = null, Func<bool> WaitUntilTrue = null)
    {
        this.StepAction = StepAction;
        this.WaitUntilTrue = WaitUntilTrue;
        this.OnlyDoIf = OnlyDoIf;
    }
    /// <summary>
    /// An impulse step is a single action which may be part of a larger chain of actions (an impulse)
    /// </summary>
    /// <param name="StepAction"></param>
    /// <param name="OnlyDoIf"></param>
    /// <param name="ExecutionTime"></param>
    public ImpulseStep(Func<float, List<ImpulseStep>> StepAction, Func<bool> OnlyDoIf = null, float ExecutionTime = 0)
    {
        this.StepAction = StepAction;
        this.ExecutionTime = ExecutionTime;
        this.OnlyDoIf = OnlyDoIf;
    }

}
