using System;
using System.Collections.Generic;
using System.Text;

namespace InterProcessCommunication
{
    [System.Runtime.Serialization.DataContract]
    class Person
    {
        [System.Runtime.Serialization.DataMember]
        public string Name { get; set; }
        [System.Runtime.Serialization.DataMember]
        public int Age { get; set; }

        [System.Runtime.Serialization.DataMember]
        public int Height { get; set; }

        public void Print()
        {
            Console.WriteLine(
                "Name: {0}\nAge: {1}\nHeight: {2}"
                , Name, Age, Height);
        }
    }

    [System.Runtime.Serialization.DataContract]
    class Knowledge
    {
        [System.Runtime.Serialization.DataMember]
        public int MotivationLevel { get; set; }

        [System.Runtime.Serialization.DataMember]

        public int Background { get; set; }

        [System.Runtime.Serialization.DataMember]
        public int ExperienceLevel { get; set; }

        public void Print()
        {
            Console.WriteLine(
                "MotivationLevel: {0}\nBackground: {1}\nExperienceLevel: {2}"
                , MotivationLevel, Background, ExperienceLevel);
        }
    }

}
