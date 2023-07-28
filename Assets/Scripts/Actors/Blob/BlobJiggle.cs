using UnityEngine;

public class BlobJiggle : MonoBehaviour
{
    public float Intensity = 1f;
    public float Mass = 1f;
    public float stiffness = 1f;
    public float damping = .075f;
    public Vector3 minVelocity = new Vector3(.05f, .05f, .05f);
    public Vector3 maxVelocity = new Vector3(.35f, .35f, .35f);
    private Mesh OriginalMesh, MeshClone;
    private SkinnedMeshRenderer meshRenderer;
    private JellyVertex[] jv;
    private Vector3[] vertexArray;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        OriginalMesh = meshRenderer.sharedMesh;
        MeshClone = Instantiate(OriginalMesh);
        meshRenderer.sharedMesh = MeshClone;

        jv = new JellyVertex[MeshClone.vertices.Length];
        for (int i = 0; i < MeshClone.vertices.Length; i++)
            jv[i] = new JellyVertex(i, transform.TransformPoint(MeshClone.vertices[i]));

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 HighestVelocity = Vector3.zero;
        vertexArray = OriginalMesh.vertices;
        for (int i = 0; i < jv.Length; i++)
        {
            Vector3 target = transform.TransformPoint(vertexArray[jv[i].ID]);
            float intensity = (1 - (meshRenderer.bounds.max.y - target.y) / meshRenderer.bounds.size.y) * Intensity;
            Vector3 velocity = jv[i].Shake(target, Mass, stiffness, damping, minVelocity, maxVelocity);
            if (velocity.sqrMagnitude > HighestVelocity.sqrMagnitude)
            {
                HighestVelocity = velocity;
            }
            target = transform.InverseTransformPoint(jv[i].Position);
            vertexArray[jv[i].ID] = Vector3.Lerp(vertexArray[jv[i].ID], target, intensity);
        }
        MeshClone.vertices = vertexArray;
    }

    public class JellyVertex
    {
        public int ID;
        public Vector3 Position;
        public Vector3 velocity, Force;

        public JellyVertex(int id, Vector3 pos)
        {
            ID = id;
            Position = pos;
        }

        public Vector3 Shake(Vector3 target, float m, float s, float d, Vector3 MinVelocity, Vector3 MaxVelocity)
        {
            Force = (target - Position) * s;
            //if (velocity.x >= 0) velocity.x = Mathf.Clamp(velocity.x, MinVelocity.x, MaxVelocity.x);
            //if (velocity.y >= 0) velocity.y = Mathf.Clamp(velocity.y, MinVelocity.y, MaxVelocity.y);
            //if (velocity.z >= 0) velocity.z = Mathf.Clamp(velocity.z, MinVelocity.z, MaxVelocity.z);
            //if (velocity.x < 0) velocity.x = Mathf.Clamp(velocity.x, -MinVelocity.x, -MaxVelocity.x);
            //if (velocity.y < 0) velocity.y = Mathf.Clamp(velocity.y, -MinVelocity.y, -MaxVelocity.y);
            //if (velocity.z < 0) velocity.z = Mathf.Clamp(velocity.z, -MinVelocity.z, -MaxVelocity.z);

            velocity = (velocity + Force / m) * d;
            Position += velocity;
            if ((velocity + Force + Force / m).magnitude < .001f)
                Position = target;
            return velocity;
        }
    }


}
