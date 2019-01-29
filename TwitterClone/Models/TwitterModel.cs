using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterClone.Models
{
    public class TwitterModel
    {
    }

    public class Tweet
    {
        public int TweetId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Created { get; set; }
    }
}