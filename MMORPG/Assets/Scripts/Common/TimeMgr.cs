using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMgr : MonoSingleton<TimeMgr>
{


    private float m_ScaleDuration;
    private float m_BeginTime;
    private bool m_ScaleTime;

    public void SetTimeScaleDuration(float scale, float duration)
    {
        Time.timeScale = scale;
        m_ScaleTime = true;
        m_BeginTime = Time.realtimeSinceStartup;
        m_ScaleDuration = duration;
    }

    void Update()
    {
        if (m_ScaleTime)
        {
            if (Time.realtimeSinceStartup > m_BeginTime + m_ScaleDuration)
            {
                Time.timeScale = 1;
                m_ScaleTime = false;
            }
        }
    }
}
