

public class ProtoCodeDef
{


    public const ushort TestProto = 1;

    /// <summary>
    /// ��¼������Э�����
    /// </summary>
    public const ushort LogOnGameServerProto = 10001;

    /// <summary>
    /// ���������ص�¼���������������ɫ�б�
    /// </summary>
    public const ushort LogOnGameServerReturnProto = 10002;

    /// <summary>
    /// ������ɫЭ��
    /// </summary>
    public const ushort CreateRoleProto = 10003;

    /// <summary>
    /// ������ɫ�������������Э��
    /// </summary>
    public const ushort CreateRoleReturnProto = 10004;

    /// <summary>
    /// ������Ϸ
    /// </summary>
    public const ushort EnterGameProto = 10005;

    /// <summary>
    /// ������Ϸ�������������Э��
    /// </summary>
    public const ushort EnterGameReturnProto = 10006;

    /// <summary>
    /// ɾ����ɫ
    /// </summary>
    public const ushort DeleteRoleProto = 10007;

    /// <summary>
    /// ɾ����ɫ�������������Э��
    /// </summary>
    public const ushort DeleteRoleReturnProto = 10008;

    /// <summary>
    /// ��ɫ��Ϣ�������������Э��
    /// </summary>
    public const ushort SelectRoleInfoReturnProto = 10009;


    /// <summary>
    /// ��ɫ������Ϣ�������������Э��
    /// </summary>
    public const ushort SkillReturnReturnProto = 10010;


    //-------------------��Ϸ�ؿ����----------------------//
    /// <summary>
    /// ����ؿ�Э��
    /// </summary>
    public const ushort GameLevel_EnterProto = 20001;
    /// <summary>
    /// ����ؿ�����������Э��
    /// </summary>
    public const ushort GameLevel_EnterReturnProto = 20002;
    /// <summary>
    /// �ؿ�ʤ��Э��
    /// </summary>
    public const ushort GameLevel_VictroyProto = 20003;
    /// <summary>
    /// �ؿ�ʤ�� ����������Э��
    /// </summary>
    public const ushort GameLevel_VictroyReturnProto = 20004;
    /// <summary>
    /// �ؿ�ʧ��Э��
    /// </summary>
    public const ushort GameLevel_FailProto = 20005;
    /// <summary>
    /// �ؿ�ʧ�� ����������Э��
    /// </summary>
    public const ushort GameLevel_FailReturnProto = 20006;
    /// <summary>
    /// �ؿ�����Э��
    /// </summary>
    public const ushort GameLevel_ResurgenceProto = 20007;
    /// <summary>
    /// �ؿ����� ����������Э��
    /// </summary>
    public const ushort GameLevel_ResurgenceReturnProto = 20008;

}
