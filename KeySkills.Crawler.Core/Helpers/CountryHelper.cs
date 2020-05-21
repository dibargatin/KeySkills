using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using KeySkills.Crawler.Core.Models;

namespace KeySkills.Crawler.Core.Helpers
{
    public static class CountryHelper
    {
        public static string GetCountryName(Country code) =>
            (new RegionInfo(code.ToString())).EnglishName;

        public static Country? TryGetCountryByName(string regionName)
        {
            if(regionName != null)
            {
                Country countryCode;
                
                return _countryCodeByName.Value.TryGetValue(regionName, out countryCode) ? 
                    countryCode :
                    (Country?) null;
            }
            else return null;
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