using System;

/// <summary>
/// Use this to decorate character system public methods with fields which 
/// are influenced in a positive way by this action occuring and are
/// necessary for the action to occur.
/// </summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
public class BreadAIInterfaceAttribute : CustomAIAttributeBase
{

}
