using DaemonShared;
using DaemonShared.Pipes;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaemonSettings
{
    static class Program
    {

        private static Task settingsTask;
        private static List<NamedPipeServerStream> Readers = new List<NamedPipeServerStream>();
        private static string ServiceIdentity;
        private static PipeSecurity pipeSecurity;
        private static List<Home> homes = new List<Home>();

        private static void TrySetIdentity(PipeMessage msg)
        {
            try
            {
                ServiceIdentity = msg.DeserializePayload<PipeServiceIdentity>().Identity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void HandleMessage(PipeMessage msg)
        {
            switch (msg.Code)
            {
                case PipeCode.LOGIN_RESPONSE:
                    homes.ForEach(h => h.MessageReceived(msg.Code, msg.Payload));
                    break;
                case PipeCode.SHOW_SETTINGS:
                    if (settingsTask != null)
                        settingsTask = Task.Run(() => ShowSettings(SendMessage)).ContinueWith((r) => { homes.Clear(); });
                    break;
                case PipeCode.NOTIFY_WAITER:
                    TrySetIdentity(msg);
                    NotifyWaiter(SendMessage);
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
            var pp = PipePopup.FromJson(msg.Payload);
            var buttons = (System.Windows.Forms.MessageBoxButtons)((int)pp.B);
            var icons = (System.Windows.Forms.MessageBoxIcon)((int)pp.I);
            var res = MessageBox.Show(null, pp.T, pp.C, buttons, icons);
            if (pp.R)
                Task.Run(() => SendMessage(PipeCode.DIALOG_RESULT, new PipeDialogResult() { R = (PipeDialogResult.DialogResult)(int)res }));
            
        }

        private static void SendMessage(PipeCode code)
        {
            using (NamedPipeClientStream serverStream = new NamedPipeClientStream(".", PipeMessage.PIPE_NAME, PipeDirection.Out))
            {
                serverStream.Connect();
                serverStream.Write(new PipeMessage() { Code = PipeCode.KILL_SERVICE }.ToSendable(), 0, PipeMessage.MAX_SIZE_IN_BYTES);
                serverStream.Flush();
            }
        }

        private static void SendMessage(PipeCode code, object content)
        {
            using (NamedPipeClientStream serverStream = new NamedPipeClientStream(".", PipeMessage.PIPE_NAME, PipeDirection.Out))
            {
                serverStream.Connect();
                serverStream.Write(new PipeMessage() { Code = code,SerializePayload = content }.ToSendable(), 0, PipeMessage.MAX_SIZE_IN_BYTES);
                serverStream.Flush();
            }
        }

        private static void KillTask()
        {
            SendMessage(PipeCode.KILL_SERVICE);
        }

        private static void NotifyWaiter(Action<PipeCode, object> SendMessage)
        {
            NotifyWaiter n = new DaemonSettings.NotifyWaiter();
            n.Run(() => Task.Run(() => ShowSettings(SendMessage)), () => Task.Run(() => KillTask()));
        }

        [STAThread]
        public static void ShowSettings(Action<PipeCode, object> SendMessage)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
            }
            catch (InvalidOperationException) { }
            Home home = new Home(SendMessage);
            homes.Add(home);
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
                        if (ServiceIdentity != null && pipeSecurity == null)
                        {
                            PipeSecurity ps = new PipeSecurity();
                            ps.AddAccessRule(new PipeAccessRule(ServiceIdentity, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));
                            serverStream.SetAccessControl(ps);
                            pipeSecurity = ps;
                        }
                        else if(pipeSecurity != null)
                            serverStream.SetAccessControl(pipeSecurity);

                        serverStream.SetAccessControl(new PipeSecurity() { });
                        serverStream.WaitForConnection();
                        byte[] b = new byte[PipeMessage.MAX_SIZE_IN_BYTES];
                        using (var reader = new BinaryReader(serverStream))
                        {
                            reader.Read(b, 0, b.Length);
                            Task.Run(() => HandleMessage(PipeMessage.Read(b)));
                        }
                    }
                }
                catch(IdentityNotMappedException e)
                {

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
