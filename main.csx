#!/usr/bin/env dotnet-script

using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

if (Args.Count != 1 || !File.Exists(Args[0])) {
    Console.WriteLine("Please provide the location of a \"sitemap.xml\" as an argument. Example: dotnet script main.csx -- ../path/sitemap.xml");
    return 1;
}

/*
var schemas = new XmlSchemaSet();
using (var httpClient = new HttpClient()) {
    // schemas.Add("http://www.sitemaps.org/schemas/sitemap/0.9", XmlReader.Create("http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd"));
    var xsdStream = await httpClient.GetStreamAsync("http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");
    schemas.Add("http://www.sitemaps.org/schemas/sitemap/0.9", XmlReader.Create(xsdStream));
}
*/

// TODO: Finn en sitemap xsd som faktisk kan gi feil
string xsdMarkup =  
    @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>  
       <xsd:element name='Root'>  
        <xsd:complexType>  
         <xsd:sequence>  
          <xsd:element name='Child3' minOccurs='1' maxOccurs='1'/>  
          <xsd:element name='Child2' minOccurs='1' maxOccurs='1'/>  
         </xsd:sequence>  
        </xsd:complexType>  
       </xsd:element>  
      </xsd:schema>";  
XmlSchemaSet schemas = new XmlSchemaSet();  
schemas.Add("", XmlReader.Create(new StringReader(xsdMarkup)));  

var doc1 = new XDocument(  
    new XElement("Root",  
        new XElement("Child1", "content1"),  
        new XElement("Child2", "content1")  
    )  
);

Console.WriteLine("Validating doc1");  
bool errors = false;  
doc1.Validate(schemas, (o, e) =>  
                     {  
                         Console.WriteLine("{0}", e.Message);  
                         errors = true;  
                     });  
Console.WriteLine("doc1 {0}", errors ? "did not validate" : "validated");  
