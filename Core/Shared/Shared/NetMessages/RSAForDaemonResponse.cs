using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class RSAForDaemonResponse : INetMessage, INetError
    {
        public string EncryptedPrivateKey { get; set; }
        public string DecryptKey(string password) => PasswordFactory.DecryptAES(EncryptedPrivateKey, password);
        public List<ErrorMessage> ErrorMessages { get; set; }
    }
}
