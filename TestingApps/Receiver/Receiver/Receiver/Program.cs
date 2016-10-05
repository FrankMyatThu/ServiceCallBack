using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
{
    public class Program
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void CSharpCallback(int value);

        //[DllImport(@"C:\Frank\SMS Project\Dummy\ServiceCallBack\TestingApps\Engine\Debug\Engine.dll", CallingConvention = CallingConvention.Cdecl)]
        [DllImport(@"C:\willowlynx\scada\arch\T-i386-ntvc\bin\DataPorting.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void DoWork_CSharpCallback([MarshalAs(UnmanagedType.FunctionPtr)] CSharpCallback callbackPointer, string _TagName);

        static void Main(string[] args)
        {
            CSharpCallback callback =
                (value) =>
                {
                    Console.WriteLine("rtdb value = {0}", value);
                };

            DoWork_CSharpCallback(callback, "KRS-LT-VOLT-1");
            Console.ReadKey(true);
        }
    }
}
