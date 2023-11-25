using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_EF_Start.Models
{
    public class Student
    {
        [Key]
        public int Student_Id { get; set; }

        public string First_Name { get; set; }
     
        public string Last_Name { get; set; }

        public string Program { get; set; }
       
        public string Email { get; set; }

        public List<ProjectDocument> ProjectDocuments { get; set; }
    }
    public class ProjectDocument
    {
        [Key]
        public int Document_Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Research_Topic { get; set; }

        public DateTime Published_Date { get; set; }

        [ForeignKey("Student")]
        public int Student_Id { get; set; }
        public Student Student { get; set; }

        public List<DownloadInformation> DownloadInformations { get; set; }
    }
    public class User
    {
        [Key]
        public int User_Id { get; set; }
     
        public string First_Name { get; set; }      
        public string Last_Name { get; set; }

        public string user_Name { get; set; }

        public List<DownloadInformation> DownloadInformations { get; set; }
    }

    public class DownloadInformation
    {
        [Key]
        public int Download_Id { get; set; }
        public DateTime Download_Date { get; set; }

        [ForeignKey("User")]
        public int User_Id { get; set; }
        public User User { get; set; }

        [ForeignKey("ProjectDocument")]
        public int Document_Id { get; set; }
        public ProjectDocument ProjectDocument { get; set; }
    }
}

