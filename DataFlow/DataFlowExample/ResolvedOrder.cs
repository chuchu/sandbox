namespace DataFlowExample
{
    internal class ResolvedOrder
    {
        internal Order Order {get;set;}

        internal IEnumerable<Study> Studies {get;set;}
    }
}