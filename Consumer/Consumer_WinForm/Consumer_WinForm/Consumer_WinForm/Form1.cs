using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Consumer_WinForm
{
    public partial class Form1 : Form
    {
        private RtdbService.RtdbClient _client;
        private Guid _Guid = new Guid("01655F23-C070-4832-9435-381C97119F94");
        
        public Form1()
        {
            InitializeComponent();
            InstanceContext context = new InstanceContext(new RtdbCallback());
            _client = new RtdbService.RtdbClient(context);
        }

        public class RtdbCallback : RtdbService.IRtdbCallback
        {
            public void OnValueChange(int Value)
            {
                MessageBox.Show("Rtdb value is " + Value);                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_client.State == CommunicationState.Opened)
            {
                MessageBox.Show("Please unsubscribe first & then subscribe for a different file type.");
                return;
            }
            try
            {
                if (_client.State == CommunicationState.Created)
                {
                    _client.Subscribe(_Guid);
                }
                else
                {
                    InstanceContext context = new InstanceContext(new RtdbCallback());
                    _client = new RtdbService.RtdbClient(context);
                    _client.Subscribe(_Guid);
                }
            }
            catch (Exception e1)
            {
                throw new Exception("Could not subscribe");
            }            
        }
    }
}
