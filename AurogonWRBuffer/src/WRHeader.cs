using System;
using AurogonTools;

namespace AurogonWRBuffer
{
	public class WRHeader:IPackage
	{
		private int m_size = 0;
		public int Size => m_size;

		private int m_count = 0;
		public int Count => m_count;

        public WRHeader(int size, int count)
        {
            m_size = size + 8;
            m_count = count;
        }


        public WRErrorType Pack(ref WriteBuffer writer)
        {
            if(writer == null)
            {
                Logger.Default.Error("WRHeader:Pack writer is null");
                return WRErrorType.PACKOBJECT_WRITER_IS_NULL;
            }

            WRErrorType ret = writer.WriteInt32(Size);
            if(ret != WRErrorType.NO_ERROR)
            {
                Logger.Default.Error($"WRHeader:write size error,size:{Size}");
                return ret;
            }

            ret = writer.WriteInt32(Count);
            if(ret != WRErrorType.NO_ERROR)
            {
                Logger.Default.Error($"WRHeader:write count error,count:{Count}");
                return ret;
            }

            return WRErrorType.NO_ERROR;
        }

        public WRErrorType UnPack(ref ReadBuffer reader)
        {
            if (reader == null)
            {
                Logger.Default.Error("WRHeader:Pack reader is null");
                return WRErrorType.UNPACKOBJECT_READER_IS_NULL;
            }

            WRErrorType ret = reader.ReadInt32(ref m_size);
            if(ret != WRErrorType.NO_ERROR)
            {
                Logger.Default.Error("WRHeader:UnPack read size is error");
                return ret;
            }

            ret = reader.ReadInt32(ref m_count);
            if(ret != WRErrorType.NO_ERROR)
            {
                Logger.Default.Error("WRHeader:Unpack read count is error");
                return ret;
            }

            return WRErrorType.NO_ERROR;
        }
    }
}

