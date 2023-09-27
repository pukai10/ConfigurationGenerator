using CommandLineOption;

namespace ConfigurationGenerator
{

    public class ConfigurationSetting
    {
        [Option("P", "path", helpText = "配置转换文件路径", required = true)]
        public string ConfigConvertFilePath
        {
            get { return m_configConvertFilePath; }
            set { m_configConvertFilePath = value; }
        }

        [Option("h","help",helpText ="所有命令的说明")]
        public bool HelpText { get; set; }

        [Option("v","version",helpText ="显示当前软件的版本号")]
        public bool Version { get; set; }

        #region Default Value

        private string m_configConvertFilePath = "./GameConfigConvert.xml";

        #endregion

        public override string ToString()
        {
            return $"ConfigConvertFilePath:{ConfigConvertFilePath}";
        }
    }
}
