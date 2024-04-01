using System;
using System.Collections.Generic;
using System.Text;
using AurogonXmlConvert;
using AurogonTools;
using System.IO;
using GameConfigurationMode;
using AurogonWRBuffer;
using System.Xml;
using AurogonExcel;

namespace ConfigurationGenerator
{
    public class ConfigurationConvert
    {
        private ConfigurationSetting m_setting = null;
        private GameConfigConvert m_gameConfigConvert = null;
        private OutMessages m_outMsg = null;
        private bool m_isInitDone = false;
        private ILogger m_logger = null;

        private Dictionary<string, ExcelReader> m_excelReaders = null;
        private Dictionary<string, ConfigMeta> m_metaDict = null;
        private List<ExcelSheetNode> m_needExportExcelList = null;
        private Dictionary<string, BaseClassInfo> m_classInfoDict = null;
        private List<string> m_stringList = null;

        public ConfigurationConvert(ConfigurationSetting setting)
        {
            m_setting = setting;
            m_outMsg = new OutMessages();
            m_isInitDone = false;
            m_excelReaders = new Dictionary<string, ExcelReader>();
            m_metaDict = new Dictionary<string, ConfigMeta>();
            m_stringList = new List<string>();
            m_logger = Logger.GetLogger(typeof(ConfigurationConvert),new LoggerSetting()
                {
                    logType = LogType.All,
                    logFileEnabled = true
                });
            Init();
            InitClassInfoDict();
        }

        private void Init()
        {
            if(m_setting == null)
            {
                AddMessage("ConfigurationSetting 为空,请检查启动参数是否正确", OutMsgType.Error);
                return;
            }

            string gameConfigConvertFilePath = m_setting.ConfigRootPath.ConcatPath(m_setting.ConfigConvertFilePath);
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

            m_logger.Debug(m_gameConfigConvert);

            m_isInitDone = true;
        }

        private void InitClassInfoDict()
        {
            m_classInfoDict = new Dictionary<string, BaseClassInfo>();
            m_classInfoDict.Add("byte", new BaseClassInfo("byte", 1, new List<string>() { "uint8","Byte" }, "0"));
            m_classInfoDict.Add("sbyte", new BaseClassInfo("sbyte", 1, new List<string>() { "int8" ,"SByte"}, "0"));
            m_classInfoDict.Add("ushort", new BaseClassInfo("ushort", 2, new List<string>() { "uint16", "UInt16" }, "0"));
            m_classInfoDict.Add("short", new BaseClassInfo("short", 2, new List<string>() { "int16", "Int16" }, "0"));
            m_classInfoDict.Add("uint", new BaseClassInfo("uint", 4, new List<string>() { "uint32" , "UInt32" }, "0"));
            m_classInfoDict.Add("int", new BaseClassInfo("int", 4, new List<string>() { "int32", "Int32" }, "0"));
            m_classInfoDict.Add("ulong", new BaseClassInfo("ulong", 8, new List<string>() { "uint64", "UInt64" }, "0"));
            m_classInfoDict.Add("long", new BaseClassInfo("long", 8, new List<string>() { "int64", "Int64" }, "0"));
            m_classInfoDict.Add("float", new BaseClassInfo("float", 4, null, false, "0"));
            m_classInfoDict.Add("double", new BaseClassInfo("double", 8, new List<string>() { "Double" }, false, "0"));
            m_classInfoDict.Add("bool", new BaseClassInfo("bool", 1, new List<string>() { "boolean" , "Boolean" }, false, "false"));
            m_classInfoDict.Add("string", new BaseClassInfo("string", 4, new List<string>() { "String"}, false, "0"));
        }

        public bool Convert()
        {
            if(m_isInitDone == false)
            {
                AddMessage("ConfigurationConvert 初始化失败", OutMsgType.Warning);
                return false;
            }

            ReadExcels();
            ReadMetas();

            m_needExportExcelList = new List<ExcelSheetNode>();

            if(m_gameConfigConvert.ConvertTree != null && m_gameConfigConvert.ConvertTree.ExcelNodes != null)
            {
                for (int i = 0; i < m_gameConfigConvert.ConvertTree.ExcelNodes.Count; i++)
                {
                    var excelNode = m_gameConfigConvert.ConvertTree.ExcelNodes[i];

                    m_needExportExcelList.AddRange(excelNode.SheetNodes);
                }
            }
            ExportExcel();

            ExportStringIDs();

            AddMessage("ConfigurationConvert 完成");
            return true;
        }


        private void ExportExcel()
        {
            Dictionary<string, List<ExportExcelRowInfo>> allExcelRowInfo = new Dictionary<string, List<ExportExcelRowInfo>>();
            foreach (var excelSheet in m_needExportExcelList)
            {
                ExcelReader reader = m_excelReaders[excelSheet.ExcelName];
                ConfigMeta meta = m_metaDict[excelSheet.MetaFile];
                Sheet sheet = reader[excelSheet.SheetName];
                var structNode = meta[excelSheet.StructName];
                var firstRow = sheet[0];

                List<ExportExcelRowInfo> rowInfoList = new List<ExportExcelRowInfo>();
                ConvertExportRowInfo(firstRow, structNode,meta,"",ref rowInfoList);

                if(allExcelRowInfo.ContainsKey(excelSheet.SheetName) == false)
                {
                    allExcelRowInfo.Add(excelSheet.SheetName, rowInfoList);
                }
                else
                {
                    m_logger.Error($"已经包含excelSheet,excel:{excelSheet.ExcelName} ,{excelSheet.SheetName}");
                }

                int totalSize = 0;
                for (int i = 0; i < rowInfoList.Count; i++)
                {
                    var info = rowInfoList[i];
                    if (info != null)
                    {
                        Debug(info.Type + ",size:" + GetTypeSize(info.Type));
                        totalSize += GetTypeSize(info.Type);
                    }
                }

                Debug($"{excelSheet.SheetName}:{totalSize}");


                XmlDocument document = new XmlDocument();
                XmlDeclaration xmlDec = document.CreateXmlDeclaration("1.0", "utf-8", "yes");
                document.AppendChild(xmlDec);
                XmlElement rootNode = document.CreateElement(excelSheet.StructName);

                int totalCount = sheet.RowNum - 1;
                WRHeader header = new WRHeader(totalCount * totalSize, totalCount);
                WriteBuffer writer = new WriteBuffer(header.Size);
                WRErrorType errorType = writer.Write<WRHeader>(header,ref writer);

                if(errorType != WRErrorType.NO_ERROR)
                {
                    m_logger.Error("Writer Header error:" + errorType);
                    break;
                }

                for (int i = 1; i <= totalCount; i++)
                {
                    var row = sheet[i];
                    for (int j = 0; j < rowInfoList.Count; j++)
                    {
                        var rowInfo = rowInfoList[j];


                        string type = GetTypeName(rowInfo.Type);
                        string data = GetBaseTypeDefaultValue(type, row[rowInfo.Index].CellValue);
                        errorType = WriteType(writer, type, data);

                        if(errorType != WRErrorType.NO_ERROR)
                        {
                            m_logger.Error($"write row data is error,excel:{excelSheet.ExcelName},sheet:{excelSheet.SheetName}");
                            return;
                        }

                        XmlElement node = document.CreateElement(rowInfo.Name);
                        node.InnerText = data;
                        rootNode.AppendChild(node);
                    }
                }
                document.AppendChild(rootNode);


                string exportMetaXmlPath = m_setting.ConfigRootPath.ConcatPath(m_gameConfigConvert.ExportFilePath,$"{excelSheet.StructName}.xml");
                document.Save(exportMetaXmlPath);

                Debug("导出meta xml 成功，flie:" + exportMetaXmlPath);

                string exportBinFile = m_setting.ConfigRootPath.ConcatPath(m_gameConfigConvert.ExportFilePath, $"{excelSheet.BinaryFile}");


                IOHelper.SaveFile(exportBinFile, writer.Buffer);

                Debug("导出bin成功，flie:" + exportMetaXmlPath);
            }
        }

        private void ExportStringIDs()
        {
            if(m_stringList == null)
            {
                return;
            }

            int totalCount = m_stringList.Count;
            int totalSize = 0;

            XmlDocument document = new XmlDocument();
            XmlDeclaration xmlDec = document.CreateXmlDeclaration("1.0", "utf-8", "yes");
            document.AppendChild(xmlDec);
            XmlElement rootNode = document.CreateElement("root");

            List<StringIDInfo> infoList = new List<StringIDInfo>();
            for (int i = 0; i < m_stringList.Count; i++)
            {
                StringIDInfo info = new StringIDInfo();
                info.key = (uint)(i + 1);
                byte[] data = Encoding.UTF8.GetBytes(m_stringList[i]);
                info.length = data == null ? 0 : data.Length;
                info.data = data;
                infoList.Add(info);
                totalSize += sizeof(int) + sizeof(uint) + info.length;

                XmlElement node = document.CreateElement("StringIDInfo");

                XmlAttribute attributeKey = document.CreateAttribute("key");
                attributeKey.Value = $"{info.key}";
                XmlAttribute attributeLength = document.CreateAttribute("length");
                attributeLength.Value = $"{info.length}";
                XmlAttribute attributeValue = document.CreateAttribute("data");
                attributeValue.Value = $"{m_stringList[i]}";

                node.Attributes.Append(attributeKey);
                node.Attributes.Append(attributeLength);
                node.Attributes.Append(attributeValue);
                rootNode.AppendChild(node);
            }

            document.AppendChild(rootNode);

            WRHeader header = new WRHeader(totalSize, totalCount);
            WriteBuffer writer = new WriteBuffer(header.Size);
            WRErrorType errorType = writer.Write<WRHeader>(header, ref writer);
            if(errorType != WRErrorType.NO_ERROR)
            {
                m_logger.Error($"write stringid header error:{errorType}");
                return;
            }

            for (int i = 0; i < infoList.Count; i++)
            {
                var info = infoList[i];
                errorType = writer.WriteUInt32(info.key);
                if(errorType != WRErrorType.NO_ERROR)
                {
                    m_logger.Error($"write string info.key error,index:{i},key:{info.key}");
                    return;
                }

                errorType = writer.WriteInt32(info.length);

                if (errorType != WRErrorType.NO_ERROR)
                {
                    m_logger.Error($"write string info.length error,index:{i},length:{info.length}");
                    return;
                }

                errorType = writer.WriteBytes(info.data, info.length);

                if (errorType != WRErrorType.NO_ERROR)
                {
                    m_logger.Error($"write string info.data error,index:{i},data:{Encoding.UTF8.GetString(info.data)}");
                    return;
                }
            }



            string exportXmlPath = m_setting.ConfigRootPath.ConcatPath(m_gameConfigConvert.ExportFilePath, $"StringIDs.xml");
            document.Save(exportXmlPath);

            Debug("导出stringid xml 成功，flie:" + exportXmlPath);

            string exportStringIDBinFile = m_setting.ConfigRootPath.ConcatPath(m_gameConfigConvert.ExportFilePath, $"StringIDs.bytes");


            IOHelper.SaveFile(exportStringIDBinFile, writer.Buffer);

            Debug("导出stringid bin成功，flie:" + exportStringIDBinFile);
        }

        private void ConvertExportRowInfo(Cell[] firstRow, ConfigStruct configStruct, ConfigMeta meta,string prefixName,ref List<ExportExcelRowInfo> rowInfoList)
        {

            Func<string, Cell> findCell = (name) =>
            {
                foreach (var cell in firstRow)
                {
                    if(cell.CellValue == name)
                    {
                        return cell;
                    }
                }

                return null;
            };

            foreach (var property in configStruct.Properties)
            {
                int count = string.IsNullOrEmpty(property.Count) ? 0 : (int.TryParse(property.Count, out int _) ? int.Parse(property.Count) : meta.GetConstValue(property.Count));
                bool isBaseType = IsBaseType(property.PropertyType);
                if(!isBaseType)
                {
                    if(count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            ConvertExportRowInfo(firstRow, meta[property.PropertyType], meta,$"{prefixName}{property.CName}{(i + 1)}",ref rowInfoList);
                        }
                    }
                    else
                    {
                        ConvertExportRowInfo(firstRow, meta[property.PropertyType], meta, $"{prefixName}{property.CName}",ref rowInfoList);
                    }
                }
                else
                {
                    if(count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            ExportExcelRowInfo info = new ExportExcelRowInfo();
                            info.Description = property.Desc;
                            info.Type = property.PropertyType;
                            info.TitleName = $"{prefixName}{property.CName}{(i + 1)}";
                            info.Name = property.PropertyName;
                            Cell cell = findCell(info.TitleName);
                            if (cell == null)
                            {
                                m_logger.Debug($"找不到对应表中的列，:{info.TitleName}");
                                continue;
                            }
                            info.Index = cell.ColumnIndex;

                            m_logger.Debug($"{info.TitleName}:{info.ToString()}");
                            rowInfoList.Add(info);
                        }
                    }
                    else
                    {
                        ExportExcelRowInfo info = new ExportExcelRowInfo();
                        info.Description = property.Desc;
                        info.Type = property.PropertyType;
                        info.TitleName = $"{prefixName}{property.CName}";
                        info.Name = property.PropertyName;
                        Cell cell = findCell(info.TitleName);
                        if(cell == null)
                        {
                            m_logger.Debug($"找不到对应表中的列，:{info.TitleName}");
                            continue;
                        }
                        info.Index = cell.ColumnIndex;

                        m_logger.Debug($"{info.TitleName}:{info.ToString()}");
                        rowInfoList.Add(info);
                    }
                }

            }
        }


        /// <summary>
        /// 读取excel
        /// </summary>
        private void ReadExcels()
        {
            string excelRootPath = m_setting.ConfigRootPath.ConcatPath(m_gameConfigConvert.ExcelFilesPath);

            m_logger.Debug("excelRootPath:" + excelRootPath);
            FileInfo[] excelFiles = IOHelper.GetAllFileInfos(excelRootPath);

            for (int i = 0; i < excelFiles.Length; i++)
            {
                m_logger.Debug("excelFile:" + excelFiles[i].Name);

                var fileInfo = excelFiles[i];

                if(fileInfo.FullName.Contains(".DS_Store"))
                {
                    continue;
                }

                ExcelReader reader = new ExcelReader(fileInfo.FullName);
                m_excelReaders.Add(excelFiles[i].Name, reader);
                m_logger.Debug(reader.ToString());
            }
        }

        /// <summary>
        /// 读取meta
        /// </summary>
        private void ReadMetas()
        {
            string metaRootPath = m_setting.ConfigRootPath.ConcatPath(m_gameConfigConvert.MetaFilesPath);

            m_logger.Debug("metaRootPath:" + metaRootPath);

            FileInfo[] metaFiles = IOHelper.GetAllFileInfos(metaRootPath);


            for (int i = 0; i < metaFiles.Length; i++)
            {
                var fileInfo = metaFiles[i];
                if (fileInfo.FullName.Contains(".DS_Store"))
                {
                    continue;
                }

                m_logger.Debug("metaFile:" + fileInfo.Name);
                var configMeta = XmlUtility.FromXml<ConfigMeta>(fileInfo.FullName);
                m_logger.Debug(configMeta.ToString());

                if(m_metaDict.ContainsKey(fileInfo.Name) == false)
                {
                    m_metaDict.Add(fileInfo.Name, configMeta);
                }
                else
                {
                    m_logger.Error("已经存在相同名字的：" + fileInfo.Name);
                }
            }

        }


        private WRErrorType WriteType(WriteBuffer writer, string typeName, string data)
        {
            string type = GetTypeName(typeName);
            data = GetBaseTypeDefaultValue(type,data);
            switch (type)
            {
                case "byte":
                    if(byte.TryParse(data,out byte byteValue))
                    {
                        return writer.WriteUInt8(byteValue);
                    }
                    else
                    {
                        throw new Exception($"write {type} value is error,data:{data}");
                    }
                case "sbyte":
                    if (sbyte.TryParse(data, out sbyte sbyteValue))
                    {
                        return writer.WriteInt8(sbyteValue);
                    }
                    else
                    {
                        throw new Exception($"write {type} value is error,data:{data}");
                    }
                case "ushort":
                    if (ushort.TryParse(data, out ushort ushortValue))
                    {
                        return writer.WriteUInt16(ushortValue);
                    }
                    else
                    {
                        throw new Exception($"write {type} value is error,data:{data}");
                    }
                case "short":
                    if (short.TryParse(data, out short shortValue))
                    {
                        return writer.WriteInt16(shortValue);
                    }
                    else
                    {
                        throw new Exception($"write {type} value is error,data:{data}");
                    }
                case "uint":
                    if (uint.TryParse(data, out uint uintValue))
                    {
                        return writer.WriteUInt32(uintValue);
                    }
                    else
                    {
                        throw new Exception($"write {type} value is error,data:{data}");
                    }
                case "int":
                    if (int.TryParse(data, out int intValue))
                    {
                        return writer.WriteInt32(intValue);
                    }
                    else
                    {
                        throw new Exception($"write {type} value is error,data:{data}");
                    }
                case "ulong":
                    if (ulong.TryParse(data, out ulong ulongValue))
                    {
                        return writer.WriteUInt64(ulongValue);
                    }
                    else
                    {
                        throw new Exception($"write {type} value is error,data:{data}");
                    }
                case "long":
                    if (long.TryParse(data, out long longValue))
                    {
                        return writer.WriteInt64(longValue);
                    }
                    else
                    {
                        throw new Exception($"write {type} value is error,data:{data}");
                    }
                case "float":
                    if (float.TryParse(data, out float floatValue))
                    {
                        return writer.WriteFloat(floatValue);
                    }
                    else
                    {
                        throw new Exception($"write {type} value is error,data:{data}");
                    }
                case "double":
                    if (double.TryParse(data, out double doubleValue))
                    {
                        return writer.WriteDouble(doubleValue);
                    }
                    else
                    {
                        throw new Exception($"write {type} value is error,data:{data}");
                    }
                case "bool":
                    if (bool.TryParse(data, out bool boolValue))
                    {
                        return writer.WriteBoolean(boolValue);
                    }
                    else
                    {
                        throw new Exception($"write {type} value is error,data:{data}");
                    }
                case "string":
                    uint id = GetStringID(data);
                    return writer.WriteUInt32(id);
                default:
                    throw new Exception($"write {type} must is base type,data:{data}");
            }
        }

        private string GetBaseTypeDefaultValue(string typeName,string data)
        {
            if(!string.IsNullOrEmpty(data) || typeName == "string")
            {
                return data;
            }

            switch(typeName)
            {
                case "byte":
                case "sbyte":
                case "ushort":
                case "short":
                case "uint":
                case "int":
                case "ulong":
                case "long":
                case "float":
                case "double":
                    return "0";
                case "bool":
                    return "false";
                default:
                    throw new Exception($"type is not base type,{typeName}");
            }
        }

        /// <summary>
        /// 获取stringid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public uint GetStringID(string value)
        {
            if(m_stringList == null)
            {
                m_stringList = new List<string>();
            }

            if(m_stringList.Contains(value))
            {
                return (uint)(m_stringList.IndexOf(value) + 1);
            }

            m_stringList.Add(value);

            return (uint)m_stringList.Count;
        }

        /// <summary>
        /// 获取类型大小
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public int GetTypeSize(string typeName)
        {
            var info = GetClassInfo(typeName);
            if(info != null)
            {
                return info.Size;
            }

            return -1;
        }

        public string GetTypeName(string typeName)
        {
            var info = GetClassInfo(typeName);
            if(info != null)
            {
                return info.Name;
            }

            return string.Empty;
        }

        public BaseClassInfo GetClassInfo(string typeName)
        {
            foreach(var classInfo in m_classInfoDict.Values)
            {
                if(classInfo != null && classInfo.IsSelfType(typeName))
                {
                    return classInfo;
                }
            }

            return null;
        }

        public bool IsBaseType(string typeName)
        {
            var info = GetClassInfo(typeName);
            if(info != null)
            {
                return !info.IsCustom;
            }

            return false;

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

        private void Debug(object obj)
        {
            m_logger.Debug(obj);
        }
    }


}
