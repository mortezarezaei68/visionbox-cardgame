using System.Diagnostics;
using FluentAssertions.Execution;
using NSubstitute.Core.Arguments;

namespace TestProject.Infrastructures
{
    public static class Verify
    {
        public static T That<T>(Action<T> action)
        {
            return ArgumentMatcher.Enqueue<T>(new AssertionMatcher<T>(action));
        }

        private class AssertionMatcher<T> : IArgumentMatcher<T>
        {
            private readonly Action<T> _assertion;

            public AssertionMatcher(Action<T> assertion)
            {
                _assertion = assertion;
            }

            public bool IsSatisfiedBy(T argument)
            {
                using var scope = new AssertionScope();
                _assertion((T)argument);

                var failures = scope.Discard().ToList();

                if (failures.Count == 0)
                {
                    return true;
                }

                failures.ForEach(x => Trace.WriteLine(x));

                return false;
            }
        }
    }
}