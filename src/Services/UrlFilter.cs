using DataStructures;

namespace Services
{
    public class UrlFilter
    {
        private static readonly BloomFilter<string> bloomFilter = new BloomFilter<string>(1000000000, 0.50F);

        public static bool Filter(Url url)
        {
            bool knownUrl = bloomFilter.Contains(url.AbsoluteUri);

            if (!knownUrl)
            {
                bloomFilter.Add(url.AbsoluteUri);
            }

            return knownUrl;
        }
    }
}
