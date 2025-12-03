using System.Windows;
using Budget.Data;
using Budget.ViewModels;

namespace Budget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BudgetVM viewModel;
        public MainWindow()
        {
            InitializeComponent();

            ApplicationDbContext dbContext = new ApplicationDbContext();
            viewModel = new BudgetVM(dbContext);
            DataContext = viewModel;
        }

    }
}