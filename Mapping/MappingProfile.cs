using AutoMapper;
using UniversityApi.DTO.Courses;
using UniversityApi.DTO.Enrollments;
using UniversityApi.DTO.Students;
using UniversityApi.DTO.Teachers;
using UniversityApi.Entities;

namespace UniversityApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Student
        CreateMap<Student, StudentDto>();
        CreateMap<Student, StudentWithCoursesDto>()
            .ForMember(d => d.Courses, o => o.MapFrom(s => s.Enrollments.Select(e => e.Course)));
        CreateMap<Course, CourseBriefDto>();
        CreateMap<StudentCreateDto, Student>();
        CreateMap<StudentUpdateDto, Student>();

        // Teacher
        CreateMap<Teacher, TeacherDto>();
        CreateMap<TeacherCreateDto, Teacher>();
        CreateMap<TeacherUpdateDto, Teacher>();

        // Course
        CreateMap<Course, CourseDto>()
            .ForMember(d => d.TeacherName, o => o.MapFrom(c =>
                c.Teacher != null ? c.Teacher.FirstName + " " + c.Teacher.LastName : null));
        CreateMap<CourseCreateDto, Course>();
        CreateMap<CourseUpdateDto, Course>();

        // Enrollment
        CreateMap<Enrollment, EnrollmentDto>()
            .ForMember(d => d.StudentName, o => o.MapFrom(e =>
                e.Student != null ? e.Student.FirstName + " " + e.Student.LastName : null))
            .ForMember(d => d.CourseName, o => o.MapFrom(e => e.Course != null ? e.Course.Name : null));
        CreateMap<EnrollmentCreateDto, Enrollment>();
        CreateMap<EnrollmentUpdateDto, Enrollment>();
    }
}