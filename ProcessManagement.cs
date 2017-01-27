using System;
using System.Collections;
using System.Diagnostics;
using System.Management;

namespace JackConsole
{
    class ProcessManagement
    {
        bool killemall = false;
        int killedCount = 0;

        public ProcessManagement(bool b)
        {
            killemall = b;
            ArrayList whiteList = XML.ReadXML();
            Process[] localProcesses = GetProcesses();
            KillProcesses(localProcesses, whiteList);

            Console.WriteLine("All Done - Total Processes Killed: " + killedCount);
        }

        Process[] GetProcesses()
        {
            Process[] plist = Process.GetProcesses();
            return plist;
        }

        void KillProcesses(Process[] plist, ArrayList slist)
        {
            bool kill = true;
            foreach (Process p in plist)
            {
                foreach (string s in slist)
                {
                    if (p.ProcessName == s)
                    {
                        kill = false;
                    }

                }
                if (kill) KillProcessAndChildren(p.Id);
            }
        }

        void KillProcessAndChildrenFake(int pid)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildrenFake(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process p = Process.GetProcessById(pid);
                //p.Kill();
                Console.WriteLine("Pew - " + p.ProcessName);
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }

        void KillProcessAndChildren(int pid)
        {
            int killedCount = 0;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process p = Process.GetProcessById(pid);

                killedCount++;

                if(killemall) p.Kill();
                if (!killemall) Console.WriteLine("Pew - " + p.ProcessName);
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }
    }
}
