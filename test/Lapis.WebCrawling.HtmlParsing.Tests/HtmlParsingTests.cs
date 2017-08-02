using Lapis.WebCrawling.HtmlParsing;
using Lapis.WebCrawling.HtmlParsing.Tokenization;
using Lapis.WebCrawling.HtmlParsing.TreeConstruction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationSample
{
    [TestClass]
    public class HtmlParsingTests
    {  
        [TestMethod]
        public void TokenizerTest()
        {
            var input = File.ReadAllText("test_data/test.html");
            var expected = File.ReadAllText("test_data/tokens.expected.txt");

            var htmlTokenizer = new Tokenizer(input);
            
            string actual;
            {
                var sb = new StringBuilder();
                Token token;
                while (!((token = htmlTokenizer.Read()) is EOFToken))
                {
                    sb.AppendLine(token.ToString());
                }
                actual = sb.ToString();                 
            }
            if (expected != actual)
            {
                File.WriteAllText("test_data/tokens.actual.txt", actual);
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TreeConstructorTest()
        {            
            var input = File.ReadAllText("test_data/test.html");
            var expected = File.ReadAllText("test_data/tree.expected.txt");

            var treeConstructor = new TreeConstructor(input);
            var tree = treeConstructor.Parse();
            var actual = tree.ToString();
            if (expected != actual)
            {
                File.WriteAllText("test_data/tree.actual.txt", actual);
                Assert.Fail();
            }
        }

        [TestMethod]
        public void HtmlToXmlTest()
        {   
            var input = File.ReadAllText("test_data/test.html");
            var expected = File.ReadAllText("test_data/test.expected.xml");

            var xml = Html.ToXml(input);
            var actual = xml.ToString();
            if (expected != actual)
            {
                File.WriteAllText("test_data/test.actual.xml", actual);
                Assert.Fail();
            }
        }
    }
}
