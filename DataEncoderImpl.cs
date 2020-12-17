using System;
using System.Text.Json;
using System.Threading;

namespace InterProcessCommunication
{
    class DataEncoderImpl
    {
        public static void Decapsulate(string pData)
        {

            Person samplePerson = new Person();
            Knowledge sampleKnowledge = new Knowledge();
            EnvironmentDetails sampleEnvironment = new EnvironmentDetails();
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(pData))
                {
                    foreach (JsonProperty element in doc.RootElement.EnumerateObject())
                    {
                        Console.WriteLine("Deserialize data:");
                        System.Console.WriteLine(element.Value.ToString());
                        
                        if (element.Name.CompareTo("samplePerson") == 0)
                        {
                            samplePerson = JsonSerializer.Deserialize<Person>(element.Value.ToString());
                            samplePerson.Print();
                        }
                        else if (element.Name.CompareTo("sampleKnowledge") == 0)
                        {
                            sampleKnowledge = JsonSerializer.Deserialize<Knowledge>(element.Value.ToString());
                            sampleKnowledge.Print();
                        }
                        else if (element.Name.CompareTo("sampleEnvironment") == 0)
                        {
                            sampleEnvironment = JsonSerializer.Deserialize<EnvironmentDetails>(element.Value.ToString());
                            sampleEnvironment.Print();
                        }
                    }
                }
            }
            catch (JsonException e)
            {
                Console.WriteLine("Can't Deserialize JSON!");
            }

        }

        // Example data read from console:
        //public static void SendSampleDataFromConsole(Func<string, int> SendFunction)
        //{
        //    Console.WriteLine("Data exchange from console. Please provide the required informations!");

        //    EnvironmentDetails sampleEnvironment = new EnvironmentDetails
        //    {
        //        StationName = "SampleDevice",
        //        Temperature = 25,
        //        Humidity = 42
        //    };

        //    while (true)
        //    {
        //        try
        //        {
        //            Console.Write("Enter Station name (text): ");
        //            sampleEnvironment.StationName = Console.ReadLine();
        //            Console.Write("Enter Temperature value (only numbers accepted): ");
        //            sampleEnvironment.Temperature = Int32.Parse(Console.ReadLine());
        //            Console.Write("Enter Humidity value (only numbers accepted: ");
        //            sampleEnvironment.Humidity = Int32.Parse(Console.ReadLine());
        //            break;
        //        }catch (Exception e)
        //        {
        //            Console.WriteLine("Wrong input parameters! Provide valid name, temp and humidity values!");
        //        }
        //    }

        //    sampleEnvironment.Print();

        //    string jsonString = JsonSerializer.Serialize(new { sampleEnvironment });

        //    Console.WriteLine(JsonSerializer.Serialize(jsonString));
        //    SendFunction(jsonString);
        //}

        public static void SendSampleData(Func<string, int> SendFunction)
        {
            Console.WriteLine("Send some sample data to the other side:");
            Thread.Sleep(500);
            Person samplePerson = new Person
            {
                Name = "First Person",
                Age = 30,
                Height = 170
            };

            Knowledge sampleKnowledge = new Knowledge
            {
                MotivationLevel = 11,
                Background = 8,
                ExperienceLevel = 10
            };

            samplePerson.Print();
            sampleKnowledge.Print();

            string jsonString = JsonSerializer.Serialize(new { samplePerson, sampleKnowledge });

            Console.WriteLine("Serialized data:");
            Console.WriteLine(jsonString);

            SendFunction(jsonString);
        }

        public static void SendRandomSampleData(Func<string, int> SendFunction)
        {
            Console.WriteLine("Send some random generated data to the other side:");
            Random rand = new Random();
            Person samplePerson = new Person
            {
                Name = "Person" + rand.Next(0,100),
                Age = rand.Next(18, 65),
                Height = rand.Next(160, 210) 
            };

            Knowledge sampleKnowledge = new Knowledge
            {
                MotivationLevel = rand.Next(1,20),
                Background = rand.Next(1,20),
                ExperienceLevel = rand.Next(1,20)
            };

            EnvironmentDetails sampleEnvironment = new EnvironmentDetails
            {
                StationName = "Device" + rand.Next(1,100),
                Temperature = rand.Next(-100,200),
                Humidity = rand.Next(10,110)
            };

            samplePerson.Print();
            sampleKnowledge.Print();
            sampleEnvironment.Print();

            string jsonString = JsonSerializer.Serialize(new { samplePerson, sampleKnowledge, sampleEnvironment });

            Console.WriteLine("Serialized data:");
            Console.WriteLine(jsonString);

            SendFunction(jsonString);
        }

    }

}
