﻿using UnityEngine;
using UnityEngine.Scripting;

namespace SoftMasking.TextMeshPro {
    [Preserve]
    [GlobalMaterialReplacer]
    public class MaterialReplacer : IMaterialReplacer {
        // SoftMask's TMPro replacer is called after the standard one.
        public int order { get { return 10; } }

        public Material Replace(Material material) {
            if (material && material.shader && material.shader.name.StartsWith("TextMeshPro/")) {
                var replacement = Shader.Find("Soft Mask/" + material.shader.name);
                if (replacement) {
                    var result = new Material(replacement);
                    result.CopyPropertiesFromMaterial(material);
                    return result;
                }
            }
            return null;
        }
    }
}
