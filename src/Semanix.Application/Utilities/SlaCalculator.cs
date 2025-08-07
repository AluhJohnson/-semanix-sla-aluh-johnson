using Semanix.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Application.Utilities
{
    public static class SlaCalculator
    {
        public static DateTime CalculateDeadline(DateTime createdUtc, PRIORITY priority) =>
            priority switch
            {
                PRIORITY.Low => createdUtc.AddHours(8),
                PRIORITY.Normal => createdUtc.AddHours(4),
                PRIORITY.High => createdUtc.AddHours(1),
                PRIORITY.Critical => createdUtc.AddMinutes(15),
                _ => throw new ArgumentOutOfRangeException(nameof(priority))
            };
    }
}
