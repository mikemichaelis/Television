using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Television.Models;

namespace Television.UnitTests.NET.Component
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TelevisionUnitTests
    {
        public TelevisionUnitTests()
        {           
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
        //
         
         [TestInitialize()]
         public void Initialize() 
         {
             m_Television = new Television.Models.Television(MAX_CHANNELS);
         }
        
         [TestCleanup()]
         public void Cleanup() 
         {
             m_Television = null;
         }
        
        #endregion

         private int MAX_CHANNELS = 99;
        private Television.Models.Television m_Television = null;

        [TestMethod]
        public void Television_Create_known_state_success()
        {
            Assert.IsNotNull(m_Television, "Unable to create Television");
            Assert.IsTrue(m_Television.Channel == 1);       // Not going to add a bunch of error messages, but they go here
            Assert.IsTrue(m_Television.State == Models.Enums.TelevisionStateEnum.OFF);
            Assert.IsTrue(m_Television.m_MaxChannel == MAX_CHANNELS);       // InternalsVisibleTo us for internal test condition checking
        }

        [TestMethod]
        public void Television_change_State_success()
        {
            m_Television.State = Models.Enums.TelevisionStateEnum.ON;
            Assert.IsTrue(m_Television.State == Models.Enums.TelevisionStateEnum.ON);            
        }

        [TestMethod]
        public void Television_Channel_increment_success()
        {
            m_Television.Channel++;
            Assert.IsTrue(m_Television.Channel == 2);
        }

        [TestMethod]
        public void Television_Channel_assignment_success()
        {
            m_Television.Channel = MAX_CHANNELS;
            Assert.IsTrue(m_Television.Channel == MAX_CHANNELS);
        }

        [TestMethod]
        public void Television_Channel_decrement_success()
        {
            m_Television.Channel = MAX_CHANNELS;
            m_Television.Channel--;
            Assert.IsTrue(m_Television.Channel == MAX_CHANNELS-1);
        }

        [TestMethod]
        public void Television_Channel_increment_rollover_success()
        {
            m_Television.Channel = MAX_CHANNELS;
            m_Television.Channel++;
            Assert.IsTrue(m_Television.Channel == 1);
        }

        [TestMethod]
        public void Television_Channel_decrement_rollover_success()
        {
            m_Television.Channel = 1;
            m_Television.Channel--;
            Assert.IsTrue(m_Television.Channel == MAX_CHANNELS);
        }

        [TestMethod]
        public void Television_Channel_invalid_set_default_success()
        {
            m_Television.Channel = MAX_CHANNELS + 1;            
            Assert.IsTrue(m_Television.Channel == 1);
        }

        [TestMethod]
        public void Television_Channel_invalid_set_MAX_CHANNELS_success()
        {
            m_Television.Channel = -1;
            Assert.IsTrue(m_Television.Channel == MAX_CHANNELS);
        }
    }
}
