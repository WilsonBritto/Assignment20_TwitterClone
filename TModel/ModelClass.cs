using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TModel
{
    public class TRegisterModel
    {
        [Display(Name = "UserName")]
        public string User_Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
    }

    public class TwitterLogin_MResult
    {
        public string User_Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public System.DateTime Joined { get; set; }
        public bool Active { get; set; }
    }

    public class GetTweets_MResult
    {
        public int Tweet_Id { get; set; }
        public string User_Id { get; set; }
        public string Message { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public System.DateTime Created { get; set; }
    }

}
