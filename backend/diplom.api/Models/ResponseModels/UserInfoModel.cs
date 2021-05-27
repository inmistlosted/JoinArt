using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models.ResponseModels
{
    public class UserInfoModel
    {
        public string Login { get; set; }
        public bool IsFollowing { get; set; }
    }
}
