using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAnalytics.Common.Commands;

namespace OL.Infrastructure.Commands.Categories
{
    public class EnableOLCategoryCommand : BaseCommand
    {
        public bool Enabled { get; set; }
    }
}