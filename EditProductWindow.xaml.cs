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
    /// Interaction logic for EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        private readonly IMongoCollection<BsonDocument> collection;
        private readonly BsonDocument product;
        private readonly Action refreshProductList;
        public EditProductWindow(IMongoCollection<BsonDocument> collection, BsonDocument product, Action refreshProductList)
        {
            InitializeComponent();
            this.collection = collection;
            this.product = product;
            this.refreshProductList = refreshProductList;

            LoadProductData();
        }
        private void LoadProductData()
        {
            string name = product.GetValue("name", "").ToString();
            decimal price = product.GetValue("price", 0.0m).ToDecimal();

            NameTextBox.Text = name;
            PriceTextBox.Text = price.ToString();
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

            var filter = Builders<BsonDocument>.Filter.Eq("_id", product.GetValue("_id"));
            var update = Builders<BsonDocument>.Update.Set("name", name).Set("price", price);

            collection.UpdateOne(filter, update);

            MessageBox.Show("Product updated successfully!");

            refreshProductList.Invoke();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
