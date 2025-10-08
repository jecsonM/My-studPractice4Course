using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WpfLaundrySystemApp.DBContext;
using WpfLaundrySystemApp.Models;
using WpfLaundrySystemApp.Modules;

namespace TestProjectForWPFLaundrySystem
{
    [TestClass]
    public sealed class TestOfDynamicTableCreator
    {
        /// <summary>
        /// Тестирую, чтоб таблица корректно меняла тип генерируемой таблицы
        /// на примере таблицы Partner без дополнительных условий where
        /// </summary>
        [TestMethod]
        public void test_SetNewTableToGenerate_Partner_null()
        {

            using (DynamicTableCreator tableCreator = new DynamicTableCreator(new LaundryDbContext()))
            {

                Type typeToChange = typeof(Partner);
                object objectToGetWhereArgsFrom = null;


                tableCreator.SetNewTableToGenerate(typeToChange, objectToGetWhereArgsFrom);


                Assert.IsTrue(
                    tableCreator.TypeOfTheDynamicallyCreatedTable == typeToChange
                    &&
                    (tableCreator.WhereArguments is null || tableCreator.WhereArguments.Count == 0)
                    );
            }

        }

        /// <summary>
        /// Тестирую, чтоб таблица корректно меняла тип генерируемой таблицы
        /// на примере таблицы Partner с дополнительными условиями where
        /// </summary>
        [TestMethod]
        public void test_SetNewTableToGenerate_Partner_partnerID_1()
        {

            using (DynamicTableCreator tableCreator = new DynamicTableCreator(new LaundryDbContext()))
            {

                Type typeToChange = typeof(Partner);
                int partnerId = 1;
                Dictionary<PropertyInfo, object> whereArguments = new Dictionary<PropertyInfo, object>(1);
                PropertyInfo propertToSet = typeToChange.GetProperty("PartnerId");

                whereArguments[propertToSet] = partnerId;


                tableCreator.SetNewTableToGenerate(typeToChange, whereArguments);


                Assert.IsTrue(
                    tableCreator.TypeOfTheDynamicallyCreatedTable == typeToChange
                    &&
                    (tableCreator.WhereArguments is not null &&
                    tableCreator.WhereArguments.Count > 0 &&
                    tableCreator.WhereArguments.ContainsKey(propertToSet) &&
                    (((int)tableCreator.WhereArguments[propertToSet]) == partnerId)
                )

                );
            }

        }

        /// <summary>
        /// Негативный тест-кейс
        /// Проверяю, что происходит исключение при попытке обратиться к disposed контексту, который пересоздаётся через ReCreateDbContext()
        /// </summary>
        [TestMethod]
        public void test_ReCreateDbContext()
        {
            LaundryDbContext initialLaundryDbContext = new LaundryDbContext();

            using (DynamicTableCreator tableCreator = new DynamicTableCreator(initialLaundryDbContext))
            {

                tableCreator.ReCreateDbContext();

                Assert.ThrowsException<ObjectDisposedException>(() => initialLaundryDbContext.Partners.ToList());
            }
    
        }

        /// <summary>
        /// Проверяю, что Получаются необходимые данные из dbContext
        /// </summary>
        [TestMethod]
        public void test_GetData()
        {
            LaundryDbContext laundryDbContext = new LaundryDbContext();

            using (DynamicTableCreator tableCreator = new DynamicTableCreator(laundryDbContext))
            {
                Type typeOfPartner = typeof(Partner);
                tableCreator.TypeOfTheDynamicallyCreatedTable = typeOfPartner;

                List<Partner> partnersFromGetData = tableCreator.GetData().Select(p => p as Partner).ToList();
                List<Partner> partnersFromDbContext = laundryDbContext.Partners.ToList();

                CollectionAssert.AreEquivalent(expected: partnersFromDbContext, actual: partnersFromGetData);

            }
        }

        /// <summary>
        /// Проверяю, что Получаются необходимые внешние ключи из Partner, ссылающиеся на PartnerType
        /// </summary>
        [TestMethod]
        public void test_GetForeignKeyProperties_Partner_PartnerType()
        {
            using (DynamicTableCreator tableCreator = new DynamicTableCreator(new LaundryDbContext()))
            {

                Type typeThatReferences = typeof(Partner);
                Type typeThatIsBeingReferencedTo = typeof(PartnerType);

                PropertyInfo[] FKpropertiesExpected =
                    typeThatReferences
                    .GetProperties()
                    .Where(p => p.Name == "PartnerTypeId")
                    .ToArray();

                PropertyInfo[] FKpropertiesActual = 
                    tableCreator.GetForeignKeyProperties(typeThatReferences, typeThatIsBeingReferencedTo);


                CollectionAssert.AreEquivalent(expected: FKpropertiesExpected, actual: FKpropertiesActual);
            }
        }

        /// <summary>
        /// Проверяю, что Получаются необходимые первичные ключи из Partner, ссылающиеся на PartnerType
        /// </summary>
        [TestMethod]
        public void test_GetForeignKeyProperties_Partner()
        {
            using (DynamicTableCreator tableCreator = new DynamicTableCreator(new LaundryDbContext()))
            {

                Type typeThatHasPKs = typeof(Partner);
                

                PropertyInfo[] PKpropertiesExpected =
                    typeThatHasPKs
                    .GetProperties()
                    .Where(p => p.Name == "PartnerId")
                    .ToArray();

                PropertyInfo[] PKpropertiesActual =
                    tableCreator.GetPrimaryKeyProperties(typeThatHasPKs);


                CollectionAssert.AreEquivalent(expected: PKpropertiesExpected, actual: PKpropertiesActual);
            }
        }
    }
}