using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    /// <summary>
    /// Přihlásí nezaregistrovaný daemon pomocí předzdíleného klíče.
    /// Platná pouze jednou
    /// </summary>
    public class IntroductionMessage : INetMessage
    {
        /// <summary>
        /// Předzdílený identifikátor
        /// </summary>
        public int id;
        /// <summary>
        /// Operační systém daemona
        /// </summary>
        public string os;
        /// <summary>
        /// Mac adresa daemona 12 dlouhá
        /// </summary>
        public char[] macAdress;
        /// <summary>
        /// Předzdílený klíč, nešifrovaný
        /// </summary>
        public string preSharedKey;

        public string PCUuid;

        /// <summary>
        /// Nutno pouze pro 1CI
        /// </summary>
        public string User;

        /// <summary>
        /// Unikátní public key vytvořený daemonem. Server nidky neposílá - UUID
        /// </summary>
        public char[] publicKey;

        /// <summary>
        /// Název počítače. Například Pepa-PC apod.
        /// </summary>
        public string pcName;

        /// <summary>
        /// Verze komunikačního protokolu
        /// </summary>
        public Version version;
    }
}


