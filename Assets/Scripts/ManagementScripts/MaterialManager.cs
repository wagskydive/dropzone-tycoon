using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace ManagementScripts
{
    public class MaterialManager : MonoBehaviour
    {
        
        public List<Material> defaultMaterials { get; private set; }

        private void Awake()
        {
            List<Material> materials = new List<Material>();
            DirectoryInfo levelDirectoryPath = new DirectoryInfo(Application.dataPath + "/Resources/Materials/");
            FileInfo[] fileInfos = levelDirectoryPath.GetFiles();
            for (int i = 0; i < fileInfos.Length; i++)
            {
                string extension = Path.GetExtension(fileInfos[i].Name);
                if (extension == ".mat")
                {
                   

                    string result = fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - extension.Length);
                    string loadString = "Materials/" + result;

                    Material mat = Resources.Load(loadString, typeof(Material)) as Material;
                    if (mat != null)
                    {
                        materials.Add(mat);
                    }

                }
            }
            defaultMaterials = materials;
        }

        public static void ChangeMaterialInstanceColor(Renderer renderer, int index, Color color)
        {
            renderer.materials[index].color = color;
        }

        bool firstChange = false;

        public void HandleObjectMaterials(GameObject gameObject)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            
            if (renderer != null)
            {
                List<Material> shared = new List<Material>();
                List<Material> intantiated = new List<Material>();
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    Material defaultMat = CheckForMaterial(renderer.materials[i].name);
                    if (defaultMat != null)
                    {
                        Material temp = new Material(defaultMat);
                        temp.name = "Instance" + temp.name;
                        //renderer.materials[i] = temp;
                        shared.Add(defaultMat);
                        intantiated.Add(temp);
                    }
                    else
                    {
                        renderer.materials[i].name = RemoveEnd(renderer.materials[i].name, " (Instance)");
                        AddMaterialToDefaults(renderer.materials[i]);
                    }
                }
                renderer.sharedMaterials = shared.ToArray();
                renderer.materials = shared.ToArray();


                
                //renderer.GetSharedMaterials(shared);//.sharedMaterials[i] = defaultMat;
            }


            int childCount = gameObject.transform.childCount;
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    HandleObjectMaterials(gameObject.transform.GetChild(i).gameObject);
                }

            }

        }

        public Material CheckForMaterial(string materialName)
        {
            string instance = " (Instance)";
            materialName = RemoveEnd(materialName, instance);

            for (int i = 0; i < defaultMaterials.Count; i++)
            {
                if (defaultMaterials[i].name + " (Instance)" == materialName || defaultMaterials[i].name == materialName)
                {
                    return defaultMaterials[i];
                }
            }
            return null;
        }

        private static string RemoveEnd(string source, string end)
        {
            if (source.EndsWith(end))
            {
                string result = source.Substring(0, source.Length - end.Length);
                source = result;
            }

            return source;
        }

        public void AddMaterialToDefaults(Material material)
        {
            //List<Material> newMats = defaultMaterials.
            defaultMaterials.Add(material);
        }
    }
}
