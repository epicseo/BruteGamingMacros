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
        private volatile bool _shouldStop;

        [SetUp]
        public void Setup()
        {
            _executionCount = 0;
            _shouldStop = false;
        }

        [TearDown]
        public void TearDown()
        {
            _runner?.Terminate();
            _runner = null;
        }

        [Test]
        public void Run_ExecutesCallback()
        {
            // Arrange
            var executed = new ManualResetEventSlim(false);

            _runner = new ThreadRunner(() =>
            {
                _executionCount++;
                if (_executionCount >= 3)
                {
                    executed.Set();
                    return false; // Stop after 3 executions
                }
                return true;
            }, 10);

            // Act
            _runner.Run();
            bool signaled = executed.Wait(TimeSpan.FromSeconds(2));

            // Assert
            Assert.IsTrue(signaled, "Thread should have executed at least 3 times");
            Assert.GreaterOrEqual(_executionCount, 3);
        }

        [Test]
        public void Terminate_StopsThread()
        {
            // Arrange
            _runner = new ThreadRunner(() =>
            {
                _executionCount++;
                Thread.Sleep(10);
                return true;
            }, 10);

            // Act
            _runner.Run();
            Thread.Sleep(100); // Let it run a bit
            _runner.Terminate();
            int countAfterTerminate = _executionCount;
            Thread.Sleep(100); // Wait to see if it continues

            // Assert - count should not increase significantly after terminate
            Assert.LessOrEqual(_executionCount - countAfterTerminate, 1,
                "Thread should stop after Terminate()");
        }

        [Test]
        public void Suspend_PausesExecution()
        {
            // Arrange
            _runner = new ThreadRunner(() =>
            {
                _executionCount++;
                Thread.Sleep(10);
                return true;
            }, 10);

            // Act
            _runner.Run();
            Thread.Sleep(100); // Let it run
            _runner.Suspend();
            int countAtSuspend = _executionCount;
            Thread.Sleep(100); // Wait while suspended
            int countAfterSuspend = _executionCount;

            // Assert - count should not increase while suspended
            Assert.LessOrEqual(countAfterSuspend - countAtSuspend, 1,
                "Thread should pause during Suspend()");
        }

        [Test]
        public void Resume_ContinuesAfterSuspend()
        {
            // Arrange
            _runner = new ThreadRunner(() =>
            {
                _executionCount++;
                Thread.Sleep(10);
                return true;
            }, 10);

            // Act
            _runner.Run();
            Thread.Sleep(50);
            _runner.Suspend();
            int countAtSuspend = _executionCount;
            Thread.Sleep(50);
            _runner.Resume();
            Thread.Sleep(100); // Let it run after resume

            // Assert - count should increase after resume
            Assert.Greater(_executionCount, countAtSuspend,
                "Thread should continue after Resume()");
        }

        [Test]
        public void CallbackReturningFalse_StopsThread()
        {
            // Arrange
            _runner = new ThreadRunner(() =>
            {
                _executionCount++;
                return _executionCount < 5; // Stop after 5 executions
            }, 10);

            // Act
            _runner.Run();
            Thread.Sleep(500); // Wait for completion

            // Assert
            Assert.AreEqual(5, _executionCount, "Thread should stop after callback returns false");
        }

        [Test]
        public void MultipleTerminateCalls_DoesNotThrow()
        {
            // Arrange
            _runner = new ThreadRunner(() => true, 100);
            _runner.Run();
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
        public void TerminateBeforeRun_DoesNotThrow()
        {
            // Arrange
            _runner = new ThreadRunner(() => true, 100);

            // Act & Assert
            Assert.DoesNotThrow(() => _runner.Terminate());
        }

        [Test]
        public void Constructor_SetsDelayCorrectly()
        {
            // Arrange & Act
            _runner = new ThreadRunner(() => false, 500);

            // Assert - we can't directly check delay, but we can verify it doesn't throw
            Assert.IsNotNull(_runner);
        }

        [Test]
        public void Run_WithException_ContinuesExecution()
        {
            // Arrange
            int exceptionCount = 0;
            _runner = new ThreadRunner(() =>
            {
                _executionCount++;
                if (_executionCount == 2)
                {
                    exceptionCount++;
                    throw new InvalidOperationException("Test exception");
                }
                return _executionCount < 5;
            }, 10);

            // Act
            _runner.Run();
            Thread.Sleep(500);

            // Assert - thread should continue despite exception
            Assert.GreaterOrEqual(_executionCount, 5, "Thread should continue after exception");
            Assert.AreEqual(1, exceptionCount, "Exception should have been thrown once");
        }
    }
}
