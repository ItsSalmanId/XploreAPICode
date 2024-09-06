using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using Newtonsoft.Json;
using BusinessOperations.CommonService;
using BusinessOperations.CommonServices;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace FoxRehabilitationAPI.App_Start
{
    [ExcludeFromCodeCoverage]
    public class LDAPClient
    {
        void LogProperties(DirectoryEntry de)
        {
            if (de != null && de.Properties != null && de.Properties.Count > 0)
            {
                PropertyCollection p = de.Properties;
                string email;
                Dictionary<string, string> properties = new Dictionary<string, string>();
                if (de.Properties.Contains("mail"))
                {
                    email = de.Properties["mail"].Value == null ? "No email provide" : de.Properties["mail"].Value.ToString();
                }
                else
                {
                    email = "No email provide";
                }
                foreach (string pName in p.PropertyNames)
                {
                    string Value = de.Properties[pName].Value == null ? string.Empty : de.Properties[pName].Value.ToString();
                    properties.Add(pName, Value);
                }
                Helper.LogADProperties(Email: email, Properties: properties);
                //de.Dispose();
            }
        }
    }
}
