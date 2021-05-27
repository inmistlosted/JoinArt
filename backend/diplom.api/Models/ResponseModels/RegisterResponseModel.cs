using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models.ResponseModels
{
    public class RegisterResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Login { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
    }
}
