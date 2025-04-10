namespace aoc_fast.Extensions
{
    public static class Threads
    {
        public static void Spawn(Action taskAction)
        {
            var numThreads = Environment.ProcessorCount;

            var tasks = new List<Task>();
            for (int i = 0; i < numThreads; i++)
            {
                tasks.Add(Task.Run(taskAction));
            }

            Task.WhenAll(tasks).Wait();
        }
        public static void SpawnBatches<U>(List<U> items, Action<List<U>> batchAction)
        {
            var numThreads = Environment.ProcessorCount;

            var batches = new List<List<U>>(numThreads);
            for (int i = 0; i < numThreads; i++)
            {
                batches.Add([]);
            }

            for (int i = 0; i < items.Count; i++)
            {
                batches[i % numThreads].Add(items[i]);
            }

            var tasks = batches.Select(batch =>
                Task.Run(() => batchAction(batch))).ToList();

            Task.WhenAll(tasks).Wait();
        }

    }
}
