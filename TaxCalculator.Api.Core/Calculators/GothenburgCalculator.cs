using static TaxCalculator.Api.Core.Calculators.CalculatorFactory;
using TaxCalculator.Api.Data.Entities;

namespace TaxCalculator.Api.Core.Calculators
{
    public static class GothenburgCalculator
    {
        public static CalculateFeeDelegate CalculateFee()
        {
            return (passages, maxDayFee, fees, tollFreeDates) =>
            {
                int totalFee = 0;
                foreach (var day in SplitPassagesIntoDays(passages))
                {
                    int dayFee = 0;
                    var passagesOfDay = day.Value.OrderBy(date => date).ToList();
                    var dateOfDay = day.Key;

                    if (IsWeekend(dateOfDay) || IsTollFreeDate(dateOfDay, tollFreeDates))
                    {
                        continue;
                    }

                    DateTime passageIntervalStart = passagesOfDay.First();
                    int intervalHighestFee = GetPassageFeeFromFees(passageIntervalStart, fees);

                    if (passagesOfDay.Count == 1)
                    {
                        totalFee += intervalHighestFee;
                        continue;
                    }

                    for (int i = 1; i < passagesOfDay.Count; i++)
                    {
                        TimeSpan timeDifference = passagesOfDay[i] - passageIntervalStart;

                        if (timeDifference.TotalMinutes < 60)
                        {
                            var nextFee = GetPassageFeeFromFees(passagesOfDay[i], fees);

                            if (nextFee > intervalHighestFee)
                            {
                                intervalHighestFee = nextFee;
                            }

                        }
                        else
                        {
                            //passage[i] is now new interval start. 
                            dayFee += intervalHighestFee; //add  highest feed to dayfee.

                            // set passage[i] to interval start, and fee to highest interval fee.
                            passageIntervalStart = passagesOfDay[i];
                            intervalHighestFee = GetPassageFeeFromFees(passagesOfDay[i], fees);
                        }
                        if (i == passagesOfDay.Count - 1)
                        {
                            dayFee += intervalHighestFee;
                        }
                    }

                    totalFee += Math.Min(dayFee, maxDayFee);
                }
                return totalFee;
            };
        }

        private static int GetPassageFeeFromFees(DateTime passage, IEnumerable<HourFee> fees)
        {
            return fees.SingleOrDefault(hourFee => passage.TimeOfDay >= hourFee.StartTime && passage.TimeOfDay < hourFee.EndTime)?.Fee ?? 0;
        }

        private static Dictionary<DateOnly, IEnumerable<DateTime>> SplitPassagesIntoDays(DateTime[] passages)
        {
            return passages
                .Select(passage => passage.Date)
                .Distinct()
                .ToDictionary(DateOnly.FromDateTime, date => passages.Where(passage => passage.Date == date));
        }

        private static bool IsWeekend(DateOnly date)
        {
            return date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;
        }

        private static bool IsTollFreeDate(DateOnly date, IEnumerable<DateOnly> tollFreeDates)
        {
            return tollFreeDates.Any(x => x == date);
        }
    }
}
