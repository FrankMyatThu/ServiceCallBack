using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ConsumerWebForm
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private RtdbService.RtdbClient _client;
        private Guid _Guid = new Guid("01655F23-C070-4832-9435-381C97119F94");
        public class RtdbCallback : RtdbService.IRtdbCallback
        {
            public void OnValueChange(int Value)
            {
                //MessageBox.Show("Rtdb value is " + Value);
                WebForm1 _obj = new WebForm1();
                _obj.SetValue(Value);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            InstanceContext context = new InstanceContext(new RtdbCallback());
            _client = new RtdbService.RtdbClient(context);
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (_client.State == CommunicationState.Opened)
            {
                //MessageBox.Show("Please unsubscribe first & then subscribe for a different file type.");
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

        public void SetValue(int Value) 
        {
            this.Response.Redirect(this.Request.Url.ToString());
            //Label1.Text = Value.ToString();
        }
    }
}