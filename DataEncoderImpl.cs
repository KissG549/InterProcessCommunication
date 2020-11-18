using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InterProcessCommunication
{
    class DataEncoderImpl
    {
        public static void Decapsulate(string pData)
        {

            Person samplePerson = new Person();
            Knowledge knowledge = new Knowledge();

            try
            {
                using (JsonDocument doc = JsonDocument.Parse(pData))
                {
                    foreach (JsonProperty element in doc.RootElement.EnumerateObject())
                    {
                        System.Console.WriteLine(element.Value.ToString());
                        
                        if (element.Name.CompareTo("samplePerson") == 0)
                        {
                            samplePerson = JsonSerializer.Deserialize<Person>(element.Value.ToString());
                        }
                        else if (element.Name.CompareTo("sampleKnowledge") == 0)
                        {
                            knowledge = System.Text.Json.JsonSerializer.Deserialize<Knowledge>(element.Value.ToString());
                        }
                    }
                }
            }
            catch (System.Text.Json.JsonException e)
            {
                System.Console.WriteLine("Can't Deserialize JSON!");
            }

            samplePerson.Print();
            knowledge.Print();
        }
    }

}
