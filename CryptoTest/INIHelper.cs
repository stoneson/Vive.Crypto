using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using HCenter;
using System.Configuration;
using System.IO;

namespace CryptoTest
{
    /// <summary>
    /// 读写INI文件的类。
    /// </summary>
    public class INIHelper
    {
        #region static Funs
        public static string BaseDirectory
        {
            get
            {
                string location = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.LastIndexOf('\\'));
                return location;
            }
        }
        static INIHelper()
        {
            try
            {
                string AppAddr = System.Windows.Forms.Application.StartupPath.Trim(); //当前路径
                if (AppAddr.Substring(AppAddr.Length - 1, 1) != @"\")
                    AppAddr += @"\";
                AppAddr += System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;

                Inipath = AppAddr + ".ini";
            }
            catch { }
        }
        #endregion

        #region 读写INI文件相关
        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString", CharSet = CharSet.Ansi)]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        /// <summary>
        /// /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNames", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, int nSize, string filePath);

        [DllImport("KERNEL32.DLL ", EntryPoint = "GetPrivateProfileSection", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string filePath);

        //public INIHelper()
        //{
        //    try
        //    {
        //        string AppAddr = System.Windows.Forms.Application.StartupPath.Trim(); //当前路径
        //        if(AppAddr.Substring(AppAddr.Length -1,1) != @"\")
        //            AppAddr += @"\";
        //        AppAddr += System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName+".ini";

        //        path = AppAddr;
        //    }
        //    catch{}
        //}
        #endregion

        /// <summary>
        /// 向INI写入数据。
        /// </summary>
        /// <PARAM name="Section">节点名。</PARAM>
        /// <PARAM name="Key">键名。</PARAM>
        /// <PARAM name="Value">值名。</PARAM>
        public static void Write(string Section, string Key, string Value, string path)
        {
            WritePrivateProfileString(Section, Key, Value, path);
        }


        /// <summary>
        /// 读取INI数据。
        /// </summary>
        /// <PARAM name="Section">节点名。</PARAM>
        /// <PARAM name="Key">键名。</PARAM>
        /// <PARAM name="Path">值名。</PARAM>
        /// <returns>相应的值。</returns>
        public static string Read(string Section, string Key, string def, string path)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, def, temp, 255, path);
            return temp.ToString();
        }

        /// <summary>
        /// 读取一个ini里面所有的节
        /// </summary>
        /// <param name="sections"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetAllSectionNames(string path)
        {
            string[] sections = null;
            int MAX_BUFFER = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem(MAX_BUFFER);
            int bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, path);
            if (bytesReturned == 0)
            {
                return sections;
            }
            string local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            //use of Substring below removes terminating null for split
            sections = local.Substring(0, local.Length - 1).Split('\0');
            return sections;
        }

        /// <summary>
        /// 得到某个节点下面所有的key和value组合
        /// </summary>
        /// <param name="section"></param>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllKeyValues(string section, string path)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            byte[] b = new byte[65535];

            GetPrivateProfileSection(section, b, b.Length, path);
            string s = System.Text.Encoding.Default.GetString(b);
            string[] tmp = s.Split((char)0);
            List<string> result = new List<string>();
            foreach (string r in tmp)
            {
                if (r.Trim() != string.Empty)
                    result.Add(r);
            }
            for (int i = 0; i < result.Count; i++)
            {
                string[] item = result[i].ToString().Split(new char[] { '=' });
                if (item.Length == 2)
                {
                    ret.Add(item[0].Trim(), item[1].Trim());
                }
                else if (item.Length == 1)
                {
                    ret.Add(item[0].Trim(), "");
                }
            }
            return ret;
        }


        #region INI文件
        static string Inipath = "";    //INI文件名

        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public static void IniWriteValue(string Key, string Value)
        {
            IniWriteValue("系统参数", Key, Value);
        }
        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public static void IniWriteValue(string Section, string Key, string Value)
        {
            Write(Section, Key, Value, Inipath);
        }

        /// <summary>
        /// 读INI文件
        /// </summary>
        /// <returns></returns>
        public static string IniReadValue(string Key, string def = "")
        {
            return IniReadValue("系统参数", Key, def);
        }
        /// <summary>
        /// 读INI文件
        /// </summary>
        /// <returns></returns>
        public static string IniReadValue(string Section, string Key, string def = "")
        {
            return Read(Section, Key, def, Inipath);
        }

        ///// <summary>
        ///// 读取一个ini里面所有的节
        ///// </summary>
        ///// <returns></returns>
        //public static string[] IniGetAllSectionNames()
        //{
        //    return INIHelper.GetAllSectionNames(Inipath);
        //}
        ///// <summary>
        ///// 得到某个节点下面所有的key和value组合
        ///// </summary>
        ///// <param name="section"></param>
        ///// <returns></returns>
        //public static Dictionary<string, string> IniGetAllKeyValues(string section)
        //{
        //    return INIHelper.GetAllKeyValues(section, Inipath);
        //}
        #endregion

        #region 读/写文件
        #region GetAppSettingsVal
        public static string GetAppSettings(string strKey, string def = "", string configPath = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(configPath))
                {
                    var config = ConfigurationManager.OpenExeConfiguration(string.IsNullOrEmpty(configPath) ? BaseDirectory : configPath);
                    foreach (string key in config.AppSettings.Settings.AllKeys)
                    {
                        if (key == strKey)
                        {
                            return config.AppSettings.Settings[strKey].Value.ToString();
                        }
                    }
                }
                var tem = System.Configuration.ConfigurationManager.AppSettings[strKey];
                if (string.IsNullOrEmpty(tem))
                    return def;
                return tem.Trim();
            }
            catch { return def; }
        }

        #endregion
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path">E:\\test.txt</param>
        /// <returns></returns>
        public static string Read(string path)
        {
            var rtStr = "";
            try
            {
                var pat = System.IO.Path.GetDirectoryName(path);
                if (pat.IsNullOrEmpty())
                {
                    pat = BaseDirectory;
                    path = System.IO.Path.Combine(pat, path);
                }
                if (!System.IO.Directory.Exists(pat)) System.IO.Directory.CreateDirectory(pat);
                using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                {
                    System.String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        rtStr += line.ToString();
                    }
                }
            }
            catch (IOException e)
            {
                rtStr = e.ToString();
                rtStr = "";
            }
            return rtStr;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void Write(string path, string content)
        {
            var pat = System.IO.Path.GetDirectoryName(path);
            if (pat.IsNullOrEmpty())
            {
                pat = BaseDirectory;
                path = System.IO.Path.Combine(pat, path);
            }
            if (!System.IO.Directory.Exists(pat)) System.IO.Directory.CreateDirectory(pat);

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    //开始写入
                    sw.Write(content);
                    //清空缓冲区
                    sw.Flush();
                }
            }
            //关闭流
            //sw.Close();
            //fs.Close();
        }
        #endregion
    }
}
