using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;

/// <summary>
///  AssetBundle窗口
/// </summary>
public class AssetBundleWindow : EditorWindow
{
    private AssetBundleDBModel dal;
    private List<AssetBundleEntitly> list;
    /// <summary>
    /// 是否选中
    /// </summary>
    private Dictionary<string, bool> dic;

    //打包标记
    private string[] arrTag = { "All", "Role", "Scene", "Effect", "Audio", "None" };
    private int tagIndex = 0;
    private int selectTagIndex = -1;
    //打包平台设置
    private string[] arrayBuildTarget = { "Windows", "Android", "iOS" };
    private int selectBuildTarget = -1;
#if UNITY_STANDALONE_WIN
    private BuildTarget buildTarget = BuildTarget.StandaloneWindows;
    private int buildTargetIndex = 0;
#elif UNITY_ANDROID
    private BuildTarget buildTarget = BuildTarget.Android;
    private int buildTargetIndex = 1;
#elif UNITY_IPHONE
    private BuildTarget buildTarget = BuildTarget.iOS;
    private int buildTargetIndex=1;
#endif
    private Vector2 pos;
    public AssetBundleWindow()
    {
        string path = @"H:\UnityWorkSpace\MMORPG\Assets\Editor\AssetBundle\AssetBundleConfig.xml";
        dal = new AssetBundleDBModel(path);
        list = dal.GetList();

        dic = new Dictionary<string, bool>();
        foreach (var item in list)
        {
            dic[item.Key] = true;
        }
    }

    /// <summary>
    /// 绘制窗口
    /// </summary>
    void OnGUI()
    {
        if (list == null || list.Count == 0)
        {
            return;
        }
        #region 按钮行
        GUILayout.BeginHorizontal("box");
        selectTagIndex = EditorGUILayout.Popup(tagIndex, arrTag, GUILayout.Width(100));
        if (selectTagIndex != tagIndex)
        {
            tagIndex = selectTagIndex;
            EditorApplication.delayCall = OnSelectTagCallBack;
        }

        selectBuildTarget = EditorGUILayout.Popup(buildTargetIndex, arrayBuildTarget, GUILayout.Width(100));
        if (selectBuildTarget != buildTargetIndex)
        {
            buildTargetIndex = selectBuildTarget;
            EditorApplication.delayCall = OnSelectTargetCallBack;
        }
        if (GUILayout.Button("保存设置", GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnSaveSettingCallBack;
        }
        if (GUILayout.Button("打包AssetBundle", GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnAssetBundleCallBack;
        }
        if (GUILayout.Button("清空AssetBundle", GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnClearAssetBundleCallBack;
        }
        GUILayout.EndHorizontal();
        #endregion

        GUILayout.BeginHorizontal("box");
        GUILayout.Label("包名");
        GUILayout.Label("标记", GUILayout.Width(100));
        GUILayout.Label("文件夹", GUILayout.Width(200));
        GUILayout.Label("初始资源", GUILayout.Width(200));
        GUILayout.EndHorizontal();

        pos = EditorGUILayout.BeginScrollView(pos);
        for (int i = 0; i < list.Count; i++)
        {
            AssetBundleEntitly entity = list[i];
            GUILayout.BeginHorizontal("box");
            dic[list[i].Key] = EditorGUILayout.Toggle(dic[list[i].Key], GUILayout.Width(20));
            GUILayout.Label(entity.Name);
            GUILayout.Label(entity.Tag, GUILayout.Width(100));
            GUILayout.Label(entity.IsFolder.ToString(), GUILayout.Width(200));
            GUILayout.Label(entity.IsFirstData.ToString(), GUILayout.Width(200));
            GUILayout.EndHorizontal();
            foreach (var item in entity.Path)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Space(40);
                GUILayout.Label(item);
                GUILayout.EndHorizontal();
            }

        }
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// 保存设置回调
    /// </summary>
    private void OnSaveSettingCallBack()
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].IsChecked = dic[list[i].Key];
        }
        for (int i = 0; i < list.Count; i++)
        {
            //如果是文件夹的话
            for (int j = 0; j < list[i].Path.Count; j++)
            {
                if (list[i].IsFolder)
                {
                    SaveFolderSetting(string.Format("{0}/{1}", "Assets", list[i].Path[j]), !list[i].IsChecked);
                }
                else
                {
                    SaveFileSetting(string.Format("{0}/{1}", "Assets", list[i].Path[j]), !list[i].IsChecked);
                }
            }


        };
    }


    /// <summary>
    /// 保存文件夹设置
    /// </summary>
    private void SaveFolderSetting(string foldPath, bool isSetNull)
    {

        //获取文件
        string[] file = Directory.GetFiles(foldPath);
        for (int i = 0; i < file.Length; i++)
        {
            SaveFileSetting(file[i], isSetNull);
        }
        //获取子文件夹
        string[] childDirectorys = Directory.GetDirectories(foldPath);
        if (childDirectorys != null && childDirectorys.Length > 0)
        {
            for (int i = 0; i < childDirectorys.Length; i++)
            {
                SaveFolderSetting(childDirectorys[i], isSetNull);
            }
        }
    }

    private void SaveFileSetting(string filePath, bool isSetNull)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        if (!File.Exists(filePath)) return;
        //如果文件后缀是meta文件 直接返回 不做处理
        if (fileInfo.Extension.EndsWith(".meta", StringComparison.CurrentCultureIgnoreCase))
        {
            return;
        }
        //新路径
        int index = filePath.IndexOf("Assets/");
        //相对路径
        string newPath = filePath.Substring(index);
        //AssetBoubdle文件名字 
        string name = newPath.Replace("Assets/", "").Replace(fileInfo.Extension, "");
        //AssetBundle 后缀
        string extension = fileInfo.Extension.EndsWith(".unity", StringComparison.CurrentCultureIgnoreCase) ? "Scene" : "AssetBundle";
        //设置AssetBundle
        AssetImporter importer = AssetImporter.GetAtPath(newPath);
        importer.SetAssetBundleNameAndVariant(name, extension);
        if (isSetNull)
        {
            importer.SetAssetBundleNameAndVariant(null, null);
        }
        importer.SaveAndReimport();

    }

    /// <summary>
    /// 打包AssetBundle回调
    /// </summary>
    private void OnAssetBundleCallBack()
    {
        string path = Application.dataPath + "/../AssetBundles/" + arrayBuildTarget[buildTargetIndex];
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, buildTarget);
        Debug.Log("打包完成");
    }


    /// <summary>
    /// 清空AssetBundle会回调
    /// </summary>
    private void OnClearAssetBundleCallBack()
    {
        string toPath = Application.dataPath + "/../AssetBundles";
        if (Directory.Exists(toPath))
        {
            Directory.Delete(toPath, true);
        }
    }
    /// <summary>
    /// 选定Taget回调
    /// </summary>
    private void OnSelectTargetCallBack()
    {
        switch (buildTargetIndex)
        {
            case 0:
                buildTarget = BuildTarget.StandaloneWindows;
                break;
            case 1:
                buildTarget = BuildTarget.Android;
                break;
            case 2:
                buildTarget = BuildTarget.iOS;
                break;
        }
        Debug.LogFormat("选定的平台是：{0}，{1}", buildTargetIndex, arrayBuildTarget[buildTargetIndex]);
    }
    /// <summary>
    ///  选定Tag回调
    /// </summary>
    private void OnSelectTagCallBack()
    {
        //private string[] arrTag = { "All", "Role", "Scene", "Effect", "Audio", "None" };
        switch (tagIndex)
        {
            case 0:     //全选
                for (int i = 0; i < list.Count; i++)
                {
                    dic[list[i].Key] = true;
                }
                break;
            case 1: //Role
                for (int i = 0; i < list.Count; i++)
                {
                    dic[list[i].Key] = list[i].Tag.EndsWith("Role");
                }
                break;
            case 2: //Scene
                for (int i = 0; i < list.Count; i++)
                {
                    dic[list[i].Key] = list[i].Tag.EndsWith("Scene");
                }
                break;
            case 3: //Effect
                for (int i = 0; i < list.Count; i++)
                {
                    dic[list[i].Key] = list[i].Tag.EndsWith("Effect");
                }
                break;
            case 4: //Audio
                for (int i = 0; i < list.Count; i++)
                {
                    dic[list[i].Key] = list[i].Tag.EndsWith("Audio");
                }
                break;
            case 5: //None
                for (int i = 0; i < list.Count; i++)
                {
                    dic[list[i].Key] = false;
                }
                break;
        }
        Debug.LogFormat("选定的标记是：{0},{1}", tagIndex, arrTag[tagIndex]);
    }
}
