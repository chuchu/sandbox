namespace DataFlowExample
{
    internal class MetaQuery
    {
        internal Task<StudyMetaData> QueryAsync(Study study)
        {
            var studyMetaData = new StudyMetaData
            {
                Description = "Study Description."
            };
            
            return Task.FromResult(studyMetaData);
        }
    }    
}