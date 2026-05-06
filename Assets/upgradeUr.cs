using UnityEditor;
using UnityEngine;

public class ProperURPUpgrade
{
    [MenuItem("Tools/URP/Proper Upgrade Selected Materials")]
    static void Upgrade()
    {
        var mats = Selection.GetFiltered<Material>(SelectionMode.DeepAssets);

        if (mats.Length == 0)
        {
            Debug.LogWarning("머티리얼 선택해라");
            return;
        }

        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");

        foreach (var mat in mats)
        {
            if (mat == null) continue;

            UpgradeMaterial(mat, urpLit);

            EditorUtility.SetDirty(mat);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"완료: {mats.Length}개 머티리얼");
    }

    [MenuItem("Tools/URP/Upgrade Materials In Selected Folder")]
    static void UpgradeFolder()
    {
        var selected = Selection.GetFiltered<Object>(SelectionMode.Assets);

        if (selected.Length == 0)
        {
            Debug.LogWarning("폴더 선택해라");
            return;
        }

        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");

        int count = 0;

        foreach (var obj in selected)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (!AssetDatabase.IsValidFolder(path))
                continue;

            // 폴더 내부 모든 Material 찾기
            string[] guids = AssetDatabase.FindAssets("t:Material", new[] { path });

            foreach (var guid in guids)
            {
                string matPath = AssetDatabase.GUIDToAssetPath(guid);
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);

                if (mat == null) continue;

                UpgradeMaterial(mat, urpLit);
                count++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"완료 (폴더): {count}개 머티리얼");
    }

    static void UpgradeMaterial(Material mat, Shader urpLit)
    {
        // 기존 텍스처 참조
        Texture albedo = mat.GetTexture("_MainTex");
        if (albedo == null)
            albedo = mat.GetTexture("_BaseMap");

        Texture metallic = mat.GetTexture("_MetallicGlossMap");
        Texture normal = mat.GetTexture("_BumpMap");
        Texture occlusion = mat.GetTexture("_OcclusionMap");

        Vector2 scale = Vector2.one;
        Vector2 offset = Vector2.zero;

        if (mat.HasProperty("_MainTex"))
        {
            scale = mat.GetTextureScale("_MainTex");
            offset = mat.GetTextureOffset("_MainTex");
        }

        // 셰이더 변경
        mat.shader = urpLit;

        // BaseMap
        if (albedo != null)
        {
            mat.SetTexture("_BaseMap", albedo);
            mat.SetTextureScale("_BaseMap", scale);
            mat.SetTextureOffset("_BaseMap", offset);
        }

        // Metallic
        if (metallic != null)
        {
            mat.SetTexture("_MetallicGlossMap", metallic);
            mat.EnableKeyword("_METALLICSPECGLOSSMAP");
        }

        // Normal
        if (normal != null)
        {
            mat.SetTexture("_BumpMap", normal);
            mat.EnableKeyword("_NORMALMAP");
        }

        // Occlusion
        if (occlusion != null)
        {
            mat.SetTexture("_OcclusionMap", occlusion);
        }

        EditorUtility.SetDirty(mat);
    }
}