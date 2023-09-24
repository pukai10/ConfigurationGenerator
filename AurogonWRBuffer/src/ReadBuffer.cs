using System;
using AurogonTools;
using System.Collections.Generic;

namespace AurogonWRBuffer
{
	public class ReadBuffer
	{
		private readonly byte[] m_buffer;
		public byte[] Buffer => m_buffer;

		private readonly int m_length;
		public int Length => m_length;

		private int m_position;
		public int Position => m_position;

		private bool m_isNetEndian;
		public bool IsNetEndian => m_isNetEndian;

		public ReadBuffer(ref byte[] buffer,int length)
		{
			m_length = length;
			m_buffer = buffer;
			m_position = 0;
			m_isNetEndian = true;
		}

		public int GetLeftLength()
		{
			return Length - Position;
		}

		public void DisableNetEndian()
		{
			m_isNetEndian = false;
		}

		public WRErrorType ReadInt8(ref sbyte dest)
		{
			byte tmp = 0;
            WRErrorType ret = ReadUInt8(ref tmp);
			dest = (sbyte)tmp;
			return ret;
		}

		public WRErrorType ReadUInt8(ref byte dest)
		{
			if(m_buffer == null)
			{
				Logger.Default.Error("ReadBuffer: m_buffer is null");
				return WRErrorType.BUFF_IS_NULL;
			}

			if(sizeof(byte) > GetLeftLength())
			{
				Logger.Default.Error($"ReadBuffer: Buffer 剩余长度不足{sizeof(byte)}字节");
				return WRErrorType.SHORT_BUF_FOR_READ;
			}

			dest = m_buffer[m_position++];
			return WRErrorType.NO_ERROR;
		}

		public WRErrorType ReadUInt16(ref ushort dest)
		{
			short tmp = 0;
			WRErrorType ret = ReadInt16(ref tmp);
			dest = (ushort)tmp;
			return ret;
		}

		public WRErrorType ReadInt16(ref short dest)
		{
            if (m_buffer == null)
            {
                Logger.Default.Error("ReadBuffer: m_buffer is null");
                return WRErrorType.BUFF_IS_NULL;
            }

            int size = sizeof(short);
            if (size > GetLeftLength())
            {
                Logger.Default.Error($"ReadBuffer: Buffer 剩余长度不足{size}字节");
                return WRErrorType.SHORT_BUF_FOR_READ;
            }

			if(m_isNetEndian || !BitConverter.IsLittleEndian)
			{
				for (int i = 0; i < size; i++)
				{
					dest = (short)(dest | (short)(m_buffer[m_position + i] << 8 * (size - i - 1)));
				}
			}
			else
			{
				for (int i = 0; i < size; i++)
				{
					dest = (short)(dest | (short)(m_buffer[m_position + i] << 8 * i));
				}
			}

			m_position += size;
			return WRErrorType.NO_ERROR;
        }

		public WRErrorType ReadUInt32(ref uint dest)
		{
			int tmp = 0;
			WRErrorType ret = ReadInt32(ref tmp);
			dest = (uint)tmp;
			return ret;
		}

		public WRErrorType ReadInt32(ref int dest)
		{
            if (m_buffer == null)
            {
                Logger.Default.Error("ReadBuffer: m_buffer is null");
                return WRErrorType.BUFF_IS_NULL;
            }

            int size = sizeof(int);
            if (size > GetLeftLength())
            {
                Logger.Default.Error($"ReadBuffer: Buffer 剩余长度不足{size}字节");
                return WRErrorType.SHORT_BUF_FOR_READ;
            }

            if (m_isNetEndian || !BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < size; i++)
                {
                    dest = (int)(dest | (int)(m_buffer[m_position + i] << (8 * (size - i - 1))));
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    dest = (int)(dest | (int)(m_buffer[m_position + i] << (8 * i)));
                }
            }
            m_position += size;
            return WRErrorType.NO_ERROR;
        }

        public WRErrorType ReadUInt64(ref ulong dest)
        {
            long tmp = 0;
            WRErrorType ret = ReadInt64(ref tmp);
            dest = (ulong)tmp;
            return ret;
        }

        public WRErrorType ReadInt64(ref long dest)
        {
            if (m_buffer == null)
            {
                Logger.Default.Error("ReadBuffer: m_buffer is null");
                return WRErrorType.BUFF_IS_NULL;
            }

            int size = sizeof(long);
            if (size > GetLeftLength())
            {
                Logger.Default.Error($"ReadBuffer: Buffer 剩余长度不足{size}字节");
                return WRErrorType.SHORT_BUF_FOR_READ;
            }

            if (m_isNetEndian || !BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < size; i++)
                {
                    dest = (long)(dest | (long)(m_buffer[m_position + i] << (8 * (size - i - 1))));
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    dest = (long)(dest | (long)(m_buffer[m_position + i] << (8 * i)));
                }
            }
            m_position += size;
            return WRErrorType.NO_ERROR;
        }

        public WRErrorType ReadFloat(ref float dest)
        {
            int tmp = 0;
            WRErrorType ret = ReadInt32(ref tmp);
            dest = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);
            return ret;
        }

        public WRErrorType ReadDouble(ref double dest)
        {
            long tmp = 0;
            WRErrorType ret = ReadInt64(ref tmp);
            dest = BitConverter.Int64BitsToDouble(tmp);
            return ret;
        }

        public WRErrorType ReadBytes(ref byte[] dest,int length)
        {
            if (m_buffer == null)
            {
                Logger.Default.Error("ReadBuffer: m_buffer is null");
                return WRErrorType.BUFF_IS_NULL;
            }

            if (length > GetLeftLength())
            {
                Logger.Default.Error($"ReadBuffer: Buffer 剩余长度不足{length}字节");
                return WRErrorType.SHORT_BUF_FOR_READ;
            }

            System.Buffer.BlockCopy(m_buffer, m_position, dest, 0, length);
            m_position += length;
            return WRErrorType.NO_ERROR;
        }

        public WRErrorType ReadBool(ref bool dest)
        {
            byte[] tmp = BitConverter.GetBytes(dest);
            WRErrorType ret = ReadBytes(ref tmp, tmp.Length);
            dest = BitConverter.ToBoolean(tmp, 0);
            return ret;
        }


        public WRErrorType Read<T>(ref T t,ref ReadBuffer reader) where T:IPackage
        {
            return t.UnPack(ref reader);
        }
    }
}

