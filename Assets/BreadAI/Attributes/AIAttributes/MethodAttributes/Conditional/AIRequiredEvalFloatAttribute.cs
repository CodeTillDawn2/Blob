using System;

/// <summary>
/// Use this to decorate character system public methods with fields which 
/// are required to evaluate a float.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIRequiredEvalFloatAttribute : BreadAIAttributeBase
{
    public Type FieldToEvaluate { get; }

    public OperatorEnum Evaluator { get; }
    public float EvalFloat { get; }

    public AIRequiredEvalFloatAttribute(Type fieldToEvaluate, OperatorEnum evaluator, float evalFloat)
    {
        FieldToEvaluate = fieldToEvaluate;
        Evaluator = evaluator;
        EvalFloat = evalFloat;
    }


    public enum OperatorEnum
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan
    }

}
