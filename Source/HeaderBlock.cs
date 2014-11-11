using System;
using System.Collections.Generic;
using System.Web;

namespace ContextIS
{
    /// <summary>
    /// Ensure that you sign the assembly, then you can install it into
    /// the GAC of your web servers and simply make the following 
    /// modification to your application’s web.config (or if you 
    /// want it to be globally applied, to the applicationHost.config):
    /// 
    /// MACHINE.CONFIG
    /// <system.webServer>
    ///     <modules runAllManagedModulesForAllRequests="true">
    ///     <add name="HeaderBlock"
    ///          type="ContextIS.HeaderBlock, HeaderBlock, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ABCDEFG" />
    ///     </modules>
    /// </system.webServer>
    /// 
    /// APPLICATIONHOST.CONFIG
    /// <system.webServer>
    ///     <modules> 
    ///     <add name="HeaderBlock" 
    ///          type="ContextIS.HeaderBlock, HeaderBlock, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ABCDEFG" />
    ///      </modules>    
    ///  </system.webServer>
    ///  
    /// </summary>
    public class HeaderBlock : IHttpModule
    {
        /// <summary>
        /// List of Headers to remove
        /// </summary>
        private List<string> _blockedHeaders = null;

        /// <summary>
        /// Add the headers that are required for blocking to 
        /// the member level _blockedHeaders variable
        /// </summary>
        public HeaderBlock()
        {
            _blockedHeaders = new List<string>();

            // Add each header that you want to be removed from the 
            // HTTP response. Note that the "X-Powered-By" header 
            // has to be removed within the IIS manager
            _blockedHeaders.Add("Server");
            _blockedHeaders.Add("X-AspNet-Version");
            _blockedHeaders.Add("X-AspNetMvc-Version");
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose(){}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpApplication"></param>
        public void Init(HttpApplication httpApplication)
        {
            httpApplication.PreSendRequestHeaders += this.OnPreSendRequestHeaders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            _blockedHeaders.ForEach(h => HttpContext.Current.Response.Headers.Remove(h));
        }
    }
}
