using System;
using System.Collections.Generic;
using System.Text;

namespace InterProcessCommunication
{
    [System.Runtime.Serialization.DataContract]
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public int Height { get; set; }

        public void Print()
        {
            Console.WriteLine(
                "######################\nPERSON\n######################\nName: {0}\nAge: {1}\nHeight: {2}\n----------------------",
                Name, Age, Height);
        }
    }

    class Knowledge
    {
        public int MotivationLevel { get; set; }

        public int Background { get; set; }

        public int ExperienceLevel { get; set; }

        public void Print()
        {
            Console.WriteLine(
                "######################\nKNOWLEDGE\n######################\nMotivationLevel: {0}\nBackground: {1}\nExperienceLevel: {2}\n----------------------",
                MotivationLevel, Background, ExperienceLevel);
        }
    }

    class EnvironmentDetails
    {
        public string StationName { get; set; }
        public int Temperature { get; set; }
        public int Humidity { get; set; }

        public void Print()
        {
            Console.WriteLine(
                "######################\nENVIRONMENT DETAILS\n######################\nStation name: {0}\nTemperature: {1}\nHumidity: {2}\n----------------------",
                StationName, Temperature, Humidity);
        }
    }

}
