using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using KeySkills.Crawler.Core.Models;

namespace KeySkills.Crawler.Core.Helpers
{
    /// <summary>
    /// Contains helper methods for Country enum
    /// </summary>
    public static class CountryHelper
    {
        /// <summary>
        /// Returns english country name by country code
        /// </summary>
        /// <param name="code">Country code</param>
        /// <returns>English country name </returns>
        public static string GetCountryName(Country code) =>
            (new RegionInfo(code.ToString())).EnglishName;

        /// <summary>
        /// Tries to get country code by region name
        /// </summary>
        /// <param name="regionName">Region name on any language</param>
        /// <returns><see cref="Country"/> or <see langword="null"/></returns>
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
        
        /// <summary>
        /// Checks string to be a US state code
        /// </summary>
        /// <returns><see langword="true"/> when string is a US state code, or <see langword="false"/> otherwise</returns>
        public static bool IsUsaState(string code) =>
            _usaState.Value.Contains(code);

        private static readonly Lazy<HashSet<string>> _usaState =
            new Lazy<HashSet<string>>(() => 
                new HashSet<string>(new[] {
                    "AL", // Alabama
                    "AK", // Alaska
                    "AS", // American Samoa
                    "AZ", // Arizona
                    "AR", // Arkansas
                    "CA", // California
                    "CO", // Colorado
                    "CT", // Connecticut
                    "DE", // Delaware
                    "DC", // District of Columbia
                    "FL", // Florida
                    "GA", // Georgia
                    "GU", // Guam
                    "HI", // Hawaii
                    "ID", // Idaho
                    "IL", // Illinois
                    "IN", // Indiana
                    "IA", // Iowa
                    "KS", // Kansas
                    "KY", // Kentucky
                    "LA", // Louisiana
                    "ME", // Maine
                    "MD", // Maryland
                    "MA", // Massachusetts
                    "MI", // Michigan
                    "MN", // Minnesota
                    "MS", // Mississippi
                    "MO", // Missouri
                    "MT", // Montana
                    "NE", // Nebraska
                    "NV", // Nevada
                    "NH", // New Hampshire
                    "NJ", // New Jersey
                    "NM", // New Mexico
                    "NY", // New York
                    "NC", // North Carolina
                    "ND", // North Dakota
                    "MP", // Northern Mariana Islands
                    "OH", // Ohio
                    "OK", // Oklahoma
                    "OR", // Oregon
                    "PA", // Pennsylvania
                    "PR", // Puerto Rico
                    "RI", // Rhode Island
                    "SC", // South Carolina
                    "SD", // South Dakota
                    "TN", // Tennessee
                    "TX", // Texas
                    "UM", // United States Minor Outlying Islands
                    "UT", // Utah
                    "VT", // Vermont
                    "VI", // Virgin Islands, U.S.
                    "VA", // Virginia
                    "WA", // Washington
                    "WV", // West Virginia
                    "WI", // Wisconsin
                    "WY", // Wyoming
                })
            );
    }
}