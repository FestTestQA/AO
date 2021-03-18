using AO.AutomationFramework.Core.GUI.Pages;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using System;
using System.Diagnostics;

namespace AO.AutomationFramework.Core.BusinessLogic.Extensions
{
    public static class NUnitExtentions
    {
        public static void IsInstanceOf<TExpected>(this Verify a, Func<PageBase> getActual, int secondsToPass)
        {
            bool fail = true;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while (fail && timer.Elapsed.Seconds <= secondsToPass)
            {
                try
                {
                    var actual = getActual();
                    Assert.IsInstanceOf<TExpected>(actual);
                    fail = false;
                }
                catch (Exception) { }
            }
            if (fail)
            {
                var actual = getActual();
                Assert.IsInstanceOf<TExpected>(actual);
            }
        }

        public static void IsTrue(this Verify a, Func<bool> conditionAction, int secondsToPass)
        {
            bool fail = true;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while (fail && timer.Elapsed.Seconds <= secondsToPass)
            {
                try
                {
                    var condition = conditionAction();
                    Assert.IsTrue(condition);
                    fail = false;
                }
                catch (Exception) { }
            }
            if (fail)
            {
                var condition = conditionAction();
                Assert.IsTrue(condition);
            }
        }

        public static void IsFalse(this Verify a, Func<bool> conditionAction, int secondsToFail)
        {
            bool fail = true;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while (fail && timer.Elapsed.Seconds <= secondsToFail)
            {
                try
                {
                    var condition = conditionAction();
                    Assert.IsFalse(condition);
                    fail = false;
                }
                catch (Exception) { }
            }
            if (fail)
            {
                var condition = conditionAction();
                Assert.IsFalse(condition);
            }
        }
    }

    public class Verify : Assert
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RetryOnAny : NUnitAttribute, IRepeatTest
    {
        private readonly int _tryCount;

        /// <summary>
        /// Construct a <see cref="RetryOnAny" />
        /// </summary>
        /// <param name="tryCount">The maximum number of times the test should be run if it fails</param>
        public RetryOnAny(int tryCount)
        {
            _tryCount = tryCount;
            TestExecutionContext.CurrentContext.CurrentRepeatCount = tryCount;
        }

        #region IRepeatTest Members

        /// <summary>
        /// Wrap a command and return the result.
        /// </summary>
        /// <param name="command">The command to be wrapped</param>
        /// <returns>The wrapped command</returns>
        public TestCommand Wrap(TestCommand command)
        {
            return new RetryCommand(command, _tryCount);
        }

        #endregion IRepeatTest Members

        #region Nested RetryCommand Class

        /// <summary>
        /// The test command for the <see cref="RetryOnAny"/>
        /// </summary>
        public class RetryCommand : DelegatingTestCommand
        {
            private readonly int _tryCount;

            /// <summary>
            /// Initializes a new instance of the <see cref="RetryCommand"/> class.
            /// </summary>
            /// <param name="innerCommand">The inner command.</param>
            /// <param name="tryCount">The maximum number of repetitions</param>
            public RetryCommand(TestCommand innerCommand, int tryCount)
                : base(innerCommand)
            {
                _tryCount = tryCount;
            }

            /// <summary>
            /// Runs the test, saving a TestResult in the supplied TestExecutionContext.
            /// </summary>
            /// <param name="context">The context in which the test should run.</param>
            /// <returns>A TestResult</returns>
            public override TestResult Execute(TestExecutionContext context)
            {
                int count = _tryCount;

                while (count-- > 0)
                {
                    try
                    {
                        context.CurrentResult = innerCommand.Execute(context);
                    }
                    // Commands are supposed to catch exceptions, but some don't
                    // and we want to look at restructuring the API in the future.
                    catch (Exception ex)
                    {
                        if (context.CurrentResult == null)
                        {
                            context.CurrentResult = context.CurrentTest.MakeTestResult();
                        }

                        context.CurrentResult.RecordException(ex);
                    }

                    if (context.CurrentResult.ResultState == ResultState.Ignored
                        || context.CurrentResult.ResultState == ResultState.Success)
                    {
                        break;
                    }

                    // Clear result for retry
                    if (count > 0)
                    {
                        context.CurrentResult = context.CurrentTest.MakeTestResult();
                        context.CurrentRepeatCount++; // increment Retry count for next iteration. will only happen if we are guaranteed another iteration
                    }
                }

                return context.CurrentResult;
            }
        }

        #endregion Nested RetryCommand Class
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ReuseTestData : NUnitAttribute
    {
        public ReuseTestData(bool reuseLastTestData, bool keepCurrentTestData)
        {
            ReuseLastTestData = reuseLastTestData;
            KeepCurrentTestData = keepCurrentTestData;
        }

        public bool ReuseLastTestData { get; }

        public bool KeepCurrentTestData { get; }
    }
}