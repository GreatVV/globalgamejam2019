using System.IO;
using System.Linq;

using UnityEngine;

public class ProceduralTexturesExporter : MonoBehaviour
{
    public Material Material;

    public string TextureName;

    public int Iterations = 1;

    public int RandomSeed = -1;

    public string Folder = "Export";

    public void Start()
    {
        if (RandomSeed >= 0) Random.InitState(RandomSeed);
        for (var i = 0; i < Iterations; ++i)
        {
            foreach (var textureName in Material.GetTexturePropertyNames())
            {
                var texture = (Texture2D)Material.GetTexture(textureName);
                if (texture != null)
                {
                    var tex2d = new Texture2D(texture.width, texture.height);
                    tex2d.SetPixels32(texture.GetPixels32(0));
                    tex2d.Apply();
                    var bytes = tex2d.EncodeToPNG();
                    var outFile = new FileInfo(Path.Combine(Application.dataPath + @"\", string.Format("{0}_{1}.png", texture.name, i)));
                    outFile.Directory.Create();
                    File.WriteAllBytes(outFile.FullName, bytes);
                }
            }
        }
    }
}
