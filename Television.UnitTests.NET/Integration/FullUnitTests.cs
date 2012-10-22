using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Television.Interfaces;
using System.Threading;

namespace Television.UnitTests.NET.Integration
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class FullUnitTests
    {
        public FullUnitTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }

        ITelevision m_Television;
        IRemoteReceiver m_RemoteReceiver;
        IRemoteControl m_RemoteControl;
        int MAX_CHANNELS = 99;

        [TestInitialize()]
        public void Initialize() 
        {
            m_Television = new Television.Models.Television(MAX_CHANNELS);
            m_RemoteReceiver = new Television.Models.RemoteReceiver(m_Television);
            m_RemoteControl = new Television.Models.RemoteControl();
            m_RemoteControl.Program((result) =>
            {
                // Verify we are programmed
                Assert.IsTrue(result);
            });

            System.Threading.Thread.Yield();    // Allow the remote time to be programmed
        }
             
        [TestCleanup()]
        public void Cleanup() 
        {
            m_RemoteReceiver.Dispose();
            m_RemoteControl.Dispose();
        }
        
        #endregion

        [TestMethod]
        public void Television_known_state()
        {            
            Assert.IsTrue(m_Television.State == Models.Enums.TelevisionStateEnum.OFF);         
            Assert.IsTrue(m_Television.Channel == 1);            
        }

        [TestMethod]
        public void Television_State_ON_OFF()
        {            
            m_Television.State = Models.Enums.TelevisionStateEnum.ON;
            Assert.IsTrue(m_Television.State == Models.Enums.TelevisionStateEnum.ON);

            m_Television.State = Models.Enums.TelevisionStateEnum.OFF;
            Assert.IsTrue(m_Television.State == Models.Enums.TelevisionStateEnum.OFF);
        }

        [TestMethod]
        public void Television_Channel_increment()
        {
            Assert.IsTrue(m_Television.Channel == 1);
            m_Television.Channel++;
            Assert.IsTrue(m_Television.Channel == 2);
        }

        [TestMethod]
        public void Television_Channel_increment_rollover_MAX_CHANNELS()
        {            
            m_Television.Channel = MAX_CHANNELS;
            m_Television.Channel++;
            Assert.IsTrue(m_Television.Channel == 1);
        }

        [TestMethod]
        public void Television_Channel_decrement()
        {
            m_Television.Channel = MAX_CHANNELS;
            m_Television.Channel--;
            Assert.IsTrue(m_Television.Channel == MAX_CHANNELS - 1);
        }

        [TestMethod]
        public void Television_Channel_decrement_rollunder()
        {
            Assert.IsTrue(m_Television.Channel == 1);
            m_Television.Channel--;
            Assert.IsTrue(m_Television.Channel == MAX_CHANNELS);
        }

        [TestMethod]
        public void RemoteControl_State_ON_OFF()
        {                       
            m_RemoteControl.State(Models.Enums.TelevisionStateEnum.ON);            
            Assert.IsTrue(m_Television.State == Models.Enums.TelevisionStateEnum.ON);
               
            m_RemoteControl.State(Models.Enums.TelevisionStateEnum.OFF);            
            Assert.IsTrue(m_Television.State == Models.Enums.TelevisionStateEnum.OFF);
        }

        [TestMethod]
        public void RemoteControl_Channel_increment()
        {
            m_RemoteControl.State(Models.Enums.TelevisionStateEnum.ON);            
            Assert.IsTrue(m_Television.Channel == 1);
            m_RemoteControl.Channel_Up();            
            Assert.IsTrue(m_Television.Channel == 2);
        }

        [TestMethod]
        public void RemoteControl_Channel_increment_rollover_MAX_CHANNELS()
        {
            m_RemoteControl.State(Models.Enums.TelevisionStateEnum.ON);           
            m_RemoteControl.Channel(MAX_CHANNELS);
            m_RemoteControl.Channel_Up();
            Assert.IsTrue(m_Television.Channel == 1);
        }

        [TestMethod]
        public void RemoteControl_Channel_decrement()
        {
            m_RemoteControl.State(Models.Enums.TelevisionStateEnum.ON);
            m_RemoteControl.Channel(MAX_CHANNELS);
            m_RemoteControl.Channel_Down();
            Assert.IsTrue(m_Television.Channel == MAX_CHANNELS - 1);
        }

        [TestMethod]
        public void RemoteControl_Channel_decrement_rollunder()
        {
            m_RemoteControl.State(Models.Enums.TelevisionStateEnum.ON);            
            Assert.IsTrue(m_Television.Channel == 1);
            m_RemoteControl.Channel_Down();
            Assert.IsTrue(m_Television.Channel == MAX_CHANNELS);
        }

    }
}
