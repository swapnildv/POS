using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MegabiteEntityLayer
{
    public partial class RFID_HW
    {
        //you just need to declare Required functions, available from ddslibnfc.dll.
        //NO NEED TO REGISTER OR REFERENCE IN THE PROJECT

        #region Declarations

        /// <summary>
        /// USED TO GET CARD UID
        /// </summary>
        /// <param name="DeviceNo">Enter Device Number to which you want to communicate, Default value is 0 
        /// and Range of value is 0 to 3</param>
        /// <param name="FirstArrayElement">Provide Address of Zeroth Element of an Array,
        /// which will store Card UID</param>
        /// <param name="Length">Length of Array</param>
        /// <param name="ErrorCode">Function will return ErrorCode, if process fails</param>
        /// <returns>Length of UID for e.g. it will return 4 in case of Mifare Classic Card</returns>        
        [DllImport("ddslibnfc.dll")]
        private static extern int GetUID(byte DeviceNo, ref byte FirstArrayElement, byte Length, ref int ErrorCode);

        /// <summary>
        /// CHANGING KEYS FOR AUTHENTICATING CARD
        /// </summary>
        /// <param name="FirstElementofKeyBuffer">Provide Address of Zeroth Element of an Array,
        /// which has Authentication keys</param>
        /// <param name="Length">Length of Array</param>
        /// <param name="ErrorCode">Function will return ErrorCode, if process fails</param>
        /// <returns>returns true in case of operation success else return false</returns>
        [DllImport("ddslibnfc.dll")]
        private static extern bool Change_Authenticating_Keys(ref byte FirstElementofKeyBuffer, int Length, ref int ErrorCode);

        /// <summary>
        /// CHANGE KEYS ON THE CARD
        /// </summary>
        /// <param name="DeviceNo">Enter Device Number to which you want to communicate, Default value is 0 
        /// and Range of value is 0 to 3</param>
        /// <param name="BlockNo">Enter Block No., whose keys has to be changed</param>
        /// <param name="FirstElementofKeyBuffer">Provide Address of Zeroth Element of an Array,
        /// which has New Changing Keys</param>
        /// <param name="Length">Length of Array</param>
        /// <param name="ErrorCode">Function will return ErrorCode, if process fails</param>
        /// <returns>returns true in case of operation success else return false</returns>

        [DllImport("ddslibnfc.dll")]
        private static extern bool Change_Card_Keys(byte DeviceNo, byte BlockNo, ref byte FirstElementofKeyBuffer,
            byte Length, ref int ErrorCode);
        /// <summary>
        /// CHANGE ALL KEYS ON THE CARD
        /// </summary>
        /// <param name="DeviceNo">Enter Device Number to which you want to communicate, Default value is 0 
        /// and Range of value is 0 to 3</param>
        /// <param name="FirstElementofKeyBuffer">Provide Address of Zeroth Element of an Array,
        /// which has New Keys</param>
        /// <param name="Length">Length of Array</param>
        /// <param name="FirstElementofFailuareSectorBuffer">Provide Address of Zeroth Element of an Array,
        /// which will store List of Sectors, whose keys not changed</param>
        /// <param name="FailureSectorBufferSize">Length of Array</param>
        /// <param name="FailedSectorsCount">No. of Sectors, whose keys not changed</param>
        /// <param name="ErrorCode">Function will return ErrorCode, if process fails</param>
        /// <returns>returns true in case of operation success else return false</returns>

        [DllImport("ddslibnfc.dll")]
        private static extern bool Change_All_Keys_On_Card(byte DeviceNo, ref byte FirstElementofKeyBuffer,
        byte Length, ref byte FirstElementofFailuareSectorBuffer, byte FailureSectorBufferSize,
        ref byte FailedSectorsCount, ref int ErrorCode);

        #region Data Operations

        /// <summary>
        /// READ BLOCK DATA
        /// </summary>
        /// <param name="DeviceNo">Enter Device Number to which you want to communicate, Default value is 0 
        /// and Range of value is 0 to 3</param>
        /// <param name="BlockNo">Enter Data Block No. to be Read</param>
        /// <param name="FirstElementofReadBuffer">Provide Address of Zeroth Element of an Array,
        /// which will store Block Data</param>
        /// <param name="BuffSize">Length of Array</param>
        /// <param name="ErrorCode">Function will return ErrorCode, if process fails</param>
        /// <returns>returns true in case of operation success else return false</returns>
        [DllImport("ddslibnfc.dll")]
        private static extern bool Read_Data_Block(byte DeviceNo, byte BlockNo, ref byte FirstElementofReadBuffer,
            byte BuffSize, ref int ErrorCode);

        /// <summary>
        /// WRITE TO BLOCK
        /// </summary>
        /// <param name="DeviceNo">Enter Device Number to which you want to communicate, Default value is 0 
        /// and Range of value is 0 to 3</param>
        /// <param name="BlockNo">Enter Data Block No. to which Data will be written</param>
        /// <param name="FirstElementofWriteBuffer">Provide Address of Zeroth Element of an Array,
        /// which has Data to be written</param>
        /// <param name="WriteBufferSize">Length of Array</param>
        /// <param name="ErrorCode">Function will return ErrorCode, if process fails</param>
        /// <returns>returns true in case of operation success else return false</returns>
        [DllImport("ddslibnfc.dll")]
        private static extern bool Write_Data_Block(byte DeviceNo, byte BlockNo, ref byte FirstElementofWriteBuffer,
            byte WriteBufferSize, ref int ErrorCode);

        /// <summary>
        /// READ FULL CARD, SUPPORT MIFARE 1K AND 4K
        /// </summary>
        /// <param name="DeviceNo">Enter Device Number to which you want to communicate, Default value is 0 
        /// and Range of value is 0 to 3</param>
        /// <param name="FirstElementofReadBuffer">Provide Address of Zeroth Element of an Array,
        /// which will store Full Card Data</param>
        /// <param name="ReadBufferSize">Length of Array</param>
        /// <param name="ReadLength">This will return No. of Bytes Read</param>
        /// <param name="AuthenticationFailureBuffer">Provide Address of Zeroth Element of an Array,
        /// which will store Authentication Failure Block Nos.</param>
        /// <param name="AuthenticationFailureBufferSize">Length of Array</param>
        /// <param name="FailedBlocksCount">No. of Authentication Failure Blocks</param>
        /// <param name="ErrorCode">Function will return ErrorCode, if process fails</param>
        /// <returns>returns true in case of operation success else return false</returns>
        [DllImport("ddslibnfc.dll")]
        private static extern bool Read_Full_Card(byte DeviceNo, ref byte FirstElementofReadBuffer,
            Int16 ReadBufferSize, ref Int16 ReadLength, ref byte AuthenticationFailureBuffer,
            Int16 AuthenticationFailureBufferSize, ref byte FailedBlocksCount, ref int ErrorCode);

        /// <summary>
        /// READ ONE SECTOR
        /// </summary>
        /// <param name="DeviceNo">Enter Device Number to which you want to communicate, Default value is 0 
        /// and Range of value is 0 to 3</param>
        /// <param name="SectorNo">Enter Sector No. to be Read</param>
        /// <param name="FirstElementofReadBuffer">Provide Address of Zeroth Element of an Array,
        /// which will store Sector Data</param>
        /// <param name="ReadBufferSize">Length of Array</param>
        /// <param name="ErrorCode">Function will return ErrorCode, if process fails</param>
        /// <returns>returns true in case of operation success else return false</returns>
        [DllImport("ddslibnfc.dll")]
        private static extern bool Read_Sector(byte DeviceNo, byte SectorNo, ref byte FirstElementofReadBuffer,
            UInt16 ReadBufferSize, ref int ErrorCode);

        #endregion

        #region Value Operations

        /// <summary>
        /// INITIALISES A BLOCK FOR VALUE OPERATION
        /// </summary>
        /// <param name="DeviceNo">Enter Device Number to which you want to communicate, Default value is 0 
        /// and Range of value is 0 to 3</param>
        /// <param name="BlockNo">Enter Block No. to be initialise as a Value Block</param>
        /// <param name="value">Enter the Initial Value to be assigned</param>
        /// <param name="ErrorCode">Function will return ErrorCode, if process fails</param>
        /// <returns>returns true in case of operation success else return false</returns>
        [DllImport("ddslibnfc.dll")]
        private static extern bool Initialise_As_Value_Block(byte DeviceNo, byte BlockNo, Int32 value,
            ref int ErrorCode);

        /// <summary>
        /// READ VALUE FROM A BLOCK
        /// </summary>
        /// <param name="DeviceNo">Enter Device Number to which you want to communicate, Default value is 0 
        /// and Range of value is 0 to 3</param>
        /// <param name="BlockNo">Enter Block No., whose Value has to be Read</param>
        /// <param name="value">Value will be stored in this variable</param>
        /// <param name="ErrorCode">Function will return ErrorCode, if process fails</param>
        /// <returns>returns true in case of operation success else return false</returns>
        [DllImport("ddslibnfc.dll")]
        private static extern bool Read_Value_Block(byte DeviceNo, byte BlockNo, ref UInt32 value, ref int ErrorCode);

        /// <summary>
        /// INCREMENT BLOCK VALUE
        /// </summary>
        /// <param name="DeviceNo">Enter Device Number to which you want to communicate, Default value is 0 
        /// and Range of value is 0 to 3</param>
        /// <param name="BlockNo">Enter Block No., whose Value has to be Increment</param>
        /// <param name="Incrementvalue">Enter Increment Value</param>
        /// <param name="ErrorCode">Function will return ErrorCode, if process fails</param>
        /// <returns>returns true in case of operation success else return false</returns>
        [DllImport("ddslibnfc.dll")]
        private static extern bool Increment_Value(byte DeviceNo, byte BlockNo, UInt32 Incrementvalue, ref int ErrorCode);

        /// <summary>
        /// DECREMENT BLOCK VALUE
        /// </summary>
        /// <param name="DeviceNo">Enter Device Number to which you want to communicate, Default value is 0 
        /// and Range of value is 0 to 3</param>
        /// <param name="BlockNo">Enter Block No., whose Value has to be Decrement</param>
        /// <param name="Decrementvalue">Enter Decrement Value</param>
        /// <param name="ErrorCode">Function will return ErrorCode, if process fails</param>
        /// <returns>returns true in case of operation success else return false</returns>
        [DllImport("ddslibnfc.dll")]
        private static extern bool Decrement_Value(byte DeviceNo, byte BlockNo, UInt32 Decrementvalue, ref int ErrorCode);

        #endregion

        #endregion




        const short MaxBlocks = 256;
        const short NoOfBytesPerBlock = 16;
        const short MaxBlocksPerSector = 16;
        const short KeyLength = 6;
        const String AuthenticationKey = "FFFFFFFFFFFF";
        int ErrorCode = 0;
        string msg = "";
        ClsRFID_ACR122U objACR = new ClsRFID_ACR122U();
        enum ErrorCodes
        {
            None,
            ReaderNotConnected,
            CardNotPresent,
            Loadkeyerror,
            Authenticationfailed,
            Readblockerror,
            WriteBlockError
        };
        public String[] isError = {      
                        "Error",    
                        "Error :Reader Not Connected",              
                        "Error :Card Not Present",
                        "Error :Invalid Keys", 
                        "Error :Authentication Failed",  
                        "Error :Read Failure",              
                        "Error :Write Failure",    
                        "Error :IncompatibleCard",         
                        "Error :InSufficientUIDLength",            
                        "Error :InvalidKeysLength", 
                        "Error :InsufficientReadBufferSize",   
                        "Error :InvalidWriteDataLength",                 
                        "Error :InvalidWriteData",           
                        "Error :InvalidBlockNo",              
                        "Error :ValueBlockNotInitialised",              
                        "Error :IncrementFailure",              
                        "Error :DecrementFailure",               
                        "Error :InvalidSectorNo",            
                         };
        #region Form Methods


        #endregion



        //#region Common Methods

        //public String WriteDataBlock(String Block_NO, string Ascii_Data)
        //{
        //    byte[] data = new byte[16];
        //    ErrorCode = 0;
        //    int i;
        //    string HexData = ConvertToHEX(Ascii_Data);
        //    if (HexData.Length % 2 == 0)
        //    {
        //        for (i = 0; i <= HexData.Length / 2 - 1; i++)
        //        {
        //            data[i] = Convert.ToByte(HexData.Substring(i * 2, 2), 16);
        //        }
        //        if (Write_Data_Block(0, Convert.ToByte(Block_NO), ref data[0], Convert.ToByte(i), ref ErrorCode))
        //        {
        //            msg = "1";
        //        }
        //        else
        //        {
        //            msg = GetErrorMessage(ErrorCode);
        //        }
        //    }
        //    else
        //    {
        //        msg = "Enter Proper HEX Data";
        //    }

        //    return msg;
        //}

        //public void Clear_Card()
        //{
        //    byte[] data = new byte[16];
        //    ErrorCode = 0;
        //    int i;
        //    string HexData = "00000000000000000000000000000000";
        //    string[] Blocks = { "1", "2", "4", "5", "6", "8", };
        //    foreach (string Block_NO in Blocks)
        //    {
        //        if (HexData.Length % 2 == 0)
        //        {
        //            for (i = 0; i <= HexData.Length / 2 - 1; i++)
        //            {
        //                data[i] = Convert.ToByte(HexData.Substring(i * 2, 2), 16);
        //            }
        //            if (Write_Data_Block(0, Convert.ToByte(Block_NO), ref data[0], Convert.ToByte(i), ref ErrorCode))
        //            {
        //                msg = "1";
        //            }
        //            else
        //            {
        //                msg = GetErrorMessage(ErrorCode);
        //            }
        //        }
        //        else
        //        {
        //            msg = "Enter Proper HEX Data";
        //        }
        //    }
        //}

        //public String ReadDataBlock(String Block_NO)
        //{
        //    byte[] data = new byte[NoOfBytesPerBlock];
        //    ErrorCode = 0;

        //    if (Read_Data_Block(0, Convert.ToByte(Block_NO), ref data[0], Convert.ToByte(data.Length), ref  ErrorCode))
        //    {
        //        msg = "";
        //        msg = ConvertToASCII(BitConverter.ToString(data).Replace("-", string.Empty));

        //    }
        //    else
        //    {
        //        msg = GetErrorMessage(ErrorCode);
        //    }

        //    return msg;


        //}

        //public string get_RFID()
        //{
        //    byte[] UID = new byte[10];
        //    int uidlength;
        //    ErrorCode = 0;
        //    uidlength = GetUID(0, ref UID[0], Convert.ToByte(UID.Length), ref ErrorCode);
        //    msg = "";
        //    if (uidlength != 0)
        //    {
        //        msg = BitConverter.ToString(UID, 0, uidlength).Replace("-", String.Empty);
        //    }
        //    else
        //    {
        //        msg = GetErrorMessage(ErrorCode);
        //    }

        //    return msg;
        //}

        //#endregion



        #region Common Methods For ACR122

        public String WriteDataBlock(String Block_NO, string Ascii_Data)
        {
            //byte[] data = new byte[16];
            //ErrorCode = 0;
            //int i;
            //string HexData = ConvertToHEX(Ascii_Data);
            //if (HexData.Length % 2 == 0)
            //{
            //    for (i = 0; i <= HexData.Length / 2 - 1; i++)
            //    {
            //        data[i] = Convert.ToByte(HexData.Substring(i * 2, 2), 16);
            //    }
            //    if (Write_Data_Block(0, Convert.ToByte(Block_NO), ref data[0], Convert.ToByte(i), ref ErrorCode))
            //    {
            //        msg = "1";
            //    }
            //    else
            //    {
            //        msg = GetErrorMessage(ErrorCode);
            //    }
            //}
            //else
            //{
            //    msg = "Enter Proper HEX Data";
            //}


            int IntLocErrorCode = 0;

            msg = objACR.FunWriteValue(int.Parse(Block_NO), Ascii_Data, out IntLocErrorCode);

            if (IntLocErrorCode != 0)
            {
                msg = isError[IntLocErrorCode].ToString();
            }
           

            return msg;
        }

        public void Clear_Card()
        {
            byte[] data = new byte[16];
            ErrorCode = 0;
            int i;
            string HexData = "00000000000000000000000000000000";
            string[] Blocks = { "1", "2", "4", "5", "6", "8", };
            foreach (string Block_NO in Blocks)
            {
                int IntLocErrorCode = 0;
                objACR.FunWriteValue(int.Parse(Block_NO), HexData, out IntLocErrorCode);
            }
        }

        public String ReadDataBlock(String Block_NO)
        {
            byte[] data = new byte[NoOfBytesPerBlock];
            ErrorCode = 0;

            //if (Read_Data_Block(0, Convert.ToByte(Block_NO), ref data[0], Convert.ToByte(data.Length), ref  ErrorCode))
            //{
            //    msg = "";
            //    msg = ConvertToASCII(BitConverter.ToString(data).Replace("-", string.Empty));

            //}
            //else
            //{
            //    msg = GetErrorMessage(ErrorCode);
            //}
            int IntLocErrorCode = 0;

            string StrLocReturnVale = objACR.FunStrReadValue(int.Parse(Block_NO), out IntLocErrorCode);
            if (IntLocErrorCode == 0)
            {
                msg = StrLocReturnVale;
            }
            else
            {
                msg = isError[IntLocErrorCode].ToString();
            }



            return msg;


        }

        public string get_RFID()
        {
            int IntLocErrorCode = 0;

            string StrLocReturnVale = objACR.FunStrReadValue(0, out IntLocErrorCode);
            if (IntLocErrorCode == 0)
            {
                msg = ConvertToHEX(StrLocReturnVale);
            }
            else
            {
                msg = isError[IntLocErrorCode].ToString();
            }



            return msg;
        }

        #endregion



        #region Uitilities

        public string GetErrorMessage(int ErrorCode)
        {
            switch (ErrorCode)
            {
                case -1:
                    return "Error :ReaderNotDetected";
                case -2:
                    return "Error :CardNotPresent";
                case -3:
                    return "Error :IncompatibleCard";
                case -4:
                    return "Error :InSufficientUIDLength";
                case -5:
                    return "Error :InvalidKeysLength";
                case -6:
                    return "Error :InvalidKeys";
                case -7:
                    return "Error :AuthenticationFailed";
                case -8:
                    return "Error :InsufficientReadBufferSize";
                case -9:
                    return "Error :ReadFailure";
                case -10:
                    return "Error: WriteFailure";
                case -11:
                    return "Error :InvalidWriteDataLength";
                case -12:
                    return "Error :InvalidWriteData";
                case -13:
                    return "Error :InvalidBlockNo";
                case -14:
                    return "Error :ValueBlockNotInitialised";
                case -15:
                    return "Error :IncrementFailure";
                case -16:
                    return "Error: DecrementFailure";
                case -17:
                    return "Error :InvalidSectorNo";
                default:
                    return null;
            }
        }

        public string ConvertToHEXString(byte data)
        {
            return data.ToString("X2");
        }

        public string ConvertToASCII(string strHex)
        {
            string strAscii = "";
            if ((strHex.Length % 2) == 0)
            {
                for (int i = 0; i <= strHex.Length - 1; i += 2)
                {
                    strAscii = strAscii + Convert.ToChar(Convert.ToInt32(strHex.Substring(i, 2), 16));
                }
            }
            else
            {
                strAscii = "Invalid HEX Data";
            }
            return strAscii;
        }

        public string ConvertToHEX(string strASCII)
        {
            string strHEX = "";
            foreach (char c in strASCII)
            {
                strHEX = strHEX + Convert.ToInt32(c).ToString("X2");
            }
            return strHEX;
        }

        #endregion


    }
}
