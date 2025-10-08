using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfLaundrySystemApp.Modules
{
    
    public static class MaterialCalc
    {

        public static int CalculateMaterialAmount(
            int serviceTypeId,
            int materialTypeId,
            int serviceCount,
            params double[] parameters)
        {
            if (serviceCount <= 0)
                throw new ArgumentException("Количество услуг должно быть больше нуля.", nameof(serviceCount));

            if (parameters is null || parameters.Length == 0)
                throw new ArgumentException("Необходимо указать хотя бы один параметр услуги.", nameof(parameters));

            double serviceCoefficient = ServiceCoefficient(serviceTypeId);
            double wastePercent = MaterialWastePercent(materialTypeId);
            double baseAmount = 1;
            foreach (var p in parameters)
            {
                if (p <= 0)
                    throw new ArgumentException("Параметры услуги должны быть положительными числами.");
                baseAmount *= p;
            }

            double totalMaterial = baseAmount * serviceCoefficient * serviceCount * ((100 + wastePercent) / 100.0);
            return (int)Math.Ceiling(totalMaterial);
        }


        private static double ServiceCoefficient(int serviceTypeId) =>((double)serviceTypeId) / 2;
        


        private static double MaterialWastePercent(int materialTypeId) => materialTypeId * 1.5;
        
    }
    


}
