﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ACE.Client.Infrastructure;

namespace ACE.Client.ViewsViewModels
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
            NavigationServices.Initialize(this.NavigationFrame);
            NavigationServices.Register("CustomersView", typeof(CustomersView));
            NavigationServices.Register("InvestorAccountView", typeof(InvestorAccountView));
        }
    }
}
