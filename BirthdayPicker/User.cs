using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayPicker
{
    internal class User
    {
        internal User()
        {
        }
        
        public DateTime Date { get; set; }

        public string Age { get; set; }

        public string WesternZodiac { get; set; }

        public string ChineseZodiac { get; set; }
    }
}
