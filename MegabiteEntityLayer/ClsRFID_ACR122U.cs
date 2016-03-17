using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace MegabiteEntityLayer
{
    public class ClsRFID_ACR122U
    {
        private byte[] SendBuff = new byte[263];
        byte[] RecvBuff = new byte[263];
        int SendLen, RecvLen;
        int retCode, hContext, hCard, Protocol, Aprotocol;
        ModWinsCard.SCARD_READERSTATE RdrState;
        ModWinsCard.SCARD_IO_REQUEST pioSendRequest;
        string StrLocReaderName = string.Empty;


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

        public string FunStrReadValue(int IntLocBlockNumber, out int IntLocErrorCode)
        {
            IntLocErrorCode = 0;
            FunConnect(IntLocBlockNumber, out IntLocErrorCode);
            if (IntLocErrorCode == 0)
            {
                return FunReadBlockNumber(IntLocBlockNumber, out IntLocErrorCode);
            }
            else
            {
                return "";
            }
        }

        public string FunWriteValue(int IntLocBlockNumber, string StrLocWriteData, out int IntLocErrorCode)
        {
            
            IntLocErrorCode = 0;
            FunConnect(IntLocBlockNumber, out IntLocErrorCode);
            if (IntLocErrorCode == 0)
            {
                FunWriteDataOnBlock(IntLocBlockNumber, StrLocWriteData, out IntLocErrorCode);
                return "1";
            }
            else
            {
                return "0";
            }
        }

        private void FunInitialize()
        {
            string ReaderList = "" + Convert.ToChar(0);
            int indx;
            int pcchReaders = 0;
            string rName = "";

            //Establish Context
            retCode = ModWinsCard.SCardEstablishContext(ModWinsCard.SCARD_SCOPE_USER, 0, 0, ref hContext);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {

                //displayOut(1, retCode, "");

                return;

            }

            // 2. List PC/SC card readers installed in the system

            retCode = ModWinsCard.SCardListReaders(this.hContext, null, null, ref pcchReaders);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {

                //displayOut(1, retCode, "");

                return;
            }

            byte[] ReadersList = new byte[pcchReaders];

            // Fill reader list
            retCode = ModWinsCard.SCardListReaders(this.hContext, null, ReadersList, ref pcchReaders);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                //mMsg.AppendText("SCardListReaders Error: " + ModWinsCard.GetScardErrMsg(retCode));

                return;
            }
            else
            {
                //displayOut(0, 0, " ");
            }

            rName = "";
            indx = 0;

            //Convert reader buffer to string
            while (ReadersList[indx] != 0)
            {

                while (ReadersList[indx] != 0)
                {
                    rName = rName + (char)ReadersList[indx];
                    indx = indx + 1;
                }
                StrLocReaderName = rName;
                //Add reader name to list
                //cbReader.Items.Add(rName);
                rName = "";
                indx = indx + 1;

            }
        }

        private string FunConnect(int IntLocBlockNumber, out int IntLocErrorCode)
        {
            string ReaderList = "" + Convert.ToChar(0);
            int indx = 0;
            int pcchReaders = 0;
            string rName = "";

            retCode = ModWinsCard.SCardEstablishContext(ModWinsCard.SCARD_SCOPE_USER, 0, 0, ref hContext);

            retCode = ModWinsCard.SCardListReaders(this.hContext, null, null, ref pcchReaders);

            if (retCode == 0)
            {
                IntLocErrorCode = 0;
                byte[] ReadersList = new byte[pcchReaders];
                retCode = ModWinsCard.SCardListReaders(this.hContext, null, ReadersList, ref pcchReaders);

                while (ReadersList[indx] != 0)
                {

                    while (ReadersList[indx] != 0)
                    {
                        rName = rName + (char)ReadersList[indx];
                        indx = indx + 1;
                    }
                    StrLocReaderName = rName;
                    //Add reader name to list
                    //cbReader.Items.Add(rName);
                    rName = "";
                    indx = indx + 1;

                }
                retCode = ModWinsCard.SCardConnect(hContext, StrLocReaderName, ModWinsCard.SCARD_SHARE_SHARED,
                                                  ModWinsCard.SCARD_PROTOCOL_T0 | ModWinsCard.SCARD_PROTOCOL_T1, ref hCard, ref Protocol);
                if (retCode == 0)
                {
                    FunLoadKey(IntLocBlockNumber, out IntLocErrorCode);
                }
                else
                {
                    IntLocErrorCode = 2;
                }
            }
            else
            {
                IntLocErrorCode = 1;
            }
            return "";

        }

        private string FunLoadKey(int IntLocBlockNumber, out int IntLocErrorCode)
        {
            string StrLocReturnValue = string.Empty;
            ClearBuffers();
            // Load Authentication Keys command
            SendBuff[0] = 0xFF;                                                                        // Class
            SendBuff[1] = 0x82;                                                                        // INS
            SendBuff[2] = 0x00;                                                                        // P1 : Key Structure
            SendBuff[3] = 01;
            SendBuff[4] = 0x06;                                                                        // P3 : Lc
            SendBuff[5] = 0xFF;        // Key 1 value
            SendBuff[6] = 0xFF;        // Key 2 value
            SendBuff[7] = 0xFF;        // Key 3 value
            SendBuff[8] = 0xFF;        // Key 4 value
            SendBuff[9] = 0xFF;        // Key 5 value
            SendBuff[10] = 0xFF;       // Key 6 value

            SendLen = 11;
            RecvLen = 2;

            retCode = SendAPDU();
            string tmpStr = "";
            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                IntLocErrorCode = 3;
                return "";
            }
            else
            {
                IntLocErrorCode = 0;
                for (int indx = RecvLen - 2; indx <= RecvLen - 1; indx++)
                {

                    tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);

                }

            }
            if (tmpStr.Trim() != "90 00")
            {
                IntLocErrorCode = 3;
                //displayOut(4, 0, "Load authentication keys error!");
            }



            ClearBuffers();

            SendBuff[0] = 0xFF;                             // Class
            SendBuff[1] = 0x86;                             // INS
            SendBuff[2] = 0x00;                             // P1
            SendBuff[3] = 0x00;                             // P2
            SendBuff[4] = 0x05;                             // Lc
            SendBuff[5] = 0x01;                             // Byte 1 : Version number
            SendBuff[6] = 0x00;                             // Byte 2
            SendBuff[7] = (byte)IntLocBlockNumber;     // Byte 3 : Block number
            SendBuff[8] = 0x60;
            SendBuff[9] = byte.Parse(01.ToString(), System.Globalization.NumberStyles.HexNumber);        // Key 5 value
            SendLen = 10;
            RecvLen = 2;
            retCode = SendAPDU();
            tmpStr = "";

            for (int indx = 0; indx <= RecvLen - 1; indx++)
            {

                tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);

            }


            if (tmpStr.Trim() == "90 00")
            {

            }
            else
            {
                IntLocErrorCode = 4;
            }

            return "";

        }


        private string FunReadBlockNumber(int IntLocBlockNumber, out int IntLocErrorCode)
        {
            string StrLocReturnValue = string.Empty;
            ClearBuffers();
            SendBuff[0] = 0xFF;
            SendBuff[1] = 0xB0;
            SendBuff[2] = 0x00;
            SendBuff[3] = (byte)IntLocBlockNumber;
            if (IntLocBlockNumber == 0)
            {
                SendBuff[4] = (byte)4;
            }
            else
            {
                SendBuff[4] = (byte)16;
            }
            

            SendLen = 5;
            RecvLen = SendBuff[4] + 2;

            retCode = SendAPDU();
            string tmpStr = "";
            for (int indx = RecvLen - 2; indx <= RecvLen - 1; indx++)
            {

                tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
            }


            if (tmpStr.Trim() == "90 00")
            {
                tmpStr = "";
                for (int indx = 0; indx <= RecvLen - 3; indx++)
                {

                    tmpStr = tmpStr + Convert.ToChar(RecvBuff[indx]);
                }

                StrLocReturnValue = tmpStr;
                IntLocErrorCode = 0;
            }
            else
            {
                StrLocReturnValue = "";
                IntLocErrorCode = 5;
            }
            return StrLocReturnValue;
        }


        private void FunWriteDataOnBlock(int IntLocBlockNumber, string StrLocBlockData, out int IntLocErrorCode)
        {
            string tmpStr = string.Empty;
            tmpStr = StrLocBlockData;
            ClearBuffers();
            SendBuff[0] = 0xFF;                                     // CLA
            SendBuff[1] = 0xD6;                                     // INS
            SendBuff[2] = 0x00;                                     // P1
            SendBuff[3] = (byte)IntLocBlockNumber;            // P2 : Starting Block No.
            SendBuff[4] = (byte)16;            // P3 : Data length

            for (int indx = 0; indx <= (tmpStr).Length - 1; indx++)
            {

                SendBuff[indx + 5] = (byte)tmpStr[indx];

            }
            SendLen = SendBuff[4] + 5;
            RecvLen = 0x02;

            retCode = SendAPDU();

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                IntLocErrorCode = 6;
            }
            else
            {
                tmpStr = "";

                for (int indx = 0; indx <= RecvLen - 1; indx++)
                {

                    tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);

                }

            }
            if (tmpStr.Trim() == "90 00")
            {
                //tBinData.Text = "";
                IntLocErrorCode = 0;
            }
            else
            {
                IntLocErrorCode = 6;
            }



        }


        public int SendAPDU()
        {
            int indx;
            string tmpStr;

            pioSendRequest.dwProtocol = Aprotocol;
            pioSendRequest.cbPciLength = 8;

            // Display Apdu In
            tmpStr = "";
            for (indx = 0; indx <= SendLen - 1; indx++)
            {

                tmpStr = tmpStr + " " + string.Format("{0:X2}", SendBuff[indx]);

            }

            retCode = ModWinsCard.SCardTransmit(hCard, ref pioSendRequest, ref SendBuff[0], SendLen, ref pioSendRequest, ref RecvBuff[0], ref RecvLen);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                return retCode;
            }

            tmpStr = "";
            for (indx = 0; indx <= RecvLen - 1; indx++)
            {
                tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
            }

            return retCode;

        }

        private void ClearBuffers()
        {

            long indx;

            for (indx = 0; indx <= 262; indx++)
            {
                RecvBuff[indx] = 0;
                SendBuff[indx] = 0;
            }
        }
    }
}
