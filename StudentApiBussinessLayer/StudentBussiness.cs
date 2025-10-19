using StudentApiDataAccessLayer;

namespace StudentApiBussinessLayer
{
    public class BussinessLayer
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public StudentDTO SDTO
        {
            get { return (new StudentDTO(this.ID, this.Name, this.Age, this.Grade)); }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }

        public BussinessLayer(StudentDTO SDTO, enMode cMode = enMode.AddNew)

        {
            this.ID = SDTO.Id;
            this.Name = SDTO.Name;
            this.Age = SDTO.Age;
            this.Grade = SDTO.Grade;

            Mode = cMode;
        }



        public static List<StudentDTO> GetAllStudents()
        {
            return DataLayer.GetAllStudents();
        }

        public static List<StudentDTO> GetPassedStudents()
        {
            return DataLayer.GetPassedStudents();
        }

        public static double GetAVGStudetnsGrades()
        {
            return DataLayer.GetAVGStudetnsGrades();
        }

        public static BussinessLayer Find(int ID)
        {

            StudentDTO SDTO = DataLayer.GetStudentById(ID);

            if (SDTO != null)
            //we return new object of that student with the right data
            {

                return new BussinessLayer(SDTO, enMode.Update);
            }
            else
                return null;
        }

        private bool AddnewStudent()
        {
            this.ID = DataLayer.AddNewStudent(SDTO);
            return (this.ID != -1);
        }

        private bool _UpdateStudent()
        {
            return DataLayer.UpdateStudent(SDTO);
        }

        public static bool DeleteStudent(int id)
        {
            return DataLayer.DeleteStudent(id);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddnewStudent())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateStudent();

            }

            return false;
        }
    }
}
