using System.Threading.Tasks.Dataflow;

namespace DataFlowExample
{
    public class Programm
    {
        public static async Task Main()
        {
            var orderResolver = new OrderResolver();

            var metaQuery = new MetaQuery();
            
            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            var resolveOrder = new TransformBlock<Order, ResolvedOrder>(async order =>
            {
                Console.WriteLine("Resolver order '{0}'...", order.Id);
                
                var studies = await orderResolver.ResolveAsync(order);
                
                return new ResolvedOrder
                {
                    Order = order,
                    Studies =studies
                };
            });

            var enrichWithMetaData = new TransformBlock<ResolvedOrder, ResolvedOrderWithMetaData>(async resolvedOrder => 
            {
                var metaDataList = new List<StudyMetaData>();
                
                foreach(var study in resolvedOrder.Studies)
                {
                    Console.WriteLine("Enrich study with id '{0}' with meta data.", study.Id);

                    StudyMetaData metaData = await metaQuery.QueryAsync(study);

                    metaDataList.Add(metaData);
                }                

                return new ResolvedOrderWithMetaData
                {
                    Order=resolvedOrder,
                    StudyMetaData=metaDataList
                };
            });

            resolveOrder.LinkTo(enrichWithMetaData, linkOptions);

            resolveOrder.Post(new Order("Order1"));

            resolveOrder.Complete();

            await enrichWithMetaData.Completion;
        }
    }
}