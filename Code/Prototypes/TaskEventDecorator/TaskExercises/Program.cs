using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskExercises
{
    abstract class Requester
    {
        public event EventHandler RequestStarted;
        public event EventHandler RequestCompleted;

        public abstract void Request();

        protected virtual void OnRequestStarted(EventArgs e)
        {
            RequestStarted?.Invoke(this, e);
        }

        protected virtual void OnRequestCompleted(EventArgs e)
        {
            RequestCompleted?.Invoke(this, e);
        }

        protected virtual Task<int> SendRequest()
        {
            // get prime number --> simulate task delay.
            Task<int> primeNumberTask = Task.Run(() =>
                Enumerable.Range(2, 3000000).Count(n =>
                Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));

            return primeNumberTask;
        }
    }

    class ImageRequester : Requester
    {
        public override void Request()
        {
            OnRequestStarted(new EventArgs());
            var awaiter = SendRequest().GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                int result = awaiter.GetResult();
                OnRequestCompleted(new EventArgs());
            });
        }

        protected override void OnRequestCompleted(EventArgs e)
        {
            base.OnRequestCompleted(e);
        }

        protected override void OnRequestStarted(EventArgs e)
        {
            base.OnRequestStarted(e);
        }
    }

    class ImageRefererRequester : Requester
    {
        private Requester imageRequester;

        public ImageRefererRequester(Requester imageRequester)
        {
            this.imageRequester = imageRequester;
        }

        public override void Request()
        {
            OnRequestStarted(new EventArgs());
            var awaiter = SendRequest().GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                int result = awaiter.GetResult();
                OnRequestCompleted(new EventArgs());
                this.imageRequester.Request();
            });
        }

        protected override void OnRequestCompleted(EventArgs e)
        {
            base.OnRequestCompleted(e);
        }

        protected override void OnRequestStarted(EventArgs e)
        {
            base.OnRequestStarted(e);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ImageRequester requester = new ImageRequester();
            requester.RequestStarted += (s, e) => { Console.WriteLine("Requesting Image..."); };
            requester.RequestCompleted += (s, e) => { Console.WriteLine("... download complete!"); };
            ImageRefererRequester imageRefererRequester = new ImageRefererRequester(requester);
            imageRefererRequester.RequestStarted += (s, e) => { Console.WriteLine("Requesting Image Referer..."); };
            imageRefererRequester.RequestCompleted += (s, e) => { Console.WriteLine("... download complete!"); };
            // Should print out the referer message and then the image message.
            imageRefererRequester.Request();

            Console.ReadLine();
        }
    }
        //class Program
        //{
        //    static void Main(string[] args)
        //    {
        //        ImageRequester requester = new ImageRequester();
        //        ImageRefererRequester imageRefererRequester = new ImageRefererRequester(requester);

        //        // Should print out the referer message and then the image message.
        //        imageRefererRequester.Request();


        //        //Task<int> primeNumberTask = Task.Run(() =>
        //        //    Enumerable.Range(2, 3000000).Count(n =>
        //        //    Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));


        //        //var awaiter = primeNumberTask.GetAwaiter();
        //        //awaiter.OnCompleted(() =>
        //        //{
        //        //    int result = awaiter.GetResult();
        //        //    Console.WriteLine(result); // Writes result
        //        //});

        //        Console.ReadLine();

        //        // Tasks are pooled threads by default, which are background threads. This means that
        //        // when the main thread ends, so do any tasks you create.
        //        // Therefore must block the thread in a console app...
        //        //Task.Run(() => Console.WriteLine("Foo"));
        //        //Console.ReadLine();

        //        //similar to this...
        //        //new Thread(() => Console.WriteLine("Foo")).Start();
        //        //Console.ReadLine();

        //        //// Here, "Wait" blocks the current thread until the task is complete.
        //        //Task task = Task.Run(() => {
        //        //    Thread.Sleep(2000);
        //        //    Console.WriteLine("Foo");
        //        //});
        //        //Console.WriteLine(task.IsCompleted); // should be false...
        //        //task.Wait(); // Blocks until task is complete. Optionally specify a timeout and cancellation token to end wait.

        //        //// Don't use the thread pool
        //        //Task task = Task.Factory.StartNew(() =>
        //        //{
        //        //    Console.WriteLine("Started running long running thread...");
        //        //    Thread.Sleep(5000);
        //        //    Console.WriteLine("Foo");
        //        //}, TaskCreationOptions.LongRunning); // don't use the thread pool...
        //        //task.Wait();

        //        //Task<int> task = Task.Run(() =>
        //        //{
        //        //    Console.WriteLine("Computing the answer to everything...");
        //        //    Thread.Sleep(5000);
        //        //    return 42;
        //        //});
        //        //int result = task.Result;  // Blocks if not already finished...
        //        //Console.WriteLine(result); // 42
        //        //Console.ReadLine();

        //        //// In the following example, we create a task that uses LINQ to count the number of prime numbers
        //        //// in the first three million (+2) integers.
        //        //Task<int> primeNumberTask = Task.Run(() =>
        //        //    Enumerable.Range(2, 3000000).Count(n =>
        //        //        Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        //        //Console.WriteLine("Task running...");
        //        //Console.WriteLine("The answer is " + primeNumberTask.Result);
        //        //Console.ReadLine(); // ensures that console remains open after task returns and result is output.
        //        //// outputs 216815.

        //        ////  If your task faults, that exception is re-thrown to whomever calls "Wait()" or accesses
        //        //// the "Result" property of a Task<TResult>.
        //        //// Start a task that throws a NullReferenceException...
        //        //Task task = Task.Run(() => { throw null; });
        //        //try
        //        //{
        //        //    task.Wait();
        //        //}
        //        //catch (AggregateException aex)
        //        //{
        //        //    if (aex.InnerException is NullReferenceException)
        //        //        Console.WriteLine("Null!");
        //        //    else
        //        //        throw;
        //        //}
        //        //Console.ReadLine();
        //        //// The CLR wraps the exception in an AggregateException in order to play well with parallel programming
        //        //// scenarios...


        //        //Task task = Task.Run(() => { throw null; });
        //        //try
        //        //{
        //        //    task.Wait();
        //        //}
        //        //catch (AggregateException aex)
        //        //{
        //        //    // CLR wraps the exception in an AggregateException in order to play well with
        //        //    // parallel programming scenarios (see C# 5.0 in a Nutshell - Chapter 23).
        //        //    if (aex.InnerException is NullReferenceException)
        //        //    {
        //        //        Console.WriteLine("Null!");
        //        //    }
        //        //    else
        //        //    {
        //        //        throw;
        //        //    }
        //        //}
        //        //// or (if both properties return false, then no exception occurred):
        //        //bool isCanceled = task.IsCanceled;  // OperationCanceledOperation was thrown...
        //        //bool isFaulted = task.IsFaulted;    // another type of exception was thrown.

        //        //Console.WriteLine("IsCanceled: {0}", isCanceled);
        //        //Console.WriteLine("IsFaulted: {0}", isFaulted);
        //        //Console.ReadLine();


        //    }
    //}
}
