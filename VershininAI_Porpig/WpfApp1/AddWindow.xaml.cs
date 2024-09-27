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
using WpfApp1.Entity;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        Entities entity;

        MainWindow mainn;

        public AddWindow(MainWindow main)
        {
            InitializeComponent();

            entity = new Entities();

            mainn = main;

            FillCB();
        }

        private void FillCB()
        {
            AgentType[] types = entity.AgentType.ToArray();

            List<string> typeTitles = new List<string>();

            foreach (AgentType type in types)
            {
                typeTitles.Add(type.Title);
            }

            cbType.ItemsSource = typeTitles;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(cbType.SelectedItem == null)
            {
                return;
            }

            if(tbTitle.Text == string.Empty || tbTitle.Text.Length > 150)
            {
                return;
            }
            if(tbName.Text.Length > 100)
            {
                return;
            }
            if(tbPriority.Text == string.Empty)
            {
                return;
            }
            if(tbPhone.Text == string.Empty || tbPhone.Text.Length > 20)
            {
                return;
            }
            if (tbINN.Text == string.Empty || tbINN.Text.Length > 12)
            {
                return;
            }
            if(tbKPP.Text.Length > 9)
            {
                return;
            }
            if(tbAddress.Text.Length > 300)
            {
                return;
            }

            try
            {
                Convert.ToInt32(tbPriority.Text);
            }
            catch
            {
                return;
            }

            AgentType types = entity.AgentType.First(_=>_.Title == cbType.SelectedValue);

            Agent agent = new Agent();

            agent.AgentTypeID = types.ID;
            agent.Title = tbTitle.Text;
            agent.DirectorName = tbName.Text;
            agent.Priority = Convert.ToInt32(tbPriority.Text);
            agent.Phone = tbPhone.Text;
            agent.Address = tbAddress.Text;
            agent.INN = tbINN.Text;
            agent.KPP = tbKPP.Text;

            entity.Agent.Add(agent);

            entity.SaveChanges();

            mainn.Update();

            Close();
        }
        void ShowMessage()
        {
            MessageBox.Show("Неверно заполнены поля");
        }
    }
}
