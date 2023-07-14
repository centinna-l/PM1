using MongoDB.Bson;
using MongoDB.Driver;
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
using System.Windows.Shapes;

namespace PM1
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        private readonly IMongoCollection<BsonDocument> collection;
        public SignUp()
        {
            InitializeComponent();
            var connectionString = "mongodb+srv://dbUser:jerry@cluster0.lwj0c.gcp.mongodb.net/?retryWrites=true&w=majority";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("test2");
            collection = database.GetCollection<BsonDocument>("User");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;
            string email = EmailTextBox.Text;

           

            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var existingUser = collection.Find(filter).FirstOrDefault();

            if (existingUser != null)
            {
                MessageBox.Show("Username already exists. Please choose a different username.");
                return;
            }

            var document = new BsonDocument
            {
                { "username", username },
                { "password", password },
                { "email", email }
            };

            collection.InsertOne(document);

            MessageBox.Show("Sign up successful!");

            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
