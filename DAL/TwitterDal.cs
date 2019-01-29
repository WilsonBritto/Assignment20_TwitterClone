using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TModel;

namespace DAL
{
    public class TwitterDal
    {
        TwitterDBEntities tdb = new TwitterDBEntities();

        public List<TwitterLogin_MResult> VerifyLogin(string User_Id, string Pass)
        {
            List<TwitterLogin_Result> result = (from p in tdb.TwitterLogin(User_Id, Pass)
                                                 select p).ToList();
            List<TwitterLogin_MResult> res = new List<TwitterLogin_MResult>();
            foreach (var item in result)
            {
                res.Add(new TwitterLogin_MResult() { User_Id = item.User_Id, Email = item.Email, FullName = item.FullName, Joined = item.Joined, Active = item.Active });
            }

            return res;
        }

        public List<TwitterLogin_MResult> GetAllPersons()
        {
            List<GetAllPerson_Result> result = (from p in tdb.GetAllPerson()
                                                select p).ToList();
            List<TwitterLogin_MResult> res = new List<TwitterLogin_MResult>();
            foreach (var item in result)
            {
                res.Add(new TwitterLogin_MResult() { User_Id = item.User_Id, Email = item.Email, FullName = item.FullName, Joined = item.Joined, Active = item.Active });
            }

            return res;
        }

        public int AddUser(TRegisterModel model)
        {
            int user = tdb.AddUser(model.User_Id, model.Password, model.FullName, model.Email);
            return user;
        }

        public void InsertTweet(string userId, string msg)
        {
            tdb.InsertTweet(msg, userId);
        }

        public List<GetTweets_MResult> GetTweets(string User_Id)
        {
            List<GetTweets_Result> result = (from p in tdb.GetTweets(User_Id)
                                                    select p).ToList();
            List<GetTweets_MResult> res = new List<GetTweets_MResult>();
            foreach (var item in result)
            {
                res.Add(new GetTweets_MResult() {Tweet_Id = item.Tweet_Id, User_Id = item.User_Id, Message = item.Message, Created = item.Created });
            }

            return res;
        }

        public List<GetTweets_MResult> GetUserTweets(string User_Id)
        {
            List<GetTweets_Result> result = (from p in tdb.GetTweets(User_Id)
                                             where p.User_Id == User_Id
                                             select p).ToList();
            List<GetTweets_MResult> res = new List<GetTweets_MResult>();
            foreach (var item in result)
            {
                res.Add(new GetTweets_MResult() { Tweet_Id = item.Tweet_Id, User_Id = item.User_Id, Message = item.Message, Created = item.Created });
            }

            return res;
        }

        public void UpdateUserDetails(string User_Id, string FullName, string Email)
        {
            tdb.UpdateUser(User_Id, Email, FullName);
        }

        public void ChangePwd(string User_Id, string Pass)
        {
            tdb.ChangeUserPassword(User_Id, Pass);
        }

        public void UpdateUserTweet(string User_Id, int TweetId, string Tmsg)
        {
            tdb.UdpateUserTweet(User_Id, TweetId, Tmsg);
        }

        public void DeleteUserTweet(string User_Id, int TweetId)
        {
            tdb.DeleteUserTweet(User_Id, TweetId);
        }

        public List<string> GetFollowing(string User_Id)
        {
            List<string> Following = new List<string>();
            Following = tdb.GetFollowing(User_Id).ToList();
            return Following;
        }

        public List<string> GetFollowers(string User_Id)
        {
            List<string> Followers = new List<string>();
            Followers = tdb.GetFollowers(User_Id).ToList();
            return Followers;
        }

        public void FollowUser(string User_Id, string FollowID)
        {
            tdb.FollowUser(User_Id, FollowID);
        }

        public void UnFollowUser(string User_Id, string UnFollowID)
        {
            tdb.UnFollowUser(User_Id, UnFollowID);
        }

    }
}
