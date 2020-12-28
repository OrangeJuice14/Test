using System;
using System.Collections.Generic;
using System.Linq;
using HRMWeb_Business.Model;

namespace HRMWeb_Service
{
    public static class DTO_BoPhanEnumerable
    {
        public static IList<DTO_BoPhan> BuildTree(this IEnumerable<DTO_BoPhan> source)
        {
            var groups = source.GroupBy(i => i.BoPhanCha);

            var roots = groups.FirstOrDefault(g => g.Key.HasValue == false).ToList();

            if (roots.Count > 0)
            {
                var dict = groups.Where(g => g.Key.HasValue).ToDictionary(g => g.Key.Value, g => g.ToList());
                for (int i = 0; i < roots.Count; i++)
                    AddChildren(roots[i], dict);
            }

            return roots;
        }

        private static void AddChildren(DTO_BoPhan node, IDictionary<Guid, List<DTO_BoPhan>> source)
        {
            if (source.ContainsKey(node.Oid))
            {
                node.items = source[node.Oid];
                for (int i = 0; i < node.items.Count; i++)
                    AddChildren(node.items[i], source);
            }
            else
            {
                node.items = new List<DTO_BoPhan>();
            }
        }
    }
}