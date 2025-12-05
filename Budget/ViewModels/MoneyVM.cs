using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Budget.Calculations;
using Budget.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

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
        public bool ReocurringMonth
        {
            get { return model.ReocurringMonth; }
            set
            {
                model.ReocurringMonth = value;
                RaisePropertyChanged();
            }
        }
        public bool ReocurringYear
        {
            get { return model.ReocurringYear; }
            set
            {
                model.ReocurringYear = value;
                RaisePropertyChanged();
            }
        }
        public string Category
        {
            get
            {
                var descriptionAttribute = model.Category.GetType()
                        .GetMember(model.Category.ToString())[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)[0] as DescriptionAttribute;
                return descriptionAttribute.Description;
            }
            set
            {
                model.Category = GetEnumFromDescriptionString(value);
            }
        }
        public ObservableCollection<string> AllCategories
        {
            get
            {
                var values = Enum.GetValues(typeof(MoneyCategoryEnum));
                ObservableCollection<string> result = new ObservableCollection<string>();
                foreach (MoneyCategoryEnum item in values)
                {
                    result.Add(GetDescriptionEnum(item));
                }
                return result;
            }
            set
            {

                AllCategories = value;
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
        private string GetDescriptionEnum(MoneyCategoryEnum test)
        {
            var descriptionAttribute = test.GetType()
                    .GetMember(test.ToString())[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)[0] as DescriptionAttribute;
            return descriptionAttribute.Description;
        }
        public MoneyCategoryEnum GetEnumFromDescriptionString(string value)
        {
            var values = Enum.GetValues(typeof(MoneyCategoryEnum));
            foreach (MoneyCategoryEnum moneyEnum in values)
            {
                if(GetDescriptionEnum(moneyEnum) == value) { return moneyEnum; }
            }
            return MoneyCategoryEnum.Food;
        }
    }
}
