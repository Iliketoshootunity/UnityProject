using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 场景管理器
/// </summary>
public class SceneMgr : Singleton<SceneMgr>
{

    /// <summary>
    /// 场景类型
    /// </summary>
    public SceneType CurrentType;

    public int TargetTransId;

    #region 加载世界地图 和野外场景
    private int m_CurWorldMapSceneId;
    public int CurWorldMapSceneId { get { return m_CurWorldMapSceneId; } }
    /// <summary>
    /// 加载到主城
    /// </summary>
    public void LoadWorldMap(int sceneId)
    {
        m_CurWorldMapSceneId = sceneId;
        CurrentType = SceneType.WorldMap;
        SceneManager.LoadScene(SceneType.Loading.ToString());
    }
    #endregion
    #region 记载到关卡
    private int m_CurGameLevelSceneId;
    public int CurGameLevelSceneId { get { return m_CurGameLevelSceneId; } }
    private GameLevelGrade m_CurGameLevelGrade;
    public GameLevelGrade CurGameLevelGrade { get { return m_CurGameLevelGrade; } }

    public void LoadGameLevel(int sceneId, GameLevelGrade grade)
    {
        m_CurGameLevelSceneId = sceneId;
        m_CurGameLevelGrade = grade;
        CurrentType = SceneType.Gamelevel;
        SceneManager.LoadScene(SceneType.Loading.ToString());
    }
    #endregion
    /// <summary>
    /// 加载到登录场景
    /// </summary>
    public void LoadLogin()
    {
        CurrentType = SceneType.LogOn;
        SceneManager.LoadScene(SceneType.Loading.ToString());
    }

    public void LoadSelectRole()
    {
        CurrentType = SceneType.SelectRole;
        SceneManager.LoadScene(SceneType.Loading.ToString());
    }

}
