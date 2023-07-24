using System.Collections.Generic;
using UnityEngine;

public static class PhysicsTools
{
    /// <summary>
    /// Used multiplied by cube width to determine how close the cube can get to terrain before not being able to move forward. Mostly for turning while against  wall.
    /// </summary>
    public static readonly float ForwardMovement_StopDistance = .1f;

    public static RaycastHit[] ConeCastAll(this Physics physics, Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle)
    {
        RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin - new Vector3(0, 0, maxRadius), maxRadius, direction, maxDistance);
        List<RaycastHit> coneCastHitList = new List<RaycastHit>();

        if (sphereCastHits.Length > 0)
        {
            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                sphereCastHits[i].collider.gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
                Vector3 hitPoint = sphereCastHits[i].point;
                Vector3 directionToHit = hitPoint - origin;
                float angleToHit = Vector3.Angle(direction, directionToHit);

                if (angleToHit < coneAngle)
                {
                    coneCastHitList.Add(sphereCastHits[i]);
                }
            }
        }

        RaycastHit[] coneCastHits = new RaycastHit[coneCastHitList.Count];
        coneCastHits = coneCastHitList.ToArray();

        return coneCastHits;
    }

    public static float GetSqDistanceBetweenPoints(Vector3 postion1, Vector3 position2)
    {
        return (postion1 - position2).sqrMagnitude;
    }
    public static Vector3 PointAlongDirection(Vector3 origin, Vector3 direction,
     float distance)
    {
        return origin + direction.normalized * distance;
    }

    public static Vector3 MultipledVector(Vector3 vector, float multiplier)
    {
        return new Vector3(vector.x * multiplier, vector.y * multiplier, vector.z * multiplier);
    }

    public static float ReturnColliderOverlapAmount(Collider col1, Collider col2)
    {
        if (Physics.ComputePenetration(col1, col1.gameObject.transform.position, col1.gameObject.transform.rotation,
                                    col2, col2.gameObject.transform.position, col2.gameObject.transform.rotation,
                                    out Vector3 dir, out float distance))
        {

            return distance;
        }
        return -1;
    }

    public static RaycastHit? RaycastAt(Vector3 startingPosition, Vector3 endingPosition, float distance = Mathf.Infinity, int layerMask = ~0)
    {
        Ray ray = new Ray(startingPosition, (endingPosition - startingPosition));

        if (Physics.Raycast(ray, out RaycastHit hit1, distance, layerMask))
        {
            return hit1;
        }
        return null;
    }


}
