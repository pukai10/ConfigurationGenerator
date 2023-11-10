using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AurogonXmlConvert;
using AurogonTools;
using System.IO;

namespace ConfigurationGenerator
{
    public class ConfigurationConvert
    {
        private ConfigurationSetting m_setting = null;
        private GameConfigConvert m_gameConfigConvert = null;
        private OutMessages m_outMsg = null;
        private bool m_isInitDone = false;
        public ConfigurationConvert(ConfigurationSetting setting)
        {
            m_setting = setting;
            m_outMsg = new OutMessages();
            m_isInitDone = false;
            Init();
        }

        private void Init()
        {
            if(m_setting == null)
            {
                AddMessage("ConfigurationSetting 为空,请检查启动参数是否正确", OutMsgType.Error);
                return;
            }

            string gameConfigConvertFilePath = AppDomain.CurrentDomain.BaseDirectory + m_setting.ConfigConvertFilePath;
            if(File.Exists(gameConfigConvertFilePath) == false)
            {
                AddMessage("GameConfigConvert 文件不存在，路径:" + gameConfigConvertFilePath, OutMsgType.Error);
                return;
            }

            m_gameConfigConvert = XmlUtility.FromXml<GameConfigConvert>(gameConfigConvertFilePath.SystemPath());

            if(m_gameConfigConvert == null)
            {
                AddMessage("GameConfigConvert文件读取失败，路径:" + gameConfigConvertFilePath, OutMsgType.Error);
                return;
            }

            m_isInitDone = true;
        }

        public bool Convert()
        {
            if(m_isInitDone == false)
            {
                AddMessage("ConfigurationConvert 初始化失败", OutMsgType.Warning);
                return false;
            }


            AddMessage("ConfigurationConvert 完成");
            return true;
        }

        private void AddMessage(string content)
        {
            AddMessage(content, OutMsgType.Info);
        }

        private void AddMessage(string content,OutMsgType type)
        {
            if(m_outMsg == null)
            {
                m_outMsg = new OutMessages();
            }
            m_outMsg.AddMsg(content, type);
        }
    }
}
