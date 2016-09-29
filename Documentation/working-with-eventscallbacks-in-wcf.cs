using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFNotificationService
{
    [ServiceContract(CallbackContract = typeof(IEventNotificationCallback), SessionMode = SessionMode.Required)]
    public interface IEventNotification
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveFile(string strFileType, string strFilename);

        [OperationContract(IsOneWay = true)]
        void Subscribe(Guid SubID, string FileTypeInterested);

        [OperationContract(IsOneWay = true)]
        void UnSubscribe(Guid SubID);
    }

    public interface IEventNotificationCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnReceiveFile(string strFilename);
    }

    public class EachSubscriber
    {
        [DataMember]
        public Guid SubID { get; set; }
        public string FileTypeInterested { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFNotificationService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class EventNotification : IEventNotification
    {
        private object locker = new object();
        private Dictionary<EachSubscriber, IEventNotificationCallback> Subscribers = new Dictionary<EachSubscriber, IEventNotificationCallback>();

        //This will be called by the Event Generator app.
        public void ReceiveFile(string strFileType, string strFilename)
        {
            //get all the subscribers
            var subscriberKeys = (from c in Subscribers
                         select c.Key).ToList();
            
            subscriberKeys.ForEach(delegate(EachSubscriber c1)
            {
                IEventNotificationCallback callback = Subscribers[c1];
                if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                {
                    //call back only those subscribers who are interested in this fileType
                    if (c1.FileTypeInterested.ToLower() == strFileType.ToLower())
                    {
                        callback.OnReceiveFile(strFilename);
                    }
                }
                else
                {
                    //These subscribers are no longer active. Delete them from subscriber list
                    subscriberKeys.Remove(c1);
                    Subscribers.Remove(c1);
                }  
            });
        }

        public void Subscribe(Guid SubID, string FileTypeInterested)
        {
            try
            {
                IEventNotificationCallback callback = OperationContext.Current.GetCallbackChannel<IEventNotificationCallback>();
                lock (locker)
                {
                    EachSubscriber _EachSubscriber = new EachSubscriber();
                    _EachSubscriber.SubID = SubID;
                    _EachSubscriber.FileTypeInterested = FileTypeInterested;
                    Subscribers.Add(_EachSubscriber, callback);
                }
            }
            catch
            {
            }
        }

        public void UnSubscribe(Guid SubID)
        {
            try
            {
                lock (locker)
                {
                    var SubToBeDeleted = from c in Subscribers.Keys
                                where c.SubID == SubID
                                select c;
                    if (SubToBeDeleted.Count() > 0)
                    {
                        Subscribers.Remove(SubToBeDeleted.First());
                    }
                }
            }
            catch
            {
            }
        }


    }
}

<?xml version="1.0" encoding="utf-8" ?>
<configuration>
<system.serviceModel>
<behaviors>
  <serviceBehaviors>
	<behavior name="">
	  <serviceMetadata httpGetEnabled="true" />
	  <serviceDebug includeExceptionDetailInFaults="false" />
	</behavior>
  </serviceBehaviors>
</behaviors>
<services>
  <service name="WCFNotificationService.EventNotification">
	<endpoint address="net.tcp://localhost:8733/Design_Time_Addresses/WCFNotificationService/EventNotification/" binding="netTcpBinding" contract="WCFNotificationService.IEventNotification">
	  <identity>
		<dns value="localhost" />
	  </identity>
	</endpoint>
	<endpoint address="mex" binding="mexHttpBinding" contract="IMetadataExchange" />
	<host>
	  <baseAddresses>
		<add baseAddress="http://localhost:8732/Design_Time_Addresses/WCFNotificationService/EventNotification/" />
	  </baseAddresses>
	</host>
  </service>
</services>
</system.serviceModel>
</configuration>

//The Client

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private ServiceReference1.EventNotificationClient _client;
        private Guid _Guid = Guid.NewGuid();
        static Label l2 = new Label();

        public Form1()
        {
            InitializeComponent();
            InstanceContext context = new InstanceContext(new FileTypeCallback());
            _client = new ServiceReference1.EventNotificationClient(context);
        }

        public class FileTypeCallback : ServiceReference1.IEventNotificationCallback
        {
            public void OnReceiveFile(string strFilename)
            {
                l2.Text = "File received: " + strFilename;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            l2.Text = "";
            if (_client.State == CommunicationState.Opened)
            {
                label2.Text = "Please unsubscribe first & then subscribe for a different file type. ";
                return;
            }
            try
            {
                if (_client.State == CommunicationState.Created)
                {
                    _client.Subscribe(_Guid, comboBox1.SelectedItem.ToString());
                }
                else
                {
                    InstanceContext context = new InstanceContext(new FileTypeCallback());
                    _client = new ServiceReference1.EventNotificationClient(context);
                    _client.Subscribe(_Guid, comboBox1.SelectedItem.ToString());
                }
            }
            catch (Exception e1)
            {
                throw new Exception("Could not subscribe");
            }
            label2.Text = "Subscribed for FileType:-  " + comboBox1.SelectedItem;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            l2.Text = "";
            if (_client.State == CommunicationState.Opened)
            {
                _client.UnSubscribe(_Guid);
                _client.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_client.State == CommunicationState.Opened)
            {
                _client.UnSubscribe(_Guid);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label2.Text = "FileType Selected:-  " + comboBox1.SelectedItem;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            l2.Width = 800;
            l2.ForeColor = Color.Red;
            panel1.Controls.Add(l2);
        }
    }
}


//The Event Generator

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

namespace WCFEventGenerator
{
    public partial class Form1 : Form
    {
        private ServiceReference1.EventNotificationClient _client;
        public Form1()
        {
            InitializeComponent();
        }

        public class FileTypeCallback : ServiceReference1.IEventNotificationCallback
        {
            public void OnReceiveFile(string strFilename)
            {
                //Won't implement anything here
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InstanceContext context = new InstanceContext(new FileTypeCallback());
            _client = new ServiceReference1.EventNotificationClient(context);

            if (_client.State != CommunicationState.Opened)
            {
                _client.ReceiveFile(textBox1.Text.Split('.')[1], textBox1.Text);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label2.Text = "Event generator file name:-  " + textBox1.Text;
        }
    }
}
