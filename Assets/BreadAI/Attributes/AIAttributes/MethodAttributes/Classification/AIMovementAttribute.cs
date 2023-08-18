using System;

/// <summary>
/// Use this to decorate character system public methods which cannot be selected by the AI, but the AI
/// should still be aware of
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIMovementAttribute : BreadAIAttributeBase
{
}
