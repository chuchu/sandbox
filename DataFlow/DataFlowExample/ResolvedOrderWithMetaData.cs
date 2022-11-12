namespace DataFlowExample
{
    internal class ResolvedOrderWithMetaData
    {
        internal ResolvedOrder Order {get;set;}

        internal IEnumerable<StudyMetaData> StudyMetaData {get;set;}
    }
}