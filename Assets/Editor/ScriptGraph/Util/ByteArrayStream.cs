using System;
using System.Text;

namespace ScriptGraph.Util
{
	public class ByteArrayStream
	{
		private byte[] _buffer;

		private int _position = 0;

		private byte[] _tempBuffer = new byte[8];

		public ByteArrayStream()
		{
		}

		public ByteArrayStream(int capacity)
		{
			_buffer = new byte[capacity];
		}

		public ByteArrayStream(byte[] buffer)
		{
			_buffer = buffer;
		}

		public byte[] GetBuffer()
		{
			return _buffer;
		}

		#region Read

		public bool ReadBool()
		{
			return _buffer[_position++] != 0;
		}

		public byte ReadByte()
		{
			return _buffer[_position++];
		}

		public int ReadInt()
		{
			return (_buffer[_position++] << 0) | (_buffer[_position++] << 8) | (_buffer[_position++] << 16) | (_buffer[_position++] << 24);
		}

		public float ReadFloat()
		{
			FloatInt fi = new FloatInt();

			fi.i = ReadInt();

			return fi.f;
		}

		public long ReadLong()
		{
			return ((long)_buffer[_position++] << 0) | ((long)_buffer[_position++] << 8) | ((long)_buffer[_position++] << 16) | ((long)_buffer[_position++] << 24) | ((long)_buffer[_position++] << 32) | ((long)_buffer[_position++] << 40) | ((long)_buffer[_position++] << 48) | ((long)_buffer[_position++] << 56);
		}

		public string ReadString()
		{
			int length = ReadInt();

			int prev_position = _position;

			_position += length;

			return Encoding.Unicode.GetString(_buffer, prev_position, length);
		}

		#endregion

		#region Write

		public void WriteBool(bool value)
		{
			_tempBuffer[0] = (byte)((value != false) ? 1 : 0);

			Write(_tempBuffer, 0, 1);
		}

		public void WriteByte(byte value)
		{
			_tempBuffer[0] = value;

			Write(_tempBuffer, 0, 1);
		}

		public void WriteInt(int value)
		{
			_tempBuffer[0] = (byte)value;
			_tempBuffer[1] = (byte)(value >> 8);
			_tempBuffer[2] = (byte)(value >> 16);
			_tempBuffer[3] = (byte)(value >> 24);

			Write(_tempBuffer, 0, 4);
		}

		public void WriteFloat(float value)
		{
			FloatInt fi = new FloatInt();

			fi.f = value;

			WriteInt(fi.i);
		}

		public void WriteLong(long value)
		{
			_tempBuffer[0] = (byte)value;
			_tempBuffer[1] = (byte)(value >> 8);
			_tempBuffer[2] = (byte)(value >> 16);
			_tempBuffer[3] = (byte)(value >> 24);
			_tempBuffer[4] = (byte)(value >> 32);
			_tempBuffer[5] = (byte)(value >> 40);
			_tempBuffer[6] = (byte)(value >> 48);
			_tempBuffer[7] = (byte)(value >> 56);

			Write(_tempBuffer, 0, 8);
		}

		public void WriteString(string value)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(value);

			WriteInt(bytes.Length);

			Write(bytes, 0, bytes.Length);
		}

		private void Write(byte[] buffer, int offset, int count)
		{
			ExtendCapacityIfNeed(count);

			if (count > 8)
			{
				Buffer.BlockCopy(buffer, offset, _buffer, _position, count);
			}
			else
			{
				for (int i = 0; i < count; i++)
				{
					_buffer[_position + i] = buffer[offset + i];
				}
			}

			_position += count;
		}

		private void ExtendCapacityIfNeed(int count)
		{
			int num = _position + count;

			if (num > _buffer.Length)
			{
				if (num < _buffer.Length * 2)
				{
					num = _buffer.Length * 2;
				}

				byte[] tmp = new byte[num];

				Buffer.BlockCopy(_buffer, 0, tmp, 0, _position);

				_buffer = tmp;
			}
		}

		#endregion
	}
}