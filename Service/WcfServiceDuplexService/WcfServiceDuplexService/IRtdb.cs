using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceDuplexService
{
    [ServiceContract(CallbackContract = typeof(IRtdbCallback), SessionMode = SessionMode.Required)]
    public interface IRtdb
    {
        [OperationContract(IsOneWay = true)]
        void ValueChange(Guid SubscriberID, int Value);

        [OperationContract(IsOneWay = true)]
        void Subscribe(Guid SubscriberID);

        [OperationContract(IsOneWay = true)]
        void UnSubscribe(Guid SubscriberID);
    }

    public interface IRtdbCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnValueChange(int Value);
    }

    public class EachSubscriber
    {
        [DataMember]
        public Guid SubscriberID { get; set; }        
    }
}
