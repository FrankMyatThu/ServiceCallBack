using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceDuplexService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Rtdb : IRtdb
    {
        private object locker = new object();
        private Dictionary<EachSubscriber, IRtdbCallback> Subscribers = new Dictionary<EachSubscriber, IRtdbCallback>();

        //This will be called by the Event Generator app.
        public void ValueChange(Guid SubscriberID, int Value)
        {
            //get all the subscribers
            var subscriberKeys = (from c in Subscribers
                                  select c.Key).ToList();

            subscriberKeys.ForEach(delegate(EachSubscriber _EachSubscriber)
            {
                IRtdbCallback callback = Subscribers[_EachSubscriber];
                if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                {
                    if (_EachSubscriber.SubscriberID.Equals(SubscriberID)) 
                    {
                        callback.OnValueChange(Value);
                    }
                }
                else
                {
                    //These subscribers are no longer active. Delete them from subscriber list
                    subscriberKeys.Remove(_EachSubscriber);
                    Subscribers.Remove(_EachSubscriber);
                }
            });
        }

        public void Subscribe(Guid SubscriberID)
        {
            try
            {
                IRtdbCallback callback = OperationContext.Current.GetCallbackChannel<IRtdbCallback>();
                lock (locker)
                {
                    EachSubscriber _EachSubscriber = new EachSubscriber();
                    _EachSubscriber.SubscriberID = SubscriberID;                    
                    Subscribers.Add(_EachSubscriber, callback);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void UnSubscribe(Guid SubscriberID)
        {
            try
            {
                lock (locker)
                {
                    var SubToBeDeleted = from c in Subscribers.Keys
                                         where c.SubscriberID == SubscriberID
                                         select c;
                    if (SubToBeDeleted.Count() > 0)
                    {
                        Subscribers.Remove(SubToBeDeleted.First());
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
