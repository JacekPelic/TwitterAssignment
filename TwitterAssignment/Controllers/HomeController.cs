using LinqToTwitter;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace TwitterAssignment.Controllers
{
    public class HomeController : Controller // Controller maintaining UI actions
    {

        public ActionResult Index()
        {
            return RedirectToAction("BeginAuthorize", "WebGate");
        }

        public ActionResult ShowTweets(int page = 1, int pageSize = 10) //Printing tweets
        {
            var tweets = Session["tweets"] as List<Status>;
            var onePageOfTweets = tweets.ToPagedList(page, pageSize);

            return View(onePageOfTweets);
        }

        public ActionResult FilterOrHash(string key) //Checking if user is looking for new # 
                                                     //or filter printed tweets
        {
            if (key.StartsWith("#"))
            {
                TempData["key"] = key;
                return RedirectToAction("GetHashedTweets", "WebGate");
            }
            else
            {
                TempData["key"] = key;
                return RedirectToAction("FilterTweets");
            }
        }

        public ActionResult FilterTweets() //Filter Listed tweets by key string
        {
            string key = (string)TempData["key"];
            var tweetsList = Session["tweets"] as List<Status>;
            var filtredTweets = new List<Status>();

            foreach (var tweet in tweetsList)
            {

                if (tweet.Text.Contains(key)
                    || tweet.User.Name.Contains(key)
                    || tweet.CreatedAt.ToString().Contains(key)
                    )
                {
                    filtredTweets.Add(tweet);
                }
            }

            Session["tweets"] = filtredTweets;


            return RedirectToAction("ShowTweets");
        }

        public ActionResult SaveTweet(ulong ID, string returnUrl) //Saving tweet on disk
        {
            var tweets = (List<Status>)Session["tweets"];
            var tweetToBeSaved = tweets.First(x => x.StatusID == ID);
            var textOfTweet = String.Format("{0}" + Environment.NewLine + "{1}" + Environment.NewLine + "{2}",
                tweetToBeSaved.CreatedAt ,tweetToBeSaved.User.Name, tweetToBeSaved.Text);

            return File(Encoding.UTF8.GetBytes(textOfTweet), "text/plain", string.Format("SavedTweet.txt"));

        }
    }
}