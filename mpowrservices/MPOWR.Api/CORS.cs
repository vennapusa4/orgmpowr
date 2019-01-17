using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MPOWR.Api
{

    public class CORS : IHttpModule
    {
        private string[] origins = null;
      //  private string methods = "*";

        public void Init(HttpApplication context)
        {
            //cofing domains which you want to allow access 
            this.origins = ConfigurationManager.AppSettings["CrossOriginURL"].Split(',');
            //context.BeginRequest += (new EventHandler(this.Application_BeginRequest));
            context.PreRequestHandlerExecute += context_PreRequestHandlerExecute;

        }
 
        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("Access-Control-Allow-Origin");

            var Origin = HttpContext.Current.Request.Headers.GetValues("Origin");

           
                if (this.origins != null && Origin != null && this.origins.Any() &&
                  this.origins.Contains(Origin[0], StringComparer.InvariantCultureIgnoreCase))
                {

                     foreach (var item in Origin)
                    {
                        HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", item);

                    }
                //if (!string.IsNullOrWhiteSpace(this.methods))
                //{
                //    HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Methods", this.methods);
                //}
                if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
                {
                    HttpContext.Current.Response.Flush();
                }

            }
        }
        public void Dispose()
        {

        }
    }
}