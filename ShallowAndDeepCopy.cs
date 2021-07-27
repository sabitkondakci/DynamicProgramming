using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ShallowDeepCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            SSNGenerator ssnGenerator = new(new Random()); // Social Security Number
            //var a = BenchmarkRunner.Run<SSNBenchmark>();
            Staff hrStaff = new(new QuickInfo("Jessy", "Herolt"),
            new DetailedInfo(Guid.NewGuid(),
                            "0001-89938749928-388293",
                            "Budgie Str. Cathood Avn. 33242 / 233",
                            new DateTime(1990, 03, 1), ssnGenerator.Ssn),
                            "dummyData");


            Staff shallowCopy = hrStaff.ShallowCopy();
            Staff deepCopy = hrStaff.DeepCopy();

            Print(hrStaff,"Original");
            
            // alter the values in hrStaff , DummyData is the only primitive type in Staff class
            hrStaff.QuickInformation.FirstName = "Dalton";
            hrStaff.QuickInformation.LastName = "Mayer";
            hrStaff.DummyData = "DummyData changed";
            hrStaff.DetailedInformation.Id = Guid.NewGuid();
            hrStaff.DetailedInformation.IBAN = "0001-19958649928-088293";
            hrStaff.DetailedInformation.Address = "Nowhere";
            hrStaff.DetailedInformation.BirthDate = new DateTime(1956, 1, 2);
            hrStaff.DetailedInformation.SSN = ssnGenerator.Ssn;

            Print(shallowCopy, "Shallow Copy");
            Print(deepCopy, "Deep Copy");
 

            static void Print(Staff hrStaff,string copyType)
            {
                Console.WriteLine($"{copyType}\n");
                Console.WriteLine($"FirstName:{hrStaff.QuickInformation.FirstName}\nLastName:{hrStaff.QuickInformation.LastName}\nDummyData:{hrStaff.DummyData}\n" +
                $"Detailed Information:\n" +
                $"  Id:{hrStaff.DetailedInformation.Id}\n  IBAN:{hrStaff.DetailedInformation.IBAN}\n" +
                $"  Address:{hrStaff.DetailedInformation.Address}\n  BirthDate:{hrStaff.DetailedInformation.BirthDate}\n" +
                $"  SSN:{hrStaff.DetailedInformation.SSN.treeDigits}-" +
                $"{hrStaff.DetailedInformation.SSN.twoDigits}-" +
                $"{hrStaff.DetailedInformation.SSN.fourDigits}\n");
            }
        }
    }


    public class SSNGenerator
    {
        public (int threeDigits, int twoDigits, int fourDigits) Ssn => GenerateSSN();
        private readonly Random _random;

        // struct doesn't accept a parameterless constructor
        // this is why I'm passing a random parameter
        public SSNGenerator(Random random)
        {
            _random = random;
        }

        private (int threeDigits, int twoDigits, int fourDigits) GenerateSSN()
        {

            var threeDigits = _random.Next(100, 1000);
            var twoDigits = _random.Next(10, 100);
            var fourDigits = _random.Next(1000, 10_000);

            return (threeDigits, twoDigits, fourDigits);
        }

    }

    public class QuickInfo
    {
        public QuickInfo(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public QuickInfo ShallowCopy()
        {
            return this.MemberwiseClone() as QuickInfo;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    public class DetailedInfo
    {
        public DetailedInfo(Guid id, string iban, string address, DateTime birthDate, (int treeDigits, int twoDigits, int fourDigits) ssn)
        {
            Id = id;
            IBAN = iban;
            Address = address;
            BirthDate = birthDate;
            SSN = ssn;
        }

        public DetailedInfo ShallowCopy()
        {
            return this.MemberwiseClone() as DetailedInfo;
        }

        public Guid Id { get; set; }
        public string IBAN { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public (int treeDigits, int twoDigits, int fourDigits) SSN { get; set; } // 123-42-6476 format    
    }

    public class Staff
    {
        public Staff(QuickInfo quicInfornamtion, DetailedInfo detailedInformation,string dummyData)
        {
            QuickInformation = quicInfornamtion;
            DetailedInformation = detailedInformation;
            DummyData = dummyData;
        }
        public QuickInfo QuickInformation{ get; set; }
        public DetailedInfo DetailedInformation { get; set; }
        public string DummyData { get; set; }
        
        public Staff ShallowCopy()
        {               
            return MemberwiseClone() as Staff;
        }
        
        public Staff DeepCopy()
        {
            var deepCopy = this.MemberwiseClone() as Staff;
            deepCopy.DetailedInformation = this.DetailedInformation.ShallowCopy(); 
            deepCopy.QuickInformation = this.QuickInformation.ShallowCopy();          
            return deepCopy;
        }
    }
}
