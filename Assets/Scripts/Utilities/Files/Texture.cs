using System.IO;
using UnityEngine;

namespace Plutono.Util
{
    public static class Texture
    {
        /// <summary>
        /// Load a PNG or JPG file from disk to a Texture2D
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>The Texture2D. Returns null if load fails</returns>
        public static Texture2D? LoadTexture(string filePath)
        {
            if (!File.Exists(filePath)) return null; // Return null if load failed
            Texture2D tex2D = new(0, 0);           // Create new "empty" texture
            return tex2D.LoadImage(File.ReadAllBytes(filePath)) ? // Load the image data into the texture (size is set automatically)
                tex2D : // If data = readable -> return texture
                null; // Return null if load failed
        }
    }
}