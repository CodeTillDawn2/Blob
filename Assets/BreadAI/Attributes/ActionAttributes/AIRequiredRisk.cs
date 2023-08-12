using System;

/// <summary>
/// Use this to decorate character system public methods with fields which 
/// are influenced in a negative way by this action occuring and are 
/// necessary for the action to occur.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIRequiredRiskAttribute : CustomAIAttributeBase
{
    public Type RiskType { get; }

    public AIRequiredRiskAttribute(Type riskType)
    {
        RiskType = riskType;
    }
}
