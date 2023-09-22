using System;
using AurogonTools;

namespace AurogonWRBuffer
{
	public class WriteBuffer
	{
        private readonly byte[] m_buffer;
        public byte[] Buffer => m_buffer;

        private readonly int m_length;
        public int Length => m_length;

        private int m_position;
        public int Position => m_position;

        private bool m_isNetEndian;
        public bool IsNetEndian => m_isNetEndian;

        public WriteBuffer(int length)
        {
            m_length = length;
            m_buffer = new byte[m_length];
            m_position = 0;
            m_isNetEndian = true;
        }

        public int GetLeftLength()
        {
            return Position - Length;
        }

        public void DisableEndian()
        {
            m_isNetEndian = false;
        }


        public WRErrorType WriteInt8(sbyte src)
        {
            return WriteUInt8((byte)src);
        }

        public WRErrorType WriteUInt8(byte src)
        {
            if (m_buffer == null)
            {
                Logger.Default.Error("WriteBuffer: m_buffer is null");
                return WRErrorType.BUFF_IS_NULL;
            }

            int size = sizeof(byte);
            if (size > GetLeftLength())
            {
                Logger.Default.Error($"WriteBuffer: Buffer 剩余长度不足{size}字节");
                return WRErrorType.SHORT_BUF_FOR_WRITE;
            }

            m_buffer[m_position++] = src;
            return WRErrorType.NO_ERROR;
        }

        public WRErrorType WriteUInt16(ushort src)
        {
            return WriteInt16((short)src);
        }

        public WRErrorType WriteInt16(short src)
        {
            if (m_buffer == null)
            {
                Logger.Default.Error("WriteBuffer: m_buffer is null");
                return WRErrorType.BUFF_IS_NULL;
            }

            int size = sizeof(short);
            if (size > GetLeftLength())
            {
                Logger.Default.Error($"WriteBuffer: Buffer 剩余长度不足{size}字节");
                return WRErrorType.SHORT_BUF_FOR_WRITE;
            }

            if(m_isNetEndian || !BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < size; i++)
                {
                    m_buffer[m_position + i] = (byte)(src >> (8 * (size - i - 1)));
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    m_buffer[m_position + i] = (byte)(src >> (8 * i));
                }
            }

            m_position += size;
            return WRErrorType.NO_ERROR;
        }

        public WRErrorType WriteUInt32(uint src)
        {
            return WriteInt32((int)src);
        }

        public WRErrorType WriteInt32(int src)
        {
            if (m_buffer == null)
            {
                Logger.Default.Error("WriteBuffer: m_buffer is null");
                return WRErrorType.BUFF_IS_NULL;
            }

            int size = sizeof(int);
            if (size > GetLeftLength())
            {
                Logger.Default.Error($"WriteBuffer: Buffer 剩余长度不足{size}字节");
                return WRErrorType.SHORT_BUF_FOR_WRITE;
            }

            if (m_isNetEndian || !BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < size; i++)
                {
                    m_buffer[m_position + i] = (byte)(src >> (8 * (size - i - 1)));
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    m_buffer[m_position + i] = (byte)(src >> (8 * i));
                }
            }

            m_position += size;
            return WRErrorType.NO_ERROR;
        }

        public WRErrorType WriteUInt64(ulong src)
        {
            return WriteInt64((long)src);
        }

        public WRErrorType WriteInt64(long src)
        {
            if (m_buffer == null)
            {
                Logger.Default.Error("WriteBuffer: m_buffer is null");
                return WRErrorType.BUFF_IS_NULL;
            }

            int size = sizeof(long);
            if (size > GetLeftLength())
            {
                Logger.Default.Error($"WriteBuffer: Buffer 剩余长度不足{size}字节");
                return WRErrorType.SHORT_BUF_FOR_WRITE;
            }

            if (m_isNetEndian || !BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < size; i++)
                {
                    m_buffer[m_position + i] = (byte)(src >> (8 * (size - i - 1)));
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    m_buffer[m_position + i] = (byte)(src >> (8 * i));
                }
            }

            m_position += size;
            return WRErrorType.NO_ERROR;
        }

        public WRErrorType WriteFloat(float src)
        {
            int tmp = BitConverter.ToInt32(BitConverter.GetBytes(src), 0);
            return WriteInt32(tmp);
        }

        public WRErrorType WriteDouble(double src)
        {
            long tmp = BitConverter.DoubleToInt64Bits(src);
            return WriteInt64(tmp);
        }

        public WRErrorType WriteBytes(byte[] src,int length)
        {
            if (m_buffer == null)
            {
                Logger.Default.Error("WriteBuffer: m_buffer is null");
                return WRErrorType.BUFF_IS_NULL;
            }

            if (length > GetLeftLength())
            {
                Logger.Default.Error($"WriteBuffer: Buffer 剩余长度不足{length}字节");
                return WRErrorType.SHORT_BUF_FOR_WRITE;
            }

            System.Buffer.BlockCopy(src, 0, m_buffer, m_position, length);
            m_position += length;
            return WRErrorType.NO_ERROR;
        }

        public WRErrorType WriteBool(bool src)
        {
            byte[] tmp = BitConverter.GetBytes(src);
            return WriteBytes(tmp, tmp.Length);
        }

        public WRErrorType Write<T>(T src,ref WriteBuffer writer) where T :IPackage
        {
            return src.Pack(ref writer);
        }
    }
}

