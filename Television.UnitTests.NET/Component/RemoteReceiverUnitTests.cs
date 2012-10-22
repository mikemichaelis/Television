using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Television.Interfaces;
using Television.Models;
using Television.Models.Enums;

namespace Television.UnitTests.NET.Component
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class RemoteReceiverUnitTests
    {
        public RemoteReceiverUnitTests()
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

        [TestInitialize()]
        public void Initialize()
        {
            
        }

        [TestCleanup()]
        public void Cleanup()
        {
                        
        }

        //private Television.Models.RemoteReceiver m_Receiver = null;

        private Guid CONTROL_CODE = Guid.Parse("E6D56146-1D81-4B7A-829E-06D230F53CC2");

        #endregion

        [TestMethod]
        public void RemoteReceiver_create_success()
        {
            MockRepository mocks = new MockRepository();
            ITelevision televisionMock = mocks.StrictMock<ITelevision>();

            Television.Models.RemoteReceiver m_Receiver = new Models.RemoteReceiver(televisionMock);

            Assert.IsNotNull(m_Receiver);  //  This would probably never be hit if creation failed, but it is good to be through

            m_Receiver.Dispose();
        }
        
        [TestMethod]
        public void RemoteReceiver_responds_CONTROL_CODE_request_success()
        {
            MockRepository mocks = new MockRepository();
            ITelevision televisionMock = mocks.StrictMock<ITelevision>();

            Television.Models.RemoteReceiver m_Receiver = new Models.RemoteReceiver(televisionMock);

            RemoteControlCommand message = null;
            Messenger.Default.Register<RemoteControlCommand>(this, (m) =>
            {
                if(m.Command == RemoteCommandsEnum.CONTROL_CODE)
                    message = m;
            });

            Messenger.Default.Send<RemoteControlCommand>(               
                   new RemoteControlCommand()
                   {
                       Command = RemoteCommandsEnum.REQUEST_CONTROL_CODE,
                       Value = null
                   });

            //System.Threading.Thread.Yield();    // Let me message be processed async

            // Verify we received a CONTROL_CODE message
            Assert.IsTrue(message.Command == RemoteCommandsEnum.CONTROL_CODE);

            // Verify we received a Guid
            Guid value = (Guid)message.Value;
            Assert.IsNotNull(value);

            // Verify the RemoteReceiver is still using the expected UnitTest control code
            Assert.IsTrue(CONTROL_CODE.Equals(value));

            // Cleanup
            m_Receiver.Dispose();
            Messenger.Default.Unregister<RemoteControlCommand>(this);            
        }

        [TestMethod]
        public void RemoteReceiver_STATE_ON_request_success()
        {
            MockRepository mocks = new MockRepository();
            ITelevision televisionMock = mocks.StrictMock<ITelevision>();

            using (mocks.Record())
            {
                Expect.Call(televisionMock.State).Return(TelevisionStateEnum.OFF);
                Expect.Call(televisionMock.State = TelevisionStateEnum.ON);                
            }

            Television.Models.RemoteReceiver m_Receiver = new Models.RemoteReceiver(televisionMock);

            Messenger.Default.Send<RemoteControlCommand>(               
                   new RemoteControlCommand()
                   {
                       Command = RemoteCommandsEnum.STATE,
                       Value = TelevisionStateEnum.ON
                   }, CONTROL_CODE);
            
            mocks.VerifyAll();

            // Cleanup
            m_Receiver.Dispose();
        }

        [TestMethod]
        public void RemoteReceiver_STATE_ON_OFF_request_success()
        {
            MockRepository mocks = new MockRepository();
            ITelevision televisionMock = mocks.StrictMock<ITelevision>();

            using (mocks.Record())
            {
                Expect.Call(televisionMock.State).Return(TelevisionStateEnum.OFF);
                Expect.Call(televisionMock.State = TelevisionStateEnum.ON);
                Expect.Call(televisionMock.State).Return(TelevisionStateEnum.ON);
                Expect.Call(televisionMock.State = TelevisionStateEnum.OFF);
            }

            Television.Models.RemoteReceiver m_Receiver = new Models.RemoteReceiver(televisionMock);

            Messenger.Default.Send<RemoteControlCommand>(
                   new RemoteControlCommand()
                   {
                       Command = RemoteCommandsEnum.STATE,
                       Value = TelevisionStateEnum.ON
                   }, CONTROL_CODE);

            System.Threading.Thread.Yield();    // Let me message be processed async

            Messenger.Default.Send<RemoteControlCommand>(               
                   new RemoteControlCommand()
                   {
                       Command = RemoteCommandsEnum.STATE,
                       Value = TelevisionStateEnum.OFF
                   }, CONTROL_CODE);
            
            mocks.VerifyAll();

            // Cleanup
            m_Receiver.Dispose();
        }

        [TestMethod]
        public void RemoteReceiver_CHANNEL_request_success()
        {
            MockRepository mocks = new MockRepository();
            ITelevision televisionMock = mocks.StrictMock<ITelevision>();

            using (mocks.Record())
            {
                Expect.Call(televisionMock.State).Return(TelevisionStateEnum.ON);
                Expect.Call(televisionMock.Channel = 50);
            }

            Television.Models.RemoteReceiver m_Receiver = new Models.RemoteReceiver(televisionMock);

            Messenger.Default.Send<RemoteControlCommand>(               
                   new RemoteControlCommand()
                   {
                       Command = RemoteCommandsEnum.CHANNEL,
                       Value = 50
                   }, CONTROL_CODE);
            
            mocks.VerifyAll();

            // Cleanup
            m_Receiver.Dispose();
        }

        [TestMethod]
        public void RemoteReceiver_CHANNEL_UP_request_success()
        {
            MockRepository mocks = new MockRepository();
            ITelevision televisionMock = mocks.StrictMock<ITelevision>();

            using (mocks.Record())
            {
                Expect.Call(televisionMock.State).Return(TelevisionStateEnum.ON);
                Expect.Call(televisionMock.Channel).Return(1);
                Expect.Call(televisionMock.Channel).SetPropertyWithArgument(2);
            }

            Television.Models.RemoteReceiver m_Receiver = new Models.RemoteReceiver(televisionMock);

            Messenger.Default.Send<RemoteControlCommand>(               
                   new RemoteControlCommand()
                   {
                       Command = RemoteCommandsEnum.CHANNEL_UP,
                       Value = null
                   }, CONTROL_CODE);
            
            mocks.VerifyAll();

            // Cleanup
            m_Receiver.Dispose();
        }

        [TestMethod]
        public void RemoteReceiver_CHANNEL_DOWN_request_success()
        {
            MockRepository mocks = new MockRepository();
            ITelevision televisionMock = mocks.StrictMock<ITelevision>();

            using (mocks.Record())
            {
                Expect.Call(televisionMock.State).Return(TelevisionStateEnum.ON);
                Expect.Call(televisionMock.Channel).Return(10);
                Expect.Call(televisionMock.Channel).SetPropertyWithArgument(9);
            }

            Television.Models.RemoteReceiver m_Receiver = new Models.RemoteReceiver(televisionMock);

            Messenger.Default.Send<RemoteControlCommand>(               
                   new RemoteControlCommand()
                   {
                       Command = RemoteCommandsEnum.CHANNEL_DOWN,
                       Value = null
                   }, CONTROL_CODE);
            
            mocks.VerifyAll();

            // Cleanup
            m_Receiver.Dispose();
        }
    }
}
