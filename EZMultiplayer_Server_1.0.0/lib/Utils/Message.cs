using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace NetworkLib.Utils
{
    public class Message
    {
        protected byte[] _data;
        protected int _position;

        private int _maxLength;
        private readonly bool _autoResize;
        private static Message _instance;
        private string firstData;

        public int Capacity
        {
            get { return _data.Length; }
        }

        public Message()
        {
            _maxLength = 64;
            _data = new byte[_maxLength];
            _autoResize = true;
        }

        public Message(bool autoResize)
        {
            _maxLength = 64;
            _data = new byte[_maxLength];
            _autoResize = autoResize;
        }

        public Message(bool autoResize, int initialSize)
        {
            _maxLength = initialSize;
            _data = new byte[_maxLength];
            _autoResize = autoResize;
        }



        public void ResizeIfNeed(int newSize)
        {
            if (_maxLength < newSize)
            {
                while (_maxLength < newSize)
                {
                    _maxLength *= 2;
                }
                Array.Resize(ref _data, _maxLength);
            }
        }

        public void Reset(int size)
        {
            ResizeIfNeed(size);
            _position = 0;
            firstData = null;
        }

        public void Reset()
        {
            _position = 0;
            firstData = null;
        }

        public byte[] CopyData()
        {
            byte[] resultData = new byte[_position];
            Buffer.BlockCopy(_data, 0, resultData, 0, _position);
            return resultData;
        }

        public byte[] Data
        {
            get { return _data; }
        }

        public int Length
        {
            get { return _position; }
        }
        public static Message FromString(string value)
        {
            var netDataWriter = new Message();
            netDataWriter.Add(value);
            return netDataWriter;
        }

        public void Add(float value)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + 4);
            FastBitConverter.GetBytes(_data, _position, value);
            _position += 4;
        }

        public void Add(double value)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + 8);
            FastBitConverter.GetBytes(_data, _position, value);
            _position += 8;
        }

        public void Add(long value)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + 8);
            FastBitConverter.GetBytes(_data, _position, value);
            _position += 8;
        }

        public void Add(ulong value)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + 8);
            FastBitConverter.GetBytes(_data, _position, value);
            _position += 8;
        }

        public void Add(int value)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + 4);
            FastBitConverter.GetBytes(_data, _position, value);
            _position += 4;
        }

        public void Add(uint value)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + 4);
            FastBitConverter.GetBytes(_data, _position, value);
            _position += 4;
        }

        public void Add(char value)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + 2);
            FastBitConverter.GetBytes(_data, _position, value);
            _position += 2;
        }

        public void Add(ushort value)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + 2);
            FastBitConverter.GetBytes(_data, _position, value);
            _position += 2;
        }

        public void Add(short value)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + 2);
            FastBitConverter.GetBytes(_data, _position, value);
            _position += 2;
        }

        public void Add(sbyte value)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + 1);
            _data[_position] = (byte)value;
            _position++;
        }

        public void Add(byte value)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + 1);
            _data[_position] = value;
            _position++;
        }

        public void Type(string value)
        {
            if (firstData == null)
            {
                firstData = value;
            }

            Add(value);
        }
        public void Add(byte[] data, int offset, int length)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + length);
            Buffer.BlockCopy(data, offset, _data, _position, length);
            _position += length;
        }

        public void Add(byte[] data)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + data.Length);
            Buffer.BlockCopy(data, 0, _data, _position, data.Length);
            _position += data.Length;
        }

        public void AddBytesWithLength(byte[] data, int offset, int length)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + length + 4);
            FastBitConverter.GetBytes(_data, _position, length);
            Buffer.BlockCopy(data, offset, _data, _position + 4, length);
            _position += length + 4;
        }

        public void AddBytesWithLength(byte[] data)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + data.Length + 4);
            FastBitConverter.GetBytes(_data, _position, data.Length);
            Buffer.BlockCopy(data, 0, _data, _position + 4, data.Length);
            _position += data.Length + 4;
        }

        public void Add(bool value)
        {
            InitMsg();
            if (_autoResize)
                ResizeIfNeed(_position + 1);
            _data[_position] = (byte)(value ? 1 : 0);
            _position++;
        }

        public void Add(Room value)
        {
            Add(value.name);
            Add(value.host.name);
            Add(value.password);
            Add(value.maxPlayers);
            AddArray((from item in value.players select item.name).ToArray());
        }

        public void AddArray(float[] value)
        {
            InitMsg();
            ushort len = value == null ? (ushort)0 : (ushort)value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 4 + 2);
            Add(len);
            for (int i = 0; i < len; i++)
            {
                Add(value[i]);
            }
        }

        public void AddArray(double[] value)
        {
            InitMsg();
            ushort len = value == null ? (ushort)0 : (ushort)value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 8 + 2);
            Add(len);
            for (int i = 0; i < len; i++)
            {
                Add(value[i]);
            }
        }

        public void AddArray(long[] value)
        {
            InitMsg();
            ushort len = value == null ? (ushort)0 : (ushort)value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 8 + 2);
            Add(len);
            for (int i = 0; i < len; i++)
            {
                Add(value[i]);
            }
        }

        public void AddArray(ulong[] value)
        {
            InitMsg();
            ushort len = value == null ? (ushort)0 : (ushort)value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 8 + 2);
            Add(len);
            for (int i = 0; i < len; i++)
            {
                Add(value[i]);
            }
        }

        public void AddArray(int[] value)
        {
            InitMsg();
            ushort len = value == null ? (ushort)0 : (ushort)value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 4 + 2);
            Add(len);
            for (int i = 0; i < len; i++)
            {
                Add(value[i]);
            }
        }

        public void AddArray(uint[] value)
        {
            InitMsg();
            ushort len = value == null ? (ushort)0 : (ushort)value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 4 + 2);
            Add(len);
            for (int i = 0; i < len; i++)
            {
                Add(value[i]);
            }
        }

        public void AddArray(ushort[] value)
        {
            InitMsg();
            ushort len = value == null ? (ushort)0 : (ushort)value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 2 + 2);
            Add(len);
            for (int i = 0; i < len; i++)
            {
                Add(value[i]);
            }
        }

        public void AddArray(short[] value)
        {
            InitMsg();
            ushort len = value == null ? (ushort)0 : (ushort)value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 2 + 2);
            Add(len);
            for (int i = 0; i < len; i++)
            {
                Add(value[i]);
            }
        }

        public void AddArray(bool[] value)
        {
            InitMsg();
            ushort len = value == null ? (ushort)0 : (ushort)value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len + 2);
            Add(len);
            for (int i = 0; i < len; i++)
            {
                Add(value[i]);
            }
        }

        public void AddArray(string[] value)
        {
            InitMsg();
            ushort len = value == null ? (ushort)0 : (ushort)value.Length;
            Add(len);
            for (int i = 0; i < len; i++)
            {
                Add(value[i]);
            }
        }

        public void AddArray(Room[] value)
        {
            Add(value.Count());
            AddArray((from item in value select item.name).ToArray());
            AddArray((from item in value select item.host.name).ToArray());
            AddArray((from item in value select item.maxPlayers).ToArray());
            AddArray((from item in value select item.isPrivate).ToArray());
            AddArray((from item in value select item.playersCount).ToArray());
        }

        public void AddArray(Player[] value)
        {
            Add(value.Count());
            AddArray((from item in value select item.name).ToArray());
        }

        public void AddArray(string[] value, int maxLength)
        {
            InitMsg();
            ushort len = value == null ? (ushort)0 : (ushort)value.Length;
            Add(len);
            for (int i = 0; i < len; i++)
            {
                Add(value[i], maxLength);
            }
        }

        public void Add(IPEndPoint endPoint)
        {
            InitMsg();
            Add(endPoint.Address.ToString());
            Add(endPoint.Port);
        }

        public void Add(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            InitMsg();
            //put bytes count
            int bytesCount = Encoding.UTF8.GetByteCount(value);
            if (_autoResize)
                ResizeIfNeed(_position + bytesCount + 4);
            Add(bytesCount);

            //put string
            Encoding.UTF8.GetBytes(value, 0, value.Length, _data, _position);
            _position += bytesCount;
        }

        public void Add(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                Add(0);
                return;
            }
            InitMsg();
            int length = value.Length > maxLength ? maxLength : value.Length;
            //calculate max count
            int bytesCount = Encoding.UTF8.GetByteCount(value);
            if (_autoResize)
                ResizeIfNeed(_position + bytesCount + 4);

            //put bytes count
            Add(bytesCount);

            //put string
            Encoding.UTF8.GetBytes(value, 0, length, _data, _position);

            _position += bytesCount;
        }
        void InitMsg()
        {
            try
            {
                if (firstData == null) { throw new Exception("В" + " Message " + "не передано имя метода,вызываемого на сервере!"); }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }


        }
        public static Message Create(string type, params object[] parameters)
        {
            try
            {
                var instance = new Message(true);
                instance.Type(type);
                foreach (object value in parameters)
                {
                    if (value is float) { instance.Add((float)value); }
                    if (value is double) { instance.Add((double)value); }
                    if (value is long) { instance.Add((long)value); }
                    if (value is ulong) { instance.Add((ulong)value); }
                    if (value is int) { instance.Add((int)value); }
                    if (value is uint) { instance.Add((uint)value); }
                    if (value is char) { instance.Add((char)value); }
                    if (value is ushort) { instance.Add((ushort)value); }
                    if (value is short) { instance.Add((short)value); }
                    if (value is sbyte) { instance.Add((sbyte)value); }
                    if (value is byte) { instance.Add((byte)value); }
                    if (value is short) { instance.Add((short)value); }
                    if (value is string) { instance.Add((string)value); }

                }
                return instance;
            }
            catch (Exception e) { Console.WriteLine(e.Message); return null; }

        }
    }
}


