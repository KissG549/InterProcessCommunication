using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InterProcessCommunication
{
    class DataEncoderImpl : IDataEncoder
    {
        public byte[] Encapsulate(string pData)
        {
            string jsonString;
            jsonString = JsonSerializer.Serialize(pData);

            byte[] data = Encoding.Unicode.GetBytes(jsonString);
            // TODO
            return data;
        }
        public string Decapsulate(byte[] pData)
        {
            string dataString;
            dataString = Encoding.Unicode.GetString(pData);

            return JsonSerializer.Deserialize<string>(dataString);
        }

    }
}
