using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models.Specifications
{
    public class PagedList<Item> : List<Item>
    {
        public MetaData MetaData { get; set; }
        public PagedList(List<Item> items, int count, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }
        public static PagedList<Item> ToPagedList(IEnumerable<Item> entity, int pageNumber, int pageSize)
        {
            var count = entity.Count();
            var items = entity.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize).ToList();
            return new PagedList<Item>(items, count, pageSize);
        }
    }
}