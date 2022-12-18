using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHighlight : MonoBehaviour
{
   public void OnHover()
    {
        // Get all the renderers of this object
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer renderer in meshRenderers)
        {
            // Enables the Emission property of the renderer's material.
            renderer.material.EnableKeyword("_EMISSION");
        }
    }

    public void ExitHover()
    {
        // Get all the renderers of this object
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in meshRenderers)
        {
            // Disables the Emission property of the renderer's material.
            renderer.material.DisableKeyword("_EMISSION");
        }
    }
}
