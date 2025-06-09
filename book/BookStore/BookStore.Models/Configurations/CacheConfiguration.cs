using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.Configurations
{
    public abstract class CacheConfiguration
    {
        public string Topic { get; set; } = string.Empty;

        public int RefreshInterval { get; set; } = 1;
    }
}
