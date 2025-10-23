using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Collections.Generic;
/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/
namespace ConsoleApp1
{
    public class Program
    {
        // Update these URLs with your actual GitHub raw URLs
        public static string xmlURL = "https://raw.githubusercontent.com/lucmol319/lucmol319.github.io/main/Hotels.xml";
        public static string xmlErrorURL = "https://raw.githubusercontent.com/lucmol319/lucmol319.github.io/main/HotelsErrors.xml";
        public static string xsdURL = "https://raw.githubusercontent.com/lucmol319/lucmol319.github.io/main/Hotels.xsd";

        public static void Main(string[] args)
        {
            // Q3: You can pick two of three functions to test in the main method.
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine("Validation of Hotels.xml:");
            Console.WriteLine(result);
            Console.WriteLine();

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine("Validation of HotelsErrors.xml:");
            Console.WriteLine(result);
            Console.WriteLine();

            result = Xml2Json(xmlURL);
            Console.WriteLine("JSON Conversion:");
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                // Create XML reader settings
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;

                // Load the XSD schema
                settings.Schemas.Add(null, xsdUrl);

                // Collect validation errors
                List<string> errors = new List<string>();
                settings.ValidationEventHandler += (sender, e) =>
                {
                    errors.Add($"Line {e.Exception.LineNumber}, Position {e.Exception.LinePosition}: {e.Exception.Message}");
                };

                // Read and validate the XML
                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { } // Read through the entire document
                }

                // Return results
                if (errors.Count == 0)
                {
                    return "No Error";
                }
                else
                {
                    return string.Join(Environment.NewLine, errors);
                }
            }
            catch (Exception ex)
            {
                return $"Exception during validation: {ex.Message}";
            }
        }

        // Q2.2
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                // Load the XML from the given URL
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlUrl);

                // Convert the XML to JSON format
                string jsonText = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);

                // Verify the JSON can be deserialized back to XML (as required)
                try
                {
                    JsonConvert.DeserializeXmlNode(jsonText);
                }
                catch (Exception ex)
                {
                    return $"Error: Generated JSON cannot be deserialized: {ex.Message}";
                }

                return jsonText;
            }
            catch (Exception ex)
            {
                return $"Error during XML to JSON conversion: {ex.Message}";
            }
        }
    }
}