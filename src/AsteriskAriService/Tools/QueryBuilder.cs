namespace AsteriskAriService.Tools
{
    public class QueryBuilder(string path)
    {
        private readonly List<string> _parts = [];

        public QueryBuilder Add(string key, object? value)
        {
            if (value is null)
                return this;

            _parts.Add($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value.ToString()!)}");
            return this;
        }

        public override string ToString()
        {
            if (_parts.Count == 0)
                return path;
            return path + "?" + string.Join("&", _parts);
        }
    }
}