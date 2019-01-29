using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TwitterClone.Models;
using BLL;
using TModel;

namespace TwitterClone.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        Person BllEntity = new Person();
        public ActionResult Index(string sortOrder, int? page)
        {
            List<GetTweets_MResult> tweets = new List<GetTweets_MResult>();

            tweets = (List<GetTweets_MResult>)BllEntity.GetbTweets(User.Identity.Name.ToString());

            List<GetTweets_MResult> userTweets = new List<GetTweets_MResult>();
            userTweets = (List<GetTweets_MResult>)BllEntity.GetbUserTweets(User.Identity.Name.ToString());
            List<string> following = new List<string>();
            following = BllEntity.GetbFollowing(User.Identity.Name);
            List<string> followers = new List<string>();
            followers = BllEntity.GetbFollowers(User.Identity.Name);

            ViewBag.TweetCount = userTweets.Count;
            ViewBag.Following = following.Count;
            ViewBag.followers = followers.Count;

            int pageSize = 4;
            int pageNumber = (page ?? 1);

            return View(tweets.ToPagedList(pageNumber, pageSize));
        }

        
        public ActionResult Update(FormCollection collection)
        {
            string tweetText = collection["TweetText"].ToString();
            string UserId = User.Identity.Name.ToString();
            BllEntity.InsertbTweet(UserId, tweetText);
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public PartialViewResult SearchPeople(string keyword="")
        {
            List<TwitterLogin_MResult> Persons = (List<TwitterLogin_MResult>)BllEntity.GetPersons();

            var data = (from p in Persons
                        where p.User_Id.ToLower().Contains(keyword.ToLower()) && p.User_Id.ToLower() != User.Identity.Name.ToLower()
                        select p).ToList();
            return PartialView(data);
        }

        public PartialViewResult ProfileOthers(string id)
        {
            List<TwitterLogin_MResult> Persons = (List<TwitterLogin_MResult>)BllEntity.GetPersons();

            var selPerson = (from p in Persons
                             where p.User_Id.ToLower() == id.ToLower()
                             select p).First();

            List<string> following = new List<string>();
            following = BllEntity.GetbFollowing(User.Identity.Name);
            if (following.Contains(selPerson.User_Id))
            {
                ViewBag.following = true;
            }
            else
            {
                ViewBag.following = false;
            }

            return PartialView(selPerson);
        }

        public PartialViewResult Tweets(int? page)
        {
            List<GetTweets_MResult> tweets = new List<GetTweets_MResult>();
            tweets = (List<GetTweets_MResult>)BllEntity.GetbUserTweets(User.Identity.Name.ToString());

            int pageSize = 4;
            int pageNumber = (page ?? 1);

            return PartialView(tweets.ToPagedList(pageNumber, pageSize));
        }

        public PartialViewResult Following()
        {
            List<string> following = new List<string>();
            following = BllEntity.GetbFollowing(User.Identity.Name);
            return PartialView(following);
        }

        public PartialViewResult Followers()
        {
            List<string> followers = new List<string>();
            followers = BllEntity.GetbFollowers(User.Identity.Name);
            return PartialView(followers);
        }

        public ActionResult FollowUser(string id)
        {
            BllEntity.FollowbUser(User.Identity.Name, id);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult UnFollowUser(string id)
        {
            BllEntity.UnFollowbUser(User.Identity.Name, id);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public PartialViewResult Edit(int id = 0)
        {
            GetTweets_MResult tweet = new GetTweets_MResult();
           tweet = (from p in (List<GetTweets_MResult>)BllEntity.GetbUserTweets(User.Identity.Name.ToString())
                    where p.Tweet_Id == id
                   select p).First();

            return PartialView(tweet);
        }

        [HttpPost]
        public ActionResult Edit(int id, GetTweets_MResult gt)
        {
            BllEntity.UpdateUserbTweet(User.Identity.Name, id, gt.Message);
            return RedirectToAction("Index","Home");
        }


        
        public ActionResult Delete(int id = 0)
        {
            GetTweets_MResult tweet = new GetTweets_MResult();
            tweet = (from p in (List<GetTweets_MResult>)BllEntity.GetbUserTweets(User.Identity.Name.ToString())
                     where p.Tweet_Id == id
                     select p).First();
            return PartialView(tweet); 
        }

        [HttpPost]
        public ActionResult Delete(int id, GetTweets_MResult gt)
        {
            BllEntity.DeleteUserbTweet(User.Identity.Name, id);
            return RedirectToAction("Index", "Home");
        }

    }
}