using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterAssignment;
using TwitterAssignment.Controllers;
using LinqToTwitter;
using PagedList;
using Moq;
using System.Web;
using MvcContrib.TestHelper;

namespace TwitterAssignment.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {

        private HomeController getHomeController(string key = null, List<Status> tweetList = null)
        {
            var homeController = new HomeController();

            homeController = setTempDataKey(homeController, key);
            homeController = setSessionTweetList(homeController, tweetList);

            return homeController;
        }

        private HomeController setTempDataKey(HomeController controller, string key)
        {
            TestControllerBuilder builder = new TestControllerBuilder(); //Used for ini

            builder.InitializeController(controller);

            var tempData = new TempDataDictionary
            {
                { "key", key }
            };
            controller.TempData = tempData;

            return controller;
        }

        private HomeController setSessionTweetList(HomeController controller, List<Status> tweetsList)
        {
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(p => p.HttpContext.Session["tweets"]).Returns(tweetsList);
            controller.ControllerContext = controllerContext.Object;

            return controller;
        }

        [TestMethod]
        public void ShowTweetsPagingCheck()
        {
            var pageSize = 5;
            var page = 2;
            var items = 10;
            var tweetList = new List<Status>();

            for (int i = 0; i < items; i++)
            {
                tweetList.Add(new Status { Text = String.Format("tweet nr {0}", i) });
            }

            var PageOfTweets = tweetList.ToPagedList(page, pageSize);

            Assert.AreEqual(pageSize * page - 4, PageOfTweets.FirstItemOnPage);
        }

        [TestMethod] //Test is never possitive becouse member of Status.User.Name is read-only which result in thorwing exepction in FilterCheck method
        public void FilterCheck() // Using Mock<User> is not an option either becose of the issue of conversion from Moq.Mock<LinqToTwitter> to LinqToTwitter
        {
            var items = 10;
            int indexOfCheckingTweet = 2;
            var tweetList = new List<Status>();

            for (int i = 0; i < items; i++)
            {
                tweetList.Add(new Status { Text = String.Format("tweet nr {0}", i),
                    CreatedAt = DateTime.Now,
                });
            }
            var home = getHomeController(indexOfCheckingTweet.ToString() , tweetList);

            home.FilterTweets();

            Assert.AreEqual(tweetList[indexOfCheckingTweet].Text, (Status)home.Session["tweets"]);

        }

    }
}
