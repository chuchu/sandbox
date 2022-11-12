namespace DataFlowExample
{
    internal class OrderResolver
    {
        internal Task<IEnumerable<Study>> ResolveAsync(Order order)
        {
            var studies = new List<Study>
            {
                new Study("1.2.3"),
                new Study("1.2.4")
            };

            return Task.FromResult<IEnumerable<Study>>(studies);
        }
    }
}