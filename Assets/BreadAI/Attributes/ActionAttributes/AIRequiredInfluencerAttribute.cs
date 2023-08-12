using System;


/// <summary>
/// Use this to decorate character system public methods with fields which may 
/// have some influence on the outcome of the action and are required for 
/// the action to occur.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIRequiredInfluencerAttribute : CustomAIAttributeBase
{
    public Type InfluencerType { get; }

    public AIRequiredInfluencerAttribute(Type influencerType)
    {
        InfluencerType = influencerType;
    }
}
