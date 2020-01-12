using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AssetBundle ʵ��
/// </summary>
public class AssetBundleEntitly
{
    /// <summary>
    /// Ψһ��
    /// </summary>
    public string Key;

    /// <summary>
    /// ��Դ����
    /// </summary>
    public string Name;

    /// <summary>
    /// ��Դ���
    /// </summary>
    public string Tag;

    /// <summary>
    /// �Ƿ���һ���ļ�
    /// </summary>
    public bool IsFolder;

    /// <summary>
    /// �Ƿ��ʼ����Դ
    /// </summary>
    public bool IsFirstData;

    /// <summary>
    /// �Ƿ�ѡ��
    /// </summary>
    public bool IsChecked;

    private List<string> path = new List<string>();

    /// <summary>
    /// ��Դ·��
    /// </summary>
    public List<string> Path
    {
        get { return path; }
    }



}
