using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReceiverWinForm
{
    public partial class Form1 : Form
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void CSharpCallback(int value);

        [DllImport(@"C:\willowlynx\scada\arch\T-i386-ntvc\bin\DataPorting.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void DoWork_CSharpCallback([MarshalAs(UnmanagedType.FunctionPtr)] CSharpCallback callbackPointer);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CSharpCallback callback =
                (value) =>
                {
                    MessageBox.Show("rtdb value = " + value);
                };

            DoWork_CSharpCallback(callback);
        }
    }
}
