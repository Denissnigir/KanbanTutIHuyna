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
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Huy.xaml
    /// </summary>
    public partial class Huy : Window
    {
        HttpClient httpClient;
        List<Status> statuses = new List<Status>();
        User curHUY { get; set; }
        public Huy(User huy)
        {
            InitializeComponent();
            curHUY = huy;
            HuyStack.DataContext = curHUY;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            GetData();
        }



        public List<Roles> roles = new List<Roles>();
        /// <summary>
        /// Kusok govna
        /// </summary>
        private async void GetData()
        {
            var response = await httpClient.GetAsync("http://localhost:63713/api/Status");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                statuses = JsonConvert.DeserializeObject<List<Status>>(content);
            }
            else
            {
                MessageBox.Show("По лбу");
            }

            var response2 = await httpClient.GetAsync("http://localhost:63713/api/Roles");
            if (response2.IsSuccessStatusCode)
            {
                var content = await response2.Content.ReadAsStringAsync();
                roles = JsonConvert.DeserializeObject<List<Roles>>(content);
            }
            else
            {
                MessageBox.Show("По лбу");
            }

            RoleCB.ItemsSource = roles;
            StatusCB.ItemsSource = statuses;
            var role = roles.ToList().FirstOrDefault(p => p.Name == curHUY.NameRole);

            var status = statuses.ToList().FirstOrDefault(p => p.name == curHUY.NameStatus);
            RoleCB.SelectedItem = role;

            StatusCB.SelectedItem = status;
        }

        private async void Change(object sender, RoutedEventArgs e)
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
   
            ResponseUser responseUser = new ResponseUser();
            responseUser.salary = curHUY.salary;
            responseUser.name = curHUY.name;
            responseUser.phoneNumber = curHUY.phoneNumber;
            responseUser.idRole = (RoleCB.SelectedItem as Roles).id;
            responseUser.idStatus = (StatusCB.SelectedItem as Status).id;
            responseUser.id = curHUY.id;
            HttpContent content = new StringContent(JsonConvert.SerializeObject(responseUser), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"http://localhost:63713/api/customers/{curHUY.id}", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("забела");
            }
            else
            {
                MessageBox.Show("Попал в глаз");
            }
        }

        private void StatusCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public partial class ResponseUser
    {
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public int idRole { get; set; }
        public int idStatus { get; set; }
        public decimal salary { get; set; }
        public int id { get; set; }
    }

    public partial class Roles
    {
        public int id { get; set; }
        public string Name { get; set; }
    }

    public partial class Status
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
