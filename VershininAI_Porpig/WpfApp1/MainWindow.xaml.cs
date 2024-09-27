using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Metadata;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Entity;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Entities entities;

        private int pages = 0;

        private int selectPage = 0;

        private List<Agent> agents;

        private const int maxDbLines = 10;

        public ObservableCollection<AgentModel> AgentModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            Update();

            Fill();
        }

        private void Fill()
        {
            AgentType[] types = entities.AgentType.ToArray();

            List<string> typeTitles = new List<string>();

            typeTitles.Add("-----");

            foreach (AgentType type in types)
            {
                typeTitles.Add(type.Title);
            }

            cbType.ItemsSource = typeTitles;
        }

        public void Update()
        {
            entities = new Entities();

            agents = entities.Agent.ToList();

            pages = (int)Math.Ceiling((double)agents.Count / maxDbLines)-1;

            selectPage = 0;

            ChangePage(0);
            

            lbCount.Content = $"Всего: {agents.Count}";

            SelectAgents();
        }

        private void RepackLines()
        {
            pages = (int)Math.Ceiling((double)agents.Count / maxDbLines)-1;

            selectPage = 0;

            ChangePage(0);
            

            lbCount.Content = $"Всего: {agents.Count}";

            SelectAgents();
        }

        public void UpdatePrior()
        {
            agents = agents.OrderBy(_ => _.Priority).ToList();

            pages = (int)Math.Ceiling((double)agents.Count / maxDbLines) - 1;

            selectPage = 0;

            ChangePage(0);
            

            lbCount.Content = $"Всего: {agents.Count}";

            SelectAgents();
        }
        public void UpdatePriorDown()
        {
            agents = agents.OrderByDescending(_ => _.Priority).ToList();

            pages = (int)Math.Ceiling((double)agents.Count / maxDbLines) - 1;

            selectPage = 0;

            ChangePage(0);
            

            lbCount.Content = $"Всего: {agents.Count}";

            SelectAgents();
        }

        private void SelectAgents()
        {
            int selectStartIndex = selectPage * maxDbLines;

            int selectEndIndex = selectStartIndex;

            if (selectStartIndex + maxDbLines <= agents.Count)
            {
                selectEndIndex += maxDbLines;
            }
            else
            {
                selectEndIndex = agents.Count - 1;
            }

            if (AgentModel == null)
            {
                AgentModel = new ObservableCollection<AgentModel>();
            }
            else
            {
                AgentModel.Clear();
            }

            for (int i = selectStartIndex;i < selectEndIndex;i++)
            {
                AgentModel model = new AgentModel();

                model.Name = agents[i].DirectorName;

                AgentType[] agentTypes = entities.AgentType.ToArray();

                model.Type = agentTypes.FirstOrDefault(_ =>_.ID == agents[i].AgentTypeID).Title;
                model.Phone = agents[i].Phone;
                model.Priority = agents[i].Priority;
                model.ID = agents[i].ID;

                AgentModel.Add(model);
            }
        }

        

       

        private void changepage_Click(object sender, RoutedEventArgs e)
        {
            int page = 0;
            try
            {
                page = Convert.ToInt32(txt_Page.Text);
            }
            catch
            {
                MessageBox.Show("Введите верный номер страницы");
            }

            ChangePage(page);

           
        }

        

        private void ChangePage(int page)
        {
            selectPage = page;

            SelectAgents();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if(lbAgents.SelectedItem == null)
            {
                return;
            }

            AgentModel agentModel = lbAgents.SelectedItem as AgentModel;


            Agent agent = entities.Agent.First(_ => _.ID == agentModel.ID);

            entities.Agent.Remove(agent);

            entities.SaveChanges();

            Update();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow(this);

            addWindow.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lbAgents.SelectedItem == null)
            {
                return;
            }

            AgentModel agentModel = lbAgents.SelectedItem as AgentModel;


            Agent agent = entities.Agent.First(_ => _.ID == agentModel.ID);

            EditWindow edit = new EditWindow(agent, this);

            edit.ShowDialog();
        }

        private void cbPrior_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sort();
        }

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sort();
        }

        private void Sort()
        {
            Update();

            if (cbType.SelectedIndex > 0)
            {
                AgentType types = entities.AgentType.First(_ => _.Title == cbType.SelectedValue);

                agents = agents.Where(_=>_.AgentTypeID == types.ID).ToList();

                RepackLines();
            }

            if(cbPrior.SelectedIndex == 1)
            {
                UpdatePriorDown();
            }
            else if(cbPrior.SelectedIndex == 2)
            {
                UpdatePrior();
            }
        }
    }
}
