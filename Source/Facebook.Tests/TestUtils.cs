namespace Facebook.Tests
{
    using System;
    using System.Threading;

    public class TestUtils
    {
        public static void DoWork(Action action, int timeout)
        {
            ManualResetEvent evt = new ManualResetEvent(false);
            AsyncCallback cb = delegate { evt.Set(); };
            IAsyncResult result = action.BeginInvoke(cb, null);
            if (evt.WaitOne(timeout))
            {
                action.EndInvoke(result);
            }
            else
            {
                throw new TimeoutException();
            }
        }

        public static T DoWork<T>(Func<T> func, int timeout)
        {
            ManualResetEvent evt = new ManualResetEvent(false);
            AsyncCallback cb = delegate { evt.Set(); };
            IAsyncResult result = func.BeginInvoke(cb, null);
            if (evt.WaitOne(timeout))
            {
                return func.EndInvoke(result);
            }
            else
            {
                throw new TimeoutException();
            }
        }
    }
}