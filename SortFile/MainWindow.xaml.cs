using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace SortFile
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    class StrList
    {
        public string Str { get; set; }
        public int Number { get; set; }

        public StrList()
        {

        }

        public StrList(int number, string str)
        {
            Str = str;
            Number = number;
        }


    }




    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "All Files (*.*)|*.*";

            if (openFile.ShowDialog() == true)
            {


                using (FileStream fs = File.Open(openFile.FileName, FileMode.Open))
                {
                    textboxOpen.Text = openFile.FileName;

                }
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (saveFile.ShowDialog() == true)
            {

                using (FileStream fs = File.Create(saveFile.FileName))
                {
                    textboxSave.Text = saveFile.FileName;
                }
            }
        }

        private void SortBtn_Click(object sender, RoutedEventArgs e)
        {

            List<StrList> AllList = new List<StrList>();

            StreamReader fs = new StreamReader(textboxOpen.Text);

            string s = (new StreamReader(textboxOpen.Text)).ReadToEnd();
            Regex regex = new Regex(@"(\w*\d\s\w*)");
            MatchCollection matches = regex.Matches(s);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    string str = match.Value;
                    string[] all = str.Split(' ');
                    AllList.Add(new StrList(Convert.ToInt32(all[0]), all[1]));

                }

            }
            else
            {
                MessageBox.Show("Совпадений не найдено");
            }

            List<StrList> resultAllList = new List<StrList>();
            var result = from user in AllList
                         orderby user.Str, user.Number, user.Str.Length
                         select user;
            foreach (StrList u in result)
            {
                resultAllList.Add(new StrList(u.Number, u.Str));
            }


            using (StreamWriter sw = new StreamWriter(textboxSave.Text, false, System.Text.Encoding.Default))
            {

                string data = string.Join(Environment.NewLine, resultAllList.Select((x) => x.Number + " " + x.Str));

                sw.WriteLine(data);
            }



        }
    }
}
