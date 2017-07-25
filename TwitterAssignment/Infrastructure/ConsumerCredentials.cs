using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using LinqToTwitter;
using TwitterAssignment.Abstract;

namespace TwitterAssignment.Infrastructure
{
    public class ConsumerCredentials : IMvcAuthorization
    {

        private MvcAuthorizer _auth = new MvcAuthorizer
        {
            CredentialStore = new SessionStateCredentialStore
            {
                ConsumerKey = ConfigurationManager.AppSettings["ConsumerKey"],
                ConsumerSecret = ConfigurationManager.AppSettings["ConsumerSecretKey"]
            }
            
        };

        public MvcAuthorizer Auth { get { return _auth; } }

    }

}