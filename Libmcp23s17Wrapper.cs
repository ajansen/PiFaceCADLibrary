﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiFaceCADLibrary
{
    using System.Runtime.InteropServices;

    internal class Libmcp23s17Wrapper
    {
        [DllImport("libmcp23s17.so")]
        internal static extern int mcp23s17_open(int bus, int chip_select);

        [DllImport("libmcp23s17.so")]
        internal static extern void mcp23s17_write_reg(uint data, uint reg, uint hw_addr, int fd);

        [DllImport("libmcp23s17.so")]
        internal static extern void mcp23s17_write_bit(uint data, uint bit_num, uint reg, uint hw_addr, int fd);

        [DllImport("libmcp23s17.so")]
        internal static extern uint mcp23s17_read_reg(uint reg, uint hw_addr, int fd);
    }
}
