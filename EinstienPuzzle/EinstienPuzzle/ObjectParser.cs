using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;
using System.Text.RegularExpressions;

/** 
 *  Utility class handles interpolation with
 *  necessary local XML game data files.
 */
namespace EinstienPuzzle {

    class ObjectParser
    {
        
        /* Testing. */
        /*
        public static int Main()
        {

            ObjectParser parser = new ObjectParser();

            Console.Write(parser.getNodeContentWithXPath("/game/scenes/scene[@id='1']/sets/objects[@type='weapon']/object"));

            Console.ReadLine();
            return 0;
        }*/

        /* Constants necessary for defaulting. */

        private const string FILEPATH = "../../raw/";
        private const string DOCUMENT = "content.xml";

        /* Objects necessary for parsing. */

        // Store the document in memory.
        private XmlDocument xmlDoc = new XmlDocument();

        /* Constructors. */

        /**
         *  Public constructor presently initialises access
         *  to a default XML document.
         * 
         */
        public ObjectParser()
        {

            // Load the default document.
            xmlDoc.Load(FILEPATH + DOCUMENT);

        }

        /**
         *  Public constructor presently initialises access
         *  to a specified XML document.
         *  
         *  @param  path     The relative filepath to doc's root.
         *  @param  doc      The name of the document to load.
         */
        public ObjectParser(string path, string doc)
        {

            // If arguments are malformed...
            if(path == null || doc == null)
            {
                // Load the default document.
                xmlDoc.Load(FILEPATH + DOCUMENT);
            }
            else
            {
                // Load the correct document.
                xmlDoc.Load(path + doc);
            }

        }

        /* Private class methods provide functional brevity. */

        /**
         *  Retrieve nodes from an XML document in their
         *  object representations using XPath expression.
         *  
         *  @param   xPath  The XPath formatted expression
         *                  targeting the nodes.
         *  @return         A node list containing all nodes
         *                  found.
         */
        private XmlNodeList getNodesWithXPath(string xPath)
        {
            return xmlDoc.DocumentElement.SelectNodes(xPath);
        }

        /**
         *  Retrieve n nodes potentially at random from an
         *  XML document in their object representations
         *  using XPath expression.
         *  
         *  @param   xPath  The XPath formatted expression
         *                  targeting the nodes.
         *  @param   n      The number of nodes to return.
         *  @param   random Randomize selection.
         *  @return         A node list containing all nodes
         *                  found.
         */
        public void getNodesWithXPath(string xPath, int n, bool random)
        {

            // Retrieve all pertinent elements.
            List<XmlNode> nodes =
                xmlDoc.DocumentElement.SelectNodes(xPath)
                .Cast<XmlNode>()
                .ToList();

            XmlNodeList lel = xmlDoc.DocumentElement.SelectNodes(xPath);

            Console.WriteLine(nodes.ToString());

        }

        /**
         *  Retrieve a node from an XML document in its
         *  object representation using XPath expression.
         *  
         *  @param   xPath  The XPath formatted expression
         *                  targeting the node.
         *  @return         The first node found matching
         *                  the expression.
         */
        private XmlNode getNodeWithXpath(string xPath)
        {
            return getNodesWithXPath(xPath)[0];
        }

        /* Multi-situational public XML parsing methods. */

        /**
         *  Retrieve a node's content from an XML document
         *  using an XPath expression.
         *  
         *  @param   xPath  The XPath formatted expression
         *                  targeting the node.
         *  @return         The content of the first node
         *                  found matching the expression.
         */
        public string getNodeContentWithXPath(string xPath)
        {
            return getNodeWithXpath(xPath).InnerText.Trim();
        }

        

    }

}
