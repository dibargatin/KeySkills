using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using KeySkills.Crawler.Core.Models;

namespace KeySkills.Crawler.Core.Helpers
{
    public static class CountryHelper
    {
        public static Country? TryGetCountryByName(string regionName)
        {
            Country countryCode;

            if(regionName != null && _countryCodeByName.Value.TryGetValue(regionName, out countryCode))
            {            
                return countryCode;
            }
            else
            {
                return null;
            }
        }

        private static readonly Lazy<ReadOnlyDictionary<string, Country>> _countryCodeByName = 
            new Lazy<ReadOnlyDictionary<string, Country>>(() => 
                new ReadOnlyDictionary<string, Country>(
                    CultureInfo
                        .GetCultures(CultureTypes.SpecificCultures)                            
                        .Select(ci => new RegionInfo(ci.ToString()))
                        .Join(Enumerable.Range(0, 2), _ => true, _ => true, (region, n) => new {
                            Code = region.TwoLetterISORegionName,
                            Name = n == 0 ? region.NativeName : region.EnglishName
                        }).GroupBy(ri => ri.Name)
                        .ToDictionary(
                            g => g.Key,
                            g => Enum.Parse<Country>(g.Select(region => region.Code).First())
                        ))
            );
    }
}