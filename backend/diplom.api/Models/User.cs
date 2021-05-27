using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsFollowing { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public IList<User> Followers { get; set; }
        public IList<User> Followings { get; set; }
    }
}
