using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impulse
{
    [SerializeField] public List<ImpulseStep> ImpulseSteps = new List<ImpulseStep>();
    private float StartStepTime;
    public ImpulseType impulseType;

    public enum ImpulseType
    {
        Spawn = 1,
        Despawn = 2,
        Attack = 3,
        Move = 4,
        Search = 5
    }

    private bool WaitUntilTimeElapsed(float StartStepTime, float WaitUntilTime)
    {
        if (WaitUntilTime == 0)
        {
            return true;
        }
        return Time.timeSinceLevelLoad - StartStepTime <= WaitUntilTime;
    }


    public IEnumerator Go()
    {

        for (int i = 0; i < ImpulseSteps.Count; i++)
        {


            ImpulseStep step = ImpulseSteps[i];

            StartStepTime = Time.timeSinceLevelLoad;
            Debug.Log("Starting impulse step: " + step.impulseStepNameDebug);
            bool FirstRun = true;


            while (FirstRun || !WaitUntilTimeElapsed(StartStepTime, step.ExecutionTime)
                || (step.WaitUntilTrue != null && !step.WaitUntilTrue()))
            {


                Debug.Log("Impulse step while loop");



                SmoothTentacle smoothTentacle = GameObject.FindObjectOfType<SmoothTentacle>();


                if (step.impulseStepNameDebug == "ShrinkStep")
                {
                    string test = "";
                }

                FirstRun = false;
                float PercentToDo;
                if (step.ExecutionTime == 0)
                {
                    PercentToDo = 1.00f;
                }
                else
                {
                    PercentToDo = (Time.timeSinceLevelLoad - StartStepTime) / step.ExecutionTime;
                }

                Debug.Log("Percent to do: " + PercentToDo);

                if (step.impulseStepNameDebug == "ShrinkStep")
                {
                    string test = "";
                }
                List<ImpulseStep> insertSteps = step.StepAction(PercentToDo); //Run action
                Debug.Log("Action complete");
                //bool IsShrinking = smoothTentacle.IsShrinking;
                if (insertSteps != null)
                {
                    foreach (ImpulseStep extraStep in insertSteps)
                    {
                        Debug.Log("Inserting extra step: " + extraStep.impulseStepNameDebug);
                        ImpulseSteps.Insert(i + 1, extraStep);
                    }
                    continue;
                }

                try
                {
                    bool WaitUntilElapsed2 = WaitUntilTimeElapsed(StartStepTime, step.ExecutionTime);
                    bool WaitUntilTrue2 = step.WaitUntilTrue();

                    Debug.Log("Wait until elapsed: " + WaitUntilElapsed2);
                    Debug.Log("Wait until true: " + WaitUntilTrue2);
                }
                catch
                {

                }

                if (step.impulseStepNameDebug == "ShrinkStep")
                {
                    string test = "";
                }
                if (step.ExecutionTime == 0 && (step.WaitUntilTrue == null || step.WaitUntilTrue()))
                {
                    Debug.Log("Special Break");
                    break;
                }


                yield return new WaitForFixedUpdate();
            }

            Debug.Log("Ending impulse step: " + step.impulseStepNameDebug);
            yield return null;
        }

    }
}
