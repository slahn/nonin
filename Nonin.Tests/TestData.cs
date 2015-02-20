using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonin.Tests
{
    static class TestData
    {
        public static string[] RandomMales = new[] {
            "21034814500",
            "26077938760",
            "29011288160",
            "08092135560",
            "02041639574",
            "25067148161",
            "04041502989",
            "15080779728",
            "06054332392",
            "15118313930",
            "17024749347",
            "17101011372",
            "02033549159",
            "12028626795",
            "25080199980",
            "22043700778",
            "16094805124",
            "10010289942",
            "15078102397",
            "05048019104",
            "04097947576",
            "06010320184",
            "28038208197",
            "11049532360",
            "10056720729",
            "06033316370",
            "28024210385",
            "27089229117",
            "18070472976",
            "28122418938",
            "28044231346",
            "12046504923",
        };

        public static string[] RandomFemales = new[] {
            "27123326615",
            "16070870618",
            "02041639493",
            "25067148080",
            "08060618217",
            "23055438218",
            "15080779647",
            "25096014859",
            "06054332201",
            "16121941299",
            "17024749266",
            "17101011291",
            "02033549078",
            "12028626604",
            "22043700697",
            "16094805043",
            "10010289861",
            "15078102206",
            "05048019023",
            "04097947495",
            "28038208006",
            "10056720648",
            "27089229036",
            "18070472895",
            "28122418857",
            "13128045818",
            "28044231265",
            "12046504842",
            "22045034627",
            "25061041608",
            "16031021409",
            "08023725097",
            "25085523085",
            "30097320280",
            "15038302601",
            "13100669441",
        };


        public static IEnumerable<string> RandomNumbers
        {
            get { return RandomMales.Concat(RandomFemales);  }
        }

        public static string[] InvalidControlDigits = new[] {
            "18099805991",
            "53124717928"
        };


        public class TestCase
        {
            public string Number { get; set; }
            public Gender Gender { get; set; }
            public DateTime DateOfBirth { get; set; }
            public NinKind NumberKind {get; set;}
        }

        public static IEnumerable<TestCase> ReadTestDataFromFile(string fileName)
        {
            foreach (var line in File.ReadLines(fileName))
            {
                var parts = line.Split('\t');

                var dateparts = parts[2].Split('.');
                var day = int.Parse(dateparts[0]);
                var month = int.Parse(dateparts[1]);
                var year = int.Parse(dateparts[2]);

                //The testcase file contains N/D/H for BirthNumber/DNumber/HNumber
                var kind = parts[3].Equals("N") ? NinKind.BirthNumber : parts[3].Equals("D") ? NinKind.DNumber : NinKind.HNumber;
                                
                yield return new TestCase()
                {
                    Number = parts[0],
                    Gender = parts[1].Equals("m", StringComparison.InvariantCultureIgnoreCase) ? Gender.Male : Gender.Female,
                    DateOfBirth = new DateTime(year, month, day, 0,0,0, DateTimeKind.Utc),
                    NumberKind = kind
                };
            }
        }

        public static IEnumerable<TestCase> AllTestCases()
        {
            return
                ReadTestDataFromFile("testdata-d.txt")
                .Concat(
                    ReadTestDataFromFile("testdata-n.txt")
                )
                .Concat(
                    ReadTestDataFromFile("testdata-h.txt")
                );
        }

    }
}
