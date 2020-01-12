using UnityEngine;
using System.Collections;
using System.IO;

public class LocalFileMgr
{

    private static LocalFileMgr _instance;

    public static LocalFileMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LocalFileMgr();
            }
            return _instance;
        }
    }

#if UNITY_EDITOR
#if UNITY_ANDROID  
    public static string localFilePath =  Application.dataPath + "/../AssetBundles/Android/";
#elif UNITY_IPHONE
    public static string localFilePath = Application.dataPath + "/../AssetBundles/iOS/";
#elif UNITY_STANDALONE_WIN
    public static string localFilePath = Application.dataPath + "/../AssetBundles/Windows/";
#endif
#elif UNITY_STANDALONE_WIN || UNITY_IPHONE || UNITY_ANDROID
#if UNITY_ANDROID
    public static string localFilePath = Application.persistentDataPath + "/Android/";
#elif UNITY_IPHONE
    public static string localFilePath = Application.persistentDataPath + "/iOS/";
#elif UNITY_STANDALONE_WIN
    public static string localFilePath = Application.persistentDataPath + "/Windows/";
#endif
#endif

#if UNITY_ANDROID
    public static string Platform =  "Android"; 
#elif UNITY_IPHONE
      public static string Platform =  "iOS";
#elif UNITY_STANDALONE_WIN
    public static string Platform = "Windows";
#endif
    public byte[] GetBuffer(string path)
    {
        byte[] buffer = null;
        using (FileStream file = new FileStream(path, FileMode.Open))
        {
            buffer = new byte[file.Length];
            file.Read(buffer, 0, buffer.Length);
        }
        return buffer;
    }



}



