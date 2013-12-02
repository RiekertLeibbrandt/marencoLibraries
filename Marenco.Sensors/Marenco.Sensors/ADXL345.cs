using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Threading;

namespace Marenco.Sensors
{
    public class ADXL345
    {
        //ADXL345 Register Addresses
        const byte DEVID = 0x00;	//Device ID Register
        const byte THRESH_TAP = 0x1D;	//Tap Threshold
        const byte OFSX = 0x1E;	//X-axis offset
        const byte OFSY = 0x1F;	//Y-axis offset
        const byte OFSZ = 0x20;	//Z-axis offset
        const byte DURATION = 0x21;	//Tap Duration
        const byte LATENT = 0x22;	//Tap latency
        const byte WINDOW = 0x23;	//Tap window
        const byte THRESH_ACT = 0x24;	//Activity Threshold
        const byte THRESH_INACT = 0x25;	//Inactivity Threshold
        const byte TIME_INACT = 0x26;	//Inactivity Time
        const byte ACT_INACT_CTL = 0x27;	//Axis enable control for activity and inactivity detection
        const byte THRESH_FF = 0x28;	//free-fall threshold
        const byte TIME_FF = 0x29;	//Free-Fall Time
        const byte TAP_AXES = 0x2A;	//Axis control for tap/double tap
        const byte ACT_TAP_STATUS = 0x2B;	//Source of tap/double tap
        const byte BW_RATE = 0x2C;	//Data rate and power mode control
        const byte POWER_CTL = 0x2D;	//Power Control Register
        const byte INT_ENABLE = 0x2E;	//Interrupt Enable Control
        const byte INT_MAP = 0x2F;	//Interrupt Mapping Control
        const byte INT_SOURCE = 0x30;	//Source of interrupts
        const byte DATA_FORMAT = 0x31;	//Data format control
        const byte DATAX0 = 0x32;	//X-Axis Data 0
        const byte DATAX1 = 0x33;	//X-Axis Data 1
        const byte DATAY0 = 0x34;	//Y-Axis Data 0
        const byte DATAY1 = 0x35;	//Y-Axis Data 1
        const byte DATAZ0 = 0x36;	//Z-Axis Data 0
        const byte DATAZ1 = 0x37;	//Z-Axis Data 1
        const byte FIFO_CTL = 0x38;	//FIFO control
        const byte FIFO_STATUS = 0x39;	//FIFO status

        SPI.Configuration spiConfig;
        SPI spiBus;
        byte xOffset, yOffset, zOffset;
        byte[] valueLocations;
        byte[] values;

        public ADXL345(Cpu.Pin pinCS, uint Freq)
        {
            spiConfig = new SPI.Configuration(
                pinCS,
                false,             // SS-pin active state
                10,                 // The setup time for the SS port
                10,                 // The hold time for the SS port
                true,              // The idle state of the clock
                true,             // The sampling clock edge
                Freq,              // The SPI clock rate in KHz
                SPI_Devices.SPI1   // The used SPI bus (refers to a MOSI MISO and SCLK pinset)
            );

            spiBus = new SPI(spiConfig);
            spiBus.Write(new byte[] { DATA_FORMAT, 0x00 });
            spiBus.Write(new byte[] { POWER_CTL, 0x08 });

            setOffsets(0, 0, 0);

            valueLocations = new byte[6] { DATAX0 | 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00 };
            values = new byte[6];
        }

        public void setOffsets(byte x, byte y, byte z)
        {
            xOffset = x;
            yOffset = y;
            zOffset = z;

            spiBus.Write(new byte[] { OFSX, xOffset });        // Unique for each device.  Need to come up with a calibration scheme
            spiBus.Write(new byte[] { OFSY, yOffset });
            spiBus.Write(new byte[] { OFSZ, zOffset });
        }

        public void getValues(ref int x, ref int y, ref int z)
        {
            spiBus.WriteRead(valueLocations, values, 1);

            x = (short)(((ushort)values[1] << 8) | (ushort)values[0]);
            y = (short)(((ushort)values[3] << 8) | (ushort)values[2]);
            z = (short)(((ushort)values[5] << 8) | (ushort)values[4]);
        }

        public void setUpAccelRate(int input)
        {
            // First we set it to 100Hz, normal operation mode.
            byte rateData = 0x00;
            if (input == 100)
            {
                rateData = 0x0A;
            }
            if (input == 200)
            {
                rateData = 0x0B;
            }
            Thread.Sleep(100);
            spiBus.Write(new byte[] { BW_RATE, rateData });
            Thread.Sleep(100);
        }

        public void setUpInterrupt()
        {
            // This command sets int1 to fire when data are available. Only bit7 is set. First disable all ints, then set it up.
            spiBus.Write(new byte[] { INT_ENABLE, 0x00 });
            Thread.Sleep(100);
            // Now map the interrupt to int1 pin (0x00 sends all interrupts to int1 pin). 
            spiBus.Write(new byte[] { INT_MAP, 0x00 });
            Thread.Sleep(100);
            // Now set the interrupt.
            spiBus.Write(new byte[] { INT_ENABLE, 0x80 });
            Thread.Sleep(100);

        }

        public void clearInterrupt()
        {
            spiBus.WriteRead(valueLocations, values, 1);
        }

    }
}
