﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    /// <summary>
    /// Počítá CRC32
    /// </summary>
    public class Crc32
    {
        uint[] table;

        /// <summary>
        /// Vypočítá CRC32 z bytů
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public uint ComputeChecksum(byte[] bytes)
        {
            uint crc = 0xffffffff;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
                crc = (uint)((crc >> 8) ^ table[index]);
            }
            return ~crc;
        }

        /// <summary>
        /// Vypočítá CRC32 z bytů
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] ComputeChecksumBytes(byte[] bytes)
        {
            return BitConverter.GetBytes(ComputeChecksum(bytes));
        }

        public Crc32()
        {
            uint poly = 0xedb88320;
            table = new uint[256];
            uint temp = 0;
            for (uint i = 0; i < table.Length; ++i)
            {
                temp = i;
                for (int j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                    {
                        temp = (uint)((temp >> 1) ^ poly);
                    }
                    else
                    {
                        temp >>= 1;
                    }
                }
                table[i] = temp;
            }
        }
    }
}
