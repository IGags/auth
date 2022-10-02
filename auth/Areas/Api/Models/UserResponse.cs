using System;

namespace Api.Areas.Api.Models
{
    [Serializable]
    public class UserResponse
    {
        public string FIO { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string LastLogin { get; set; }
    }
}
