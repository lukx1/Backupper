using DaemonShared;
using DaemonShared.Pipes;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaemonSettings
{
    static class Program
    {

        private static Task settingsTask;

        private static void HandleMessage(PipeMessage msg)
        {
            switch (msg.Code)
            {
                case PipeCode.SHOW_SETTINGS:
                    if (settingsTask != null)
                        settingsTask = Task.Run(() => ShowSettings());
                    break;
                case PipeCode.NOTIFY_WAITER:
                    NotifyWaiter();
                    break;
                case PipeCode.KILL_SERVICE:
                    KillTask();
                    break;
                case PipeCode.NOTIFY_ALREADY_RUNNING:
                    break;
                case PipeCode.POPUP_ERR:
                    PopupErr(msg);
                    break;
            }
        }



        private static void PopupErr(PipeMessage msg)
        {
            var pp = PipePopup.FromJsonZip(msg.Payload);
            var buttons = (System.Windows.Forms.MessageBoxButtons)((int)pp.T);
            var icons = (System.Windows.Forms.MessageBoxIcon)((int)pp.I);
            MessageBox.Show(null,pp.B,pp.C, buttons, icons);
        }
        private static void KillTask()
        {
            using (NamedPipeServerStream serverStream = new NamedPipeServerStream(PipeMessage.PIPE_NAME, PipeDirection.InOut, 16, PipeTransmissionMode.Message))
            {
                serverStream.WaitForConnection();
                serverStream.Write(new PipeMessage() { Code = PipeCode.KILL_SERVICE }.ToSendable(), 0, PipeMessage.MAX_SIZE_IN_BYTES);
                serverStream.Flush();
                serverStream.WaitForPipeDrain();
            }
        }

        private static void NotifyWaiter()
        {
            NotifyWaiter n = new DaemonSettings.NotifyWaiter();
            n.Run(() => Task.Run(() => ShowSettings()), () => Task.Run(() => KillTask()));
        }

        [STAThread]
        public static void ShowSettings()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Home home = new Home();
            home.ShowDialog();
        }

        private static Task PipeThread()
        {
            return Task.Run(() =>
            {

                    try
                    {
                        using (NamedPipeServerStream serverStream = new NamedPipeServerStream(PipeMessage.PIPE_NAME, PipeDirection.InOut, 2, PipeTransmissionMode.Message))
                        {
                            serverStream.WaitForConnection();
                            byte[] b = new byte[PipeMessage.MAX_SIZE_IN_BYTES];
                            using (var reader = new BinaryReader(serverStream))
                            {
                                reader.Read(b, 0, b.Length);
                                Task.Run(()=>HandleMessage(PipeMessage.Read(b)));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Application.Exit();
                        return;
                    }

                
            }
            );
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        static void Main()
        {
            while (true)
            {
                var thread = PipeThread();
                thread.Wait();
            }

        }
    }
}
