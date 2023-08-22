﻿using System;

namespace SuperPanel.App.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserViewModel() { }

        public UserViewModel(int id)
        {
            this.Id = id;
        }
    }
}
