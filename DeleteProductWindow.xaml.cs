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
    /// Interaction logic for DeleteProductWindow.xaml
    /// </summary>
    public partial class DeleteProductWindow : Window
    {
        private readonly IMongoCollection<BsonDocument> collection;
        private readonly BsonDocument product;
        private readonly Action refreshProductList;
        public DeleteProductWindow(IMongoCollection<BsonDocument> collection, BsonDocument product, Action refreshProductList)
        {
            InitializeComponent();
            this.collection = collection;
            this.product = product;
            this.refreshProductList = refreshProductList;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", product.GetValue("_id"));
            collection.DeleteOne(filter);

            MessageBox.Show("Product deleted successfully!");

            refreshProductList.Invoke();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
