using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Impulse
{
    [SerializeField] public List<ImpulseStep> ImpulseSteps = new List<ImpulseStep>();
    private float StartStepTime;

    public IEnumerator Go()
    {
        for (int i = 0; i < ImpulseSteps.Count; i++)
        {
            ImpulseStep step = ImpulseSteps[i];
            StartStepTime = Time.timeSinceLevelLoad;
            while (Time.timeSinceLevelLoad - StartStepTime <= step.ExecutionTime)
            {

                List<ImpulseStep> insertSteps = step.StepAction((Time.timeSinceLevelLoad - StartStepTime) / step.ExecutionTime);
                if (insertSteps !=  null)
                {
                    foreach(ImpulseStep extraStep in insertSteps)
                    {
                        ImpulseSteps.Insert(i + 1, extraStep);
                    }
                    continue;
                }
                yield return new WaitForFixedUpdate();
            }

            yield return null;
        }

    }
}
