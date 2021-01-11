using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Globalization;

namespace CEP_DLL
{
    public class GetIni
    {
        // Declaração das funções não gerenciadas: GetPrivateProfileString e 
        // WritePrivateProfileString
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        public static string PathIni(string nomeArquivo)
        {
            string SystemRoot, UserProfile;
            bool WinIni, UserIni;
            //seta variáveis da function com as variáveis do ssitema operacional
            SystemRoot = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            UserProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            //verifica se o ini está no c:\windows
            WinIni = File.Exists(SystemRoot + "\\" + nomeArquivo);
            if (WinIni == true)
            {
                nomeArquivo = SystemRoot + "\\";
            }
            //verifica se o ini está no diretório de perfil do usuário
            else
            {
                UserIni = File.Exists(UserProfile + "\\" + nomeArquivo);
                if (UserIni == true)
                {
                    nomeArquivo = UserProfile + "\\";
                }
                else
                {

                }
            }
            return nomeArquivo;
        }

        public static string GetIniValue(string section, string key, string nomeArquivo)
        {
            int chars = 256;
            StringBuilder buffer = new StringBuilder(chars);
            string sDefault = "";
            if (nomeArquivo == "QIClass.ini")
            {
                nomeArquivo = PathIni(nomeArquivo) + nomeArquivo;
            }

            if (GetPrivateProfileString(section, key, sDefault, buffer, chars, nomeArquivo) != 0)
            {
                return buffer.ToString();
            }
            else
            {
                // Verifica o último erro Win32.
                int err = Marshal.GetLastWin32Error();
                return null;
            }
        }

        public static bool WriteIniValue(string section, string key, string value, string nomeArquivo)
        {
            return WritePrivateProfileString(section, key, value, nomeArquivo);
        }

        public static string getCaminhoArquivoINI(string caminhoArquivo)
        {
            if (caminhoArquivo.IndexOf("\\bin\\Debug") != -1)
            {
                caminhoArquivo = caminhoArquivo.Replace("\\bin\\Debug", "");
            }
            else if (caminhoArquivo.IndexOf("\\bin\\Release") != -1)
            {
                caminhoArquivo = caminhoArquivo.Replace("\\bin\\Release", "");
            }
            return caminhoArquivo;
        }
    }
}
