using System.Text;
using System.Web;

namespace OpenElectricity.Sdk
{
    public class UriQueryParams
    {
        private readonly StringBuilder _query = new();

        public void Add(string key, string? value)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value)) 
                return;

            if (_query.Length > 0)
            {
                _query.Append('&');
            }
            _query.Append(key);
            _query.Append('=');
            _query.Append(value);
        }

        public void Add(string key, IEnumerable<string> values)
        {
            foreach(string value in values)
            {
                Add(key, value);
            }
        }

        public override string ToString()
        {
            if (_query.Length > 0)
            {
                _query.Insert(0, '?');
            }
            return _query.ToString();
        }
    }
}
