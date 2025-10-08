using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfLaundrySystemApp.Modules
{
    public class DynamicTableCreatorHistory
    {
        public Type TypeOfTheDynamicallyCreatedTable { get; set; }
        public Dictionary<PropertyInfo, object> WhereArguments { get; set; } = null;

        public DynamicTableCreatorHistory(DynamicTableCreator dynamicTableCreator)
        {
            TypeOfTheDynamicallyCreatedTable = dynamicTableCreator.TypeOfTheDynamicallyCreatedTable;
            WhereArguments = dynamicTableCreator.WhereArguments;
        }

        public DynamicTableCreatorHistory(Type typeOfTheDynamicallyCreatedTable, Dictionary<PropertyInfo, object> whereArguments)
        {
            TypeOfTheDynamicallyCreatedTable = typeOfTheDynamicallyCreatedTable;
            WhereArguments = whereArguments;
        }
    }
}
