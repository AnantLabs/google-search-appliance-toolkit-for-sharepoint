<%@ WebHandler Language="C#" Class="OneBox.OneBoxHandler" %>

using System;
using System.Web;
using System.IO;

using System.Security.Principal;
using System.Net.Security;
using Microsoft.Office.Server.ApplicationRegistry.Infrastructure;

namespace OneBox
{
    public class OneBoxHandler : IHttpHandler
    {

        public static Boolean inited = false;
        public void ProcessRequest(HttpContext context)
        {
            if (!inited)
            {
                Bdc.init(context.Server.MapPath("bdc.xml"));
                inited = true;
            }
            context.Response.ContentType = "text/xml";
            String query = context.Request.Params["query"];
            String lob = context.Request.Params["lob"];
            String entity = context.Request.Params["entity"];
            String field = context.Request.Params["fieldname"];
            if (lob == null || lob.Equals("")
                || entity == null || entity.Equals("")
                || field == null || field.Equals("")
                || query == null || query.Equals(""))
            {
                showError(context, "Missing parameters");
                return;
            }


            try
            {
                context.Response.Write(new Bdc().find(lob, entity, field, query));
            }
            catch (AccessDeniedException e)
            {
                context.Response.Write(e.Message);
                context.Response.Write(e.StackTrace);

            }
        }

        void showError(HttpContext context, String msg)
        {
            context.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            context.Response.Write("<msg>");
            context.Response.Write(msg);
            context.Response.Write("</msg>");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public void impersonate()
        {

        }
    }
}