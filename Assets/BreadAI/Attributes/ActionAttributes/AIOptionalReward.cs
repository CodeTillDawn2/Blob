using System;

/// <summary>
/// Use this to decorate character system public methods with fields which 
/// are influenced in a positive way by this action occuring but are not 
/// necessary for the action to occur.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIOptionalRewardAttribute : CustomAIAttributeBase
{
    public Type RewardType { get; }

    public AIOptionalRewardAttribute(Type rewardType)
    {
        RewardType = rewardType;
    }
}
