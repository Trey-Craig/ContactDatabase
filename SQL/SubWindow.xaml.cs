using SQL.Classes;
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

namespace SQL {
    /// <summary>
    /// Interaction logic for SubWindow.xaml
    /// </summary>
    public partial class SubWindow : Window {
        List<properties> people_results = new List<properties>();
        DataAccess database_data = new DataAccess();
        
        public SubWindow(string text) {
            InitializeComponent();
            RefreshListBoxBinding(text);
            
        }

        public void RefreshListBoxBinding(string text) {
            cmbobox.Items.Clear();
            people_results = database_data.GetFalsePeople(text);
            foreach (properties item in people_results) {
                cmbobox.Items.Add(new CheckBox() {Content = $"{item.first_name} {item.last_name}", Tag = item.id });
            }
        }

        private void Delete_Record(object sender, RoutedEventArgs e) {
            foreach (CheckBox item in cmbobox.Items) {
                
                if ((bool)item.IsChecked) {
                    database_data.DeleteData(item.Tag.ToString());
                }
            }
            RefreshListBoxBinding("");
        }

        

        private void Delete_All(object sender, RoutedEventArgs e) {

            foreach (CheckBox item in cmbobox.Items) {
               
                //deletes all contacts marked or not
                database_data.DeleteData(item.Tag.ToString());
            }
            RefreshListBoxBinding("");
        }

        private void Restore_Record(object sender, RoutedEventArgs e) {
            foreach (CheckBox item in cmbobox.Items) {
                
                //only restores contacts that were marked
                if ((bool)item.IsChecked) {
                    database_data.Reactivate(item.Tag.ToString());
                }
            }
            RefreshListBoxBinding("");
        }

        private void Restore_All(object sender, RoutedEventArgs e) {
            //restores all contacts
            foreach (CheckBox item in cmbobox.Items) {
                    database_data.Reactivate(item.Tag.ToString());
            }
            RefreshListBoxBinding("");
        }
    }
}
