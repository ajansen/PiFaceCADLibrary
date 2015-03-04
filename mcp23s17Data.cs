namespace PiFaceCADLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Mcp23s17Data
    {
        public static uint WRITE_CMD = 0;
        public static uint READ_CMD = 1;
        // Register addresses
        public static uint IODIRA = 0x00;// I/O direction A
        public static uint IODIRB = 0x01; // I/O direction B
        public static uint IPOLA = 0x02; // I/O polarity A
        public static uint IPOLB = 0x03; // I/O polarity B
        public static uint GPINTENA = 0x04;// interupt enable A
        public static uint GPINTENB = 0x05; // interupt enable B
        public static uint DEFVALA = 0x06;// register default value A (interupts)
        public static uint DEFVALB = 0x07; // register default value B (interupts)
        public static uint INTCONA = 0x08; // interupt control A
        public static uint INTCONB = 0x09; // interupt control B
        public static uint IOCON = 0x0A;// I/O config (also 0x0B)
        public static uint GPPUA = 0x0C;// port A pullups
        public static uint GPPUB = 0x0D;// port B pullups
        public static uint INTFA = 0x0E;// interupt flag A (where the interupt came from)
        public static uint INTFB = 0x0F;// interupt flag B
        public static uint INTCAPA = 0x10;// interupt capture A (value at interupt is saved here)
        public static uint INTCAPB = 0x11; // interupt capture B
        public static uint GPIOA = 0x12; // port A
        public static uint GPIOB = 0x13; // port B
        public static uint OLATA = 0x14; // output latch A
        public static uint OLATB = 0x15; // output latch B
        // I/O config
        public static uint BANK_OFF = 0x00; // addressing mode
        public static uint BANK_ON = 0x80;
        public static uint INT_MIRROR_ON = 0x40; // interupt mirror (INTa|INTb)
        public static uint INT_MIRROR_OFF = 0x00;
        public static uint SEQOP_OFF = 0x20;// incrementing address pointer
        public static uint SEQOP_ON = 0x00;
        public static uint DISSLW_ON = 0x10; // slew rate
        public static uint DISSLW_OFF = 0x00;
        public static uint HAEN_ON = 0x08;// hardware addressing
        public static uint HAEN_OFF = 0x00;
        public static uint ODR_ON = 0x04;// open drain for interupts
        public static uint ODR_OFF = 0x00;
        public static uint INTPOL_HIGH = 0x02;// interupt polarity
        public static uint INTPOL_LOW = 0x00;
        public static uint GPIO_INTERRUPT_PIN = 25;
    }
}
