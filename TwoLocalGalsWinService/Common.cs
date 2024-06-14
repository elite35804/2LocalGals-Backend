﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace TwoLocalGalsWinService
{
    class Common
    {
        #region LogThis
        public static void LogThis(string message, Exception ex)
        {
            try
            {
                string path = @"C:\2LGWinService\TwoLocalGalsWinService.txt";
                string oldPath = @"C:\2LGWinService\TwoLocalGalsWinService_OLD.txt";

                try
                {
                    if (File.Exists(path))
                    {
                        if (new FileInfo(path).Length > 1048576)
                        {
                            if (File.Exists(oldPath)) File.Delete(oldPath);
                            File.Move(path, oldPath);
                        }
                    }
                }
                catch { }

                string line = DateTime.Now.ToString("MM/dd/yy HH:mm:ss ") + message;
                if (ex != null) line += " EX: " + ex.Message;

                using (StreamWriter streamWriter = new StreamWriter(path, true))
                    streamWriter.WriteLine(line);

               // Console.WriteLine(line);

#if DEBUG
                Debug.WriteLine(line);
#endif
            }
            catch { }
        }
        #endregion
    }
}
