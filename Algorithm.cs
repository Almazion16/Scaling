using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma
{
    class Algorithm
    {
        public static string Ru = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789 ";
        public static string En = "abcdefghijklmnopqrstuvwxyz0123456789 ";
        public string Result;
        public Algorithm(string text, string key)//lang = 0 - ru, 1 - en; mode = 0 - encrypt, 1 - decrypt
        {
            string result = "";
            
            for(int i = 0; i < text.Length; i++)
            {
                if (text[i] == key[i])
                {
                    result += "0";
                }
                else
                {
                    result += "1";
                }
            }
            Result = result;
           
        }
    }
}
