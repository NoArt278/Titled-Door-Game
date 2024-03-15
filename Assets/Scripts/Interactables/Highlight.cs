using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    private List<Renderer> renderers;
    private List<Material> materials;
    private Color color = Color.white;
    void Awake()
    {
        renderers = new List<Renderer>();
        materials = new List<Material>();
        for (int i=0; i<transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Renderer>())
                renderers.Add(transform.GetChild(i).GetComponent<Renderer>());
        }
        // Add own material
        renderers.Add(GetComponent<Renderer>());
        foreach (Renderer renderer in renderers)
        {
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }

    public void ToggleHighlight(bool val)
    {
        if (val)
        {
            foreach (var material in materials)
            {
                //We need to enable the EMISSION
                material.EnableKeyword("_EMISSION");
                //before we can set the color
                material.SetColor("_EmissionColor", color);
            }
        }
        else
        {
            foreach (var material in materials)
            {
                //we can just disable the EMISSION
                //if we don't use emission color anywhere else
                material.DisableKeyword("_EMISSION");
            }
        }
    }

    public void RefreshMaterials()
    {
        foreach (Renderer renderer in renderers)
        {
            materials = new List<Material>(renderer.materials);
        }
    }
}
