using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impulse
{
    [SerializeField] public List<ImpulseStep> ImpulseSteps = new List<ImpulseStep>();
    public ImpulseType impulseType;
    private const int MAX_STEPS = 1000; // A precautionary limit to prevent infinite loops

    public enum ImpulseType
    {
        Spawn = 1,
        Despawn = 2,
        Attack = 3,
        Move = 4,
        Search = 5
    }

    public IEnumerator Go()
    {
        int stepCounter = 0;

        for (int i = 0; i < ImpulseSteps.Count; i++)
        {
            // Exit if we exceed the max number of allowed steps
            if (stepCounter++ > MAX_STEPS)
            {
                Debug.LogWarning("Terminating Impulse due to potential infinite loop.");
                yield break;
            }

            ImpulseStep step = ImpulseSteps[i];
            List<ImpulseStep> insertSteps = step.StepAction(); //Run action

            if (insertSteps != null)
            {
                foreach (ImpulseStep extraStep in insertSteps)
                {
                    ImpulseSteps.Insert(i + 1, extraStep);
                }
            }

            yield return null; // Yield for one frame before the next iteration
        }
    }
}
