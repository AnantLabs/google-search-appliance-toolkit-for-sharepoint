/*
 * Copyright (C) 2006 Google Inc.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */

using System;
using System.Globalization;
using System.Xml;
using System.Web;
using System.Net;
using System.IO;
using System.Security.Principal;
using System.Configuration;
using System.Collections;

//using System.IO.Compression;

namespace OneBox
{
	/// <summary>
	/// Summary description for Common.
	/// </summary>
	public class Common
	{
		// String to hold the SAML namespace declaration
		public static String SAML_NAMESPACE = "urn:oasis:names:tc:SAML:2.0:protocol";
		// String used as prefix for artifacts
		public static String ARTIFACT = "Artifact";
		// SAML element name when resolving artifacts
		public static String ArtifactResolve = "ArtifactResolve";
		public static String ID="ID";
		public static String AuthNResponseTemplate = null;
		public static String AuthZResponseTemplate = null;
		public static String LogFile = null;    
		public static bool bDebug = false;
		public static String artifactConsumer = null;
		public static int DEBUG=0, INFO=1, ERROR=2;
		public static int LOG_LEVEL  = INFO;
		public static Hashtable  alias = new Hashtable();
		
		/// <summary>
		/// read the host name of the GSA from the Web.config file
		/// </summary>
		public static String GSA
		{
			get
			{
				return ConfigurationSettings.AppSettings["gsa"];
			}
		}

		/// <summary>
		/// When log level lower or equal to debug, show debug(most detailed msg)
		/// </summary>
		/// <param name="msg"></param>
	 
		public static void debug(String msg)
		{
			if (LOG_LEVEL <= DEBUG)
				log(msg);
		}

		/// <summary>
		/// If log level is lower than error, then show all error msgs
		/// </summary>
		/// <param name="msg"></param>
		public static void error(String msg)
		{
			if (LOG_LEVEL <= ERROR)
				log(msg);
		}

		public static void info(String msg)
		{
			if (LOG_LEVEL <= INFO)
				log(msg);
		}

		/// <summary>
		/// Method for logging debug/trace information to a file.
		///NOTE:  Read/Write privileges must be given to the web service
		/// process owner and domain users if impersonation is performed
		/// </summary>
		/// <param name="msg"></param>
		private static void log(String msg)
		{

			StreamWriter logger = File.AppendText(LogFile);
			System.Diagnostics.StackFrame frame = new System.Diagnostics.StackTrace().GetFrame(2);
			logger.WriteLine(System.DateTime.Now + ", " + frame.GetMethod().Name + ": " +  msg );
			logger.Close();
		}

		 
		/// <summary>
		///Method to determine the URL to which the user is redirected
		/// after login.
		/// Based on whether this is a simulation or not.
		///The simulator is a test utility that simulates
		///the SAML requests that come from a GSA.
		/// 
		/// </summary>
		public static String GSAArtifactConsumer
		{
			get
			{
				return artifactConsumer;
			}
			set
			{
				artifactConsumer = value;
			}
		}

		/// <summary>
		///  Method to obtain the current time, converted
		/// tto a specific format for insertion into responses
		/// </summary>
		/// <param name="time"></param>
		/// <returns>Universal time format</returns>
		public static String FormatInvariantTime(DateTime time)
		{
			return time.ToString("s", DateTimeFormatInfo.CurrentInfo) + "Z";
		}
		/// <summary>
		/// Method to obtain an XML element within an XML string,
		///  given the element name.
		/// </summary>
		/// <param name="xml">The xml to be searched</param>
		/// <param name="name">element name</param>
		/// <returns></returns>
		public static XmlNode FindOnly(String xml, String name)
		{
			XmlDocument doc = new XmlDocument();
			doc.InnerXml = xml;
			return FindOnly(doc, name);
		}

		
		/// <summary>
		/// Method to obtain an XML element within an XMLDocument,
		/// given the element name.
		/// </summary>
		/// <param name="doc">Xml Document</param>
		/// <param name="name">element name to be found</param>
		/// <returns></returns>
		public static XmlNode FindOnly(XmlDocument doc, String name)
		{
			XmlNodeList list = doc.GetElementsByTagName(name, Common.SAML_NAMESPACE);
			return list.Item(0);
		}

		
		/// <summary>
		/// Method to add an XML attribute to an element
		/// </summary>
		/// <param name="node"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public static void AddAttribute(XmlNode node, String name, String value)
		{
			XmlAttribute attr = node.OwnerDocument.CreateAttribute(name);
			attr.Value = value;
			node.Attributes.Append(attr);
		}


		public static String GetFile(String url)
		{
				debug("GetFile: " + url);
				FileInfo fi = new FileInfo(url);
				debug("after new FileInfo: ");
				FileStream fs = fi.OpenRead();
				debug("after open read");
				fs.Close();
				debug("after filestream close");
				return "";
		}
		#region Decompression requires .NET Framework v 2.0
/*
		public static String Decompress(String samlRequest)
		{
			//base64 decode
			try
			{
				Common.log("before base64 decode string: " + samlRequest);
				byte[] decData = Convert.FromBase64String(samlRequest);
				//compress inflate
				GZipStream zipStream = new GZipStream(new MemoryStream(decData), CompressionMode.Decompress);
				MemoryStream ms = new MemoryStream();
				byte []writeData = new byte[1096];
				int size = -1;
				while (true)
				{
					size = zipStream.Read(writeData, 0, writeData.Length);
					if (size > 0)
						ms.Write(writeData, 0, size) ;
					else
						break;
				}
				ms.Close();
				return System.Text.Encoding.UTF8.GetString(ms.ToArray()); 
			}
			catch(Exception e)
			{
				Common.log(e.Message);
				return null;
			}
		}
*/
		#endregion
	}
}

