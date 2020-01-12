using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// ����������
/// </summary>
public class SceneMgr : Singleton<SceneMgr>
{

    /// <summary>
    /// ��������
    /// </summary>
    public SceneType CurrentType;

    public int TargetTransId;

    #region ���������ͼ ��Ұ�ⳡ��
    private int m_CurWorldMapSceneId;
    public int CurWorldMapSceneId { get { return m_CurWorldMapSceneId; } }
    /// <summary>
    /// ���ص�����
    /// </summary>
    public void LoadWorldMap(int sceneId)
    {
        m_CurWorldMapSceneId = sceneId;
        CurrentType = SceneType.WorldMap;
        SceneManager.LoadScene(SceneType.Loading.ToString());
    }
    #endregion
    #region ���ص��ؿ�
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
    /// ���ص���¼����
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
