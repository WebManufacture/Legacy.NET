// Класс-обертка для микроконтроллера ATMega16 и USB-драйвера V-USB (AVR-USB) Objective Development
// Подробности реализации - на сайте http://microsin.ru
// Copyright: (c) 2009 by Sergey Kukhtetskiy
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace MRS.Hardware.VUSB
{
    public unsafe class VusbManager
    {
        #region Интерфейс с libusb

        #region Константы
        
        public const int LIBUSB_PATH_MAX = 512;
        const string LIBUSB_NATIVE_LIBRARY = "libusb0.dll";

        // Тип запроса (bmRequestType) формируется побитовым "или" 
        // констант из трех групп ниже.
        const byte USB_ENDPOINT_IN          = 0x80;
        const byte USB_ENDPOINT_OUT         = 0x00;

        const byte USB_TYPE_STANDARD        = (0x00 << 5);
        const byte USB_TYPE_CLASS           = (0x01 << 5);
        const byte USB_TYPE_VENDOR          = (0x02 << 5);
        const byte USB_TYPE_RESERVED        = (0x03 << 5);

        const byte USB_RECIP_DEVICE         = 0x00;
        const byte USB_RECIP_INTERFACE      = 0x01;
        const byte USB_RECIP_ENDPOINT       = 0x02;
        const byte USB_RECIP_OTHER          = 0x03;

        // Коды стандартных запросов (bRequest) с номерами 0 - 12 зарезервированы 
        // в спецификации USB поэтому все пользовательские будут начинаться с RQ_USER_BEG
        const byte USB_RQ_USER_BEG = 0x0C;

        const int USB_RQ_IO_READ = 0x11;
        const int USB_RQ_IO_WRITE = 0x12;

        const int USB_CUSTOM_RQ_GET = 0x2;
        const int USB_CUSTOM_RQ_SET = 0x1;
        const int USB_CUSTOM_RQ_ECHO = 0x0;
        #endregion

        #region Структуры

        /* Оригинальная структура из usb.h
        struct usb_endpoint_descriptor {
            unsigned char  bLength;
            unsigned char  bDescriptorType;
            unsigned char  bEndpointAddress;
            unsigned char  bmAttributes;
            unsigned short wMaxPacketSize;
            unsigned char  bInterval;
            unsigned char  bRefresh;
            unsigned char  bSynchAddress;
            unsigned char *extra;	// Extra descriptors
            int extralen;
        }; */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct usb_endpoint_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public byte bEndpointAddress;
            public byte bmAttributes;
            public byte wMaxPacketSize;
            public byte bInterval;
            public byte bRefresh;
            public byte bSynchAddress;
            public byte* extra;	// Extra descriptors
            public int extralen;
        };

        /* Оригинальная структура  из usb.h
        struct usb_interface_descriptor {
            unsigned char  bLength;
            unsigned char  bDescriptorType;
            unsigned char  bInterfaceNumber;
            unsigned char  bAlternateSetting;
            unsigned char  bNumEndpoints;
            unsigned char  bInterfaceClass;
            unsigned char  bInterfaceSubClass;
            unsigned char  bInterfaceProtocol;
            unsigned char  iInterface;
            struct usb_endpoint_descriptor *endpoint;
            unsigned char *extra;	// Extra descriptors 
            int extralen;
        }; */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct usb_interface_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public byte bInterfaceNumber;
            public byte bAlternateSetting;
            public byte bNumEndpoints;
            public byte bInterfaceClass;
            public byte bInterfaceSubClass;
            public byte bInterfaceProtocol;
            public byte iInterface;
            public usb_endpoint_descriptor* endpoint;
            public byte* extra;	// Extra descriptors 
            public int extralen;
        };

        /* Оригинальная структура  из usb.h
        struct usb_interface {
            struct usb_interface_descriptor *altsetting;
            int num_altsetting;
        }; */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct usb_interface
        {
            public usb_interface_descriptor* altsetting;
            public int num_altsetting;
        };

        /* Оригинальная структура  из usb.h
        struct usb_config_descriptor {
            unsigned char  bLength;
            unsigned char  bDescriptorType;
            unsigned short wTotalLength;
            unsigned char  bNumInterfaces;
            unsigned char  bConfigurationValue;
            unsigned char  iConfiguration;
            unsigned char  bmAttributes;
            unsigned char  MaxPower;
            struct usb_interface *interface;
            unsigned char *extra;	// Extra descriptors
            int extralen;
        }; */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct usb_config_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public ushort wTotalLength;
            public byte bNumInterfaces;
            public byte bConfigurationValue;
            public byte iConfiguration;
            public byte bmAttributes;
            public byte MaxPower;
            public usb_interface* pinterface;
            public byte* extra;	// Extra descriptors
            public int extralen;
        };

        /*  Оригинальная структура  из usb.h
        struct usb_device_descriptor {
            unsigned char  bLength;
            unsigned char  bDescriptorType;
            unsigned short bcdUSB;
            unsigned char  bDeviceClass;
            unsigned char  bDeviceSubClass;
            unsigned char  bDeviceProtocol;
            unsigned char  bMaxPacketSize0;
            unsigned short idVendor;
            unsigned short idProduct;
            unsigned short bcdDevice;
            unsigned char  iManufacturer;
            unsigned char  iProduct;
            unsigned char  iSerialNumber;
            unsigned char  bNumConfigurations;
        }; */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct usb_device_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public ushort bcdUSB;
            public byte bDeviceClass;
            public byte bDeviceSubClass;
            public byte bDeviceProtocol;
            public byte bMaxPacketSize0;
            public ushort idVendor;
            public ushort idProduct;
            public ushort bcdDevice;
            public byte iManufacturer;
            public byte iProduct;
            public byte iSerialNumber;
            public byte bNumConfigurations;
        };

        /*  Оригинальная структура  из usb.h
        struct usb_device 
        {
            struct usb_device *next, *prev;
            char filename[LIBUSB_PATH_MAX];
            struct usb_bus *bus;
            struct usb_device_descriptor descriptor;
            struct usb_config_descriptor *config;
            void *dev;		// Darwin support
            unsigned char devnum;
            unsigned char num_children;
            struct usb_device **children;
        }; */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct usb_device
        {
            public usb_device* next;
            public usb_device* prev;
            public fixed byte filename[LIBUSB_PATH_MAX];
            public usb_bus* bus;
            public usb_device_descriptor descriptor;
            public usb_config_descriptor* config;
            public IntPtr dev;		// Darwin support
            public byte devnum;
            public byte num_children;
            public IntPtr children;
        };

        /* Оригинальная структура  из usb.h
        struct usb_bus 
        {
            struct usb_bus *next, *prev;
            char dirname[LIBUSB_PATH_MAX];
            struct usb_device *devices;
            unsigned long location;
            struct usb_device *root_dev;
        };
        */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct usb_bus
        {
            public usb_bus* next;
            public usb_bus* prev;
            public fixed byte filename[LIBUSB_PATH_MAX];
            public usb_device* devices;
            public uint location;
            public usb_device* root_dev;
        };

        /* Оригинальная структура  из usbi.h
        struct usb_dev_handle {
            int fd;
            struct usb_bus *bus;
            struct usb_device *device;
            int config;
            int interface;
            int altsetting; 
            void *impl_info; // Added by RMT so implementations can store other per-open-device data
        };*/
        internal struct usb_dev_handle
        {
            public int fd;
            public usb_bus* bus;
            public usb_device* device;
            public int config;
            public int iinterface;
            public int altsetting;
            public IntPtr impl_info; // Added by RMT so implementations can store other per-open-device data
        };
        #endregion

        #region Импорт функций libusb
        [DllImport(LIBUSB_NATIVE_LIBRARY)]
        internal static extern void usb_init();
        [DllImport(LIBUSB_NATIVE_LIBRARY)]
        internal static extern int usb_find_busses();
        [DllImport(LIBUSB_NATIVE_LIBRARY)]
        internal static extern int usb_find_devices();
        [DllImport(LIBUSB_NATIVE_LIBRARY)]
        internal static extern usb_bus* usb_get_busses();
        [DllImport(LIBUSB_NATIVE_LIBRARY)]
        internal static extern usb_dev_handle* usb_open(usb_device* dev);
        [DllImport(LIBUSB_NATIVE_LIBRARY)]
        internal static extern int usb_close(usb_dev_handle* dev);
        [DllImport(LIBUSB_NATIVE_LIBRARY)]
        internal static extern int usb_control_msg(usb_dev_handle* dev, int requesttype, int request,
                                                 int value, int index, byte[] bytes, int size, int timeout);
        #endregion

        #endregion

        #region Инкапсуляция микроконтроллера ATMega16

        #region Приватные члены
        
        private ushort vid, pid;
        private usb_dev_handle* handle = null;
        private usb_bus* bus;
        private usb_device* dev;
        private byte[] buffer = new byte[4];

        #endregion

        public static int RequestTimeout = 500;

        public VusbManager(ushort vid, ushort pid)
        {
            this.vid = vid; this.pid = pid;
            usb_init();
            usb_find_busses();
            usb_find_devices();
            for (bus = usb_get_busses(); bus != null && handle == null; bus = bus->next)
                for (dev = bus->devices; dev != null; dev = dev->next)
                    if (dev->descriptor.idVendor == vid && dev->descriptor.idProduct == pid)
                    {
                        handle = usb_open(dev);
                        break;
                    }
        }

        public bool IsOpen()
        {
            return handle != null;
        }

        public bool Close()
        {
            if (handle == null)
                return false;
            usb_close(handle);
            return true;
        }

        public byte SendMessage(int value)
        {
            buffer[0] = 0;
            usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_OUT,
                               USB_CUSTOM_RQ_SET, value, 0, buffer, 1, RequestTimeout);
            return buffer[0];
        }

        public byte GetMessage()
        {
            buffer[0] = 0;
            usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_IN,
                                USB_CUSTOM_RQ_GET, 0, 0, buffer, 1, RequestTimeout);
            return buffer[0];
        }

        #endregion
    }
}
