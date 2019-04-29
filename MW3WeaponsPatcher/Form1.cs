using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MW3WeaponsPatcher
{
    public partial class MainForm : Form
    {
        private static bool mw3Open = false;
        private static byte[][] deagleData = new byte[3][];
        private static IntPtr[] deagleDataPtrs = new IntPtr[3];
        private static byte[][] ak47Data = new byte[3][];
        private static IntPtr[] ak47DataPtrs = new IntPtr[3];
        private static IntPtr[] ak47CamoDataPtrs = new IntPtr[3];
        private static byte[][] m16Data = new byte[3][];
        private static IntPtr[] m16DataPtrs = new IntPtr[3];
        private static IntPtr[] m16CamoDataPtrs = new IntPtr[3];
        private static IntPtr[] m16AnimPtrs = new IntPtr[7];
        private static byte[][] f2000Data = new byte[3][];
        private static IntPtr[] f2000DataPtrs = new IntPtr[3];
        private static IntPtr[] f2000CamoDataPtrs = new IntPtr[3];
        private static IntPtr[] f2000AnimPtr = new IntPtr[2] { IntPtr.Zero, IntPtr.Zero };
        private static byte[] f2000AnimData = new byte[164];
        private static IntPtr[] f2000AnimOpticPtrs = new IntPtr[8];
        private static IntPtr[] f2000AnimOpticPtrsSecondary = new IntPtr[8];
        private static byte[][] f2000AnimOpticData = new byte[8][];
        private static IntPtr f2000LensTagPtr;
        private static byte f2000LensTag;
        private static IntPtr f2000NamePtr;
        private static byte[] f2000NameData;
        private static int f2000ImageData = 0;
        private static IntPtr[] augModelPtrs = new IntPtr[8];
        private static IntPtr[] l86ModelsPtr = new IntPtr[2];
        private static byte augModelTag = 0x0A;
        private static IntPtr f2000ImagePtr;
        private static IntPtr m4AnimPtr;
        private static byte[] m4AnimData = new byte[144];
        private static IntPtr m4GripPtr;
        private static byte m4GripData;
        private static IntPtr uspModelPtr;
        private static IntPtr uspModelTagPtr;
        private static byte[] uspModelData;
        private static byte uspModelTagData;
        private static IntPtr[] viewmodel_m4_ads = new IntPtr[2];//Reflex & acog
        private static IntPtr[] viewmodel_m16_hybrid = new IntPtr[2];//Ironsight
        private static IntPtr[] viewmodel_m16_reflex = new IntPtr[2];//EOtech
        private static IntPtr[] viewmodel_fn2000_ads_acog = new IntPtr[2];
        private static IntPtr viewmodel_cm901_ads_up;
        private static IntPtr[] viewmodel_sa80_optics = new IntPtr[2];
        private static IntPtr sa80AnimPtr;
        private static IntPtr sa80OpticAnimPtr;
        //private static IntPtr viewmodel_fn2000_ads_fire;
        private static IntPtr[] mp5Struct = new IntPtr[2] { IntPtr.Zero, IntPtr.Zero };
        private static IntPtr[] viewmodel_acr_ads = new IntPtr[2];
        private static Timer mw3Checker = new Timer();
        private static Process mw3Process = null;

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

        public MainForm()
        {
            InitializeComponent();
            ak47CamoTooltip.SetToolTip(ak47Camo, "Sets the Classic AK-47 model to the Classic Camo of the AK-47 instead of the base weapon");
            m16CamoTooltip.SetToolTip(m16Camo, "Sets the MW2 M16 model to the Classic Camo of the M16 instead of the base weapon");
            //Initialize data
            deagleData[0] = new byte[7] { 0x78, 0x41, 0x24, 0x31, 0x07, 0x01, 0x03 };
            deagleData[1] = new byte[44] { 0x98, 0x41, 0x24, 0x31, 0xA6, 0x41, 0x24, 0x31, 0xAC, 0x41, 0x24, 0x31, 0xDC, 0x41, 0x24, 0x31, 0x24, 0x42, 0x24, 0x31, 0x2C, 0x42, 0x24, 0x31, 0x0C, 0x43, 0x24, 0x31, 0x00, 0x00, 0x7A, 0x44, 0x03, 0x00, 0x00, 0x00, 0xA4, 0xB0, 0x4B, 0x01, 0x00, 0x00, 0x00, 0xE8 };
            deagleData[2] = new byte[4] { 0x40, 0x44, 0x24, 0x31 };
            ak47Data[0] = new byte[7] { 0xF0, 0x50, 0xDD, 0x23, 0x08, 0x01, 0x05 };//Modelname & part initializer
            ak47Data[1] = new byte[44]{ 0x00, 0x51, 0xDD, 0x23, 0x10, 0x51, 0xDD, 0x23, 0x18, 0x51, 0xDD, 0x23, 0x50, 0x51, 0xDD, 0x23, 0xA4, 0x51, 0xDD, 0x23, 0xAC, 0x51, 0xDD, 0x23, 0xAC, 0x52, 0xDD, 0x23, 0x00, 0x00, 0x7A, 0x44, 0x05, 0x00, 0x00, 0x00, 0xD4, 0x1D, 0x4B, 0x01, 0x00, 0x00, 0x00, 0xEC };//Model info
            ak47Data[2] = new byte[4] { 0xA8, 0x54, 0xDD, 0x23 };//viewmodel struct
            m16Data[0] = new byte[8] { 0xFC, 0xA3, 0x14, 0x24, 0x23, 0x01, 0x20, 0x01 };//Modelname & part initializer
            m16Data[1] = new byte[44] { 0x0A, 0xA4, 0x14, 0x24, 0x50, 0xA4, 0x14, 0x24, 0x72, 0xA4, 0x14, 0x24, 0x84, 0xA5, 0x14, 0x24, 0x1C, 0xA7, 0x14, 0x24, 0x40, 0xA7, 0x14, 0x24, 0xA0, 0xAB, 0x14, 0x24, 0x00, 0x00, 0x7A, 0x44, 0x05, 0x00, 0x00, 0x00, 0x60, 0x31, 0x4B, 0x01, 0xEA, 0xFF, 0xE9, 0xFF };//Model info
            m16Data[2] = new byte[4] { 0x34, 0xAE, 0x14, 0x24 };//viewmodel struct
            f2000Data[0] = new byte[8] { 0x64, 0xBE, 0xFF, 0x23, 0x22, 0x01, 0x21, 0x01 };//Modelname & part initializer
            f2000Data[1] = new byte[44] { 0x74, 0xBE, 0xFF, 0x23, 0xB8, 0xBE, 0xFF, 0x23, 0xDA, 0xBE, 0xFF, 0x23, 0xE4, 0xBF, 0xFF, 0x23, 0x70, 0xC1, 0xFF, 0x23, 0x94, 0xC1, 0xFF, 0x23, 0xD4, 0xC5, 0xFF, 0x23, 0x00, 0x00, 0x7A, 0x44, 0x05, 0x00, 0x00, 0x00, 0xE8, 0x2A, 0x4B, 0x01, 0xD4, 0xFF, 0xD7, 0xFF };//Model info
            f2000Data[2] = new byte[4] { 0x18, 0xDF, 0xFF, 0x23 };//viewmodel struct
            //m4AnimData = new byte[] { 0xD0, 0x25, 0x21, 0x25, 0xD0, 0x25, 0x21, 0x25, 0xEC, 0x2E, 0x21, 0x25, 0x4A, 0x80, 0x53, 0x23, 0xEC, 0x2E, 0x21, 0x25, 0x4A, 0x80, 0x53, 0x23, 0xD7, 0xEF, 0xC0, 0x23, 0x10, 0x0B, 0xC1, 0x23, 0x9F, 0x39, 0x21, 0x25, 0x5C, 0x78, 0x21, 0x25, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x54, 0xC5, 0x21, 0x25, 0x10, 0xE1, 0x21, 0x25, 0x4A, 0x80, 0x53, 0x23, 0xEB, 0x16, 0x22, 0x25, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0xE0, 0x3B, 0x22, 0x25, 0xEB, 0x16, 0x22, 0x25, 0x54, 0xC5, 0x21, 0x25, 0xEB, 0x16, 0x22, 0x25, 0xCA, 0x59, 0x22, 0x25, 0x41, 0x6C, 0x22, 0x25, 0x40, 0x81, 0x22, 0x25, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0xEC, 0x2E, 0x21, 0x25, 0xEC, 0x2E, 0x21, 0x25, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23 };
            m4GripData = 0x07;
            uspModelData = new byte[4] { 0x70, 0x5F, 0x8F, 0x25};
            uspModelTagData = 0x04;
            //f2000LensTagPtr = new IntPtr(0x23FFBEC4);
            f2000LensTag = 0x0F;
            //f2000AnimPtr = new IntPtr(0x338943D8);
            //f2000AnimData = new byte[] { 0x2C, 0xBB, 0xFE, 0x23, 0x2C, 0xBB, 0xFE, 0x23, 0xB0, 0xC4, 0xFE, 0x23, 0x4A, 0x80, 0x53, 0x23, 0xB0, 0xC4, 0xFE, 0x23, 0x4A, 0x80, 0x53, 0x23, 0xD7, 0xEF, 0xC0, 0x23, 0x10, 0x0B, 0xC1, 0x23, 0xC1, 0xD0, 0xFE, 0x23, 0xC1, 0xD0, 0xFE, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x19, 0x29, 0xFF, 0x23, 0x19, 0x29, 0xFF, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x57, 0x4E, 0xFF, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x19, 0x29, 0xFF, 0x23, 0x57, 0x4E, 0xFF, 0x23, 0x19, 0x29, 0xFF, 0x23, 0x57, 0x4E, 0xFF, 0x23, 0xA3, 0x6D, 0xFF, 0x23, 0xED, 0x80, 0xFF, 0x23, 0x4B, 0x96, 0xFF, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0xCC, 0x10, 0x00, 0x24, 0xCC, 0x10, 0x00, 0x24, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x4A, 0x80, 0x53, 0x23, 0x51, 0xAF, 0x3B, 0x24, 0x03, 0x11, 0x00, 0x24, 0x4A, 0x80, 0x53, 0x23 };
            //f2000AnimOpticPtrs[0] = new IntPtr(0x338945C0);
            //f2000AnimOpticPtrs[1] = new IntPtr(0x338945D8);
            //f2000AnimOpticPtrs[2] = new IntPtr(0x338945F0);
            //f2000AnimOpticPtrs[3] = new IntPtr(0x33894608);
            //f2000AnimOpticPtrs[4] = new IntPtr(0x33894620);
            //f2000AnimOpticPtrs[5] = new IntPtr(0x33894638);
            //f2000AnimOpticPtrs[6] = new IntPtr(0x33894650);//ADS fire
            //f2000AnimOpticData[6] = new byte[4] { 0x21, 0xAA, 0xFF, 0x23 };
            //f2000AnimOpticPtrs[7] = new IntPtr(0x33894668);
            //f2000AnimOpticData[7] = new byte[4] { 0x21, 0xAA, 0xFF, 0x23 };
            //f2000NamePtr = new IntPtr(0x338939B2);
            f2000NameData = new byte[12] { 0x46, 0x32, 0x30, 0x30, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };


            //deagleDataPtrs[0] = new IntPtr(0x015A9928);
            //deagleDataPtrs[1] = new IntPtr(0x015A994C);
            //deagleDataPtrs[2] = new IntPtr(0x015A998C);
            //ak47DataPtrs[0] = new IntPtr(0x0155A9E0);//Modelname & part
            //ak47DataPtrs[1] = new IntPtr(0x0155AA04);//model data
            //ak47DataPtrs[2] = new IntPtr(0x0155AA44);//viewmodel struct
            //m16DataPtrs[0] = new IntPtr(0x01587220);//Modelname & part
            //m16DataPtrs[1] = new IntPtr(0x01587244);//model data
            //m16DataPtrs[2] = new IntPtr(0x01587284);//viewmodel struct
            //f2000DataPtrs[0] = new IntPtr(0x0154D4EC);//Modelname & part
            //f2000DataPtrs[1] = new IntPtr(0x0154D510);//model data
            //f2000DataPtrs[2] = new IntPtr(0x0154D550);//viewmodel struct

            //ak47CamoDataPtrs[0] = new IntPtr(0x015544D0);
            //ak47CamoDataPtrs[1] = new IntPtr(0x015544F4);
            //ak47CamoDataPtrs[2] = new IntPtr(0x01554534);
            //m16CamoDataPtrs[0] = new IntPtr(0x01587354);
            //m16CamoDataPtrs[1] = new IntPtr(0x01587378);
            //m16CamoDataPtrs[2] = new IntPtr(0x015873B8);
            //m4AnimPtr = new IntPtr(0x24235210);
            //m4GripPtr = new IntPtr(0x0158D9DC);
            //uspModelPtr = new IntPtr(0x25634CB8);
            //uspModelTagPtr = new IntPtr(0x015AC120);


            mw3Checker.Interval = 5000;
            mw3Checker.Tick += checkForMW3Open;
            mw3Checker.Start();
        }

        private void checkForMW3Open(object sender, EventArgs e)
        {
            Process[] currentProcesses = Process.GetProcessesByName("iw5mp");
            if (currentProcesses.Length == 0)
            {
                mw3Process = null;
                mw3Open = false;
                mw3Status.Text = "MW3 is not open";
                mw3Status.ForeColor = Color.Red;
                m4PatchButton.Enabled = false;
                ak47PatchButton.Enabled = false;
                m16PatchButton.Enabled = false;
                f2000PatchButton.Enabled = false;
                uspPatchButton.Enabled = false;
                deaglePatchButton.Enabled = false;
                augPatchButton.Enabled = false;

                return;
            }

            if (mw3Process == null)
            {
                mw3Process = currentProcesses[0];
                mw3Open = true;
                scanMemoryForAddresses();
                mw3Status.Text = "MW3 is open";
                mw3Status.ForeColor = Color.Green;
            }
        }
        private void scanMemoryForAddresses()
        {
            mw3Status.Text = "Scanning...";
            mw3Status.ForeColor = Color.Blue;

            IntPtr baseAddress = new IntPtr(0x23000000);
            byte[] buffer = new byte[2048];
            int bytesRead = 0;
            string s = null;
            bool akFound = false;
            bool m16Found = false;
            bool m4Found = false;
            bool m4AnimsFound = false;
            bool uspFound = false;
            bool f2000Found = false;
            bool f2000AnimsFound = false;
            bool deagleFound = true;
            bool f2000ImageFound = true;
            bool mp5ImageFound = false;
            bool augFound = false;
            bool l86Found = false;
            bool l86AnimsFound = false;

            while (!akFound || !m16Found || !m4Found || !uspFound || !f2000Found || !deagleFound || !augFound || !m4AnimsFound || !f2000AnimsFound/* || !l86Found*/)
            {
                if (!mw3Open) break;
                if ((int)baseAddress > 0x27000000)
                {
                    MessageBox.Show("Unable to find all data values! Some features may be unavailable.", "Scanning Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }

                s = null;
                ReadProcessMemory(mw3Process.Handle, baseAddress, buffer, 2048, out bytesRead);
                s = Encoding.Default.GetString(buffer);

                if (s.Contains("weapon_f2000") && !f2000ImageFound)
                {
                    f2000ImageData = (int)baseAddress + s.IndexOf("weapon_f2000") + 0x18;
                    f2000ImageFound = true;
                }
                if (s.Contains("weapon_mp5k") && !mp5ImageFound)//F2000 CaC icon
                {
                    if (f2000ImageData != 0)
                        f2000ImagePtr = baseAddress + s.IndexOf("weapon_mp5k") + 0x14;
                    mp5ImageFound = true;
                }
                if (s.Contains("viewmodel_f2000"))
                {
                    f2000LensTagPtr = baseAddress + s.IndexOf("viewmodel_f2000") + 0x60;
                }
                if (s.Contains("WEAPON_UAV_STRIKE") && !f2000AnimsFound)
                {
                    ReadProcessMemory(mw3Process.Handle, baseAddress + s.IndexOf("WEAPON_UAV_STRIKE") + 0x56, f2000AnimData, 164, out bytesRead);
                    //Fix empty reload anim
                    f2000AnimData[36] = f2000AnimData[32];
                    f2000AnimData[37] = f2000AnimData[33];
                    f2000AnimData[38] = f2000AnimData[34];
                    f2000AnimData[39] = f2000AnimData[35];
                    f2000AnimsFound = true;
                }
                if (s.Contains("WEAPON_MK12SPR") && !m4AnimsFound)
                {
                    ReadProcessMemory(mw3Process.Handle, baseAddress + s.IndexOf("WEAPON_MK12SPR") + 0x2C, m4AnimData, 144, out bytesRead);
                    m4AnimsFound = true;
                }

                if (s.Contains("iw5_ak47_mp") && !akFound)
                {
                    IntPtr tempAddress = findModelDataForWeaponAsset(baseAddress + s.IndexOf("iw5_ak47_mp"));
                    ak47DataPtrs[0] = tempAddress;
                    ak47DataPtrs[1] = tempAddress + 0x24;
                    ak47DataPtrs[2] = tempAddress + 0x64;
                    tempAddress = findModelDataForWeaponAsset(baseAddress+4 + s.IndexOf("iw5_ak47_mp"));
                    ak47CamoDataPtrs[0] = tempAddress;
                    ak47CamoDataPtrs[1] = tempAddress + 0x24;
                    ak47CamoDataPtrs[2] = tempAddress + 0x64;
                    akFound = true;
                    ak47PatchButton.Enabled = true;
                }
                if (s.Contains("iw5_usp45_mp") && !uspFound)
                {
                    uspModelPtr = baseAddress + s.IndexOf("iw5_usp45_mp") + 0x14;
                    IntPtr tempAddress = new IntPtr(BitConverter.ToInt32(uspModelData, 0));
                    uspModelTagPtr = tempAddress + 0x44;
                    //uspFound = true;
                }
                if (s.Contains("iw5_deserteagle_mp") && !deagleFound)
                {
                    IntPtr tempAddress = findModelDataForWeaponAsset(baseAddress + 7 + s.IndexOf("iw5_deserteagle_mp"));
                    deagleDataPtrs[0] = tempAddress;
                    deagleDataPtrs[1] = tempAddress + 0x24;
                    deagleDataPtrs[2] = tempAddress + 0x64;
                    deagleFound = true;
                    deaglePatchButton.Enabled = true;
                }
                if (s.Contains("throwingknife_mp") && !uspFound)
                {
                    IntPtr tempAddress = findModelDataForWeaponAsset(baseAddress+2 + s.IndexOf("throwingknife_mp"));
                    uspModelTagPtr = tempAddress + 0x44;
                    uspFound = true;
                    uspPatchButton.Enabled = true;
                }
                if (s.Contains("iw5_m16_mp") && !m16Found)
                {
                    IntPtr tempAddress = findModelDataForWeaponAsset(baseAddress-4 + s.IndexOf("iw5_m16_mp"));
                    m16DataPtrs[0] = tempAddress;
                    m16DataPtrs[1] = tempAddress + 0x24;
                    m16DataPtrs[2] = tempAddress + 0x64;
                    tempAddress = findModelDataForWeaponAsset(baseAddress + s.IndexOf("iw5_m16_mp"));
                    m16CamoDataPtrs[0] = tempAddress;
                    m16CamoDataPtrs[1] = tempAddress + 0x24;
                    m16CamoDataPtrs[2] = tempAddress + 0x64;
                    m16Found = true;
                    m16PatchButton.Enabled = true;
                }
                if (s.Contains("iw5_mp5_mp") && !f2000Found)
                {
                    IntPtr tempAddress = findModelDataForWeaponAsset(baseAddress-1 + s.IndexOf("iw5_mp5_mp"));
                    f2000DataPtrs[0] = tempAddress;
                    f2000DataPtrs[1] = tempAddress + 0x24;
                    f2000DataPtrs[2] = tempAddress + 0x64;
                    f2000Found = true;

                    //Grab anim data
                    tempAddress = baseAddress + s.IndexOf("iw5_mp5_mp");
                    mp5Struct[0] = tempAddress;
                }
                if (s.Contains("iw5_m60jugg_mp") && !augFound)
                {
                    IntPtr tempAddress = baseAddress + s.IndexOf("iw5_m60jugg_mp") + 0x7B8;
                    for (int offset = 0; offset < 32; offset += 4)
                    {
                        augModelPtrs[offset / 4] = tempAddress + offset;
                    }
                    augFound = true;
                    //baseAddress = new IntPtr(0x23000000);//Reset search for the SA80
                }
                if (s.Contains("iw5_sa80_mp") && !l86Found)
                {
                    IntPtr tempAddress = baseAddress + s.IndexOf("iw5_sa80_mp") + 0x7B4;
                    l86ModelsPtr[0] = tempAddress;
                    l86Found = true;
                }
                if (s.Contains("iw5_m4_mp") && !m4Found)
                {
                    IntPtr weapon = (baseAddress + s.IndexOf("iw5_m4_mp"));
                    IntPtr tempAddress = findModelDataForWeaponAsset(weapon-5);
                    m4GripPtr = tempAddress + 0x44;
                }
                else if (s.Contains("WEAPON_M4_CARBINE") && !m4Found)
                {
                    m4AnimPtr = baseAddress + s.IndexOf("WEAPON_M4_CARBINE") + 0x4A;
                    m4Found = true;
                    m4PatchButton.Enabled = true;
                }

                if (s.Contains("viewmodel_m4_ads_up"))
                    viewmodel_m4_ads[0] = baseAddress + s.IndexOf("viewmodel_m4_ads_up");
                if (s.Contains("viewmodel_m4_ads_down"))
                    viewmodel_m4_ads[1] = baseAddress + s.IndexOf("viewmodel_m4_ads_down");
                if (s.Contains("viewmodel_m16_hybrid_eotech_ads_up"))
                    viewmodel_m16_hybrid[0] = baseAddress + s.IndexOf("viewmodel_m16_hybrid_eotech_ads_up");
                if (s.Contains("viewmodel_m16_hybrid_eotech_ads_down"))
                    viewmodel_m16_hybrid[1] = baseAddress + s.IndexOf("viewmodel_m16_hybrid_eotech_ads_down");
                if (s.Contains("viewmodel_m16_reflex_ads_up"))
                    viewmodel_m16_reflex[0] = baseAddress + s.IndexOf("viewmodel_m16_reflex_ads_up");
                if (s.Contains("viewmodel_m16_reflex_ads_down"))
                    viewmodel_m16_reflex[1] = baseAddress + s.IndexOf("viewmodel_m16_reflex_ads_down");
                //F2000 anims
                if (s.Contains("viewmodel_acr_reflex_ads_up"))
                    viewmodel_acr_ads[0] = baseAddress + s.IndexOf("viewmodel_acr_reflex_ads_up");
                if (s.Contains("viewmodel_acr_reflex_ads_down"))
                    viewmodel_acr_ads[1] = baseAddress + s.IndexOf("viewmodel_acr_reflex_ads_down");
                if (s.Contains("viewmodel_fn2000_ads_acog_up"))
                {
                    IntPtr address = baseAddress + s.IndexOf("viewmodel_fn2000_ads_acog_up");
                    byte[] data = BitConverter.GetBytes((int)address);
                    f2000AnimOpticData[0] = data;
                    f2000AnimOpticData[2] = data;
                    f2000AnimOpticData[4] = data;
                }
                if (s.Contains("viewmodel_fn2000_ads_acog_down"))
                {
                    IntPtr address = baseAddress + s.IndexOf("viewmodel_fn2000_ads_acog_down");
                    byte[] data = BitConverter.GetBytes((int)address);
                    f2000AnimOpticData[1] = data;
                    f2000AnimOpticData[3] = data;
                    f2000AnimOpticData[5] = data;
                }
                if (s.Contains("viewmodel_fn2000_ads_fire"))
                {
                    IntPtr address = baseAddress + s.IndexOf("viewmodel_fn2000_ads_fire");
                    byte[] data = BitConverter.GetBytes((int)address);
                    f2000AnimOpticData[6] = data;
                    f2000AnimOpticData[7] = data;
                }
                //AUG Anims
                if (s.Contains("viewmodel_cm901_ads_up"))
                    viewmodel_cm901_ads_up = baseAddress + s.IndexOf("viewmodel_cm901_ads_up");
                if (s.Contains("viewmodel_sa80lmg_ads_up"))
                    viewmodel_sa80_optics[0] = baseAddress + s.IndexOf("viewmodel_sa80lmg_ads_up");
                if (s.Contains("viewmodel_sa80lmg_ads_down"))
                    viewmodel_sa80_optics[1] = baseAddress + s.IndexOf("viewmodel_sa80lmg_ads_down");
                if (s.Contains("viewmodel_sa80lmg_idle") && !l86AnimsFound)
                {
                    IntPtr tempAddress = baseAddress + s.IndexOf("viewmodel_sa80lmg_idle") - 0x0C;
                    byte[] check = new byte[1];
                    int read = 0;
                    if (ReadProcessMemory(mw3Process.Handle, tempAddress + 0x23, check, 1, out read) && check[0] == 0x76)
                    {
                        //Debug.WriteLine(check[0].ToString());
                        sa80AnimPtr = tempAddress;
                        sa80OpticAnimPtr = tempAddress + 0x324;
                        l86AnimsFound = true;
                        //augPatchButton.Enabled = true;
                    }
                }

                baseAddress += 2048;
                Application.DoEvents();
            }

            //fix f2000 ads anim
            if (f2000AnimData.Length > 0)
            {
                byte[] newAnim = new byte[4];
                newAnim = BitConverter.GetBytes(viewmodel_acr_ads[0].ToInt32());
                f2000AnimData[0x98] = newAnim[0];
                f2000AnimData[0x99] = newAnim[1];
                f2000AnimData[0x9A] = newAnim[2];
                f2000AnimData[0x9B] = newAnim[3];

                newAnim = BitConverter.GetBytes(viewmodel_acr_ads[1].ToInt32());
                f2000AnimData[0x9C] = newAnim[0];
                f2000AnimData[0x9D] = newAnim[1];
                f2000AnimData[0x9E] = newAnim[2];
                f2000AnimData[0x9F] = newAnim[3];
            }

            bool f2000NameFound = false;
            bool m16AnimPtrsFound = false;
            l86Found = false;//Reset flag for second search
            baseAddress = new IntPtr(0x30000000);

            while (!f2000NameFound || !l86Found || !m16AnimPtrsFound || !l86AnimsFound)
            {
                if (!mw3Open) break;
                if ((int)baseAddress > 0x38000000)
                {
                    MessageBox.Show("Unable to find all data values! Some features may be unavailable.", "Scanning Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }

                s = null;
                ReadProcessMemory(mw3Process.Handle, baseAddress, buffer, 2048, out bytesRead);
                s = Encoding.Default.GetString(buffer);

                if (s.Contains("WEAPON_MP5") && !f2000NameFound)
                {
                    f2000NamePtr = baseAddress + s.IndexOf("WEAPON_MP5");
                    f2000NameFound = true;
                }
                if (s.Contains("iw5_mp5_mp"))
                {
                    //Grab anim data secondary
                    IntPtr tempAddress = baseAddress + s.IndexOf("iw5_mp5_mp");
                    mp5Struct[1] = tempAddress;
                }
                if (s.Contains("viewmodel_m16_idle") && !m16AnimPtrsFound)
                {
                    IntPtr tempAddress = baseAddress + s.IndexOf("viewmodel_m16_idle");
                    m16AnimPtrs[0] = tempAddress - 0x0C;//Ironsight
                    tempAddress += 0x5A4;
                    m16AnimPtrs[1] = tempAddress;//Acog ADS Up
                    tempAddress += 0x18;
                    m16AnimPtrs[2] = tempAddress;//Acog ADS Down
                    tempAddress += 0x18;
                    m16AnimPtrs[3] = tempAddress;//Eotech ADS Up
                    tempAddress += 0x18;
                    m16AnimPtrs[4] = tempAddress;//EOtech ADS Down
                    tempAddress += 0x18;
                    m16AnimPtrs[5] = tempAddress;//Reflex ADS Up
                    tempAddress += 0x18;
                    m16AnimPtrs[6] = tempAddress;//Reflex ADS Down
                    m16AnimPtrsFound = true;
                }
                if (s.Contains("iw5_sa80_mp") && augFound)
                {
                    IntPtr tempAddress = baseAddress + s.IndexOf("iw5_sa80_mp") + 0x7B4;
                    l86ModelsPtr[1] = tempAddress;
                    l86Found = true;
                    //augPatchButton.Enabled = true;
                }
                if (s.Contains("viewmodel_sa80lmg_idle") && !l86AnimsFound)
                {
                    IntPtr tempAddress = baseAddress + s.IndexOf("viewmodel_sa80lmg_idle") - 0x0C;
                    byte[] check = new byte[1];
                    int read = 0;
                    if (ReadProcessMemory(mw3Process.Handle, tempAddress + 0x23, check, 1, out read) && check[0] == 0x76)
                    {
                        //Debug.WriteLine(check[0].ToString());
                        sa80AnimPtr = tempAddress;
                        sa80OpticAnimPtr = tempAddress + 0x324;
                        l86AnimsFound = true;
                        augPatchButton.Enabled = true;
                    }
                }

                baseAddress += 2048;
                Application.DoEvents();
            }

            //Search for our MP5 struct
            baseAddress = new IntPtr(0x01000000);
            bool foundMP5Struct = false;

            while (!foundMP5Struct)
            {
                if (!mw3Open) break;
                if ((int)baseAddress > 0x5000000)
                {
                    MessageBox.Show("Unable to find all data values! Some features may be unavailable.", "Scanning Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }

                ReadProcessMemory(mw3Process.Handle, baseAddress, buffer, 2048, out bytesRead);

                if (mp5Struct[0] != IntPtr.Zero)
                {
                    int result = Search(buffer, BitConverter.GetBytes(mp5Struct[0].ToInt32()));
                    if (result != -1)
                    {
                        mp5Struct[0] = baseAddress + result;
                        f2000AnimPtr[0] = mp5Struct[0] + 0x1C;
                        byte[] newF2000AnimPtr = new byte[4];
                        ReadProcessMemory(mw3Process.Handle, f2000AnimPtr[0], newF2000AnimPtr, 4, out bytesRead);
                        f2000AnimPtr[0] = new IntPtr(BitConverter.ToInt32(newF2000AnimPtr, 0));
                        f2000AnimPtr[0] += 0x04;//Skip the first null anim
                        f2000AnimOpticPtrs[0] = mp5Struct[0] + 0x24;//attachment anims
                        //Debug.WriteLine(f2000AnimOpticPtrs[0].ToString("X"));
                        ReadProcessMemory(mw3Process.Handle, f2000AnimOpticPtrs[0], newF2000AnimPtr, 4, out bytesRead);
                        f2000AnimOpticPtrs[0] = new IntPtr(BitConverter.ToInt32(newF2000AnimPtr, 0));//acog up
                        f2000AnimOpticPtrs[0] += 0x04;//Skip the first identifier
                        f2000AnimOpticPtrs[1] = f2000AnimOpticPtrs[0] + 0x18;//acog down
                        f2000AnimOpticPtrs[2] = f2000AnimOpticPtrs[1] + 0x18;//eotech up
                        f2000AnimOpticPtrs[3] = f2000AnimOpticPtrs[2] + 0x18;//eotech down
                        f2000AnimOpticPtrs[4] = f2000AnimOpticPtrs[3] + 0x18;//reflex up
                        f2000AnimOpticPtrs[5] = f2000AnimOpticPtrs[4] + 0x18;//reflex down
                        f2000AnimOpticPtrs[6] = f2000AnimOpticPtrs[5] + 0x18;//ads fire
                        f2000AnimOpticPtrs[7] = f2000AnimOpticPtrs[6] + 0x18;//acog fire?
                    }
                }
                if (mp5Struct[1] != IntPtr.Zero)
                {
                    int result = Search(buffer, BitConverter.GetBytes(mp5Struct[1].ToInt32()));
                    if (result != -1)
                    {
                        mp5Struct[1] = baseAddress + result;
                        f2000AnimPtr[1] = mp5Struct[1] + 0x1C;
                        byte[] newF2000AnimPtr = new byte[4];
                        ReadProcessMemory(mw3Process.Handle, f2000AnimPtr[1], newF2000AnimPtr, 4, out bytesRead);
                        f2000AnimPtr[1] = new IntPtr(BitConverter.ToInt32(newF2000AnimPtr, 0));
                        f2000AnimPtr[1] += 0x04;//Skip the first null anim
                        f2000AnimOpticPtrsSecondary[0] = mp5Struct[1] + 0x24;//attachment anims
                        ReadProcessMemory(mw3Process.Handle, f2000AnimOpticPtrsSecondary[0], newF2000AnimPtr, 4, out bytesRead);
                        f2000AnimOpticPtrsSecondary[0] = new IntPtr(BitConverter.ToInt32(newF2000AnimPtr, 0));//acog up
                        f2000AnimOpticPtrsSecondary[0] += 0x04;//Skip the first identifier
                        //Debug.WriteLine(f2000AnimOpticPtrsSecondary[0].ToString("X"));
                        f2000AnimOpticPtrsSecondary[1] = f2000AnimOpticPtrsSecondary[0] + 0x18;//acog down
                        f2000AnimOpticPtrsSecondary[2] = f2000AnimOpticPtrsSecondary[1] + 0x18;//eotech up
                        f2000AnimOpticPtrsSecondary[3] = f2000AnimOpticPtrsSecondary[2] + 0x18;//eotech down
                        f2000AnimOpticPtrsSecondary[4] = f2000AnimOpticPtrsSecondary[3] + 0x18;//reflex up
                        f2000AnimOpticPtrsSecondary[5] = f2000AnimOpticPtrsSecondary[4] + 0x18;//reflex down
                        f2000AnimOpticPtrsSecondary[6] = f2000AnimOpticPtrsSecondary[5] + 0x18;//ads fire
                        f2000AnimOpticPtrsSecondary[7] = f2000AnimOpticPtrsSecondary[6] + 0x18;//acog fire?
                    }
                }

                if (f2000AnimPtr[0] != IntPtr.Zero || f2000AnimPtr[1] != IntPtr.Zero)
                {
                    foundMP5Struct = true;
                    f2000PatchButton.Enabled = true;
                }

                baseAddress += 2048;
                Application.DoEvents();
            }
        }
        private static int Search(byte[] src, byte[] pattern)
        {
            int c = src.Length - pattern.Length + 1;
            int j;
            for (int i = 0; i < c; i++)
            {
                if (src[i] != pattern[0]) continue;
                for (j = pattern.Length - 1; j >= 1 && src[i + j] == pattern[j]; j--) ;
                if (j == 0) return i;
            }
            return -1;
        }

        private IntPtr findModelDataForWeaponAsset(IntPtr weapon)
        {
            IntPtr ret;
            weapon += 0x7B3;
            byte[] address = new byte[4];
            int bytesRead = 0;
            ReadProcessMemory(mw3Process.Handle, weapon, address, 4, out bytesRead);
            ret = new IntPtr(BitConverter.ToInt32(address, 0));
            return ret;
        }

        private void ak47PatchButton_Click(object sender, EventArgs e)
        {
            if (!checkifProcessOpen()) return;
            if (ak47DataPtrs[0] == null)
            {
                showFailMessage();
                return;
            }

            int writeCount = 0;
            if (ak47Camo.Checked) WriteProcessMemory(mw3Process.Handle, ak47CamoDataPtrs[0], ak47Data[0], 7, ref writeCount);
            else WriteProcessMemory(mw3Process.Handle, ak47DataPtrs[0], ak47Data[0], 7, ref writeCount);
            if (writeCount != 7)
            {
                showFailMessage();
                return;
            }
            if (ak47Camo.Checked) WriteProcessMemory(mw3Process.Handle, ak47CamoDataPtrs[1], ak47Data[1], 44, ref writeCount);
            else WriteProcessMemory(mw3Process.Handle, ak47DataPtrs[1], ak47Data[1], 44, ref writeCount);
            if (writeCount != 44)
            {
                showFailMessage();
                return;
            }
            if (ak47Camo.Checked) WriteProcessMemory(mw3Process.Handle, ak47CamoDataPtrs[2], ak47Data[2], 4, ref writeCount);
            else WriteProcessMemory(mw3Process.Handle, ak47DataPtrs[2], ak47Data[2], 4, ref writeCount);
            if (writeCount != 4)
            {
                showFailMessage();
                return;
            }

            showSuccessMessage();
            ak47PatchButton.Enabled = false;
        }

        private void m16PatchButton_Click(object sender, EventArgs e)
        {
            if (!checkifProcessOpen()) return;
            if (m16DataPtrs[0] == null)
            {
                showFailMessage();
                return;
            }

            int writeCount = 0;
            if (m16Camo.Checked) WriteProcessMemory(mw3Process.Handle, m16CamoDataPtrs[0], m16Data[0], 8, ref writeCount);
            else WriteProcessMemory(mw3Process.Handle, m16DataPtrs[0], m16Data[0], 8, ref writeCount);
            if (writeCount != 8)
            {
                showFailMessage();
                return;
            }
            if (m16Camo.Checked) WriteProcessMemory(mw3Process.Handle, m16CamoDataPtrs[1], m16Data[1], 44, ref writeCount);
            else WriteProcessMemory(mw3Process.Handle, m16DataPtrs[1], m16Data[1], 44, ref writeCount);
            if (writeCount != 44)
            {
                showFailMessage();
                return;
            }
            if (m16Camo.Checked) WriteProcessMemory(mw3Process.Handle, m16CamoDataPtrs[2], m16Data[2], 4, ref writeCount);
            else WriteProcessMemory(mw3Process.Handle, m16DataPtrs[2], m16Data[2], 4, ref writeCount);
            if (writeCount != 4)
            {
                showFailMessage();
                return;
            }

            byte[] buffer = new byte[4];
            buffer = BitConverter.GetBytes((int)viewmodel_m16_hybrid[0]);
            WriteProcessMemory(mw3Process.Handle, m16AnimPtrs[0], buffer, 4, ref writeCount);//Ironsight

            buffer = BitConverter.GetBytes((int)viewmodel_m4_ads[0]);
            WriteProcessMemory(mw3Process.Handle, m16AnimPtrs[1], buffer, 4, ref writeCount);
            WriteProcessMemory(mw3Process.Handle, m16AnimPtrs[5], buffer, 4, ref writeCount);
            buffer = BitConverter.GetBytes((int)viewmodel_m4_ads[1]);
            WriteProcessMemory(mw3Process.Handle, m16AnimPtrs[2], buffer, 4, ref writeCount);//Acog ads
            WriteProcessMemory(mw3Process.Handle, m16AnimPtrs[6], buffer, 4, ref writeCount);//Reflex

            buffer = BitConverter.GetBytes((int)viewmodel_m16_reflex[0]);
            WriteProcessMemory(mw3Process.Handle, m16AnimPtrs[3], buffer, 4, ref writeCount);
            buffer = BitConverter.GetBytes((int)viewmodel_m16_reflex[1]);
            WriteProcessMemory(mw3Process.Handle, m16AnimPtrs[4], buffer, 4, ref writeCount);//eotech ads

            showSuccessMessage();
            m16PatchButton.Enabled = false;
        }

        private void m4PatchButton_Click(object sender, EventArgs e)
        {
            if (!checkifProcessOpen()) return;
            if (m4AnimPtr == null)
            {
                showFailMessage();
                return;
            }

            int writeCount = 0;
            WriteProcessMemory(mw3Process.Handle, m4AnimPtr, m4AnimData, m4AnimData.Length, ref writeCount);
            if (writeCount != m4AnimData.Length)
            {
                showFailMessage();
                return;
            }

            WriteProcessMemory(mw3Process.Handle, m4GripPtr, new byte[] { m4GripData }, 1, ref writeCount);
            if (writeCount != 1)
            {
                showFailMessage();
                return;
            }

            showSuccessMessage();
            m4PatchButton.Enabled = false;
        }

        private void uspPatchButton_Click(object sender, EventArgs e)
        {
            if (!checkifProcessOpen()) return;
            if (uspModelPtr == null)
            {
                showFailMessage();
                return;
            }

            int writeCount = 0;
            WriteProcessMemory(mw3Process.Handle, uspModelPtr, uspModelData, uspModelData.Length, ref writeCount);
            if (writeCount != uspModelData.Length)
            {
                showFailMessage();
                return;
            }

            WriteProcessMemory(mw3Process.Handle, uspModelTagPtr, new byte[] { uspModelTagData }, 1, ref writeCount);
            if (writeCount != 1)
            {
                showFailMessage();
                return;
            }

            showSuccessMessage();
            uspPatchButton.Enabled = false;
        }

        private void f2000PatchButton_Click(object sender, EventArgs e)
        {
            if (!checkifProcessOpen()) return;
            if (f2000DataPtrs[0] == null)
            {
                showFailMessage();
                return;
            }

            int writeCount = 0;
            WriteProcessMemory(mw3Process.Handle, f2000DataPtrs[0], f2000Data[0], 8, ref writeCount);
            if (writeCount != 8)
            {
                showFailMessage();
                return;
            }
            WriteProcessMemory(mw3Process.Handle, f2000DataPtrs[1], f2000Data[1], 44, ref writeCount);
            if (writeCount != 44)
            {
                showFailMessage();
                return;
            }
            WriteProcessMemory(mw3Process.Handle, f2000DataPtrs[2], f2000Data[2], 4, ref writeCount);
            if (writeCount != 4)
            {
                showFailMessage();
                return;
            }

            //Hide leftover lens
            WriteProcessMemory(mw3Process.Handle, f2000LensTagPtr, new byte[] { f2000LensTag }, 1, ref writeCount);
            if (writeCount != 1)
            {
                showFailMessage();
                return;
            }

            //Patch anims
            if (f2000AnimPtr[0] != IntPtr.Zero)
            {
                WriteProcessMemory(mw3Process.Handle, f2000AnimPtr[0], f2000AnimData, f2000AnimData.Length, ref writeCount);
                if (writeCount != f2000AnimData.Length)
                {
                    showFailMessage();
                    return;
                }
            }
            //Patch secondary if found
            if (f2000AnimPtr[1] != IntPtr.Zero)
            {
                WriteProcessMemory(mw3Process.Handle, f2000AnimPtr[1], f2000AnimData, f2000AnimData.Length, ref writeCount);
                if (writeCount != f2000AnimData.Length)
                {
                    showFailMessage();
                    return;
                }
            }

            //Patch optics
            if (f2000AnimOpticPtrs[0] != IntPtr.Zero)
            {
                for (int i = 0; i < f2000AnimOpticPtrs.Length; i++)
                {
                    //Debug.Write("Writing to " + f2000AnimOpticPtrs[i].ToString("X"));
                    WriteProcessMemory(mw3Process.Handle, f2000AnimOpticPtrs[i], f2000AnimOpticData[i], 4, ref writeCount);
                    if (writeCount != 4)
                    {
                        showFailMessage();
                        return;
                    }
                }
            }
            if (f2000AnimOpticPtrsSecondary[0] != IntPtr.Zero)
            {
                for (int i = 0; i < f2000AnimOpticPtrsSecondary.Length; i++)
                {
                    //Debug.Write("Writing to  secondary " + f2000AnimOpticPtrs[i].ToString("X"));
                    WriteProcessMemory(mw3Process.Handle, f2000AnimOpticPtrsSecondary[i], f2000AnimOpticData[i], 4, ref writeCount);
                    if (writeCount != 4)
                    {
                        showFailMessage();
                        return;
                    }
                }
            }


            if (f2000NamePtr.ToInt32() != 0)
            {
                WriteProcessMemory(mw3Process.Handle, f2000NamePtr, f2000NameData, f2000NameData.Length, ref writeCount);
                if (writeCount != f2000NameData.Length)
                {
                    showFailMessage();
                    return;
                }
            }

            if (f2000ImageData != 0)
            {
                byte[] data = BitConverter.GetBytes(f2000ImageData);
                WriteProcessMemory(mw3Process.Handle, f2000ImagePtr, data, 4, ref writeCount);
                if (writeCount != 4)
                {
                    showFailMessage();
                    return;
                }
            }

            showSuccessMessage();
            f2000PatchButton.Enabled = false;
        }

        private void deaglePatchButton_Click(object sender, EventArgs e)
        {
            if (!checkifProcessOpen()) return;
            if (deagleDataPtrs[0] == null)
            {
                showFailMessage();
                return;
            }

            int writeCount = 0;
            WriteProcessMemory(mw3Process.Handle, deagleDataPtrs[0], deagleData[0], 7, ref writeCount);
            if (writeCount != 7)
            {
                showFailMessage();
                return;
            }
            WriteProcessMemory(mw3Process.Handle, deagleDataPtrs[1], deagleData[1], 44, ref writeCount);
            if (writeCount != 44)
            {
                showFailMessage();
                return;
            }
            WriteProcessMemory(mw3Process.Handle, deagleDataPtrs[2], deagleData[2], 4, ref writeCount);
            if (writeCount != 4)
            {
                showFailMessage();
                return;
            }

            showSuccessMessage();
            deaglePatchButton.Enabled = false;
        }
        private void augPatchButton_Click(object sender, EventArgs e)
        {
            if (!checkifProcessOpen()) return;
            if (augModelPtrs[0] == null)
            {
                showFailMessage();
                return;
            }

            int writeCount = 0;
            int readCount = 0;
            byte[] buffer = new byte[4];
            for (int i = 0; i < augModelPtrs.Length; i++)
            {
                readCount = 0;
                ReadProcessMemory(mw3Process.Handle, augModelPtrs[i], buffer, 4, out readCount);
                if (readCount != 4)
                {
                    showFailMessage();
                    return;
                }

                WriteProcessMemory(mw3Process.Handle, l86ModelsPtr[0] + (i*4), buffer, 4, ref writeCount);
                if (writeCount != 4)
                {
                    showFailMessage();
                    return;
                }
                WriteProcessMemory(mw3Process.Handle, l86ModelsPtr[1] + (i * 4), buffer, 4, ref writeCount);
                if (writeCount != 4)
                {
                    showFailMessage();
                    return;
                }

                //Patch tags for this model
                int address = BitConverter.ToInt32(buffer, 0);
                WriteProcessMemory(mw3Process.Handle, new IntPtr(address + 0x44), new byte[1] { augModelTag }, 1, ref writeCount);
                if (writeCount != 1)
                {
                    showFailMessage();
                    return;
                }
            }

            //Patch out the scope for all models
            ReadProcessMemory(mw3Process.Handle, augModelPtrs[0], buffer, 4, out readCount);
            if (readCount != 4)
            {
                showFailMessage();
                return;
            }
            int modelAddress = BitConverter.ToInt32(buffer, 0);//Grabs the weapon struct address
            ReadProcessMemory(mw3Process.Handle, new IntPtr(modelAddress + 0x64), buffer, 4, out readCount);
            if (readCount != 4)
            {
                showFailMessage();
                return;
            }
            int viewmodelData = BitConverter.ToInt32(buffer, 0);//Grabs the raw model data

            WriteProcessMemory(mw3Process.Handle, new IntPtr(viewmodelData + 0x06), new byte[1] { 0x00 }, 1, ref writeCount);//Remove scope
            if (writeCount != 1)
            {
                showFailMessage();
                return;
            }
            WriteProcessMemory(mw3Process.Handle, new IntPtr(viewmodelData + 0x15A), new byte[1] { 0x00 }, 1, ref writeCount);//Remove scope lens
            if (writeCount != 1)
            {
                showFailMessage();
                return;
            }

            //Patch ads anims
            byte[] ads_up = BitConverter.GetBytes(viewmodel_cm901_ads_up.ToInt32());
            WriteProcessMemory(mw3Process.Handle, sa80AnimPtr, ads_up, 4, ref writeCount);
            if (writeCount != 4)
            {
                showFailMessage();
                return;
            }
            byte[] optic_ads_up = BitConverter.GetBytes(viewmodel_sa80_optics[0].ToInt32());
            byte[] optic_ads_down = BitConverter.GetBytes(viewmodel_sa80_optics[1].ToInt32());
            WriteProcessMemory(mw3Process.Handle, sa80OpticAnimPtr, optic_ads_up, 4, ref writeCount);//acog up
            if (writeCount != 4)
            {
                showFailMessage();
                return;
            }
            WriteProcessMemory(mw3Process.Handle, sa80OpticAnimPtr + 0x18, optic_ads_down, 4, ref writeCount);//acog down
            if (writeCount != 4)
            {
                showFailMessage();
                return;
            }
            WriteProcessMemory(mw3Process.Handle, sa80OpticAnimPtr + 0x60, optic_ads_up, 4, ref writeCount);//eotech up
            if (writeCount != 4)
            {
                showFailMessage();
                return;
            }
            WriteProcessMemory(mw3Process.Handle, sa80OpticAnimPtr + 0x78, optic_ads_down, 4, ref writeCount);//eotech up
            if (writeCount != 4)
            {
                showFailMessage();
                return;
            }
            WriteProcessMemory(mw3Process.Handle, sa80OpticAnimPtr + 0xC0, optic_ads_up, 4, ref writeCount);//eotech up
            if (writeCount != 4)
            {
                showFailMessage();
                return;
            }
            WriteProcessMemory(mw3Process.Handle, sa80OpticAnimPtr + 0xD8, optic_ads_down, 4, ref writeCount);//eotech up
            if (writeCount != 4)
            {
                showFailMessage();
                return;
            }

            showSuccessMessage();
            augPatchButton.Enabled = false;
        }

        private static bool checkifProcessOpen()
        {
            if (!mw3Open)
            {
                MessageBox.Show("You must have TeknoMW3 open to patch data!", "Open TeknoMW3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (mw3Process == null)
            {
                MessageBox.Show("There was an error finding your TeknoMW3 process!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private static void showFailMessage()
        {
            MessageBox.Show("Patching data failed to write to TeknoMW3 Memory!", "Patching Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private static void showSuccessMessage()
        {
            MessageBox.Show("Successfully patched data to TeknoMW3!", "Patching Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
