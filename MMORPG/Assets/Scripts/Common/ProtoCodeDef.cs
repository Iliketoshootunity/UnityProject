

public class ProtoCodeDef
{


    public const ushort TestProto = 1;

    /// <summary>
    /// 登录服务器协议编码
    /// </summary>
    public const ushort LogOnGameServerProto = 10001;

    /// <summary>
    /// 服务器返回登录服务器结果——角色列表
    /// </summary>
    public const ushort LogOnGameServerReturnProto = 10002;

    /// <summary>
    /// 创建角色协议
    /// </summary>
    public const ushort CreateRoleProto = 10003;

    /// <summary>
    /// 创建角色结果服务器返回协议
    /// </summary>
    public const ushort CreateRoleReturnProto = 10004;

    /// <summary>
    /// 进入游戏
    /// </summary>
    public const ushort EnterGameProto = 10005;

    /// <summary>
    /// 进入游戏结果服务器返回协议
    /// </summary>
    public const ushort EnterGameReturnProto = 10006;

    /// <summary>
    /// 删除角色
    /// </summary>
    public const ushort DeleteRoleProto = 10007;

    /// <summary>
    /// 删除角色结果服务器返回协议
    /// </summary>
    public const ushort DeleteRoleReturnProto = 10008;

    /// <summary>
    /// 角色信息结果服务器返回协议
    /// </summary>
    public const ushort SelectRoleInfoReturnProto = 10009;


    /// <summary>
    /// 角色技能信息结果服务器返回协议
    /// </summary>
    public const ushort SkillReturnReturnProto = 10010;


    //-------------------游戏关卡相关----------------------//
    /// <summary>
    /// 进入关卡协议
    /// </summary>
    public const ushort GameLevel_EnterProto = 20001;
    /// <summary>
    /// 进入关卡服务器返回协议
    /// </summary>
    public const ushort GameLevel_EnterReturnProto = 20002;
    /// <summary>
    /// 关卡胜利协议
    /// </summary>
    public const ushort GameLevel_VictroyProto = 20003;
    /// <summary>
    /// 关卡胜利 服务器返回协议
    /// </summary>
    public const ushort GameLevel_VictroyReturnProto = 20004;
    /// <summary>
    /// 关卡失败协议
    /// </summary>
    public const ushort GameLevel_FailProto = 20005;
    /// <summary>
    /// 关卡失败 服务器返回协议
    /// </summary>
    public const ushort GameLevel_FailReturnProto = 20006;
    /// <summary>
    /// 关卡复活协议
    /// </summary>
    public const ushort GameLevel_ResurgenceProto = 20007;
    /// <summary>
    /// 关卡复活 服务器返回协议
    /// </summary>
    public const ushort GameLevel_ResurgenceReturnProto = 20008;

}
