using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.POCO
{
    public interface ICacheItem<T>
    {
        public abstract DateTime DateInserted { get; set; }

        public abstract T GetKey();
    }
}
