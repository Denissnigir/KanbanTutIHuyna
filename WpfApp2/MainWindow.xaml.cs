using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient httpClient;
        List<User> users = new List<User>();
        public MainWindow()
        {
            InitializeComponent();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            GetData();
        }

        public async void GetData()
        {
            var response = await httpClient.GetAsync("http://localhost:63713/api/getusers");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<User>>(content);
                //Test.ItemsSource = users;
            }
            else
            {
                MessageBox.Show("По лбу");
            }
        }

        private void Test_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Huy huy = new Huy(Test.SelectedItem as User);
            //huy.Show();
            //this.Close();
        }
    }


    public partial class User
    {
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public string NameRole { get; set; }
        public string NameStatus { get; set; }
        public decimal salary { get; set; }
        public int id { get; set; }
    }
}

