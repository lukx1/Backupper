using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonShared.Pipes
{
    public enum PipeCode : int
    {
        SHOW_SETTINGS,NOTIFY_WAITER,KILL_SERVICE,NOTIFY_ALREADY_RUNNING,POPUP_ERR,
        INTEND_READ,YES,NO,DIALOG_RESULT,USER_LOGIN,LOGIN_RESPONSE
    }
}
