using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;


namespace Gamma
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Конвертация числа в бинарку
        private string ConvertNumberToBinary(int number)
        {
            string BinaryCode = Convert.ToString(number, 2);
            if (BinaryCode.Length < 8)
            {
                string zeros = string.Concat(Enumerable.Repeat("0", 8 - BinaryCode.Length));
                BinaryCode = zeros + BinaryCode;
            }
            return BinaryCode;
        }
        //конвертация текста в бинарку по индексам
        private string ConvertTextToBinary(string text, int lang)
        {
            string result = "";
            if (lang == 0)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    result += ConvertNumberToBinary(Algorithm.Ru.IndexOf(text[i]));
                }
            }
            else
            {
                for (int i = 0; i < text.Length; i++)
                {
                    result += ConvertNumberToBinary(Algorithm.En.IndexOf(text[i]));
                }
            }
            return result;
        }
        //нормализация текста
        private string Normalize(string text)
        {
            if (Ru.IsChecked == true)
                return Regex.Replace(text.ToLower(), @"[^а-я\s0-9ё]+", "");
            else if (En.IsChecked == true)
                return Regex.Replace(text.ToLower(), @"[^a-z\s0-9]+", "");
            else
                return "";
        }
        //Генерация ПСГ
        private string PSG(int length)
        {
            Random rand = new Random();
            string result = "";
            int[] arr = new int[length];
            for(int i = 0; i < length; i++)
            {
                arr[i]=1;
            }

            
            for(int i = 0; i < length / 2;)
            {
                int RandNum = rand.Next(length - 1);
                if (arr[RandNum] != 0)
                {
                    arr[RandNum] = 0;
                    i++;
                }
            }
            for(int i=0;i<length;i++)
            {
                result += arr[i].ToString();
            }
            
            return result;
        }





        //Зашифровать
        private void Crypt_Click(object sender, RoutedEventArgs e)
        {
            if (KeyBox.Text == "")
            {
                MessageBox.Show("Внимание, поле 'ключ' пустое");
                return;
            }
            int lang;
            if (Ru.IsChecked == true)
            {
                lang = 0;
            }
            else
            {
                lang = 1;
            }
            string key = KeyBox.Text;
            string text = StartText.Text;
            if (KeyType.SelectedIndex == 0)
            {
                if (key.Length < text.Length * 8)
                {
                    MessageBox.Show("Внимание, длина ключа будет автоматически увеличена до длины сообщения");
                    key = (string.Concat(Enumerable.Repeat(key, text.Length*8 / key.Length + 1))).Substring(0, text.Length*8);
                }
                else if (key.Length > text.Length * 8)
                {
                    key = key.Substring(0, text.Length * 8);
                }
            }
            else
            {
                if (key.Length < text.Length)
                {
                    MessageBox.Show("Внимание, длина ключа будет автоматически увеличена до длины сообщения");
                    key = (string.Concat(Enumerable.Repeat(key, text.Length / key.Length + 1))).Substring(0, text.Length);
                }
                else if (key.Length > text.Length)
                {
                    key = key.Substring(0, text.Length);
                }
                key = Normalize(key);
                key = ConvertTextToBinary(key,lang);
            }

            KeyBlock.Text = key;
            BinaryStartText.Text = ConvertTextToBinary(Normalize(text), lang);
            BinaryResultText.Text = (new Algorithm(BinaryStartText.Text, key)).Result;
/*            string binaryresult = BinaryResultText.Text;
            string understring = "";
            string res = "";
            int index;
            for (int i = 0; i < binaryresult.Length / 8; i++)
            {
                understring = binaryresult.Substring(i * 8, 8);
                understring = Regex.Replace(understring, @"^{1,7}", "");

                index = Convert.ToInt32(understring, 2);
                if (Ru.IsChecked == true)
                {
                    while (index < Algorithm.Ru.Length && index >= 0)
                    {
                        if (index >= Algorithm.Ru.Length)
                            index = index - index % Algorithm.Ru.Length;
                        if (index < 0)
                            index = Algorithm.Ru.Length + index;
                        res += Algorithm.Ru[index];
                    }
                }
                if (En.IsChecked == true)
                {
                    if (index >= Algorithm.En.Length)
                        index = index - index % Algorithm.En.Length;
                    if (index < 0)
                        index = Algorithm.En.Length + index;
                    res += Algorithm.En[index];
                }
            }
            Fast.Text = res;*/

        }
        //расшифровать
        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            if (BinaryResultText.Text == "")
            {
                return;
            }
            else
            {
                string binaryresult = (new Algorithm(BinaryResultText.Text, KeyBlock.Text)).Result;
                string understring = "";
                string result = "";
                int index;
                for(int i =0; i < binaryresult.Length / 8; i++)
                {
                    understring = binaryresult.Substring(i * 8, 8);
                    understring = Regex.Replace(understring, @"^{1,7}", "");
                    StartResultText.Text = understring;
                    index = Convert.ToInt32(understring, 2);
                    if (Ru.IsChecked == true) result += Algorithm.Ru[index];
                    if (En.IsChecked == true) result += Algorithm.En[index];
                }
                StartResultText.Text = result;
            }
        }
        //Тестовый клик
        private void Clear_Click(object sender, RoutedEventArgs e)
        {

            StartText.Text = "";
            StartResultText.Text = "";
            BinaryResultText.Text = "";
            BinaryStartText.Text = "";
            KeyBox.Text = "";
            KeyBlock.Text = "";
        }
        //ПСГ
        private void GenerateKey_Click(object sender, RoutedEventArgs e)
        {
            KeyBox.Text = PSG(StartText.Text.Length * 8);
        }





        //проверка ввода в текст
        private void StartText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Ru.IsChecked == true && Regex.IsMatch(e.Text, @"[a-zA-Z]"))
            {
                e.Handled = true;
                MessageBox.Show("Вы вводите в текст символы, не соотвествующие выбранному алфавиту/символы, не являющиеся цифрами");
            }
            if (En.IsChecked == true && Regex.IsMatch(e.Text, @"[а-яА-ЯёЁ]"))
            {
                e.Handled = true;
                MessageBox.Show("Вы вводите в текст символы, не соотвествующие выбранному алфавиту/символы, не являющиеся цифрами");
            }
        }
        //проверка вставки в текст 
        private void StartText_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            string TextFotChecking = (string)e.DataObject.GetData(typeof(String));
            if (Ru.IsChecked == true && Regex.IsMatch(TextFotChecking, @"[a-zA-Z]+"))
            {
                e.CancelCommand();
               

            }

            if (En.IsChecked == true && Regex.IsMatch(TextFotChecking, @"[а-яА-ЯёЁ]+"))
            {
                e.CancelCommand();
               
            }
        }
        //проверка ввода в ключ
        private void KeyBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (KeyType.SelectedIndex == 0)
            {
                if (e.Text != "0" && e.Text != "1")
                {
                    e.Handled = true;
                    MessageBox.Show("Вы вводите в ключ символы, не соотвествующие выбранному алфавиту/символы, не являющиеся 0/1");
                }
            }
            else
            {
                if (Ru.IsChecked == true && Regex.IsMatch(e.Text, @"[^а-яА-я0-9Ёё]"))
                {
                    e.Handled = true;
                    MessageBox.Show("Вы вводите в ключ символы, не соотвествующие выбранному алфавиту/символы, не являющиеся цифрами");
                }
                if (En.IsChecked == true && Regex.IsMatch(e.Text, @"[^a-zA-Z0-9]"))
                {
                    e.Handled = true;
                    MessageBox.Show("Вы вводите в ключ символы, не соотвествующие выбранному алфавиту/символы, не являющиеся цифрами");
                }
            }
        }
        //проверка вставки в ключ
        private void KeyBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            string TextFotChecking = (string)e.DataObject.GetData(typeof(String));
            if (KeyType.SelectedIndex == 0)
            {
                if (Regex.IsMatch(TextFotChecking, @"[^0-1]+"))
                {
                    e.CancelCommand();
                    MessageBox.Show("Вы вводите в ключ символы, не являющиеся 0/1");
                }
            }
            else
            {
                
                if ((Ru.IsChecked == true && Regex.IsMatch(TextFotChecking, @"[a-zA-Z]+")) || (En.IsChecked == true && Regex.IsMatch(TextFotChecking, @"[а-яА-ЯёЁ]+")) || (Regex.IsMatch(TextFotChecking, @"[^\da-zA-ZЁёа-яА-Я0-9]")))
                {
                    e.CancelCommand();
                }
            }
        }
        //пробел в ключе
        private void KeyBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            {
                e.Handled = true;
                MessageBox.Show("Вы вводите в ключ пробел.");
            }
        }

      //чеки
        private void Ru_Checked(object sender, RoutedEventArgs e)
        {
            En.IsChecked = false;
            StartText.Text = ""; 
            StartResultText.Text = "";
            BinaryResultText.Text = "";
            BinaryStartText.Text = "";
            KeyBox.Text = "";
            KeyBlock.Text = "";
        }

        private void En_Checked(object sender, RoutedEventArgs e)
        {
            Ru.IsChecked = false;
            StartText.Text = "";
            StartResultText.Text = "";
            BinaryResultText.Text = "";
            BinaryStartText.Text = "";
            KeyBox.Text = "";
            KeyBlock.Text = "";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KeyType.SelectedIndex == 0)
            {
                GenerateKey.IsEnabled = true;
                KeyBox.Text = "";
                KeyBlock.Text = "";
            }
            else
            {
                GenerateKey.IsEnabled = false;
                KeyBox.Text = "";
                KeyBlock.Text = "";
            }
        }

    }
}
