using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
using WpfApp1.Entity;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        Agent editedAgent;

        MainWindow mainWindow;

        public EditWindow(Agent agent, MainWindow main)
        {
            InitializeComponent();

            editedAgent = agent;

            mainWindow = main;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Convert.ToInt32(tbPrior.Text);
            }
            catch 
            {
                return;
            }

            editedAgent.Priority = Convert.ToInt32(tbPrior.Text);

            Entities entities = new Entities();

            entities.Agent.AddOrUpdate(editedAgent);

            entities.SaveChanges();

            mainWindow.Update();

            Close();
        }
    }
}
