using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using DataLayer;
namespace TestingLayer
{
    [TestFixture]
    public class FieldContextTests
    {
        static FieldContext fieldContext;
        static FieldContextTests()
        {
            fieldContext = new FieldContext(TestManager.dbContext);
        }

        [Test]
        public void CreateField()
        {
            Field Field = new Field("Data Science");
            int fieldsBefore = TestManager.dbContext.Fields.Count();

            fieldContext.Create(Field);

            int fieldsAfter = TestManager.dbContext.Fields.Count();
            Field lastField = TestManager.dbContext.Fields.Last();
            Assert.That(fieldsBefore + 1 == fieldsAfter, "Field is not created!");
        }

        [Test]
        public void ReadField()
        {
            Field newField = new Field("Software Engeneering");
            fieldContext.Create(newField);

            Field field = TestManager.dbContext.Fields.Last();

            Assert.That(field.Name == "Software Engeneering", "Read() does not get Field by id!");
        }

        [Test]
        public void ReadAllField()
        {
            int fieldsBefore = TestManager.dbContext.Fields.Count();

            int fieldsAfter = fieldContext.ReadAll().Count;

            Assert.That(fieldsBefore == fieldsAfter, "ReadAll() does not return all of the Field!");
        }


        [Test]
        public void UpdateField()
        {
            Field newField = new Field("novel");
            fieldContext.Create(newField);

            Field lastField = fieldContext.ReadAll().Last();
            lastField.Name = "Updated Field";

            fieldContext.Update(lastField, false);

            Assert.That(fieldContext.Read(lastField.Id).Name == "Updated Field", "Update() does not change the Field's name!");
        }


        [Test]
        public void DeleteField()
        {
            Field newField = new Field("English");
            fieldContext.Create(newField);

            Field field = fieldContext.ReadAll().Last();
            int fieldId = field.Id;

            fieldContext.Delete(fieldId);

            ArgumentException ex = Assert.Throws<ArgumentException>(() => fieldContext.Read(fieldId));
            Assert.That(ex.Message, Is.EqualTo($"Field with id = {fieldId} does not exist!"));
        }
    }
}
