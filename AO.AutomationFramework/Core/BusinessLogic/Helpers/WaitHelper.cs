using AO.AutomationFramework.Core.BusinessLogic.Variables;
using System;
using System.Threading;

namespace AO.AutomationFramework.Core.BusinessLogic.Helpers
{
    public static class WaitHelper
    {
        private static readonly int TimeStep = CommonVar.WaitTimeStep;

        private static readonly int TimeFactor = CommonVar.WaitTimeFactor;

        public static int Timeout = CommonVar.WaitTimeout;

        public static TResult WaitResult<TResult>(Func<TResult> action, string errorMessage = null) where TResult : class
        {
            var stopDate = DateTime.Now.AddMilliseconds(Timeout);
            TResult result = null;
            do
            {
                try
                {
                    result = action();
                }
                catch (Exception ex)
                {
                    errorMessage = (!string.IsNullOrWhiteSpace(errorMessage))
                                       ? string.Format("{0}, wait error: {1}", errorMessage, ex.Message)
                        : ex.Message;
                }
                if (result != null)
                {
                    return result;
                }
                Thread.Sleep(TimeStep);
            } while (DateTime.Now < stopDate);

            if (result == null)
            {
                throw new Exception(errorMessage);
            }

            return result;
        }

        public static TResult WaitResult<TResult>(Func<TResult> action, int timeout, string errorMessage = null) where TResult : class
        {
            var stopDate = DateTime.Now.AddMilliseconds(timeout);
            TResult result = null;
            do
            {
                try
                {
                    result = action();
                }
                catch (Exception ex)
                {
                    errorMessage = (!string.IsNullOrWhiteSpace(errorMessage))
                                       ? string.Format("{0}, wait error: {1}", errorMessage, ex.Message)
                        : ex.Message;
                }
                if (result != null)
                {
                    return result;
                }
                Thread.Sleep(TimeStep);
            } while (DateTime.Now < stopDate);

            if (result == null)
            {
                throw new Exception(errorMessage);
            }

            return result;
        }

        public static bool WaitUntil(Func<bool> action)
        {
            var stopDate = DateTime.Now.AddMilliseconds(Timeout);
            bool result = false;
            do
            {
                try { result = action(); }
                catch (Exception)
                {
                    // ignored
                }
                if (!result)
                {
                    Thread.Sleep(TimeStep);
                }
            } while (!result && DateTime.Now < stopDate);

            return result;
        }

        public static bool WaitUntil(Func<bool> action, int timeout)
        {
            var stopDate = DateTime.Now.AddMilliseconds(timeout);
            bool result = false;
            do
            {
                try { result = action(); }
                catch (Exception)
                {
                    // ignored
                }
                if (!result)
                {
                    Thread.Sleep(TimeStep);
                }
            } while (!result && DateTime.Now < stopDate);

            return result;
        }

        public static void WaitFor(int timeOut)
        {
            Thread.Sleep(timeOut * TimeFactor);
        }
    }
}