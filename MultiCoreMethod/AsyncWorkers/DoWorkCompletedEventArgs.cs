using System;
using System.ComponentModel;

namespace AsyncWorkers
{
    public class DoWorkCompletedEventArgs : AsyncCompletedEventArgs
    {
        public EapWorker Worker { get; set; }
        public DoWorkCompletedEventArgs(EapWorker result, Exception error,bool cancelled, object state) 
            : base(error,cancelled,state)
        {
            this.Worker = result;
        }
    }
}
