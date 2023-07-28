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


    /// <summary> Returns a vector which is relative to the facing and position of 'c'. </summary>
    public static Vector3 RelativePosition(Transform c, Vector3 target)
    {
        return c.right * target.x + c.up * target.y + c.forward * target.z + c.position;
    }

    /// <summary> Returns a vector which is relative. </summary>
    public static Vector3 RelativePosition(Vector3 position, Vector3 forward, Vector3 up, Vector3 right, Vector3 target)
    {
        return right * target.x + up * target.y + forward * target.z + position;
    }

    /// <summary> Returns a vector which is relative to the facing of 'c'. </summary>
    public static Vector3 RelativePositionZero(Transform c, Vector3 target)
    {
        return c.right * target.x + c.up * target.y + c.forward * target.z;
    }

    /// <summary> Returns a vector which is relative. </summary>
    public static Vector3 RelativePositionZero(Vector3 forward, Vector3 up, Vector3 right, Vector3 target)
    {
        return right * target.x + up * target.y + forward * target.z;
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

    public static void DrawBounds(Bounds b, Color color, float delay = 0)
    {
        // bottom
        var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
        var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
        var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
        var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

        Debug.DrawLine(p1, p2, color, delay);
        Debug.DrawLine(p2, p3, color, delay);
        Debug.DrawLine(p3, p4, color, delay);
        Debug.DrawLine(p4, p1, color, delay);

        // top
        var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
        var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
        var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
        var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

        Debug.DrawLine(p5, p6, color, delay);
        Debug.DrawLine(p6, p7, color, delay);
        Debug.DrawLine(p7, p8, color, delay);
        Debug.DrawLine(p8, p5, color, delay);

        // sides
        Debug.DrawLine(p1, p5, color, delay);
        Debug.DrawLine(p2, p6, color, delay);
        Debug.DrawLine(p3, p7, color, delay);
        Debug.DrawLine(p4, p8, color, delay);
    }

    public class OBB
    {

        readonly public Vector3 pos;
        readonly public Vector3 forward, right, up;
        readonly public Vector3 size;
        readonly public float len;

        public OBB(Transform ts, Vector3 sizeValue)
        {
            pos = ts.position;
            forward = ts.forward; right = ts.right; up = ts.up;
            size = sizeValue;
            len = sizeValue.magnitude;
        }

        public OBB(Vector3 pos, Vector3 forward, Vector3 right, Vector3 up, Vector3 sizeValue)
        {
            this.pos = pos;
            this.forward = forward; this.right = right; this.up = up;
            size = sizeValue;
            len = sizeValue.magnitude;
        }
    }

    public static bool TestIntersection(OBB obb1, OBB obb2)
    {
        Vector3[] SeparateAxis = new Vector3[15] {
            obb1.forward, obb1.right, obb1.up,
            obb2.forward, obb2.right, obb2.up,
            Vector3.Cross(obb1.forward, obb2.forward).normalized,
            Vector3.Cross(obb1.forward, obb2.right).normalized,
            Vector3.Cross(obb1.forward, obb2.up).normalized,
            Vector3.Cross(obb1.right, obb2.forward).normalized,
            Vector3.Cross(obb1.right, obb2.right).normalized,
            Vector3.Cross(obb1.right, obb2.up).normalized,
            Vector3.Cross(obb1.up, obb2.forward).normalized,
            Vector3.Cross(obb1.up, obb2.right).normalized,
            Vector3.Cross(obb1.up, obb2.up).normalized
        };

        Vector3 dist1to2 = obb1.pos - obb2.pos;
        Vector3 obb1half1 = 0.5f * (obb1.right * obb1.size.x + obb1.up * obb1.size.y + obb1.forward * obb1.size.z);
        Vector3 obb1half2 = 0.5f * (obb1.right * obb1.size.x + obb1.up * obb1.size.y - obb1.forward * obb1.size.z);
        Vector3 obb2half1 = 0.5f * (obb2.right * obb2.size.x + obb2.up * obb2.size.y + obb2.forward * obb2.size.z);
        Vector3 obb2half2 = 0.5f * (obb2.right * obb2.size.x + obb2.up * obb2.size.y - obb2.forward * obb2.size.z);

        float R;
        float[] R1 = new float[2];
        float[] R2 = new float[2];
        for (uint n = 0; n < 15; ++n)
        {
            if (SeparateAxis[n] == Vector3.zero) continue;
            R = Vector3.Dot(dist1to2, SeparateAxis[n]);
            R1[0] = Vector3.Dot(obb1half1, SeparateAxis[n]);
            R1[1] = Vector3.Dot(obb1half2, SeparateAxis[n]);
            R2[0] = Vector3.Dot(obb2half1, SeparateAxis[n]);
            R2[1] = Vector3.Dot(obb2half2, SeparateAxis[n]);

            if (Mathf.Abs(R) > Mathf.Abs(R1[0]) + Mathf.Abs(R2[0]) || Mathf.Abs(R) > Mathf.Abs(R1[1]) + Mathf.Abs(R2[1]))
            {
                return false;
            }
        }
        return true;
    }

    //Draws just the box at where it is currently hitting.
    public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float hitInfoDistance, Color color)
    {
        origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
        DrawBox(origin, halfExtents, orientation, color);
    }

    //Draws the full box from start of cast to its end distance. Can also pass in hitInfoDistance instead of full distance
    public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color)
    {
        direction.Normalize();
        Box bottomBox = new Box(origin, halfExtents, orientation);
        Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

        Debug.DrawLine(bottomBox.backBottomLeft, topBox.backBottomLeft, color);
        Debug.DrawLine(bottomBox.backBottomRight, topBox.backBottomRight, color);
        Debug.DrawLine(bottomBox.backTopLeft, topBox.backTopLeft, color);
        Debug.DrawLine(bottomBox.backTopRight, topBox.backTopRight, color);
        Debug.DrawLine(bottomBox.frontTopLeft, topBox.frontTopLeft, color);
        Debug.DrawLine(bottomBox.frontTopRight, topBox.frontTopRight, color);
        Debug.DrawLine(bottomBox.frontBottomLeft, topBox.frontBottomLeft, color);
        Debug.DrawLine(bottomBox.frontBottomRight, topBox.frontBottomRight, color);

        DrawBox(bottomBox, color);
        DrawBox(topBox, color);
    }

    public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
    {
        DrawBox(new Box(origin, halfExtents, orientation), color);
    }
    public static void DrawBox(Box box, Color color)
    {
        Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color);
        Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color);
        Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color);
        Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color);

        Debug.DrawLine(box.backTopLeft, box.backTopRight, color);
        Debug.DrawLine(box.backTopRight, box.backBottomRight, color);
        Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color);
        Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color);

        Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color);
        Debug.DrawLine(box.frontTopRight, box.backTopRight, color);
        Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color);
        Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color);
    }

    public struct Box
    {
        public Vector3 localFrontTopLeft { get; private set; }
        public Vector3 localFrontTopRight { get; private set; }
        public Vector3 localFrontBottomLeft { get; private set; }
        public Vector3 localFrontBottomRight { get; private set; }
        public Vector3 localBackTopLeft { get { return -localFrontBottomRight; } }
        public Vector3 localBackTopRight { get { return -localFrontBottomLeft; } }
        public Vector3 localBackBottomLeft { get { return -localFrontTopRight; } }
        public Vector3 localBackBottomRight { get { return -localFrontTopLeft; } }

        public Vector3 frontTopLeft { get { return localFrontTopLeft + origin; } }
        public Vector3 frontTopRight { get { return localFrontTopRight + origin; } }
        public Vector3 frontBottomLeft { get { return localFrontBottomLeft + origin; } }
        public Vector3 frontBottomRight { get { return localFrontBottomRight + origin; } }
        public Vector3 backTopLeft { get { return localBackTopLeft + origin; } }
        public Vector3 backTopRight { get { return localBackTopRight + origin; } }
        public Vector3 backBottomLeft { get { return localBackBottomLeft + origin; } }
        public Vector3 backBottomRight { get { return localBackBottomRight + origin; } }

        public Vector3 origin { get; private set; }

        public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
        {
            Rotate(orientation);
        }
        public Box(Vector3 origin, Vector3 halfExtents)
        {
            this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
            this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

            this.origin = origin;
        }


        public void Rotate(Quaternion orientation)
        {
            localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
            localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
            localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
            localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
        }
    }

    //This should work for all cast types
    static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance)
    {
        return origin + (direction.normalized * hitInfoDistance);
    }

    static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot;
        return pivot + rotation * direction;
    }





}
