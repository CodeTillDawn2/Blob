using UnityEngine;

public static class Shortcuts
{

    public static float GappedRandom(float start, float end, float gapStart, float gapEnd)
    {

        float gapAmount = gapEnd - gapStart;
        float randomResult = Random.Range(start, end - gapAmount);

        if (randomResult >= gapStart)
        {
            randomResult += gapAmount;
        }

        return randomResult;
    }

    public static string GetPath(this Transform current)
    {
        if (current == null) return "";
        if (current.parent == null)
            return "/" + current.name;
        return current.parent.GetPath() + "/" + current.name;
    }

    public static bool PathMatches(this GameObject obj1, GameObject obj2)
    {
        string path1 = GetPath(obj1.transform);
        string path2 = GetPath(obj2.transform);

        if (path1 == path2)
        {
            return true;
        }
        return false;
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
        LayerMask_PlayerAndTentacles = 1 << UnityLayers.Player | 1 << UnityLayers.Player,
        LayerMask_EdibleOnly = 1 << UnityLayers.CanBeEaten,
        LayerMask_NotPlayer = ~(1 << UnityLayers.Player),
        LayerMask_NotPlayerOrTentacles = ~((1 << UnityLayers.Player) | (1 << UnityLayers.PlayerTentacle)),
        LayerMask_NotGround = ~(1 << UnityLayers.Ground),
        LayerMask_NotPlayerOrTentaclesOrGround = ~((1 << UnityLayers.Player) | (1 << UnityLayers.PlayerTentacle) | (1 << UnityLayers.Ground))
    }
}


