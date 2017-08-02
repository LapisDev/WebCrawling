using Lapis.WebCrawling.HtmlParsing;
using Lapis.WebCrawling.HtmlParsing.CssSelectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationSample
{
    [TestClass]
    public class CssSelectorParsingTests
    {
        [TestMethod]
        public void TokenizerTest()
        {            
            var input = "html > p";
            var expected = File.ReadAllText("test_data/csstokens.expected.txt");

            var tokenizer = new Tokenizer(input);
            string actual;
            {
                var sb = new StringBuilder();
                Token token;
                while (!((token = tokenizer.Next()).Type == TokenType.EOF))
                {
                    sb.AppendLine($"{token.Position}: {token.Type} {token.Value}");
                }
                actual = sb.ToString();                 
            }
            if (expected != actual)
            {
                File.WriteAllText("test_data/csstokens.actual.txt", actual);
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ParserTests()
        {
            var input = "html > p";
            var parser = new Parser(input);
            var expected = "html>p";
            string actual;
            {
                var selector = parser.Parse();
                actual = selector.ToString();
            }
            
            Assert.AreEqual(expected, actual);
        }

    }
}
