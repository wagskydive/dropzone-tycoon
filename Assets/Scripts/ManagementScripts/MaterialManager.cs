using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

namespace ManagementScripts
{
    public class BackupMaterials
    {
        public GameObject backupObject { get; private set; }

        public Material[] rootMaterials { get; private set; }
        public Material[] rootSharedMaterials { get; private set; }
        public List<Material[]> ChildMaterials { get; private set; }
        public List<Material[]> ChildSharedMaterials { get; private set; }
        public BackupMaterials(GameObject objectRef)
        {
            backupObject = objectRef;
            Renderer renderer = objectRef.GetComponent<Renderer>();
            if (renderer != null)
            {
                rootMaterials = renderer.materials;
                rootSharedMaterials = renderer.sharedMaterials;
            }
            GetChildMaterials();
        }

        void GetChildMaterials()
        {
            List<Material[]> materials = new List<Material[]>();
            List<Material[]> sharedMaterials = new List<Material[]>();
            Renderer[] renderers = backupObject.GetComponentsInChildren<Renderer>();
            if (renderers != null)
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    materials.Add(renderers[i].materials);
                    sharedMaterials.Add(renderers[i].sharedMaterials);
                }
            }

            ChildMaterials= materials;
            ChildSharedMaterials = sharedMaterials;
        }

        public void ResetMaterials()
        {

            Renderer renderer = backupObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.materials = rootMaterials;
                renderer.sharedMaterials = rootSharedMaterials;
            }
            ResetChildren();
        }
        void ResetChildren()
        {
            Renderer[] renderers = backupObject.GetComponentsInChildren<Renderer>();
            if (renderers != null)
            {
                
                for (int i = 0; i < renderers.Length; i++)
                {
                    if (ChildMaterials.Any() && ChildMaterials[i] != null)
                    {
                        renderers[i].materials = ChildMaterials[i];
                    }
                    if (ChildSharedMaterials.Any() && ChildSharedMaterials[i] != null)
                    {
                        renderers[i].sharedMaterials = ChildSharedMaterials[i];
                    }                                           
                }
            }
        }
    }

    public class MaterialManager : MonoBehaviour
    {

        public List<Material> defaultMaterials { get; private set; }

        BackupMaterials backup;

        Material placementMaterial;
        Material hightlightMaterial;

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


            placementMaterial = Resources.Load("Materials/PlacementMaterial") as Material;
            hightlightMaterial = Resources.Load("Materials/ObjectHighlightMaterial") as Material;

            SelectableObject.OnMouseEnterDetected += SetHighLightMaterials;
            SelectableObject.OnMouseExitDetected += SetOriginalMaterials;
            SelectableObject.OnObjectSelected += SetOriginalMaterials;
            ItemPlacer.OnPlaceHolderInstantiated += SetPlacementMaterial;
        }

        void SetPlacementMaterial(GameObject go)
        {
            backup = new BackupMaterials(go);
            SetMaterialToAllRenderers(go, placementMaterial);
        }

        void SetHighLightMaterials(SelectableObject selectableObject)
        {
            backup = new BackupMaterials(selectableObject.gameObject);

            SetMaterialToAllRenderers(selectableObject.gameObject, hightlightMaterial);
        }

        void SetOriginalMaterials(SelectableObject selectableObject)
        {
            if(backup != null)
            {
                backup.ResetMaterials();
            }            
        }

        public static void ChangeMaterialInstanceColor(Renderer renderer, int index, Color color)
        {
            renderer.materials[index].color = color;
        }


        public static void SetMaterialToAllRenderers(GameObject go, Material placementMaterial)
        {
            Renderer renderer = go.GetComponent<Renderer>();

            SetAllMaterialsInRenderer(renderer, placementMaterial);
            Renderer[] childRenderers = go.GetComponentsInChildren<Renderer>();
            if (childRenderers != null)
            {
                for (int i = 0; i < childRenderers.Length; i++)
                {
                    SetAllMaterialsInRenderer(childRenderers[i], placementMaterial);
                    childRenderers[i].material = placementMaterial;
                }
            }
        }

        public static Material[] GetAllMaterialsFromRenderersAndChildren(GameObject go)
        {
            List<Material> mats = new List<Material>();
            Renderer renderer = go.GetComponent<Renderer>();
            if (renderer != null)
            {
                mats.AddRange(GetAllMaterialsInRenderer(renderer));
            }

            Renderer[] childRenderers = go.GetComponentsInChildren<Renderer>();
            if (childRenderers != null)
            {
                for (int i = 0; i < childRenderers.Length; i++)
                {
                    mats.AddRange(GetAllMaterialsInRenderer(childRenderers[i]));

                }
            }
            return mats.ToArray();
        }

        private static Material[] GetAllMaterialsInRenderer(Renderer renderer)
        {
            List<Material> mats = new List<Material>();
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                mats.Add(renderer.materials[i]);
            }
            return mats.ToArray();
        }

        private static void SetAllMaterialsInRenderer(Material[] materials, Renderer renderer)
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i] = materials[i];
            }

        }

        private static void SetAllMaterialsInRendererAndChildren(BackupMaterials backupMaterials, GameObject go)
        {

            Material[] rootMats = backupMaterials.rootMaterials;
            Renderer renderer = go.GetComponent<Renderer>();
            if (renderer != null)
            {

                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    SetAllMaterialsInRenderer(renderer, rootMats[i]);

                }

            }

            Renderer[] childRenderers = go.GetComponentsInChildren<Renderer>();
            if (childRenderers != null)
            {
                for (int i = 0; i < childRenderers.Length; i++)
                {
                    childRenderers[i].materials = backupMaterials.ChildMaterials[i];



                }
            }


        }



        public static void SetAllMaterialsInRenderer(Renderer renderer, Material material)
        {
            if (renderer != null)
            {
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    renderer.materials[i] = material;
                }
            }

        }

        bool firstChange = false;

        public void HandleObjectMaterials(GameObject gameObject)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();

            if (renderer != null)
            {
                List<Material> shared = new List<Material>();
                List<Material> instantiated = new List<Material>();
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    Material defaultMat = CheckForMaterial(renderer.materials[i].name);
                    if (defaultMat != null)
                    {
                        Material temp = new Material(defaultMat);
                        temp.name = "Instance" + temp.name;
                        //renderer.materials[i] = temp;
                        shared.Add(defaultMat);
                        instantiated.Add(temp);
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
