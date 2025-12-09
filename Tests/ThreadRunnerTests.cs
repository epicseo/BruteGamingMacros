using NUnit.Framework;
using BruteGamingMacros.Core.Utils;
using System;
using System.Threading;

namespace BruteGamingMacros.Tests
{
    [TestFixture]
    public class ThreadRunnerTests
    {
        private ThreadRunner _runner;
        private int _executionCount;

        [SetUp]
        public void Setup()
        {
            _executionCount = 0;
        }

        [TearDown]
        public void TearDown()
        {
            _runner?.Terminate();
            _runner = null;
        }

        [Test]
        public void Start_ExecutesCallback()
        {
            // Arrange
            var executed = new ManualResetEventSlim(false);

            _runner = new ThreadRunner((delay) =>
            {
                _executionCount++;
                if (_executionCount >= 3)
                {
                    executed.Set();
                }
                return delay; // Return delay as-is
            });

            // Act
            ThreadRunner.Start(_runner);
            bool signaled = executed.Wait(TimeSpan.FromSeconds(2));

            // Assert
            Assert.IsTrue(signaled, "Thread should have executed at least 3 times");
            Assert.GreaterOrEqual(_executionCount, 3);
        }

        [Test]
        public void Terminate_StopsThread()
        {
            // Arrange
            _runner = new ThreadRunner((delay) =>
            {
                _executionCount++;
                Thread.Sleep(10);
                return delay;
            });

            // Act
            ThreadRunner.Start(_runner);
            Thread.Sleep(100); // Let it run a bit
            _runner.Terminate();
            int countAfterTerminate = _executionCount;
            Thread.Sleep(100); // Wait to see if it continues

            // Assert - count should not increase significantly after terminate
            Assert.LessOrEqual(_executionCount - countAfterTerminate, 1,
                "Thread should stop after Terminate()");
        }

        [Test]
        public void Stop_PausesExecution()
        {
            // Arrange
            _runner = new ThreadRunner((delay) =>
            {
                _executionCount++;
                Thread.Sleep(10);
                return delay;
            });

            // Act
            ThreadRunner.Start(_runner);
            Thread.Sleep(100); // Let it run
            ThreadRunner.Stop(_runner);
            int countAtStop = _executionCount;
            Thread.Sleep(100); // Wait while stopped
            int countAfterStop = _executionCount;

            // Assert - count should not increase while stopped
            Assert.LessOrEqual(countAfterStop - countAtStop, 1,
                "Thread should pause during Stop()");
        }

        [Test]
        public void Start_ContinuesAfterStop()
        {
            // Arrange
            _runner = new ThreadRunner((delay) =>
            {
                _executionCount++;
                Thread.Sleep(10);
                return delay;
            });

            // Act
            ThreadRunner.Start(_runner);
            Thread.Sleep(50);
            ThreadRunner.Stop(_runner);
            int countAtStop = _executionCount;
            Thread.Sleep(50);
            ThreadRunner.Start(_runner); // Resume by calling Start again
            Thread.Sleep(100); // Let it run after resume

            // Assert - count should increase after resume
            Assert.Greater(_executionCount, countAtStop,
                "Thread should continue after Start() following Stop()");
        }

        [Test]
        public void MultipleTerminateCalls_DoesNotThrow()
        {
            // Arrange
            _runner = new ThreadRunner((delay) => delay);
            ThreadRunner.Start(_runner);
            Thread.Sleep(50);

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                _runner.Terminate();
                _runner.Terminate();
                _runner.Terminate();
            });
        }

        [Test]
        public void TerminateBeforeStart_DoesNotThrow()
        {
            // Arrange
            _runner = new ThreadRunner((delay) => delay);

            // Act & Assert
            Assert.DoesNotThrow(() => _runner.Terminate());
        }

        [Test]
        public void Constructor_CreatesInstance()
        {
            // Arrange & Act
            _runner = new ThreadRunner((delay) => delay);

            // Assert - we can't directly check internal state, but we can verify it doesn't throw
            Assert.IsNotNull(_runner);
        }

        [Test]
        public void Start_WithException_ContinuesExecution()
        {
            // Arrange
            int exceptionCount = 0;
            _runner = new ThreadRunner((delay) =>
            {
                _executionCount++;
                if (_executionCount == 2)
                {
                    exceptionCount++;
                    throw new InvalidOperationException("Test exception");
                }
                return delay;
            });

            // Act
            ThreadRunner.Start(_runner);
            Thread.Sleep(500);

            // Assert - thread should continue despite exception
            Assert.GreaterOrEqual(_executionCount, 5, "Thread should continue after exception");
            Assert.AreEqual(1, exceptionCount, "Exception should have been thrown once");
        }

        [Test]
        public void Stop_OnNullRunner_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => ThreadRunner.Stop(null));
        }

        [Test]
        public void Start_OnNullRunner_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => ThreadRunner.Start(null));
        }
    }
}
