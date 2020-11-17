using System;
using System.Collections.Generic;
using System.Text;

namespace InterProcessCommunication
{
    interface IDataEncoder
    {
        abstract byte[] Encapsulate(string pData);
        abstract string Decapsulate(byte[] pData);
    }
}
