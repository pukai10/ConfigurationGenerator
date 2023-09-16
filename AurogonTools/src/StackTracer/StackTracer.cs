using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace AurogonTools
{
    public class StackTracer
    {
        private int m_offset;

        private List<string> m_stackTraceList;
        public string GetStackTrace(string separator)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m_stackTraceList.Count; i++)
            {
                sb.AppendLine($"{separator}{m_stackTraceList[i]}");
            }

            return sb.ToString();
        }

        public string StackTrace
        {
            get
            {
                return string.Join("\n", m_stackTraceList.ToArray());
            }
        }



        private int m_selfOffset = 2;

        public StackTracer(int offset)
        {
            m_offset = offset + m_selfOffset;
            Records(string.Empty);
        }

        public StackTracer(int offset,string filterMethods)
        {
            m_offset = offset + m_selfOffset;
            Records(filterMethods);
        }

        private void Records(string filterMethods)
        {
            m_stackTraceList = new List<string>();
            StackTrace stackTrace = new StackTrace(true);
            int frameCount = stackTrace.FrameCount;
            if(m_offset > frameCount)
            {
                throw new Exception($"堆栈偏移数({m_offset})大于堆栈总数({frameCount})了");
            }

            for(int i = m_offset;i < frameCount; i++)
            {
                StackFrame stackFrame = stackTrace.GetFrame(i);
                if(stackFrame.GetILOffset() == StackFrame.OFFSET_UNKNOWN)
                {
                    break;
                }

                var method = stackFrame.GetMethod();
                if(!string.IsNullOrEmpty(filterMethods) && filterMethods.Contains(method.Name))
                {
                    continue;
                }

                string fileName = stackFrame.GetFileName();

                ParameterInfo[] parameterInfos = method.GetParameters();
                StringBuilder parameters = new StringBuilder();
                if(parameterInfos != null)
                {
                    for (int j = 0; j < parameterInfos.Length; j++)
                    {
                        parameters.Append(parameterInfos[j].ParameterType.Name);
                        if( j != parameterInfos.Length - 1)
                        {
                            parameters.Append(",");
                        }
                    }
                }

                string content = (string.IsNullOrEmpty(fileName) == true) ?
                        $"{method.DeclaringType.Name}:{method.Name}({parameters.ToString()})" :
                        $"{method.DeclaringType.Name}:{method.Name}({parameters.ToString()})(at {fileName} {stackFrame.GetFileLineNumber()})";
                m_stackTraceList.Add(content);
            }
        }
    }
}
