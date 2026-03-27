using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uchebka.Model
{
    public static class MaterialCalculationHelper
    {
        public static int CalculateMaterial(int productTypeId, int materialTypeId, int productQuantity, double param1, double param2)
        {
            try
            {
                if (productQuantity <= 0 || param1 <= 0 || param2 <= 0)
                    return -1;
                var productType = ConnectionClass.comfortEntities.ProductType.FirstOrDefault(pt => pt.Id_prodtype == productTypeId);
                if (productType == null)
                    return -1;
                var materialType = ConnectionClass.comfortEntities.TypeMaterial.FirstOrDefault(mt => mt.Id_type_material == materialTypeId);
                if (materialType == null)
                    return -1;
                double coefficient;
                if (!double.TryParse(productType.Coefficient, out coefficient))
                    return -1;
                double lossPercent = (double)(materialType.LostProcent ?? 0);
                double materialPerUnit = param1 * param2 * coefficient;
                double totalMaterial = materialPerUnit * productQuantity;
                double materialWithLoss = totalMaterial * (1 + lossPercent);
                return (int)Math.Ceiling(materialWithLoss);
            }
            catch
            {
                return -1;
            }
        }
    }
}
