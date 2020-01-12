using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class Tools
{

    [MenuItem("Tools/CreateAssetBound")]
    public static void AssetBoundCreate()
    {
        AssetBundleWindow window = EditorWindow.GetWindow<AssetBundleWindow>();
        window.titleContent = new GUIContent("AssetBundle");
        window.Show();
    }
    [MenuItem("Tools/CreateAssetBound2")]
    public static void AssetBoundCreate2()
    {
        string toPath = Application.dataPath + "/../AssetBundles/" + "MyAssetBound";
        if (!Directory.Exists(toPath))
        {
            Directory.CreateDirectory(toPath);
        }
        BuildPipeline.BuildAssetBundles(toPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

    }
}
