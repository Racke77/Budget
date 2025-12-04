using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Budget.Calculations;
using Budget.Models;

namespace Budget.ViewModels
{
    public class MoneyVM : ViewModelBase
    {
        private readonly MoneyTransaction model;

        public MoneyVM(MoneyTransaction model)
        {
            this.model = model;
        }

        public string Name
        {
            get { return model.Name; }
            set
            {
                model.Name = value;
                RaisePropertyChanged();
            }
        }
        public bool Reocurring
        {
            get { return model.Reocurring; }
            set
            {
                model.Reocurring = value;
                RaisePropertyChanged();
            }
        }
        public bool IsThisSalary
        {
            get { return model.IsThisSalary; }
            set
            {
                model.IsThisSalary = value;
                RaisePropertyChanged();
            }
        }
        public bool IsThisIncome
        {
            get { return model.IsThisIncome; }
            set
            {
                model.IsThisIncome = value;
                model.CalculateValue();
                RaisePropertyChanged();
            }
        }
        public int SickDays
        {
            get { return model.SickDays; }
            set
            {
                model.SickDays = value;
                model.CalculateValue();
                RaisePropertyChanged();
            }
        }
        public int Id
        {
            get { return model.Id; }
            set
            {
                model.Id = value;
                RaisePropertyChanged();
            }
        }
        public float Money
        {
            get { return model.Money; }
            set
            {
                model.Money = value;
                model.CalculatedValue = model.Money; //same value
                model.CalculateValue();
                RaisePropertyChanged();
            }
        }
        public float CalculatedValue
        {
            get { return model.CalculatedValue; }
            set
            {
                model.CalculatedValue = model.Money; //set only internally not by user
                RaisePropertyChanged();
            }
        }

    }
}
