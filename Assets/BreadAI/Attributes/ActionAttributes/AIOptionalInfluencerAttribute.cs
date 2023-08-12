using System;

/// <summary>
/// Use this to decorate character system public methods with fields which may 
/// have some influence on the outcome of the action but are not required for 
/// the action to occur.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIOptionalInfluencerAttribute : CustomAIAttributeBase
{
    public Type InfluencerType { get; }

    public AIOptionalInfluencerAttribute(Type influencerType)
    {
        InfluencerType = influencerType;
    }
}
