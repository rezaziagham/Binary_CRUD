using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinCrud.Classes
{
    class Binary
    {
        //string to binary with input byte
        public string ToBinary(string Value, int MaxLength)
        {
            string bin = "";
            if (Value.Length != MaxLength)
            {
                int lenght = MaxLength - Value.Length;
                for (int i = 0; i < lenght; i++)
                {
                    Value = Value + " ";
                }
            }
            foreach (char c in Value)
                bin = bin + Convert.ToString(c, 2).PadLeft(8, '0');

            return bin;
        }
        //string to binary whitout maximum byte
        public string ToBinary(string Value)
        {
            string bin = "";
            foreach (char c in Value)
                bin = bin + Convert.ToString(c, 2).PadLeft(8, '0');

            return bin;
        }
        //long to binary with input byte
        public string ToBinary(long number, int MaxByte)
        {
            string value = Convert.ToString(number, 2);
            if (value.Length != MaxByte * 4)
            {
                int lenght = MaxByte * 4 - value.Length;
                for (int i = 0; i < lenght; i++)
                {
                    value = "0" + value;
                }
            }
            return value;
        }
        //long to binary whitout maximum byte
        public string ToBinary(long number)
        {
            string value = Convert.ToString(number, 2);
            return value;
        }
        //int to binary with input byte
        public string ToBinary(int number, int MaxByte)
        {
            string value = Convert.ToString(number, 2);
            if (value.Length != MaxByte * 4)
            {
                int lenght = MaxByte * 4 - value.Length;
                for (int i = 0; i < lenght; i++)
                {
                    value = "0" + value;
                }
            }
            return value;
        }
        //int to binary whitout maximum byte
        public string ToBinary(int number)
        {
            string value = Convert.ToString(number, 2);
            return value;
        }
        //binary to long
        public long ToLong(string binary)
        {
            return Convert.ToInt64(binary, 2);
        }
        //binary to int
        public int ToInt(string binary)
        {
            return Convert.ToInt32(binary, 2);
        }
        //binary to string
        public string ToString(string binary)
        {
            var list = new List<Byte>();

            for (int i = 0; i < binary.Length; i += 8)
            {
                String t = binary.Substring(i, 8);

                list.Add(Convert.ToByte(t, 2));
            }

            return Encoding.ASCII.GetString(list.ToArray()).Trim();
        }
    }
}
