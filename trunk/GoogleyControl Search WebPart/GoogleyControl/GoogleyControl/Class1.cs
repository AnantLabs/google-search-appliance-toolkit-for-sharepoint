using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Microsoft.SharePoint.Utilities;
using System.Net;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;

//TODO:  Change so that you can deploy part without config file, and pass in parameters

namespace GoogleyControl
{
    class GooglePropertyBag
    {
        public string GSALocation;
        public string accessLevel;
        public string sortType;
        public string siteCollection;
        public string frontEnd;
        public string customXSLTLocation;
        public string forceRefreshFrontEnd;
        public string enableLogging;


        //Object to hold properties loaded from AppSettings in Web.config
        public GooglePropertyBag()
        {
            GSALocation = "";
            accessLevel = "p";
            sortType = "date";
            siteCollection = "default_collection";
            frontEnd = "default_frontend";
            customXSLTLocation = "";
            forceRefreshFrontEnd = "false";
            enableLogging = "true";

        }

        //Method to extract configuration properties into GooglePropertyBag
        public void initGooglePropertyBag()
        {
            GSALocation = ConfigurationManager.AppSettings["GSALocation"];
            siteCollection = ConfigurationManager.AppSettings["siteCollection"];
            sortType = ConfigurationManager.AppSettings["sortType"];
            forceRefreshFrontEnd = ConfigurationManager.AppSettings["forceRefreshFrontEnd"];
            enableLogging = ConfigurationManager.AppSettings["enableLogging"];
            customXSLTLocation = ConfigurationManager.AppSettings["customXSLTLocation"];
            accessLevel = ConfigurationManager.AppSettings["accessLevel"];
            frontEnd = ConfigurationManager.AppSettings["frontEnd"];
        }


    }
    
    public class GSAPart : System.Web.UI.WebControls.WebParts.WebPart
    {


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(writer);
        }


        public string GSASearch(NameValueCollection inquery)
        {
            GooglePropertyBag gProps = new GooglePropertyBag();

            
            try
            {
                gProps.initGooglePropertyBag();
                return postQueryString(gProps, inquery);
            
            }
            catch (Exception err)
            {
                // Check if user wants exceptions thrown to the users display screen
                if (gProps.enableLogging.ToLower().Trim().Equals("true"))
                {
                    return err.ToString();
                }
                else
                {
                    return "Oops!  Google Search is experiencing hiccups.  Contact your Admin ASAP";
                }
            }

        }

        public string GSASearch(NameValueCollection inquery, string inGSAServer)
        {
            GooglePropertyBag gProps = new GooglePropertyBag();
            gProps.GSALocation = inGSAServer;
            string searchReq = "";
            string searchResp = ""; 

            try
            {

                return postQueryString(gProps, inquery);
            }
            catch (Exception err)
            {
                if (gProps.enableLogging.ToLower().Trim().Equals("true"))
                {
                    return err.ToString();
                }
                else
                {
                    return "Oops!  Google Search is experiencing hiccups.  Contact your Admin ASAP";
                }
            }
            
        }

        public string GSASearch(NameValueCollection inquery, string inGSAServer, string inFrontEnd)
        {
            GooglePropertyBag gProps = new GooglePropertyBag();
            gProps.GSALocation = inGSAServer;
            gProps.frontEnd = inFrontEnd;
            string searchReq = "";
            string searchResp = "";

            try
            {
                return postQueryString(gProps, inquery);
            }
            catch (Exception err)
            {
                if (gProps.enableLogging.ToLower().Trim().Equals("true"))
                {
                    return err.ToString();
                }
                else
                {
                    return "Oops!  Google Search is experiencing hiccups.  Contact your Admin ASAP";
                }
            }

        }

        
        // Method abstracted out which creates the GSA query string for the HTTP get based
        //   on the HTTP paramaters passed in from Sharepoint search control
        private string postQueryString(GooglePropertyBag gProps, NameValueCollection inquery)
        {
            //TODO:  Improve XML config framework to be generic loading and reading value pairs
            //TODO:  Add more config abstraction like ProxyReload
            string searchResp;
            string searchReq = gProps.GSALocation + "/search?q=" + inquery["k"] + "&access=" + gProps.accessLevel + "&entqr=0&output=xml_no_dtd&sort=" + gProps.sortType + "&ud=1&client="+ gProps.frontEnd + "&oe=UTF-8&getfields=*&ie=UTF-8&site=" + gProps.siteCollection;
            

            //Specialised checks - converting Sharepoint paramaters to GSA paramaters
            
            //Custom Frontend?
            if (gProps.frontEnd.Trim() != "") searchReq += "&proxystylesheet=" + gProps.frontEnd;
            //Force Proxy Reloading   - should be off unless debugging and stylesheet has changed
            if (gProps.forceRefreshFrontEnd.ToLower().Trim() == "true") searchReq += "&proxyreload=1";
            //Paging?
            if (inquery["start1"] != null) searchReq = searchReq + "&start=" + inquery["start1"];
            //Sorting by date?
            if ((inquery["v1"] != null) && (inquery["v1"]=="date"))
            {
                searchReq += "&sort=date%3AD%3AS%3Ad1"; 
            }
            if ((inquery["v1"] != null) && (inquery["v1"]=="relevance"))
            {
                searchReq += "&sort=relevance";
            }
            //TODO:  Scope flags to limit to collections

            //append all other query string items to preserve sharepoint flags
            //TODO:  Replace manipulated items in querystring ... clean up this as the querystring could become excessively long
            //TODO:  This below method tried to include all other query string params from sharepoint to GSA and back;  it can cause problems if the 
            //  filter misses 'dedupping' below.   May choose to completely remove this code and add individual rules for query objects
            //  like 'Filter'  as we did above
            for (int x = 0; x < inquery.Count; x++)
            {
                switch (inquery.GetKey(x))
                {
                    //do not pass straight-thru known query objects
                    case "q":
                    case "sort":
                    case "proxyreload":
                    case "proxystylesheet":
                    case "v1":
                    case "ie":
                    case "oe":
                    case "ud":
                    case "client":
                    case "site":
                    case "start":
                    case "start1":
                    case "access":
                    case "output":
                        break;
                    case "k":
                    default:
                        searchReq = searchReq + "&" + inquery.GetKey(x) + "=" + inquery.Get(x);
                        break;
                }
                
            }




            try
            {
                System.Net.HttpWebRequest myReq = WebRequest.Create(searchReq) as HttpWebRequest;
                using (HttpWebResponse response = myReq.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    searchResp = reader.ReadToEnd();
                }
            }
            catch
            {
                throw new Exception("Error with URL:  " + searchReq);
            }
            return searchResp;
        }

             
    }
}
