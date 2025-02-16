using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace comradewolfxl
{
    internal class ComradeWolfUtils
    {
        private const string REGISTRY_PATH = @"Comradewolf\Comradewolf\XLaddin";

        // В файле хранится список хостов бэкенда
        private string pathHost = "comradewolf\\hosts.xml";

        public void saveHostInfo(Dictionary<int, string> hostList)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this.pathHost);
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            MessageBox.Show(path);


            List<Entry> entries = new List<Entry>();

            foreach (var host in hostList)
            {
                Entry entry = new Entry();
                entry.Key = host.Key;
                entry.Value = host.Value;
                entries.Add(entry);
            }


            XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fileStream, entries);
            }
        }

        public void StoreInRegistry(string keyName, string value)
        {
            RegistryKey rootKey = Registry.CurrentUser;
            string registryPath = REGISTRY_PATH;
            using (RegistryKey rk = rootKey.CreateSubKey(registryPath))
            {
                rk.SetValue(keyName, value, RegistryValueKind.String);
            }
        }

        public void DeleteFromRegistry(string keyName)
        {
            RegistryKey rootKey = Registry.CurrentUser;
            string registryPath = REGISTRY_PATH;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath, true))
            {
                if (key.GetValue(keyName) != null)
                {
                    key.DeleteValue(keyName);
                }

            }

        }

        public bool fileExistis(string path)
        {
            return File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path));
        }


        public Dictionary<int, string> getHostInfo()
        {
            // Deserilizes object with host name

            // 0 — selected host
            // 1-... other available options
            Dictionary<int, string> hosts = new Dictionary<int, string>();

            if (this.fileExistis(this.pathHost))
            {
                var serializer = new XmlSerializer(typeof(List<Entry>));

                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this.pathHost);

                List<Entry> list;

                using (var fileStream = new FileStream(path, FileMode.Open))
                {
                    if (fileStream.Length == 0)
                    {
                        return hosts;
                    }
                    list = (List<Entry>)serializer.Deserialize(fileStream);
                }

                foreach (Entry entry in list)
                {
                    hosts[(int)entry.Key] = (string)entry.Value;
                }

            }

            return hosts;
        }

        public Dictionary<int, string> checkHost(string currentHost)
        {
            // currentHost - host that will have 0 int in dictionary
            /**
             * Creates Dictionary of hosts
             * [0] is oldest link
             */
            Dictionary<int, string> oldHosts = getHostInfo();

            if (oldHosts.ContainsKey(0) && oldHosts[0] == currentHost)
            {
                return oldHosts;
            }

            Dictionary<int, string> newHosts = new Dictionary<int, string>();

            newHosts[0] = currentHost;

            int i = 1;

            foreach (var host in oldHosts)
            {
                if (host.Value != currentHost)
                {
                    newHosts[i] = host.Value;
                    i++;
                }
            }

            return newHosts;
        }

        public string ReadFromRegistry(string keyName, string defaultValue)
        {
            RegistryKey rootKey = Registry.CurrentUser;
            string registryPath = REGISTRY_PATH;
            using (RegistryKey rk = rootKey.OpenSubKey(registryPath, false))
            {
                if (rk == null)
                {
                    return defaultValue;
                }

                var res = rk.GetValue(keyName, defaultValue);
                if (res == null)
                {
                    return defaultValue;
                }

                return res.ToString();
            }
        }

        

    }

    public class Entry
    {
        public object Key;
        public object Value;
        public Entry()
        {
        }

        public Entry(object key, object value)
        {
            Key = key;
            Value = value;
        }
    }


}
