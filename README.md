# WebCrawling
A collection of portable class libraries for collecting web data written in C#. 
Now it has been ported to .NET Core.

## Lapis.WebCrawling
[Lapis.WebCrawling](src/Lapis.WebCrawling) provides basic infrastructure for 
downloading web pages.

## Lapis.WebCrawling.HtmlParsing
[Lapis.WebCrawling.HtmlParsing](src/Lapis.WebCrawling.HtmlParsing) mainly contains:
- HTML parser and DOM representation;
- css selectors.

Below is an example using a css selector to search a node in HTML DOM.
```html
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Test Document</title>
        <link rel="copyright copyleft" hreflang="en-us">
	</head>
	<body>
	    <h1>This is just a test document</h1>
	    <p>It will be used in tests.</p>
	    <p id="info">It probably will not be used anywhere else.</p>
	    <h2>Really, it's unfortunate.</h2>
	    <q lang="en-us">Here's a quotation.</q>
	    <p id="google">You might want to check out <a href="http://www.google.com">Google</a></p>
	    <form action="">
	        <input type="text" disabled="disabled">
	        <input type="text">
	        <input type="checkbox" checked="checked">
	    </form>
	    <p class="more">Nothing to really talk about.</p>
	</body>
</html>
```

```csharp
HtmlDocument doc = Html.Parse(html);
var tag = doc.Find("body > :last-child") as HtmlElement;
Console.WriteLine(tag.Name);                        // p
Console.WriteLine(tag.Attribute("class").Value);    // more
Console.WriteLine(tag.Children[0]);      // Nothing to really talk about.
```

## Lapis.WebCrawling.Processing
[Lapis.WebCrawling.Processing](src/Lapis.WebCrawling.Processing) provides basic 
infrastructure for extracting data from HTML.