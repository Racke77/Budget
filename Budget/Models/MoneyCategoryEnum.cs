using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Models
{
    public enum MoneyCategoryEnum
    {
        [Description("Mat")]
        Food =0,
        [Description("Bostad")]
        Rent =1,
        [Description("Reparation")]
        Repair =2,
        [Description("Transport")]
        Transportation =3,
        [Description("Fritid")]
        Leisure =4,
        [Description("Barn")]
        Child =5,
        [Description("Prenumeration")]
        Subscription =6,
        [Description("Lön")]
        Salary =7,
        [Description("Bidrag")]
        Subsidy =8,
        [Description("Hobby")]
        Hobby = 9,
    }
}
