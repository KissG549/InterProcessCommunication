using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InterProcessCommunication
{
    class DataEncoderImpl
    {
        public string Encapsulate(string pData)
        {
            return JsonSerializer.Serialize(pData); ;
        }
        public string Decapsulate(byte[] pData)
        {
            string dataString;
            dataString = Encoding.Unicode.GetString(pData);

            return JsonSerializer.Deserialize<string>(dataString);
        }

    }
}
