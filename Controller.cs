namespace PiFaceCADLibrary
{
    using System;
    using System.Threading;

    /// <summary>
    /// Controller to manage text on device
    /// </summary>
    public class Controller
    {
        private int bus = 0;
        private int chipSelect = 1;
        private uint hardwareAddr = 0;
        private int mcp23S17Fd = 0; // MCP23S17 SPI file descriptor

        private uint lcdPort = Mcp23s17Data.GPIOB;
		private uint switchPort = Mcp23s17Data.GPIOA;
        private uint curAddress = 0;
        private uint curEntryMode = 0;
        private uint curFunctionSet = 0;

        private uint curDisplayControl = 0;

        /// <summary>
        /// Opens the display to messages.
        /// </summary>
        /// <returns>Returns message</returns>
        public int Open()
        {
            this.OpenNoinit();
            
            //Set IO config
            uint ioconfig = Mcp23s17Data.BANK_OFF | Mcp23s17Data.INT_MIRROR_OFF | Mcp23s17Data.SEQOP_OFF
                            | Mcp23s17Data.DISSLW_OFF | Mcp23s17Data.HAEN_ON | Mcp23s17Data.ODR_OFF
                            | Mcp23s17Data.INTPOL_LOW;

            Libmcp23s17Wrapper.mcp23s17_write_reg(ioconfig, Mcp23s17Data.IOCON, this.hardwareAddr, this.mcp23S17Fd);
            
            // Set GPIO Port A as inputs (switches)
            Libmcp23s17Wrapper.mcp23s17_write_reg(0xff, Mcp23s17Data.IODIRA, this.hardwareAddr, this.mcp23S17Fd);
            Libmcp23s17Wrapper.mcp23s17_write_reg(0xff, Mcp23s17Data.GPPUA, this.hardwareAddr, this.mcp23S17Fd);

            // Set GPIO Port B as outputs (connected to HD44780)
            Libmcp23s17Wrapper.mcp23s17_write_reg(0x00, Mcp23s17Data.IODIRB, this.hardwareAddr, this.mcp23S17Fd);
            
            // enable interrupts
            Libmcp23s17Wrapper.mcp23s17_write_reg(0xFF, Mcp23s17Data.GPINTENA, this.hardwareAddr, this.mcp23S17Fd);
            this.Init();
            return this.mcp23S17Fd;
        }

        /// <summary>
        /// Opens the noinit.
        /// </summary>
        /// <returns>Opens noinit</returns>
        public int OpenNoinit()
        {
            // All PiFace Digital are connected to the same SPI bus, only need 1 fd.
            if ((this.mcp23S17Fd = Libmcp23s17Wrapper.mcp23s17_open(this.bus, this.chipSelect)) < 0)
            {
                return -1;
            }

            return this.mcp23S17Fd; // returns the fd in case user wants to use it
        }

        /// <summary>
        /// Backlight is turned on.
        /// </summary>
        public void BacklightOn()
        {
            this.SetBacklight(1);
        }


		/// <summary>
		/// Backlight is turned off.
		/// </summary>
		public void BacklightOff()
		{
			this.SetBacklight(0);
		}

        /// <summary>
        /// Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Returns the current address</returns>
        public uint Write(string message)
        {
            this.SendCommand(pifacecadData.LCD_SETDDRAMADDR | this.curAddress);

            foreach (var c in message)
            {
                if (c == '\n')
                {
                    this.SetCursor(0, 1);
                }
                else
                {
                    this.SendData(c);
                    this.curAddress++;
                }
            }

            return this.curAddress;
        }

        /// <summary>
        /// Writes a scrolling text.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ScrollingText(string message)
        {
            this.SendCommand(pifacecadData.LCD_SETDDRAMADDR | this.curAddress);

            foreach (var c in message)
            {
                if (c == '\n')
                {
                    this.SetCursor(0, 1);
                }
                else
                {
                    this.SendData(Convert.ToChar(" ")); //Laver mellemrum mellem tegnene
                    this.SendData(c); //Skriver data
                }
            }
        }

        /// <summary>
        /// Sets the cursor.
        /// </summary>
        /// <param name="col">The coloumn.</param>
        /// <param name="row">The row.</param>
        /// <returns>Returns the cursor at new setting</returns>
        public uint SetCursor(uint col, uint row)
        {
            col = Math.Max(0, Math.Min(col, (pifacecadData.LCD_RAM_WIDTH / 2) - 1));
            row = Math.Max(0, Math.Min(row, pifacecadData.LCD_MAX_LINES - 1));
            this.SetCursorAddress(this.ColRowToAddress(col, row));
            return this.curAddress;
        }

        /// <summary>
        /// Sets the cursor address.
        /// </summary>
        /// <param name="address">The address.</param>
        public void SetCursorAddress(uint address)
        {
            this.curAddress = address % pifacecadData.LCD_RAM_WIDTH;
            this.SendCommand(pifacecadData.LCD_SETDDRAMADDR | this.curAddress);
        }

        /// <summary>
        /// Sends the command to device.
        /// </summary>
        /// <param name="command">The command.</param>
        public void SendCommand(uint command)
        {
            this.SetRs(0);
            this.SendByte(command);
        }

        /// <summary>
        /// Pulses the enable for the display.
        /// </summary>
        public void PulseEnable()
        {
            this.SetEnable(1);
            this.SetEnable(0);
        }

        /// <summary>
        /// Clears the display.
        /// </summary>
        public void Clear()
        {
            this.SendCommand(pifacecadData.LCD_CLEARDISPLAY);
            Thread.Sleep(pifacecadData.DELAY_CLEAR_NS);
            this.curAddress = 0;
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            // disable interrupts if enabled
            uint intenb = Libmcp23s17Wrapper.mcp23s17_read_reg(Mcp23s17Data.GPINTENA, this.hardwareAddr, this.mcp23S17Fd);
            if (intenb == 0)
            {
                Libmcp23s17Wrapper.mcp23s17_write_reg(0, Mcp23s17Data.GPINTENA, this.hardwareAddr, this.mcp23S17Fd);
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init()
        {
            Thread.Sleep(pifacecadData.DELAY_SETUP_0_NS);
            Libmcp23s17Wrapper.mcp23s17_write_reg(0x3, this.lcdPort, this.hardwareAddr, this.mcp23S17Fd);
            this.PulseEnable();
            Thread.Sleep(pifacecadData.DELAY_SETUP_1_NS);
            Libmcp23s17Wrapper.mcp23s17_write_reg(0x3, this.lcdPort, this.hardwareAddr, this.mcp23S17Fd);
            this.PulseEnable();
            Thread.Sleep(pifacecadData.DELAY_SETUP_2_NS);
            Libmcp23s17Wrapper.mcp23s17_write_reg(0x3, this.lcdPort, this.hardwareAddr, this.mcp23S17Fd);
            this.PulseEnable();
            Libmcp23s17Wrapper.mcp23s17_write_reg(0x2, this.lcdPort, this.hardwareAddr, this.mcp23S17Fd);
            this.PulseEnable();
            this.curFunctionSet |= pifacecadData.LCD_4BITMODE | pifacecadData.LCD_2LINE | pifacecadData.LCD_5X8DOTS;
            this.SendCommand(pifacecadData.LCD_FUNCTIONSET | this.curFunctionSet);
            this.curDisplayControl |= pifacecadData.LCD_DISPLAYOFF | pifacecadData.LCD_CURSOROFF | pifacecadData.LCD_BLINKOFF;
            this.SendCommand(pifacecadData.LCD_DISPLAYCONTROL | this.curDisplayControl);
            this.Clear();
            this.curEntryMode |= pifacecadData.LCD_ENTRYLEFT | pifacecadData.LCD_ENTRYSHIFTDECREMENT;
            this.SendCommand(pifacecadData.LCD_ENTRYMODESET | this.curEntryMode);
            this.curDisplayControl |= pifacecadData.LCD_DISPLAYON | pifacecadData.LCD_CURSORON | pifacecadData.LCD_BLINKON;
            this.SendCommand(pifacecadData.LCD_DISPLAYCONTROL | this.curDisplayControl);
        }


		/// <summary>
		/// Reads the entire switch port.
		/// </summary>
		/// <returns>Returns the status of the switches</returns>
		public uint ReadSwitches()
		{
			return Libmcp23s17Wrapper.mcp23s17_read_reg(this.switchPort, this.hardwareAddr, this.mcp23S17Fd);
		}

		/// <summary>
		/// Reads the entire switch port.
		/// </summary>
		/// <returns>Returns the status of the "switch_numm"switch</returns>
		public uint ReadSwitch(int switch_num)
		{
			return (Libmcp23s17Wrapper.mcp23s17_read_reg(this.switchPort, this.hardwareAddr, this.mcp23S17Fd)>> switch_num) & 1;
		
		}



        /// <summary>
        /// Coloumns the row to address.
        /// </summary>
        /// <param name="col">The coloumn.</param>
        /// <param name="row">The row.</param>
        /// <returns>Returns the address</returns>
        private uint ColRowToAddress(uint col, uint row)
        {
            return col + pifacecadData.ROW_OFFSETS[row];
        }

        /// <summary>
        /// Sets the rs.
        /// </summary>
        /// <param name="state">The state.</param>
        private void SetRs(uint state)
        {
            Libmcp23s17Wrapper.mcp23s17_write_bit(state, pifacecadData.PIN_RS, this.lcdPort, this.hardwareAddr, this.mcp23S17Fd);
        }

        /// <summary>
        /// Sends the data.
        /// </summary>
        /// <param name="data">The data.</param>
        private void SendData(uint data)
        {
            this.SetRs(1);
            this.SendByte(data);

            //Thread.Sleep(pifacecadData.DELAY_SETTLE_NS);
        }





        /// <summary>
        /// Sets the backlight.
        /// </summary>
        /// <param name="state">The state of the backlight - ON or OFF</param>
        private void SetBacklight(uint state)
        {
            Libmcp23s17Wrapper.mcp23s17_write_bit(state, pifacecadData.PIN_BACKLIGHT, this.lcdPort, this.hardwareAddr, this.mcp23S17Fd);
        }

        /// <summary>
        /// Sets the enable.
        /// </summary>
        /// <param name="state">The state of LCD Display.</param>
        private void SetEnable(uint state)
        {
            Libmcp23s17Wrapper.mcp23s17_write_bit(state, pifacecadData.PIN_ENABLE, this.lcdPort, this.hardwareAddr, this.mcp23S17Fd);
        }

        /// <summary>
        /// Sends the byte.
        /// </summary>
        /// <param name="byteNumber">The byte number.</param>
        private void SendByte(uint byteNumber)
        {
            uint currentState = Libmcp23s17Wrapper.mcp23s17_read_reg(this.lcdPort, this.hardwareAddr, this.mcp23S17Fd);
            currentState &= 0xF0; // clear the data bits
            uint newByte = currentState | ((byteNumber >> 4) & 0xF); // set nibble
            Libmcp23s17Wrapper.mcp23s17_write_reg(newByte, this.lcdPort, this.hardwareAddr, this.mcp23S17Fd);
            this.PulseEnable();
            newByte = currentState | (byteNumber & 0xF); // set nibble
            Libmcp23s17Wrapper.mcp23s17_write_reg(newByte, this.lcdPort, this.hardwareAddr, this.mcp23S17Fd);
            this.PulseEnable();
        }



    }
}