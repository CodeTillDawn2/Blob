using System;

/// <summary>
/// Use this to decorate interface methods to indicate that this interface is only usable by self
/// </summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
public class SelfAttribute : BreadAIAttributeBase
{

}
