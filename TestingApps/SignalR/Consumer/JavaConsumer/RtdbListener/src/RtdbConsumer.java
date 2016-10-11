public class RtdbConsumer {
	public static void main(String[] args) {
		System.out.println("testing RtdbConsumer");	 
		SignalRService _SignalRService = new SignalRService();	
		_SignalRService.invokeCallback("KRS-IN-RV1-SETPT");
	}
}
