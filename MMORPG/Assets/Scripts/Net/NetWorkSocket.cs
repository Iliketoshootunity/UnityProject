using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetWorkSocket : MonoBehaviour
{

    /// <summary>
    /// 压缩界限 大于此值则需要压缩
    /// </summary>
    private const int m_CompressLen = 200;

    private static NetWorkSocket _instance;

    public static NetWorkSocket Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = GameObject.Find("NetWorkSocket");
                if (go == null)
                {
                    go = new GameObject("NetWorkSocket");
                }
                _instance = go.GetOrCreateComponen<NetWorkSocket>();
            }
            return _instance;
        }
    }


    private Socket m_Socket;

    public Action OnConnectOk;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        MessageProcessing();
    }

    public void OnDestroy()
    {
        DisConnect();
    }

    #region 发送消息

    private Action m_CheckSendQueue;

    private Queue<byte[]> m_SendQueue = new Queue<byte[]>();

    /// <summary>
    /// 连接
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public void Connect(string ip, int port)
    {
        if (m_Socket == null || !m_Socket.Connected)
        {
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        try
        {
            m_Socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            m_CheckSendQueue = OnCheckSendQueue;
            ReceiveMsg();
            if (OnConnectOk != null)
            {
                OnConnectOk();
            }
            Debug.Log(string.Format("连接成功"));
        }
        catch (Exception e)
        {
            Debug.Log(string.Format("连接失败：{0}", e.ToString()));
        }

    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void DisConnect()
    {
        if (m_Socket != null && m_Socket.Connected)
        {
            m_Socket.Shutdown(SocketShutdown.Both);
            m_Socket.Close();
        }
    }
    /// <summary>
    /// 发送消息
    /// </summary>
    public void SendMsg(byte[] msg)
    {
        byte[] buffer = MakeData(msg);
        m_SendQueue.Enqueue(buffer);
        m_CheckSendQueue.BeginInvoke(null, null);
    }

    /// <summary>
    /// 创建包数据：包头+包体
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    private byte[] MakeData(byte[] buffer)
    {
        //1.压缩
        bool isCompress = buffer.Length > m_CompressLen ? true : false;
        if (isCompress)
        {
            buffer = ZlibHelper.CompressBytes(buffer);
        }
        //2.CRC1校验
        ushort crc = Crc16.CalculateCrc16(buffer);
        //3.异或
        buffer = SecurityUtil.Xor(buffer);

        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            //包头长度 压缩标识1+CRC16校验+buffer的长度
            ms.WriteUShort((ushort)(buffer.Length + 3));
            ms.WriteBool(isCompress);
            ms.WriteUShort(crc);
            ms.Write(buffer, 0, buffer.Length);
            return ms.ToArray();
        }
    }

    /// <summary>
    /// 检查消息队列回调
    /// </summary>
    private void OnCheckSendQueue()
    {
        if (m_SendQueue.Count > 0)
        {
            byte[] buffer = m_SendQueue.Dequeue();
            Send(buffer);
        }
    }
    /// <summary>
    /// 真正发消息的方法
    /// </summary>
    /// <param name="msg"></param>
    private void Send(byte[] msg)
    {
        m_Socket.BeginSend(msg, 0, msg.Length, SocketFlags.None, OnSendCallBack, m_Socket);
    }

    private void OnSendCallBack(IAsyncResult ar)
    {
        m_Socket.EndSend(ar);

        OnCheckSendQueue();
    }



    #endregion

    #region 接收消息

    //接受到数组的缓存数组
    private byte[] m_ReceiveBuffer = new byte[10240];
    //接收到的数据的缓存数据流
    private MMO_MemoryStream m_ReceiveMS = new MMO_MemoryStream();

    private int msgIndex;
    private Queue<byte[]> m_ReceiveQueue = new Queue<byte[]>();
    private void ReceiveMsg()
    {
        m_Socket.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, m_Socket);
    }
    /// <summary>
    /// 异步接受回调
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            int len = m_Socket.EndReceive(ar);
            if (len > 0)
            {
                //将接受的数组缓存到缓存数据流的尾部
                m_ReceiveMS.Position = m_ReceiveMS.Length;
                //byte[] 缓存到数据流中
                m_ReceiveMS.Write(m_ReceiveBuffer, 0, len);
                //说明至少有一个不完整的包传了过来
                if (m_ReceiveBuffer.Length > 2)
                {
                    while (true)
                    {
                        //指针位置经过writey已经下移，要读的话需要归0
                        m_ReceiveMS.Position = 0;
                        //读取包头包含的长度信息
                        ushort bodylen = m_ReceiveMS.ReadUShort();
                        //整个包大小
                        int fullLen = bodylen + 2;
                        //说明已经有一个完整的包读取下来
                        if (m_ReceiveMS.Length >= bodylen + 2)
                        {

                            //读取包头
                            byte[] newBuffer = new byte[bodylen - 3];
                            bool isCompress = m_ReceiveMS.ReadBool();
                            ushort crc = m_ReceiveMS.ReadUShort();
                            m_ReceiveMS.Read(newBuffer, 0, bodylen - 3);
                            //1..异或
                            newBuffer = SecurityUtil.Xor(newBuffer);
                            //2.CRC1校验
                            ushort newCrc = Crc16.CalculateCrc16(newBuffer);
                            if (newCrc != crc) break;
                            if (isCompress)
                            {
                                //3.解压
                                newBuffer = ZlibHelper.DeCompressBytes(newBuffer);
                            }
                            Debug.Log(newCrc);
                            lock (m_ReceiveQueue)
                            {
                                m_ReceiveQueue.Enqueue(newBuffer);
                            }
                            //处理剩余的包
                            int remainingLen = (int)m_ReceiveMS.Length - fullLen;
                            //---------
                            //重新写入数据流
                            //---------
                            if (remainingLen > 0)
                            {
                                //剩余数组缓冲
                                byte[] remianingBuffer = new byte[m_ReceiveMS.Length - fullLen];
                                //读取到剩余数组缓冲
                                m_ReceiveMS.Read(remianingBuffer, 0, remainingLen);
                                //格式化数据流
                                m_ReceiveMS.Position = 0;
                                m_ReceiveMS.SetLength(0);
                                //重新写入数据流
                                m_ReceiveMS.Write(remianingBuffer, 0, remianingBuffer.Length);
                            }
                            else
                            {
                                //格式化数据流
                                m_ReceiveMS.Position = 0;
                                m_ReceiveMS.SetLength(0);
                                break;
                            }

                        }
                        //包不完整
                        else
                        {
                            break;
                        }

                    }
                }

                ReceiveMsg();
            }
            //客户端主动断开连接
            else
            {
                Debug.Log(string.Format("服务器{0}主动断开连接", m_Socket.RemoteEndPoint));


            }
        }
        //客户端进程结束 被迫断开连接
        catch
        {
            Console.WriteLine(string.Format("服务器{0}被迫断开连接", m_Socket.RemoteEndPoint));
        }

    }

    /// <summary>
    /// 消息处理 在update 中执行
    /// </summary>
    private void MessageProcessing()
    {
        while (true)
        {
            if (msgIndex < 5)
            {
                if (m_ReceiveQueue.Count > 0)
                {
                    byte[] buffer = m_ReceiveQueue.Dequeue();

                    using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
                    {
                        ushort protoCode = ms.ReadUShort();
                        byte[] protoContent = new byte[buffer.Length - 2];
                        ms.Read(protoContent, 0, protoContent.Length);

                        SocketDispatcher.Instance.Dispatc(protoCode, protoContent);
                    }
                }
                else
                {
                    break;
                }
            }
            else
            {
                msgIndex = 0;
                break;
            }
        }

    }

    #endregion


}
