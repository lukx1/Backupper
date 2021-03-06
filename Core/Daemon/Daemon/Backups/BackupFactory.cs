﻿using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Backups
{
    /// <summary>
    /// Vytvoří vhodné zálohy
    /// </summary>
    public class BackupFactory
    {
        /*public BackupType BackupType;
        public string DestinationPath;
        public string SourcePath;
        public bool IsZip;
        public int ID;*/

            /// <summary>
            /// Vytvoří zálohovací objekt z údajů
            /// </summary>
            /// <param name="taskLocation"></param>
            /// <param name="backupType"></param>
            /// <param name="details"></param>
            /// <param name="id"></param>
            /// <param name="actionBefore"></param>
            /// <param name="actionAfter"></param>
            /// <returns></returns>
        public static IBackup CreateBackup(IEnumerable<DbTaskLocation> taskLocation, DbBackupType backupType, DbTaskDetails details, int id, string actionBefore, string actionAfter)
        {
            return new SmartBackup() {TaskLocations = taskLocation,BackupType=backupType,TaskDetails = details,ID = id,ActionAfter = actionAfter, ActionBefore = actionBefore };
        }

        /*
        public IBackup CreateFromBackupType()
        {
            if (BackupType == BackupType.NORM)
                return CreateFullBackup();
            else
                throw new NotImplementedException();
        }

        public FullBackup CreateFullBackup()
        {
            FullBackup fullBackup = new FullBackup(SourcePath);
            return fullBackup;
        }*/

    }
}
