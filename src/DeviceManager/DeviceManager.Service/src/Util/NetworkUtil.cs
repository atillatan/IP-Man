/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Xml;

namespace DeviceManager.Service.Util
{
    public static class NetworkUtil
    {
        public static List<string> GetInterfaceIps()
        {
            List<string> ip = new List<string>();

            foreach (System.Net.NetworkInformation.NetworkInterface f in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                if (f.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                {
                    foreach (System.Net.NetworkInformation.GatewayIPAddressInformation d in f.GetIPProperties().GatewayAddresses)
                    {
                        ip.Add(d.Address.ToString());
                    }
                }
            }

            return ip;
        }

        public static string GetHostName(string ip_address)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(ip_address);
                if (entry != null)
                {
                    return entry.HostName;
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public static string GetMacAddress(string ipAdrs)
        {
            string macAddress = string.Empty;
            System.Diagnostics.Process Process = new System.Diagnostics.Process();
            Process.StartInfo.FileName = "arp";
            Process.StartInfo.Arguments = "-a " + ipAdrs;
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.CreateNoWindow = true;
            Process.Start();
            string strOutput = Process.StandardOutput.ReadToEnd();
            string[] substrings = strOutput.Split('-');
            if (substrings.Length >= 8)
            {
                macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                         + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                         + "-" + substrings[7] + "-"
                         + substrings[8].Substring(0, 2);
                return macAddress;
            }

            else
            {
                return "OWN Machine";
            }
        }
    }
}