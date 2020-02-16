using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class LessonsView
    {
        public int ID { get; set; }
        public int LectureID { get; set; }
        public string GroupID { get; set; }
        public string LectureName { get; set; }
        public string LecturerName { get; set; }
        public string GroupName { get; set; }

        public LessonsView()
        {

        }

        public LessonsView(LessonsView _lessonsView)
        {
            ID = _lessonsView.ID;
            LectureID = _lessonsView.LectureID;
            GroupID = _lessonsView.GroupID;
            LectureName = _lessonsView.LectureName;
            LecturerName = _lessonsView.LecturerName;
            GroupName = _lessonsView.GroupName;
        }

    }
}
