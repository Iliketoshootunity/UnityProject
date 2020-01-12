using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AssetBundle 实体
/// </summary>
public class AssetBundleEntitly
{
    /// <summary>
    /// 唯一键
    /// </summary>
    public string Key;

    /// <summary>
    /// 资源名字
    /// </summary>
    public string Name;

    /// <summary>
    /// 资源标记
    /// </summary>
    public string Tag;

    /// <summary>
    /// 是否打包一个文件
    /// </summary>
    public bool IsFolder;

    /// <summary>
    /// 是否初始化资源
    /// </summary>
    public bool IsFirstData;

    /// <summary>
    /// 是否被选中
    /// </summary>
    public bool IsChecked;

    private List<string> path = new List<string>();

    /// <summary>
    /// 资源路径
    /// </summary>
    public List<string> Path
    {
        get { return path; }
    }



}
