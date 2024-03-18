using CommandLineOption;

namespace ConfigurationGenerator
{

    public class ConfigurationSetting
    {
        [Option("C", "config-file", helpText = "配置转换文件路径", required = true)]
        public string ConfigConvertFilePath
        {
            get { return m_configConvertFilePath; }
            set { m_configConvertFilePath = value; }
        }

        [Option("R","root-path",helpText ="配置文件根目录", required = true)]
        public string ConfigRootPath
        {
            get;
            set;
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
