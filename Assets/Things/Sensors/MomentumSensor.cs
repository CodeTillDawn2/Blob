using System.Linq;
using UnityEngine;

public class MomentumSensor : MonoBehaviour
{
    private Vector3[] positions = new Vector3[4];
    private int index = 0;

    // FixedUpdate is called once per physics step
    void FixedUpdate()
    {
        // Update positions
        index = index % 4; // Make sure the index is within array bounds
        positions[index] = transform.position; // Record current position

        index++; // Increment for next frame
    }

    public Vector3 ReturnVector()
    {
        int oldestIndex = index % 4; // Index of the oldest recorded frame
        int newestIndex = (index + 3) % 4; // Index of the most recent frame
        return (positions[newestIndex] - positions[oldestIndex]).normalized; // Direction vector from oldest to newest position
    }

    public float ReturnSpeed()
    {
        int oldestIndex = index % 4;
        int newestIndex = (index + 3) % 4;
        return Mathf.Sqrt((positions[newestIndex] - positions[oldestIndex]).sqrMagnitude) / (3 * Time.fixedDeltaTime); // Speed over three frames
    }

    public float ReturnSpeedSq()
    {
        return (positions[(index + 3) % 4] - positions[index % 4]).sqrMagnitude / Mathf.Pow(3 * Time.fixedDeltaTime, 2);
    }

    public float ReturnAverageAcceleration()
    {
        // Only one frame of data available
        if (positions.Count() == 1)
        {
            // Return the instantaneous velocity change (which will be zero in this case)
            return 0f;
        }
        // Two frames of data available
        else if (positions.Count() == 2)
        {
            // Calculate and return the instantaneous acceleration
            Vector3 acceleration = (positions[1] - positions[0]) / (Time.fixedDeltaTime * Time.fixedDeltaTime);
            return acceleration.magnitude;
        }
        // At least three frames of data available
        else
        {
            // Calculate and return average acceleration over the last three frames
            Vector3 averageAcceleration = (positions[2] - 2 * positions[1] + positions[0]) / (Time.fixedDeltaTime * Time.fixedDeltaTime);
            return averageAcceleration.magnitude;
        }
    }

    public float ReturnAverageAccelerationSq()
    {
        // Only one frame of data available
        if (positions.Count() == 1)
        {
            // Return the instantaneous velocity change (which will be zero in this case)
            return 0f;
        }
        // Two frames of data available
        else if (positions.Count() == 2)
        {
            // Calculate and return the instantaneous acceleration
            Vector3 acceleration = (positions[1] - positions[0]) / (Time.fixedDeltaTime * Time.fixedDeltaTime);
            return acceleration.sqrMagnitude;
        }
        // At least three frames of data available
        else
        {
            // Calculate and return average acceleration over the last three frames
            Vector3 averageAcceleration = (positions[2] - 2 * positions[1] + positions[0]) / (Time.fixedDeltaTime * Time.fixedDeltaTime);
            return averageAcceleration.sqrMagnitude;
        }
    }
}