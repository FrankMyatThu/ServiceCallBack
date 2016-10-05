using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Runtime.InteropServices;
using RtdbService.Models;

namespace RtdbService.Hubs
{
    public class RtdbHub : Hub
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void CSharpCallback(int value);

        [DllImport(@"C:\willowlynx\scada\arch\T-i386-ntvc\bin\DataPorting.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void DoWork_CSharpCallback([MarshalAs(UnmanagedType.FunctionPtr)] CSharpCallback callbackPointer, string _TagName);

        public void ConnectBroadcastRtdbValue(string TagName)
        {
            CSharpCallback callback =
            (value) =>
            {
                RtdbValueModel _RtdbValueModel = new RtdbValueModel();
                _RtdbValueModel.Value = value;
                Clients.All.broadcastRtdbValue(_RtdbValueModel);
            };
            DoWork_CSharpCallback(callback, TagName);
        }
    }
}