import microsoft.aspnet.signalr.client.SignalRFuture;
import microsoft.aspnet.signalr.client.hubs.HubConnection;
import microsoft.aspnet.signalr.client.hubs.HubProxy;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler1;
import microsoft.aspnet.signalr.client.transport.ClientTransport;
import microsoft.aspnet.signalr.client.transport.ServerSentEventsTransport;

public class SignalRService {
	private HubConnection mHubConnection;
	private HubProxy mHubProxy;	 
	
	public SignalRService() {
		startSignalR();
    }
	
	public void invokeCallback(String TagName){
		String SERVER_METHOD_SEND_TO = "ConnectBroadcastRtdbValue";
        mHubProxy.invoke(SERVER_METHOD_SEND_TO, TagName);
	}
	
	private void startSignalR() {
        //Platform.loadPlatformComponent(new AndroidPlatformComponent());
        String serverUrl = "http://localhost:62680/";
        mHubConnection = new HubConnection(serverUrl);
        String SERVER_HUB_RTDB = "RtdbHub";
        mHubProxy = mHubConnection.createHubProxy(SERVER_HUB_RTDB);
        ClientTransport clientTransport = new ServerSentEventsTransport(mHubConnection.getLogger());
        SignalRFuture<Void> signalRFuture = mHubConnection.start(clientTransport);

        try {
            signalRFuture.get();
        } catch (Exception e) {        	
        	System.err.println(e.toString());            
            return;
        }        

        String CLIENT_METHOD_BROADAST_MESSAGE = "broadcastRtdbValue";
        mHubProxy.on(CLIENT_METHOD_BROADAST_MESSAGE,
                new SubscriptionHandler1<RtdbValueModel>() {
                    //@Override
                    public void run(RtdbValueModel _RtdbValueModel) {
                        System.out.println("Latest Rtdb value is "+ _RtdbValueModel.value +" .");
                    }
                }
                , RtdbValueModel.class);
    }
}