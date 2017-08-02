using Lapis.WebCrawling.HtmlParsing;
using Lapis.WebCrawling.HtmlParsing.CssSelectors;
using Lapis.WebCrawling.HtmlParsing.TreeConstruction;
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
    public class CssSelectorTests
    {
        HtmlDocument doc;

        [TestInitialize]
        public void Setup()
        {
            var html = File.ReadAllText("test_data/test.html");
            doc = Html.Parse(html);
        }

        [TestMethod]
        public void TestTypeSelector()
        {
            var tag = doc.Find("p") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("It will be used in tests.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestFindFail()
        {
            var tag = doc.Find("fake");
            Assert.IsNull(tag);
        }

        [TestMethod]
        public void TestFindAll()
        {
            var tags = doc.FindAll("fake");
            Assert.AreEqual(0, tags.Count());

            tags = Html.Parse("<html><p><b></p><p></p>").FindAll("p");
            Assert.AreEqual(2, tags.Count());
            Assert.AreEqual("p", (tags.First() as HtmlElement).Name);
            Assert.AreEqual("b", (tags.First().Children().First() as HtmlElement).Name);
            Assert.AreEqual("p", (tags.Last() as HtmlElement).Name);
            Assert.AreEqual(0, tags.Last().Children().Count());
        }


        [TestMethod]
        public void TestDescendantCombinator()
        {
            var tag = doc.Find("html p") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("It will be used in tests.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestChildCombinator()
        {
            var tag = doc.Find("html > p") as HtmlElement;
            Assert.IsNull(tag);

            tag = doc.Find("body > p") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("It will be used in tests.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestSiblingCombinator()
        {
            var tag = doc.Find("body + h1") as HtmlElement;
            Assert.IsNull(tag);

            tag = doc.Find("h1 + p") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("It will be used in tests.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestGeneralSiblingCombinator()
        {
            var tag = doc.Find("body ~ h2") as HtmlElement;
            Assert.IsNull(tag);

            tag = doc.Find("h1 ~ h2") as HtmlElement;
            Assert.AreEqual("h2", tag.Name);
            Assert.AreEqual("Really, it's unfortunate.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestIDFilter()
        {
            var tag = doc.Find("#info") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("info", tag.Attribute("id").Value);
            Assert.AreEqual("It probably will not be used anywhere else.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestClassFilter()
        {
            var tag = doc.Find(".more") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("more", tag.Attribute("class").Value);
            Assert.AreEqual("Nothing to really talk about.", tag.Children[0].ToString());
        }

        

        [TestMethod]
        public void TestAttributeExistsSelector()
        {
            var tag = doc.Find("[id]") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("info", tag.Attribute("id").Value);
            Assert.AreEqual("It probably will not be used anywhere else.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestAttributeEqualsSelector()
        {
            var tag = doc.Find("[id=fake]") as HtmlElement;
            Assert.IsNull(tag);

            tag = doc.Find("[id=info]") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("info", tag.Attribute("id").Value);
            Assert.AreEqual("It probably will not be used anywhere else.", tag.Children[0].ToString());

            tag = doc.Find("[id='fake']") as HtmlElement;
            Assert.IsNull(tag);

            tag = doc.Find("[id='info']") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("info", tag.Attribute("id").Value);
            Assert.AreEqual("It probably will not be used anywhere else.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestAttributeDashFilter()
        {
            var tag = doc.Find("[hreflang|=fake]") as HtmlElement;
            Assert.IsNull(tag);

            tag = doc.Find("[hreflang|=en]") as HtmlElement;
            Assert.AreEqual("link", tag.Name);
            Assert.AreEqual("copyright copyleft", tag.Attribute("rel").Value);
            Assert.AreEqual("en-us", tag.Attribute("hreflang").Value);
        }

        [TestMethod]
        public void TestAttributeIncludesFilter()
        {
            var tag = doc.Find("[rel~=fake]") as HtmlElement;
            Assert.IsNull(tag);

            tag = doc.Find("[rel~=copyleft]") as HtmlElement;
            Assert.AreEqual("link", tag.Name);
            Assert.AreEqual("copyright copyleft", tag.Attribute("rel").Value);
            Assert.AreEqual("en-us", tag.Attribute("hreflang").Value);
        }

        [TestMethod]
        public void TestAttributePrefixFilter()
        {
            var tag = doc.Find("[id^=fake]") as HtmlElement;
            Assert.IsNull(tag);

            tag = doc.Find("[id^=in]") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("info", tag.Attribute("id").Value);
            Assert.AreEqual("It probably will not be used anywhere else.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestAttributeSuffixSelector()
        {
            var tag = doc.Find("[id$=fake]") as HtmlElement;
            Assert.IsNull(tag);

            tag = doc.Find("[id$=fo]") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("info", tag.Attribute("id").Value);
            Assert.AreEqual("It probably will not be used anywhere else.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestAttributeSubstringSelector()
        {
            var tag = doc.Find("[id*=fake]") as HtmlElement;
            Assert.IsNull(tag);

            tag = doc.Find("[id*=nf]") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("info", tag.Attribute("id").Value);
            Assert.AreEqual("It probably will not be used anywhere else.", tag.Children[0].ToString());
        }
               
        [TestMethod]
        public void TestNthChildFilter()
        {
            var tag = doc.Find("body :nth-child(3)") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("info", tag.Attribute("id").Value);
            Assert.AreEqual("It probably will not be used anywhere else.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestNthLastChildFilter()
        {
            var tag = doc.Find("head :nth-last-child(1)") as HtmlElement;
            Assert.AreEqual("link", tag.Name);
            Assert.AreEqual("copyright copyleft", tag.Attribute("rel").Value);
            Assert.AreEqual("en-us", tag.Attribute("hreflang").Value);
        }

        [TestMethod]
        public void TestNthOfTypeFilter()
        {
            var tag = doc.Find("p:nth-of-type(2)") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("info", tag.Attribute("id").Value);
            Assert.AreEqual("It probably will not be used anywhere else.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestNthLastOfTypeFilter()
        {
            var tag = doc.Find("p:nth-last-of-type(1)") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("more", tag.Attribute("class").Value);
            Assert.AreEqual("Nothing to really talk about.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestFirstChildFilter()
        {
            var tag = doc.Find("head :first-child") as HtmlElement;
            Assert.AreEqual("title", tag.Name);
            Assert.AreEqual("Test Document", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestLastChildFilter()
        {
            var tag = doc.Find("body > :last-child") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("more", tag.Attribute("class").Value);
            Assert.AreEqual("Nothing to really talk about.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestFirstOfTypeFilter()
        {
            var tag = doc.Find("p:first-of-type") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("It will be used in tests.", tag.Children[0].ToString());

            //make sure it finds the first sibling of type, not next sibling of type
            tag = doc.Find("#info:first-of-type") as HtmlElement;
            Assert.IsNull(tag);
        }

        [TestMethod]
        public void TestLastOfTypeFilter()
        {
            var tag = doc.Find("p:last-of-type") as HtmlElement;
            Assert.AreEqual("p", tag.Name);
            Assert.AreEqual("more", tag.Attribute("class").Value);
            Assert.AreEqual("Nothing to really talk about.", tag.Children[0].ToString());
        }

        [TestMethod]
        public void TestOnlyChildFilter()
        {
            var tag = doc.Find("#google :only-child") as HtmlElement;
            Assert.AreEqual("a", tag.Name);
            Assert.AreEqual("http://www.google.com", tag.Attribute("href").Value);
            Assert.AreEqual("Google", tag.Children[0].ToString());

            tag = doc.Find("body:only-child") as HtmlElement;
            Assert.IsNull(tag);
        }

        [TestMethod]
        public void TestOnlyOfTypeFilter()
        {
            var tag = doc.Find("a:only-of-type") as HtmlElement;
            Assert.AreEqual("a", tag.Name);
            Assert.AreEqual("http://www.google.com", tag.Attribute("href").Value);
            Assert.AreEqual("Google", tag.Children[0].ToString());

            tag = doc.Find("p:only-of-type") as HtmlElement;
            Assert.IsNull(tag);
        }

        [TestMethod]
        public void TestEmptyFilter()
        {
            var tag = doc.Find("link:empty") as HtmlElement;
            Assert.AreEqual("link", tag.Name);
            Assert.AreEqual("copyright copyleft", tag.Attribute("rel").Value);
            Assert.AreEqual("en-us", tag.Attribute("hreflang").Value);

            tag = doc.Find("p:empty") as HtmlElement;
            Assert.IsNull(tag);
        }
            

        [TestMethod]
        public void TestNotFilter()
        {
            var tag = doc.Find("head > :not(link)") as HtmlElement;
            Assert.AreEqual("title", tag.Name);
            Assert.AreEqual("Test Document", tag.Children[0].ToString());
        }
    }
}

