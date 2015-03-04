using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiFaceCADLibrary
{
    static class pifacecadData
    {
        public static uint DELAY_PULSE_NS = 1; //1ms
        public static uint DELAY_SETTLE_NS = 1; //1ms
        public static int DELAY_CLEAR_NS = 3; // 3ms
        public static int DELAY_SETUP_0_NS = 15; // 15ms
        public static int DELAY_SETUP_1_NS = 5; // 5ms
        public static int DELAY_SETUP_2_NS = 1; // 1ms
        // mcp23s17 GPIOB to HD44780 pin map
        public static uint PIN_D4 = 0;
        public static uint PIN_D5 = 1;
        public static uint PIN_D6 = 2;
        public static uint PIN_D7 = 3;
        public static uint PIN_ENABLE = 4;
        public static uint PIN_RW = 5;
        public static uint PIN_RS = 6;
        public static uint PIN_BACKLIGHT = 7;
        // commands
        public static uint LCD_CLEARDISPLAY = 0x01;
        public static uint LCD_RETURNHOME = 0x02;
        public static uint LCD_ENTRYMODESET = 0x04;
        public static uint LCD_DISPLAYCONTROL = 0x08;
        public static uint LCD_CURSORSHIFT = 0x10;
        public static uint LCD_FUNCTIONSET = 0x20;
        public static uint LCD_SETCGRAMADDR = 0x40;
        public static uint LCD_SETDDRAMADDR = 0x80;
        public static uint LCD_NEWLINE = 0xC0;
        // flags for display entry mode
        public static uint LCD_ENTRYRIGHT = 0x00;
        public static uint LCD_ENTRYLEFT = 0x02;
        public static uint LCD_ENTRYSHIFTINCREMENT = 0x01;
        public static uint LCD_ENTRYSHIFTDECREMENT = 0x00;
        // flags for display on/off control
        public static uint LCD_DISPLAYON = 0x04;
        public static uint LCD_DISPLAYOFF = 0x00;
        public static uint LCD_CURSORON = 0x02;
        public static uint LCD_CURSOROFF = 0x00;
        public static uint LCD_BLINKON = 0x01;
        public static uint LCD_BLINKOFF = 0x00;
        // flags for display/cursor shift
        public static uint LCD_DISPLAYMOVE = 0x08;
        public static uint LCD_CURSORMOVE = 0x00;
        public static uint LCD_MOVERIGHT = 0x04;
        public static uint LCD_MOVELEFT = 0x00;
        // flags for function set
        public static uint LCD_8BITMODE = 0x10;
        public static uint LCD_4BITMODE = 0x00;
        public static uint LCD_2LINE = 0x08;
        public static uint LCD_1LINE = 0x00;
        public static uint LCD_5X10DOTS = 0x04;
        public static uint LCD_5X8DOTS = 0x00;
        public static uint LCD_MAX_LINES = 2;
        public static uint LCD_WIDTH = 16;
        public static uint LCD_RAM_WIDTH = 80; // RAM is 80 wide, split over two lines

        public static uint[] ROW_OFFSETS = {0, 0x40};
    }
}
