using BMS_Logistics.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BMS_Logistics.Infrastructure.Helpers
{
    public static class StatusHelper
    {
        private static Dictionary<string, int> statusCache = new();

        public static async Task<int> GetStatusId(string table, string code, DataContext _context)
        {
            var key = $"{table}:{code}";

            if (statusCache.TryGetValue(key, out var id))
                return id;

            id = await _context.Statuses
                .Where(e => e.Table == table && e.Code == code)
                .Select(e => e.Id)
                .FirstOrDefaultAsync();

            if (id == 0)
                throw new InvalidOperationException(
                    $"No existe Status para Table='{table}' y Code='{code}'"
                );

            //statusCache[key] = id;
            statusCache.TryAdd(key, id);

            return id;
        }
    }
}
