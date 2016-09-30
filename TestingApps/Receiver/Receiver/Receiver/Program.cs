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
        delegate void ProgressCallback(int value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate string GetFilePathCallback(string filter);

        [DllImport(@"C:\Frank\SMS Project\Dummy\ServiceCallBack\TestingApps\Engine\Debug\Engine.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void DoWork([MarshalAs(UnmanagedType.FunctionPtr)] ProgressCallback callbackPointer);

        static void Main(string[] args)
        {           
            ProgressCallback callback =
                (value) =>
                {
                    Console.WriteLine("Progress = {0}", value);
                };
 
            Console.WriteLine("Press any key to run DoWork....");
            Console.ReadKey(true);
            
            DoWork(callback);

            Console.ReadKey(true);
        }
    }
}
