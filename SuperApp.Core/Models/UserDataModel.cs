using System;
using System.Collections.Generic;
using System.Text;

namespace SuperApp.Core.Models
{
    public class UserDataModel : BaseDataModel
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserDataModel() : base() { }

        public UserDataModel(int id) : base(id)
        {
        }
    }
}
