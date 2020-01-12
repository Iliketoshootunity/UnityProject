using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceUtil
{

    /// <summary>
    /// 设备表示符
    /// </summary>
    public static string DeviceIdentifier
    {
        get
        {
            return SystemInfo.deviceUniqueIdentifier;
        }
    }

    /// <summary>
    /// 设备类型
    /// </summary>
    public static string DeviceModel
    {
        get
        {

#if !UNITY_EDITOR && UNITY_IPHONE
            return UnityEngine.iOS.Device.generation.ToString(); 
#else
            return SystemInfo.deviceModel;
#endif
        }
    }

}
