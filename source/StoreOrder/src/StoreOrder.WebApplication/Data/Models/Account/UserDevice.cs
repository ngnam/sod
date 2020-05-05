using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Account
{
    public class UserDevice
    {
        public string Id { get; set; }
        public string CodeDevice { get; set; } // AppId
        public int? IsVerified { get; set; }
        public int? VerifiedCode { get; set; } // mã xác nhận add login trên thiết bị mới
        public int? TimeCode { get; set; } // thời gian sử dụng mã.
        public DateTime? LastLogin { get; set; }
        public string CurrentUserId { get; set; }
        public User CurrentUser { get; set; }
    }
}
