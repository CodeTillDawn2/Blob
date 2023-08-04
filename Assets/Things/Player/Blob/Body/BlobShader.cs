using UnityEngine;

public class BlobShader : MonoBehaviour
{
    Renderer[] renderers;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            foreach (Material material in materials)
            {
                material.SetInt("_Stencil", (int)UnityEngine.Rendering.CompareFunction.Always);
                material.SetInt("_StencilOp", (int)UnityEngine.Rendering.StencilOp.Replace);
                material.SetInt("_StencilWriteMask", 1);
                material.SetInt("_StencilRef", 1);
                material.SetInt("_StencilComp", (int)UnityEngine.Rendering.CompareFunction.Equal);
            }
        }
    }
}
