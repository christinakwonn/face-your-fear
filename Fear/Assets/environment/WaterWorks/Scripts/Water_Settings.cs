using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Water_Settings : MonoBehaviour
{
    public Material waterVolume; // assign this in the Inspector
    private Material waterMaterial;

    void Update()
    {
        if (waterVolume == null)
        {
            Debug.LogWarning("Water Volume material not assigned.");
            return;
        }

        if (waterMaterial == null)
        {
            var renderer = GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                Debug.LogWarning("No MeshRenderer found.");
                return;
            }

            waterMaterial = renderer.sharedMaterial;
            if (waterMaterial == null)
            {
                Debug.LogWarning("No shared material on MeshRenderer.");
                return;
            }
        }

        float displacement = waterMaterial.GetFloat("_Displacement_Amount");

        Vector4 pos = new Vector4(0, transform.position.y + (displacement / 3f), 0, 0);
        waterVolume.SetVector("pos", pos);
    }
}
