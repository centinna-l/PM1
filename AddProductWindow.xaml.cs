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
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        private readonly IMongoCollection<BsonDocument> collection;
        private readonly Action refreshProductList;
        public AddProductWindow(IMongoCollection<BsonDocument> collection, Action refreshProductList)
        {
            InitializeComponent();
            this.collection = collection;
            this.refreshProductList = refreshProductList;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string priceText = PriceTextBox.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(priceText))
            {
                MessageBox.Show("Please enter product name and price.");
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price))
            {
                MessageBox.Show("Invalid price format.");
                return;
            }

            var document = new BsonDocument
            {
                { "name", name },
                { "price", price }
            };

            collection.InsertOne(document);

            MessageBox.Show("Product added successfully!");

            refreshProductList.Invoke();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
