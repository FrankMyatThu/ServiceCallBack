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

namespace EventGeneratorWinForm
{
    public partial class Form1 : Form
    {
        private RtdbService.RtdbClient _client;
        public Form1()
        {
            InitializeComponent();
        }

        public class RtdbCallback : RtdbService.IRtdbCallback
        {
            public void OnValueChange(int Value)
            {
                //Won't implement anything here
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InstanceContext context = new InstanceContext(new RtdbCallback());
            _client = new RtdbService.RtdbClient(context);

            if (_client.State != CommunicationState.Opened)
            {
                // 01655F23-C070-4832-9435-381C97119F94
                _client.ValueChange(new Guid("01655F23-C070-4832-9435-381C97119F94"), 204);
            }
        }
    }
}
