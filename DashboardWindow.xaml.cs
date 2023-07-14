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
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        private readonly IMongoCollection<BsonDocument> collection;
        private List<BsonDocument> products;
        public DashboardWindow()
        {
            InitializeComponent();
            var connectionString = "mongodb+srv://dbUser:jerry@cluster0.lwj0c.gcp.mongodb.net/?retryWrites=true&w=majority";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("test2");
            collection = database.GetCollection<BsonDocument>("Product");
            LoadProductList();
        }

        private void LoadProductList()
        {

            var products = collection.Find(new BsonDocument()).ToList();
            List<string> productNames = new List<string>();
            // products = collection.Find(new BsonDocument()).ToList();
            // ProductListBox.ItemsSource = products;
            foreach (var product in products)
            {
                string name = product.GetValue("name").AsString;
                decimal price = product.GetValue("price").AsDecimal;
                string fullName = $"{name} - CA$ {price}";

                productNames.Add(fullName);
            }

            ProductListBox.ItemsSource = productNames;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a window to add a new product
            AddProductWindow addProductWindow = new AddProductWindow(collection, LoadProductList);
            addProductWindow.ShowDialog();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //// Check if a product is selected
            //if (ProductListBox.SelectedItem != null)
            //{
            //    // Open a window to edit the selected product
            //    var selectedProduct = (BsonDocument)ProductListBox.SelectedItem;
            //    EditProductWindow editProductWindow = new EditProductWindow(collection, (BsonDocument)ProductListBox.SelectedItem, LoadProductList);
            //    editProductWindow.ShowDialog();
            //}
            //else
            //{
            //    MessageBox.Show("Please select a product to edit.");
            //}
            if (ProductListBox.SelectedItem != null)
            {
                string selectedProductFullName = ProductListBox.SelectedItem.ToString();
                string[] parts = selectedProductFullName.Split('-');
                string productName = parts[0].Trim();

                var filter = Builders<BsonDocument>.Filter.Eq("name", productName);
                var selectedProduct = collection.Find(filter).FirstOrDefault();

                if (selectedProduct != null)
                {
                    EditProductWindow editProductWindow = new EditProductWindow(collection, selectedProduct, LoadProductList);
                    editProductWindow.ShowDialog();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //// Check if a product is selected
            //if (ProductListBox.SelectedItem != null)
            //{
            //    // Confirm deletion
            //    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButton.YesNo);
            //    if (result == MessageBoxResult.Yes)
            //    {
            //        // Delete the selected product
            //        //var selectedProduct = ProductListBox.SelectedItem.AsBsonDocument;
            //        var selectedProduct = (BsonDocument)ProductListBox.SelectedItem;
            //        var filter = Builders<BsonDocument>.Filter.Eq("_id", selectedProduct["_id"]);
            //        collection.DeleteOne(filter);

            //        // Reload the product list
            //        LoadProductList();
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Please select a product to delete.");
            //}
            if (ProductListBox.SelectedItem != null)
            {
                string selectedProductFullName = ProductListBox.SelectedItem.ToString();
                string[] parts = selectedProductFullName.Split('-');
                string productName = parts[0].Trim();

                var filter = Builders<BsonDocument>.Filter.Eq("name", productName);
                var selectedProduct = collection.Find(filter).FirstOrDefault();

                if (selectedProduct != null)
                {
                    DeleteProductWindow deleteProductWindow = new DeleteProductWindow(collection, selectedProduct, LoadProductList);
                    deleteProductWindow.ShowDialog();
                }
            }
        }
    }
}
