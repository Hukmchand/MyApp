using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AntiTheftClub.Services
{
    public static class ServiceExtension
    {
        public static async Task<List<T>> ToListAsync<T>(this ObjectResult<T> source)
        {
            var list = new List<T>();
            await Task.Run(() => list.AddRange(source.ToList()));
            return list;
        }
    }
}