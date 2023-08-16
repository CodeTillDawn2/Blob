using System;

/// <summary>
/// Use this to decorate character system public methods with fields which 
/// are influenced in a negative way by this action occuring but are not 
/// necessary for the action to occur.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIOptionalRiskAttribute : BreadAIAttributeBase
{
    public Type RiskType { get; }

    public AIOptionalRiskAttribute(Type riskType)
    {
        RiskType = riskType;
    }
}
