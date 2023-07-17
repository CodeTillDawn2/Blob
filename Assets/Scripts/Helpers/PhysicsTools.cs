using System;
using UnityEngine;

public static class PhysicsTools
{

    public static Vector3 GetPlayerSideDirectionNormalizer(BoxCollider side)
    {
        if (side == PlayerController.Player.topSideCollider)
        {
            return side.transform.up;
        }
        else if (side == PlayerController.Player.bottomSideCollider)
        {
            return -side.transform.up;
        }
        else if (side == PlayerController.Player.frontSideCollider)
        {
            return side.transform.forward;
        }
        else if (side == PlayerController.Player.backSideCollider)
        {
            return -side.transform.forward;
        }
        else if (side == PlayerController.Player.rightSideCollider)
        {
            return side.transform.right;
        }
        else if (side == PlayerController.Player.leftSideCollider)
        {
            return -side.transform.right;
        }
        else
        {
            throw new NotImplementedException();
        }
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
