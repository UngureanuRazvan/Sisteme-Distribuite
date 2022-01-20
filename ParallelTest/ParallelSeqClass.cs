using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelTest
{
    public class ParallelSeqClass
    {
        public ParallelSeqClass()
        {

        }

        public void ParallelMethod(ParallelOptions ops,int range)
        {
            int sum = 0;
            Parallel.ForEach<int>(Enumerable.Range(1, range), ops, x => { sum += x; });
        }

        public void ParallelMethod(int range)
        {
            int sum = 0;
            Parallel.ForEach<int>(Enumerable.Range(1, range), x => { sum += x; });
        }

        public void SequentialMethod(int range)
        {
            int sum = 0;

            foreach (var x in Enumerable.Range(1, range))
            {
                sum += x;
            }
        }
    }
}
