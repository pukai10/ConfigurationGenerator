
using System.Collections.Generic;
using System.Linq;

namespace AurogonTools
{
    public enum OutMsgType
    {
        Info,
        Warning,
        Error,
        All
    }

    /// <summary>
    /// 输出信息
    /// </summary>
    public class OutMessage
    {
        #region 成员

        private OutMsgType m_msgType;
        public OutMsgType MsgType => m_msgType;

        private string m_msgContent;
        public string MsgContent => m_msgContent;

        private int m_msgID;
        public int MsgID => m_msgID;

        public OutMessage(string content, int msgID) : this(content, msgID, OutMsgType.Info)
        {

        }

        public OutMessage(string content, int msgID, OutMsgType msgType)
        {
            m_msgContent = content;
            m_msgType = msgType;
            m_msgID = msgID;
        }


        public override string ToString()
        {
            return $"MsgID:{MsgID} MsgType:{MsgType} MsgContent:{MsgContent}";
        }
        #endregion

    }

    public class OutMessages
    {
        private List<OutMessage> m_msgList = null;
        public List<OutMessage> MsgList { get { return m_msgList; } }

        private string m_msgTitle;
        public string MsgTitle => m_msgTitle;

        public OutMessages():this(string.Empty)
        {
        }

        public OutMessages(string title)
        {
            m_msgTitle = title;
            m_msgList = new List<OutMessage>();
        }


        public bool HasMsg()
        {
            return m_msgList != null && m_msgList.Count > 0;
        }

        public List<OutMessage> ErrorMsgs()
        {
            return GetMessages(OutMsgType.Error);
        }

        public List<OutMessage> WarningMsgs()
        {
            return GetMessages(OutMsgType.Warning);
        }

        public List<OutMessage> InfoMsgs()
        {
            return GetMessages(OutMsgType.Info);
        }

        public void AddMsg(string content)
        {
            AddMsg(content,OutMsgType.Info);
        }

        public void AddMsg(string content, OutMsgType type)
        {
            if(m_msgList == null)
            {
                m_msgList= new List<OutMessage>();
            }

            m_msgList.Add(new OutMessage(content, m_msgList.Count, type));
        }

        public List<OutMessage> GetMessages(OutMsgType type)
        {
            return m_msgList.Where((msg) => msg.MsgType == type).ToList();
        }

        public void Print()
        {
            Print(OutMsgType.All);
        }

        public void Print(OutMsgType type)
        {
            if (m_msgList == null)
            {
                return;
            }

            for (int i = 0; i < m_msgList.Count; i++)
            {
                var msg = m_msgList[i];
                if (msg == null)
                {
                    continue;
                }

                if(type != OutMsgType.All && type != msg.MsgType)
                {
                    continue;
                }

                string outContent = string.IsNullOrEmpty(MsgTitle) ? msg.MsgContent : $"[{MsgTitle}] {msg.MsgContent}";
                switch (msg.MsgType)
                {
                    case OutMsgType.Info:
                        Logger.Default.Info(outContent);
                        break;
                    case OutMsgType.Warning:
                        Logger.Default.Warning(outContent);
                        break;
                    case OutMsgType.Error:
                        Logger.Default.Warning(outContent);
                        break;
                }
            }
        }
    }

}
