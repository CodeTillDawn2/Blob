using System.Collections.Generic;
using UnityEngine;

public static class PhysicsTools
{
    /// <summary>
    /// Used multiplied by cube width to determine how close the cube can get to terrain before not being able to move forward. Mostly for turning while against  wall.
    /// </summary>
    public static readonly float ForwardMovement_StopDistance = .1f;

    /// <summary>
    /// Performs a cone-shaped raycast from 'origin' with 'maxRadius' and 'maxDistance',
    /// constrained by a 'coneAngle'. Returns an array of RaycastHit containing hits within the cone.
    /// </summary>
    /// <param name="origin">Starting point of the cone cast.</param>
    /// <param name="maxRadius">Maximum radius of the cone at its starting point.</param>
    /// <param name="direction">Normalized direction of the cone.</param>
    /// <param name="maxDistance">Maximum distance the cone will travel.</param>
    /// <param name="coneAngle">Angle (in degrees) constraining the cone's aperture.</param>
    /// <returns>An array of RaycastHit with hits within the cone's area.</returns>
    public static RaycastHit[] ConeCastAll(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle)
    {
        direction = direction.normalized;
        RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin - direction * maxRadius, maxRadius, direction, maxDistance);
        List<RaycastHit> coneCastHitList = new List<RaycastHit>();

        foreach (var hit in sphereCastHits)
        {
            Vector3 directionToHit = (hit.point - origin).normalized;
            float angleToHit = Vector3.Angle(direction, directionToHit);

            if (angleToHit < coneAngle)
            {
                coneCastHitList.Add(hit);
            }
        }

        return coneCastHitList.ToArray();
    }



    /// <summary>
    /// Converts a point from a custom local coordinate system to world coordinates.
    /// </summary>
    /// <param name="position">The origin of the custom local system.</param>
    /// <param name="forward">The forward direction vector of the custom local system.</param>
    /// <param name="up">The up direction vector of the custom local system.</param>
    /// <param name="right">The right direction vector of the custom local system.</param>
    /// <param name="target">The target point in local coordinates to convert.</param>
    /// <returns>The world position of the 'target' point.</returns>
    public static Vector3 RelativePosition(Transform c, Vector3 target)
    {
        return c.right * target.x + c.up * target.y + c.forward * target.z + c.position;
    }

    /// <summary>
    /// Converts a point in the local coordinate system of 'c' to world coordinates.
    /// </summary>
    /// <param name="c">The Transform representing the local coordinate system.</param>
    /// <param name="target">The point in local coordinates to convert.</param>
    /// <returns>The world position of the 'target' point.</returns>
    public static Vector3 RelativePosition(Vector3 position, Vector3 forward, Vector3 up, Vector3 right, Vector3 target)
    {
        return right * target.x + up * target.y + forward * target.z + position;
    }

    /// <summary>
    /// Converts a point in the local coordinate system of 'c' to relative coordinates with respect to 'c'.
    /// The method assumes that the origin of the local coordinate system is at 'c.position'.
    /// </summary>
    /// <param name="c">The Transform representing the local coordinate system.</param>
    /// <param name="target">The point in local coordinates to convert.</param>
    /// <returns>The relative position of the 'target' point with respect to 'c'.</returns>
    public static Vector3 RelativePositionZero(Transform c, Vector3 target)
    {
        return c.right * target.x + c.up * target.y + c.forward * target.z;
    }

    /// <summary>
    /// Converts a point in a custom local coordinate system to relative coordinates with respect to the origin (0,0,0).
    /// </summary>
    /// <param name="forward">The forward direction vector of the custom local system.</param>
    /// <param name="up">The up direction vector of the custom local system.</param>
    /// <param name="right">The right direction vector of the custom local system.</param>
    /// <param name="target">The point in local coordinates to convert.</param>
    /// <returns>The relative position of the 'target' point with respect to the origin.</returns>
    public static Vector3 RelativePositionZero(Vector3 forward, Vector3 up, Vector3 right, Vector3 target)
    {
        return right * target.x + up * target.y + forward * target.z;
    }

    /// <summary>
    /// Performs a raycast from 'startingPosition' to 'endingPosition', looking for collisions with objects.
    /// Returns the first RaycastHit that intersects with any object along the ray's path, or null if no intersection.
    /// </summary>
    /// <param name="startingPosition">The starting point of the raycast.</param>
    /// <param name="endingPosition">The ending point of the raycast.</param>
    /// <param name="distance">The maximum distance the raycast can travel. (Default: Mathf.Infinity)</param>
    /// <param name="layerMask">A bitmask specifying which layers should be tested for collisions. (Default: ~0, all layers)</param>
    /// <returns>The first RaycastHit hit by the raycast or null if no intersection.</returns>
    public static RaycastHit? RaycastAt(Vector3 startingPosition, Vector3 endingPosition, float distance = Mathf.Infinity, int layerMask = ~0)
    {
        Ray ray = new Ray(startingPosition, (endingPosition - startingPosition));

        if (Physics.Raycast(ray, out RaycastHit hit1, distance, layerMask))
        {
            return hit1;
        }
        return null;
    }

    /// <summary>
    /// Draws the wireframe of the given Bounds object using Debug.DrawLine for visual debugging.
    /// </summary>
    /// <param name="bounds">The Bounds object to draw the wireframe for.</param>
    /// <param name="color">The color of the wireframe.</param>
    /// <param name="delay">Optional delay for the Debug.DrawLine calls. (Default: 0)</param>
    public static void DrawBounds(Bounds bounds, Color color, float delay = 0)
    {
        if (bounds.size == Vector3.zero)
        {
            Debug.LogError("Bounds are not valid!");
            return;
        }

        Vector3[] points = {
        new Vector3(bounds.min.x, bounds.min.y, bounds.min.z),
        new Vector3(bounds.max.x, bounds.min.y, bounds.min.z),
        new Vector3(bounds.max.x, bounds.min.y, bounds.max.z),
        new Vector3(bounds.min.x, bounds.min.y, bounds.max.z),
        new Vector3(bounds.min.x, bounds.max.y, bounds.min.z),
        new Vector3(bounds.max.x, bounds.max.y, bounds.min.z),
        new Vector3(bounds.max.x, bounds.max.y, bounds.max.z),
        new Vector3(bounds.min.x, bounds.max.y, bounds.max.z)
    };

        // Draw bottom
        for (int i = 0; i < 4; i++)
            Debug.DrawLine(points[i], points[(i + 1) % 4], color, delay);

        // Draw top
        for (int i = 4; i < 8; i++)
            Debug.DrawLine(points[i], points[((i - 4) + 1) % 4 + 4], color, delay);

        // Draw sides
        for (int i = 0; i < 4; i++)
            Debug.DrawLine(points[i], points[i + 4], color, delay);
    }


    /// <summary>
    /// Draws a box at the collision point of a BoxCast, given the origin, halfExtents, orientation, direction, and hitInfoDistance.
    /// </summary>
    /// <param name="origin">The starting point of the BoxCast.</param>
    /// <param name="halfExtents">The half size of the box being cast.</param>
    /// <param name="orientation">The orientation (rotation) of the box being cast.</param>
    /// <param name="direction">The direction of the BoxCast.</param>
    /// <param name="hitInfoDistance">The distance of the hit point from the origin, obtained from a BoxCast.</param>
    /// <param name="color">The color of the drawn box.</param>
    public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float hitInfoDistance, Color color)
    {
        origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
        DrawBox(origin, halfExtents, orientation, color);
    }

    /// <summary>
    /// Draws a pair of boxes representing the starting and ending positions of a BoxCast.
    /// </summary>
    /// <param name="origin">Starting point of the BoxCast.</param>
    /// <param name="halfExtents">Half size of the box being cast.</param>
    /// <param name="orientation">Orientation (rotation) of the box being cast.</param>
    /// <param name="direction">Direction of the BoxCast.</param>
    /// <param name="distance">Distance of the BoxCast.</param>
    /// <param name="color">Color of the drawn boxes and lines.</param>
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

    /// <summary>
    /// Draws a box at the specified origin with the given half extents and orientation.
    /// </summary>
    /// <param name="origin">The center point of the box.</param>
    /// <param name="halfExtents">The half size of the box (extent from the center to the edge in each axis).</param>
    /// <param name="orientation">The rotation of the box.</param>
    /// <param name="color">The color of the drawn box.</param>

    public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
    {
        DrawBox(new Box(origin, halfExtents, orientation), color);
    }
    /// <summary>
    /// Draws the edges of a box using Debug.DrawLine based on the provided Box object.
    /// </summary>
    /// <param name="box">The Box object representing the box to draw.</param>
    /// <param name="color">The color of the drawn box edges.</param>
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

    /// <summary>
    /// Calculates the center point of a collision based on the given origin, direction, and hitInfoDistance.
    /// </summary>
    /// <param name="origin">The starting point of the cast.</param>
    /// <param name="direction">The direction of the cast.</param>
    /// <param name="hitInfoDistance">The distance of the collision point from the origin, obtained from a cast hit info.</param>
    /// <returns>The center point of the collision.</returns>

    static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance)
    {
        return origin + (direction.normalized * hitInfoDistance);
    }

    /// <summary>
    /// Rotates a point around a pivot using the provided rotation.
    /// </summary>
    /// <param name="point">The point to be rotated.</param>
    /// <param name="pivot">The pivot point around which the rotation will occur.</param>
    /// <param name="rotation">The quaternion representing the rotation to apply.</param>
    /// <returns>The rotated point.</returns>

    static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot;
        return pivot + rotation * direction;
    }





}
