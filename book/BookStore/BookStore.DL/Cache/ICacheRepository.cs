using BookStore.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DL.Cache
{
    public interface ICacheRepository<TKey, TData> where TData : ICacheItem<TKey>
    {
        Task<IEnumerable<TData?>> FullLoad();

        Task<IEnumerable<TData?>> DifLoad(DateTime lastExecuted);
    }
}
