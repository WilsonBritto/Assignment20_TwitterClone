using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using TModel;

namespace BLL
{
    public class Person
    {

        TwitterDal td = new TwitterDal();

        public object verifyLogin(string User_Id, string Pass)
        {
            object res = td.VerifyLogin(User_Id, Pass);            
            return res;
        }

        public object GetPersons()
        {
            object res = td.GetAllPersons();
            return res;
        }

        public void InsertbTweet(string userid, string msg)
        {
            td.InsertTweet(userid, msg);
        }

        public object GetbTweets(string userId)
        {
            object res = td.GetTweets(userId);
            return res;
        }

        public object GetbUserTweets(string userId)
        {
            object res = td.GetUserTweets(userId);
            return res;
        }        

        public int AddbUser(TRegisterModel m)
        {
            return td.AddUser(m);
        }

        public void UpdateUserbDetails(string Userid,string Fullname, string Email)
        {
            td.UpdateUserDetails(Userid, Email, Fullname);
        }

        public void ChangebPwd(string UserId, string Pwd)
        {
            td.ChangePwd(UserId, Pwd);
        }

        public void UpdateUserbTweet(string UserId, int TweetId, string Msg)
        {
            td.UpdateUserTweet(UserId, TweetId, Msg);
        }

        public void DeleteUserbTweet(string User_Id, int TweetId)
        {
            td.DeleteUserTweet(User_Id, TweetId);
        }

        public List<string> GetbFollowing(string User_Id)
        {
            List<string> Following = new List<string>();
            Following = td.GetFollowing(User_Id).ToList();
            return Following;
        }

        public List<string> GetbFollowers(string User_Id)
        {
            List<string> Followers = new List<string>();
            Followers = td.GetFollowers(User_Id).ToList();
            return Followers;
        }

        public void FollowbUser(string User_Id, string FollowID)
        {
            td.FollowUser(User_Id, FollowID);
        }

        public void UnFollowbUser(string User_Id, string UnFollowID)
        {
            td.UnFollowUser(User_Id, UnFollowID);
        }

    }
}
