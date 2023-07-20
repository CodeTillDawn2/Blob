using System;
using UnityEngine;

public static class PhysicsTools
{
    /// <summary>
    /// Used multiplied by cube width to determine how close the cube can get to terrain before not being able to move forward. Mostly for turning while against  wall.
    /// </summary>
    public static readonly float ForwardMovement_StopDistance = .1f;
    public static Vector3 GetPlayerSideDirectionNormalizer(BoxCollider side)
    {
        if (side == PlayerController.me.collider_TopSide)
        {
            return side.transform.up;
        }
        else if (side == PlayerController.me.collider_BottomSide)
        {
            return -side.transform.up;
        }
        else if (side == PlayerController.me.collider_FrontSide)
        {
            return side.transform.forward;
        }
        else if (side == PlayerController.me.collider_BackSide)
        {
            return -side.transform.forward;
        }
        else if (side == PlayerController.me.collider_RightSide)
        {
            return side.transform.right;
        }
        else if (side == PlayerController.me.collider_LeftSide)
        {
            return -side.transform.right;
        }
        else
        {
            throw new NotImplementedException();
        }
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

    public static RaycastHit? RaycastAt(Vector3 startingPosition, Vector3 endingPosition, float distance = 0, int layerMask = ~0) 
    {
        Ray ray = new Ray(startingPosition, (endingPosition - startingPosition));
        if (Physics.Raycast(ray, out RaycastHit hit1, distance, layerMask, QueryTriggerInteraction.Ignore))
        {
            return hit1;
        }
        return null;
    }


    public static void NormalizedDeclip(ActorController colidee, Collider colideeCollider, Vector3 positionA, Quaternion rotationA,
        Collider colliderB, Vector3 positionB, Quaternion rotationB, Vector3 Normalizer)
    {
        if (Physics.ComputePenetration(colideeCollider, positionA, rotationA, colliderB, positionB, rotationB, out Vector3 angle, out float distance))
        {


            Vector3 normalizedAngle = new Vector3(angle.x * Normalizer.x, angle.y * Normalizer.y, angle.z * Normalizer.z);


            if (!Physics.BoxCast(colidee.transform.position, colideeCollider.bounds.extents / 2f, angle, colidee.transform.rotation, distance))
            {
                colidee.transform.position += angle * distance * Time.deltaTime;
                Physics.SyncTransforms();
            }

        }
    }

    public static void Declip()
    {

    }
}
