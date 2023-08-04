using UnityEngine;

public static class Shortcuts
{
    #region Get Midpoint Of Object 
    /// <summary>
    /// Returns the midpoint of a GameObject based on its collider or renderer bounds.
    /// If the GameObject has children, the midpoint is the average position of all child objects.
    /// </summary>
    /// <param name="target">The GameObject for which to find the midpoint.</param>
    /// <returns>The calculated midpoint position.</returns>

    public static Vector3 GetMidPointOfObject(GameObject target)
    {
        if (target.GetComponent<Collider>() != null)
        {
            return target.GetComponent<Collider>().bounds.center;
        }
        else if (target.GetComponent<Renderer>() != null)
        {
            return target.GetComponent<Renderer>().bounds.center;
        }

        Vector3 sumVector = Vector3.zero;
        int totalCount = CountTotalChildren(target.transform);

        if (totalCount > 0)
        {
            AddChildPositions(target.transform, ref sumVector);
            return sumVector / totalCount;
        }

        return target.transform.position;
    }
    /// <summary>
    /// Recursively adds the positions of all child Transforms to the given sumVector.
    /// </summary>
    /// <param name="target">The parent Transform whose children positions are being added.</param>
    /// <param name="sumVector">The accumulated sum of child positions.</param>

    private static void AddChildPositions(Transform target, ref Vector3 sumVector)
    {
        foreach (Transform child in target)
        {
            sumVector += child.position;
            AddChildPositions(child, ref sumVector);
        }
    }
    /// <summary>
    /// Recursively counts the total number of child Transforms under the given parent Transform.
    /// </summary>
    /// <param name="target">The parent Transform whose children are being counted.</param>
    /// <returns>The total number of child Transforms.</returns>

    private static int CountTotalChildren(Transform target)
    {
        int count = 0;
        foreach (Transform child in target)
        {
            count++;
            count += CountTotalChildren(child);
        }
        return count;
    }

    #endregion

    /// <summary>
    /// Checks if a layer is present in the given LayerMask.
    /// </summary>
    /// <param name="layer">The layer to check.</param>
    /// <param name="layermask">The LayerMask to test against.</param>
    /// <returns>True if the layer is present in the LayerMask; otherwise, false.</returns>
    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return ((layermask.value & (1 << layer)) != 0);
    }

    /// <summary>
    /// Generates a random float within the specified range, excluding the specified gap range.
    /// </summary>
    /// <param name="start">The start of the overall range.</param>
    /// <param name="end">The end of the overall range.</param>
    /// <param name="gapStart">The start of the gap range to exclude.</param>
    /// <param name="gapEnd">The end of the gap range to exclude.</param>
    /// <returns>A random float value within the specified range, excluding the specified gap.</returns>
    public static float GappedRandom(float start, float end, float gapStart, float gapEnd)
    {
        // Clamping the gap to be within the range.
        gapStart = Mathf.Max(start, gapStart);
        gapEnd = Mathf.Min(end, gapEnd);

        // Ensure the gap is valid.
        if (gapEnd < gapStart)
        {
            throw new System.ArgumentException("gapEnd must be greater than gapStart");
        }

        // Calculate the valid range excluding the gap.
        float validRange = (end - start) - (gapEnd - gapStart);

        // Generate a random number within the valid range.
        float randomResult = Random.Range(start, start + validRange);

        // Shift the result if it falls within the gap.
        if (randomResult >= gapStart)
        {
            randomResult += (gapEnd - gapStart);
        }

        return randomResult;
    }


    public enum UnityLayers
    {
        @Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Ground = 3,
        Water = 4,
        UI = 5,
        Player = 6,
        CanBeEaten = 7,
        BeingEaten = 8,
        PlayerTentacle = 9,
        Inedible = 10
    }

    public enum LayerMasks
    {
        LayerMask_GroundOnly = 1 << UnityLayers.Ground,
        LayerMask_PlayerOnly = 1 << UnityLayers.Player,
        LayerMask_PlayerAndTentacles = 1 << UnityLayers.Player | 1 << UnityLayers.PlayerTentacle,
        LayerMask_EdibleOnly = 1 << UnityLayers.CanBeEaten,
        LayerMask_NotPlayer = ~(1 << UnityLayers.Player),
        LayerMask_NotPlayerOrTentacles = ~((1 << UnityLayers.Player) | (1 << UnityLayers.PlayerTentacle)),
        LayerMask_NotGround = ~(1 << UnityLayers.Ground),
        LayerMask_NotPlayerOrTentaclesOrGround = ~((1 << UnityLayers.Player) | (1 << UnityLayers.PlayerTentacle) | (1 << UnityLayers.Ground))
    }
}


