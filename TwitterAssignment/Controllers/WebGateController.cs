using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToTwitter;
using System.Configuration;
using TwitterAssignment.Abstract;
using System.Threading.Tasks;

namespace TwitterAssignment.Controllers
{
    public class WebGateController : Controller // Controler used to connecting to Twitter API
    {

        private IMvcAuthorization _provider;

        public WebGateController(IMvcAuthorization auth)
        {
            _provider = auth;
        }

        public async Task<ActionResult> BeginAuthorize() //Begining authorize process allowing user to put his credentials
        {
            string twitterCallbackUrl = Request.Url.ToString().Replace("Begin", "Complete"); 
            return await _provider.Auth.BeginAuthorizationAsync(new Uri(twitterCallbackUrl)); //Start authorization to recive OAuthToken
        }

        public async Task<ActionResult> CompleteAuthorize()
        {
            await _provider.Auth.CompleteAuthorizeAsync(Request.Url); //Get access Token
            return RedirectToAction("GetTimeLineTweets");
        }

        public ActionResult GetTimeLineTweets() // Get tweets from User Timeline
        {

            var twitterCtx = new TwitterContext(_provider.Auth);

            var statusResponse = new List<Status>();
            statusResponse = (from tweet in twitterCtx.Status
                              where tweet.Type == StatusType.User
                              && tweet.Count == 100
                              select tweet).ToList();

            Session["tweets"] = statusResponse;

            return RedirectToAction("ShowTweets", "Home");
        }


        public ActionResult GetHashedTweets() // Find tweets matched on the #
        {
            var key = (string)TempData["key"];
            var twitterCtx = new TwitterContext(_provider.Auth);
            var hashedTweets = new List<Status>();

            hashedTweets = (from search in twitterCtx.Search
                            where search.Type == SearchType.Search
                            && search.Query == key
                            && search.Count == 100
                            select search.Statuses
                            ).SingleOrDefault();

            Session["tweets"] = hashedTweets;

            return RedirectToAction("ShowTweets", "Home");


        }
    }
}