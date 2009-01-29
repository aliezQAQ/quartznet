/* 
 * Copyright 2004-2007 OpenSymphony 
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not 
 * use this file except in compliance with the License. You may obtain a copy 
 * of the License at 
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0 
 *   
 * Unless required by applicable law or agreed to in writing, software 
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations 
 * under the License.
 */

using Common.Logging;

using MbUnit.Framework;

using Quartz.Job;
using Quartz.Plugin.History;
using Quartz.Spi;

using Rhino.Mocks;

namespace Quartz.Tests.Unit.Plugin.History
{
    [TestFixture]
    public class LoggingJobHistoryPluginTest
    {
        private LoggingJobHistoryPlugin plugin;
        private ILog mockLog;

        [SetUp]
        public void SetUp()
        {
            mockLog = MockRepository.GenerateMock<ILog>();           
            plugin = new LoggingJobHistoryPlugin();
            plugin.Log = mockLog;
        }

        [Test]
        public void TestJobFailedMessage()
        {
            // arrange
            mockLog.Stub(log => log.IsWarnEnabled).Return(true);

            // act
            JobExecutionException ex = new JobExecutionException("test error");
            plugin.JobWasExecuted(CreateJobExecutionContext(), ex);
            
            // assert
            mockLog.AssertWasCalled(log => log.Warn(null, null), options => options.IgnoreArguments());
        }

        [Test]
        public void TestJobSuccessMessage()
        {
            // arrange
            mockLog.Stub(log => log.IsInfoEnabled).Return(true);

            // act
            plugin.JobWasExecuted(CreateJobExecutionContext(), null);

            // assert
            mockLog.AssertWasCalled(log => log.Info(null), options => options.IgnoreArguments());
        }

        [Test]
        public void TestJobToBeFiredMessage()
        {
            // arrange
            mockLog.Stub(log => log.IsInfoEnabled).Return(true);

            // act
            plugin.JobToBeExecuted(CreateJobExecutionContext());
        
            // assert
            mockLog.AssertWasCalled(log => log.Info(null), options => options.IgnoreArguments());
        }

        [Test]
        public void TestJobWasVetoedMessage()
        {
            // arrange
            mockLog.Stub(log => log.IsInfoEnabled).Return(true);

            // act
            plugin.JobExecutionVetoed(CreateJobExecutionContext());

            // assert
            mockLog.AssertWasCalled(log => log.Info(null), options => options.IgnoreArguments());
        }

        protected virtual JobExecutionContext CreateJobExecutionContext()
        {
            Trigger t = new SimpleTrigger();
            TriggerFiredBundle firedBundle = TestUtil.CreateMinimalFiredBundleWithTypedJobDetail(typeof(NoOpJob), t);
            JobExecutionContext ctx = new JobExecutionContext(null,  firedBundle, null);

            return ctx;
        }

    }
}
