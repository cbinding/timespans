using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimespanLib
{
    [TestClass]
    public class TestYearSpanParser
    {
        //private IYearSpanParser parser = new YearSpanParser();

        private void compare(String input, IYearSpan expected, EnumLanguage language)
        {
            //IInterval<int> result = parser.Parse(input);
            IYearSpan result = YearSpan.Parse(input, language);
            compare(result, expected);
        }
        private void compare(IYearSpan input, IYearSpan expected)
        { 
            //Assert.AreEqual(expected, result, "spans are not equal");            
            Assert.IsNotNull(input, "input should not be null");
            Assert.IsNotNull(expected, "expected should not be null");
            Assert.AreEqual(expected.min, input.min, "unexpected min value");
            Assert.AreEqual(expected.max, input.max, "unexpected max value");
        }
          
        [TestMethod]
        public void TestYear1digit()
        {
            string input = @"1";
            IYearSpan expected = new YearSpan(1, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestYear2digit()
        {
            string input = @"15";
            IYearSpan expected = new YearSpan(15, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestYear3digit()
        {
            string input = @"152";
            IYearSpan expected = new YearSpan(152, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestYear4digit()
        {
            string input = @"1521";
            IYearSpan expected = new YearSpan(1521, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestYear5digit()
        {
            string input = @"15210";
            IYearSpan expected = new YearSpan(15210, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestYearAD()
        {
            string input = @"1521AD";
            IYearSpan expected = new YearSpan(1521, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestYearSpaceAD()
        {
            string input = @"1521 AD";
            IYearSpan expected = new YearSpan(1521, input);
            compare(input, expected, EnumLanguage.EN);                
        }
        [TestMethod]
        public void TestYearAD_IT()
        {
            string input = @"1521 d.C.";
            IYearSpan expected = new YearSpan(1521, input);
            compare(input, expected, EnumLanguage.IT);
        }

        [TestMethod]
        public void TestYearBP()
        {
            string input = @"1521 BP";
            IYearSpan expected = new YearSpan(429);
            compare(input, expected, EnumLanguage.EN);     
        }

        [TestMethod]
        public void TestYearSpaceBP()
        {
            string input = @"1521 BP";
            IYearSpan expected = new YearSpan(429, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestYearBC()
        {
            string input = @"1521 BC";
            IYearSpan expected = new YearSpan(-1521, input);
            compare(input, expected, EnumLanguage.EN);      
        }
        [TestMethod]
        public void TestYearBC_IT()
        {
            string input = @"1521 a.C.";
            IYearSpan expected = new YearSpan(-1521, input);
            compare(input, expected, EnumLanguage.IT);
        }
            
        [TestMethod]
        public void TestCircaYear()
        {
            string input = @"C. 1521";
            IYearSpan expected = new YearSpan(1521, input);
            compare(input, expected, EnumLanguage.EN);    
        }

        [TestMethod]
        public void TestCircaYearAD()
        {
            string input = @"C. 1521 AD";
            IYearSpan expected = new YearSpan(1521, input);
            compare(input, expected, EnumLanguage.EN);    
        }

        [TestMethod]
        public void TestCircaYearBC()
        {
            string input = @"C. 1521 BC";
            IYearSpan expected = new YearSpan(-1521, input);
            compare(input, expected, EnumLanguage.EN);   
        }

        [TestMethod]
        public void TestCircaYearBP()
        {
            string input = @"C. 1521 BP";
            IYearSpan expected = new YearSpan(429, input);
            compare(input, expected, EnumLanguage.EN);   
        }

        [TestMethod]
        public void TestNonMatchReturnsZero()
        {
            string input = @"abc/xyz";
            IYearSpan expected = new YearSpan(0, input);
            compare(input, expected, EnumLanguage.EN);            
        }

        [TestMethod]
        public void TestNamedPeriod()
        {
            string input = @"Georgian";
            IYearSpan expected = new YearSpan(1714, 1837, input, "en");           
            compare(input, expected, EnumLanguage.EN);             
        }
        
        [TestMethod]
        public void TestNamedPeriod_IT()
        {
            string input = @"Mesolitico";
            IYearSpan expected = new YearSpan(-9999, -5000, input, "it");
            compare(input, expected, EnumLanguage.IT);
        }

        [TestMethod]
        public void TestDecade_EN()
        {
            string input = @"1930s";
            IYearSpan expected = new YearSpan(1930, 1939, input);
            compare(input, expected, EnumLanguage.EN);      
        }

        [TestMethod]
        public void TestDecade_IT()
        {
            string input = @"Intorno al decennio 1850esimo";
            IYearSpan expected = new YearSpan(1850, 1859, input);
            compare(input, expected, EnumLanguage.IT);
        }

        [TestMethod]
        public void TestCircaDotDecade()
        {
            string input = @"C. 1930s";
            IYearSpan expected = new YearSpan(1930, 1939, input);
            compare(input, expected, EnumLanguage.EN);   
        }

        [TestMethod]
        public void TestCircaDotDecadeApostrophe()
        {
            string input = @"C. 1930's";
            IYearSpan expected = new YearSpan(1930, 1939, input);
            compare(input, expected, EnumLanguage.EN);   
        }


        [TestMethod]
        public void TestLookupEnumDayEN()
        {
            EnumDay expected = EnumDay.MON;
            EnumDay actual = Rx.Lookup<EnumDay>.Match(@"Monday", EnumLanguage.EN);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestLookupEnumDayIT()
        {
            EnumDay expected = EnumDay.MON;
            EnumDay actual = Rx.Lookup<EnumDay>.Match(@"Lunedì", EnumLanguage.IT);
            Assert.AreEqual(expected, actual);
        } 

        [TestMethod]
        public void TestMonthYear()
        {
            string input = @"January 1900";
            IYearSpan expected = new YearSpan(1900, 1900, input);
            compare(input, expected, EnumLanguage.EN);
        }   
       
        [TestMethod]
        public void Test1DigitMaxYear()
        {
            string input = @"1521-7";
            IYearSpan expected = new YearSpan(1521, 1527, input);
            //IInterval<int> result = parser.Parse(input);
            IYearSpan result = YearSpan.Parse(input, EnumLanguage.EN);
            compare(expected, result);
        }

        [TestMethod]
        public void Test2DigitMaxYear()
        {
            string input = @"1521-27";
            IYearSpan expected = new YearSpan(1521, 1527, input);
            compare(input, expected, EnumLanguage.EN);   
        }

        [TestMethod]
        public void TestOrdinalCenturyAD()
        {
            string input = @"FIFTH CENTURY"; 
            IYearSpan expected = new YearSpan(401, 500, input);
            compare(input, expected, EnumLanguage.EN);   
        }

        [TestMethod]
        public void TestOrdinalCenturyBC()
        {
            string input = @"FIFTH CENTURY BC";
            IYearSpan expected = new YearSpan(-500, -401, input);
            compare(input, expected, EnumLanguage.EN);   
        }

        [TestMethod]
        public void TestOrdinalCenturyBP()
        {
            string input = @"FIFTH CENTURY BP";
            IYearSpan expected = new YearSpan(1451, 1550, input);
            compare(input, expected, EnumLanguage.EN);   
        }
        
        [TestMethod]
        public void TestOrdinaCenturyEarlyAD()
        {
            string input = @"EARLY FIFTH CENTURY AD";
            IYearSpan expected = new YearSpan(401, 440, input);
            compare(input, expected, EnumLanguage.EN);   
        }

        [TestMethod]
        public void TestOrdinalCenturyEarlyBC()
        {
            string input = @"EARLY FIFTH CENTURY BC";
            IYearSpan expected = new YearSpan(-500, -460, input);
            compare(input, expected, EnumLanguage.EN);   
        }

        [TestMethod]
        public void TestOrdinalCenturyMidAD()
        {
            string input = @"MID FIFTH CENTURY AD";
            IYearSpan expected = new YearSpan(430, 470, input);
            compare(input, expected, EnumLanguage.EN);   
        }

        [TestMethod]
        public void TestOrdinalCenturyMidBC()
        {
            string input = @"MID FIFTH CENTURY BC";
            IYearSpan expected = new YearSpan(-470, -430, input);
            compare(input, expected, EnumLanguage.EN);   
        }

        [TestMethod]
        public void TestOrdinalCenturyLateAD()
        {
            string input = @"LATE FIFTH CENTURY AD";
            IYearSpan expected = new YearSpan(460, 500, input);
            compare(input, expected, EnumLanguage.EN);   
        }

        [TestMethod]
        public void TestOrdinalCenturyLateBC()
        {
            string input = @"LATE FIFTH CENTURY BC";
            IYearSpan expected = new YearSpan(-440, -401, input);
            compare(input, expected, EnumLanguage.EN);   
        }


        [TestMethod]
        public void TestCardinalCenturyAD()
        {
            string input = @"5TH CENTURY AD";
            IYearSpan expected = new YearSpan(401, 500, input);
            compare(input, expected, EnumLanguage.EN);   
        }

        [TestMethod]
        public void TestCardinalCenturyBC()
        {
            string input = @"5th CENTURY BC";
            IYearSpan expected = new YearSpan(-500, -401, input);
            compare(input, expected, EnumLanguage.EN);   
        }

        [TestMethod]
        public void TestCardinalCenturyAD_IT()
        {
            string input = @"sec. VIII d.C.";
            IYearSpan expected = new YearSpan(701, 800, input);
            IYearSpan result = YearSpan.Parse(input, EnumLanguage.IT);
            compare(result, expected);
        }

        [TestMethod]
        public void TestCardinalCenturyBC_IT()
        {
            string input = @"sec. VIII a.C.";
            IYearSpan expected = new YearSpan(-800, -701, input);
            IYearSpan result = YearSpan.Parse(input, EnumLanguage.IT);
            compare(result, expected);
        }

        [TestMethod]
        public void TestCardinalMillenniumAD()
        {
            string input = @"1st millennium AD";
            IYearSpan expected = new YearSpan(1, 1000, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestCardinalMillenniumBC()
        {
            string input = @"1st millennium BC";
            IYearSpan expected = new YearSpan(-1, -1000, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestFromCenturyToCenturyAD()
        {
            string input = @"4th-5th century";
            IYearSpan expected = new YearSpan(301, 500, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestFromCenturyToCenturyOrdinal()
        {
            string input = @"Late eighth to early ninth century";
            IYearSpan expected = new YearSpan(760, 840, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestFromCenturyToCenturyBC()
        {
            string input = @"4th-5th century BC";
            IYearSpan expected = new YearSpan(-500, -301, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestFromYearToYearAD()
        {
            string input = @"C. 1521-1585";
            IYearSpan expected = new YearSpan(1521, 1585, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestFromYearToYearBC()
        {
            string input = @"C. 1585-1521 BC";
            IYearSpan expected = new YearSpan(-1585, -1521, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestFromYearToYearBCReversed()
        {
            string input = @"C. 1521-1585 BC";
            IYearSpan expected = new YearSpan(-1585, -1521, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestFromYearToYearADReversed()
        {
            string input = @"C. 1585-1521 AD";
            IYearSpan expected = new YearSpan(1521, 1585, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestShortMaxYearAD()
        {
            string input = @"C. 1521-85";
            IYearSpan expected = new YearSpan(1521, 1585, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestMonthYearAD()
        {
            string input = @"April 1572";
            IYearSpan expected = new YearSpan(1572, input);
            compare(input, expected, EnumLanguage.EN);
        }

        [TestMethod]
        public void TestSeasonYearAD()
        {
            string input = @"Summer 1572";
            IYearSpan expected = new YearSpan(1572, input);
            compare(input, expected, EnumLanguage.EN);
        }


        [TestMethod]
        public void TestArrayOfInput()
        {
            string[] input = { @"5th CENTURY", @"6th CENTURY" };
            IYearSpan[] expected = 
            { 
                new YearSpan(401, 500, input[0]), 
                new YearSpan(501, 600, input[1]) 
            };
            IYearSpan[] result = YearSpan.Parse(input, EnumLanguage.EN);
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
