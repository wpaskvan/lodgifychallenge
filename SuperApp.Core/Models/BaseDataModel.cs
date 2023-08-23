using System;
using System.Collections.Generic;
using System.Text;

namespace SuperApp.Core.Models
{
    public abstract class BaseDataModel
    {
        public int Id { get; set; }

        protected BaseDataModel()
        {
        }

        protected BaseDataModel(int id)
        {
            this.Id = id;
        }
    }
}
