using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;
using System.Runtime.Remoting.Contexts;

namespace DataAccessLayer
{
    public class DataAccess
    {
        
        DataClasses1DataContext context = new DataClasses1DataContext(ConfigurationManager.ConnectionStrings["AuthContext"].ToString());


        public void UpdateUser(Guid userId, int roleId, string userName, string firstName, string lastName, string phone, string title, string department, Guid clientGUID)
        {
            context.usp_UpdateUser(userId.ToString(), roleId, userName, firstName, lastName, phone, title, department, clientGUID);
        }

        public List<usp_GetLocationsResult> GetLocations(Guid userId)
        {
            return context.usp_GetLocations(userId.ToString()).ToList();
        }

        public List<usp_GetUserLanguagesResult> GetUserLanguages(string userId)
        {
            return context.usp_GetUserLanguages(userId).ToList();
        }

        public List<usp_GetHPJobsResult> GetHPJobs(string userId)
        {
            return context.usp_GetHPJobs(userId, null).ToList();
        }

        public List<usp_GetHPJobsResult> GetHPJobs(string userId, Guid jobId)
        {
            return context.usp_GetHPJobs(userId, jobId).ToList();
        }

        public void DeleteUserLanguage(Guid id)
        {
            context.usp_DeleteUserLanguage(id);
        }

        public void AddUserLanguage(string userId, string description)
        {
            context.usp_AddUserLanguage(userId, description);
        }

        public void AddLocation(Guid userId, int addressTypeId, string address1, string address2, string city, string state, string zip)
        {
            context.usp_AddAddress(userId.ToString(), addressTypeId, address1, address2, city, state, zip);
        }

        public void UpdateLocation(Guid locationId, int addressTypeId, string address1, string address2, string city, string state, string zip)
        {
            context.usp_UpdateAddress(locationId, addressTypeId, address1, address2, city, state, zip);
        }

        public void DeleteLocation(Guid locationId)
        {
            context.usp_DeleteAddres(locationId);
        }

        public List<usp_GetEducationsResult> GetEducations(Guid userId)
        {
            return context.usp_GetEducations(userId.ToString()).ToList();
        }

        public void AddEducation(Guid userId, string institutionName, string major, DateTime? startDate, DateTime? endDate, int highestDegreeEarnedId, string otherDegree, bool? graduated, DateTime? graduationDate)
        {
            context.usp_AddEducation(userId.ToString(), institutionName, major, startDate, endDate, highestDegreeEarnedId, otherDegree, graduated, graduationDate);
        }

        public void UpdateEducation(Guid educationId, string institutionName, string major, DateTime? startDate, DateTime? endDate, int highestDegreeEarnedId, string otherDegree, bool? graduated, DateTime? graduationDate)
        {
            context.usp_UpdateEducation(educationId, institutionName, major, startDate, endDate, highestDegreeEarnedId, otherDegree, graduated, graduationDate);
        }

        public void DeleteEducation(Guid educationId)
        {
            context.usp_DeleteEducation(educationId);
        }

        public List<usp_GetCertificationsResult> GetCertifications(Guid userId)
        {
            return context.usp_GetCertifications(userId.ToString()).ToList();
        }

        public void AddCertification(Guid userId, string certificationName, string issuingBody, DateTime? issuanceDate, DateTime? expirationDate)
        {
            context.usp_AddCertification(userId.ToString(), certificationName, issuingBody, issuanceDate, expirationDate);
        }

        public void UpdateCertification(Guid certificationId, string certificationName, string issuingBody, DateTime? issuanceDate, DateTime? expirationDate)
        {
            context.usp_UpdateCertification(certificationId, certificationName, issuingBody, issuanceDate, expirationDate);
        }

        public void DeleteCertification(Guid certificationId)
        {
            context.usp_DeleteCertification(certificationId);
        }

        public List<usp_GetWorkHistoriesResult> GetWorkHistories(Guid userId)
        {
            return context.usp_GetWorkHistories(userId.ToString()).ToList();
        }

        public void AddWorkHistory(Guid userId, string companyName, string companyCity, string companyState, string jobTitle, string jobResponsibilities, DateTime? startDate, DateTime? endDate)
        {
            context.usp_AddWorkHistory(userId.ToString(), companyName, companyCity, companyState, jobTitle, jobResponsibilities, startDate, endDate);
        }

        public void UpdateWorkHistory(Guid workHistoryId, string companyName, string companyCity, string companyState, string jobTitle, string jobResponsibilities, DateTime? startDate, DateTime? endDate)
        {
            context.usp_UpdateWorkHistory(workHistoryId, companyName, companyCity, companyState, jobTitle, jobResponsibilities, startDate, endDate);
        }

        public void DeleteWorkHistory(Guid workHistoryId)
        {
            context.usp_DeleteWorkHistory(workHistoryId);
        }
        
        public List<usp_GetReferencesResult> GetReferences(Guid userId)
        {
            return context.usp_GetReferences(userId.ToString()).ToList();
        }

        public void AddReference(Guid userId, string name, string title, bool directSupervisor, string contactPhone)
        {
            context.usp_AddReference(userId.ToString(), name, title, directSupervisor, contactPhone);
        }

        public void UpdateReference(Guid referenceId, string name, string title, bool directSupervisor, string contactPhone)
        {
            context.usp_UpdateReference(referenceId, name, title, directSupervisor, contactPhone);
        }

        public void DeleteReference(Guid referenceId)
        {
            context.usp_DeleteReference(referenceId);
        }

        public List<usp_GetSpecialtiesResult> GetSpecialties(int? specialtyId)
        {
            return context.usp_GetSpecialties(specialtyId).ToList();
        }

        public int AddSpecialty(string name, string description)
        {
            return context.usp_AddSpecialty(name, description).FirstOrDefault().ID.Value;
        }

        public void UpdateSpecialty(int specialtyId, string name, string description)
        {
            context.usp_UpdateSpecialty(specialtyId, name, description);
        }

        public void DeleteSpecialty(int specialtyId)
        {
            context.usp_DeleteSpecialty(specialtyId);
        }

        public void UpdateUserDetails(Guid userId, string secondaryEmail, string cellPhone, string personalSummary, string personalInterests, bool disableNotifications, string imageUrl, string videoUrl, int availabilityInDays)
        {
            context.usp_UpdateUserDetails(userId.ToString(), secondaryEmail, cellPhone, personalSummary, personalInterests, disableNotifications, availabilityInDays, imageUrl, videoUrl);
        }

        public void UpdateQuestion(Guid surveyId, Guid skillsChecklistQuestionId, string text, int questionTypeId, bool required, int? position)
        {
            context.usp_UpdateQuestion(surveyId, skillsChecklistQuestionId, text, questionTypeId, required, position);
        }

        public void DeleteQuestions(Guid skillsChecklistId, Guid? skillsChecklistQuestionId)
        {
            context.usp_DeleteQuestion(skillsChecklistId, skillsChecklistQuestionId);
        }

        public List<usp_GetSkillsChecklistsResult> GetSkillsChecklists(Guid? skillsChecklistId)
        {
            return context.usp_GetSkillsChecklists(skillsChecklistId).ToList();
        }

        public List<usp_GetQuestionTypesResult> GetQuestionTypes()
        {
            return context.usp_GetQuestionTypes().ToList();
        }

        public Guid AddSkillsChecklist(string title, string description, bool template)
        {
            return context.usp_AddSkillsChecklist(title, description, template).FirstOrDefault().GUID;
        }

        public void UpdateSkillsChecklist(Guid skillsChecklistId, string title, string description, bool template)
        {
            context.usp_UpdateSkillsChecklist(skillsChecklistId, title, description, template);
        }

        public void DeleteSkillsChecklist(Guid skillsChecklistId)
        {
            context.usp_DeleteSkillsChecklist(skillsChecklistId);
        }

        public List<usp_GetQuestionsResult> GetSkillsChecklistQuestions(Guid surveyGUID)
        {
            return context.usp_GetQuestions(surveyGUID, null).ToList();
        }

        public usp_AddQuestionResult AddQuestion(Guid surveyGuid, string text, int questionTypeId, bool required)
        {
            return context.usp_AddQuestion(surveyGuid, text, questionTypeId, required).FirstOrDefault();
        }

        public List<usp_GetJobSpecialtiesResult> GetJobSpecialties(Guid jobID, int? specialtyId)
        {
            return context.usp_GetJobSpecialties(jobID, specialtyId).ToList();
        }

        public int AddJobSpecialty(Guid jobId, int specialtyId)
        {
            return context.usp_AddJobSpecialty(jobId, specialtyId);
        }

        public void DeleteJobSpecialty(Guid jobId, int? specialtyId)
        {
            context.usp_DeleteJobSpecialty(jobId, specialtyId);
        }

        public List<usp_GetJobSkillsChecklistResult> GetJobSkillsChecklist(Guid jobID, Guid? skillsChecklistId)
        {
            return context.usp_GetJobSkillsChecklist(jobID, skillsChecklistId).ToList();
        }

        public int AddJobSkillsChecklist(Guid jobId, Guid skillsChecklistId)
        {
            return context.usp_AddJobSkillsChecklist(jobId, skillsChecklistId);
        }

        public void DeleteJobSkillsChecklist(Guid jobId, Guid skillsChecklistId)
        {
            context.usp_DeleteJobSkillsChecklist(jobId, skillsChecklistId);
        }



        public List<usp_GetRolesResult> GetRoles(string UserId)
        {
            return context.usp_GetRoles(UserId).ToList();
        }

        public List<usp_GetJobsResult> GetJobs(Guid? clientId, Guid? jobId)
        {
            return context.usp_GetJobs(clientId, jobId).ToList();
        }

        public void UpdateJob(Guid jobId, Guid clientId, string name, string description, DateTime? startDate, DateTime? endDate)
        {
            context.usp_UpdateJob(jobId, clientId, name, description, startDate, endDate);
        }

        public void DeleteJob(Guid jobId)
        {
            context.usp_DeleteJob(jobId);
        }

        public List<usp_GetMarketingBulletsResult> GetMarketingBullets(Guid clientId)
        {
            return context.usp_GetMarketingBullets(clientId).ToList();
        }

        public void DeleteMarketingBullets(Guid clientId)
        {
            context.usp_DeleteMarketingBullets(clientId);
        }

        public void AddMarketingBullet(Guid clientId, string bulletPoint)
        {
            context.usp_AddMarketingBullet(clientId, bulletPoint);
        }

        public Guid? AddJob(Guid clientId, string name, string description, DateTime? startDate, DateTime? endDate)
        {
            return context.usp_AddJob(clientId, name, description, startDate, endDate).FirstOrDefault().ID;
        }

        public Guid? AddClient(string name, string description, string address1, string address2, string city, string state, string zip, Guid parentGUID, string supplementalDescr, string urlRoute, int profileTemplateId)
        {
            return context.usp_AddClient(name, address1, address2, city, state, zip, description, parentGUID, supplementalDescr, urlRoute, profileTemplateId).FirstOrDefault().CLIENT_GUID.Value;
        }

        public void UpdateClient(Guid clientGuid, string name, string description, string address1, string address2, string city, string state, string zip, Guid? parentGUID, string supplementalDescr, string urlRoute, int profileTemplateId)
        {
            context.usp_UpdateClient(null, clientGuid, name, address1, address2, city, state, zip, description, parentGUID, supplementalDescr, urlRoute, profileTemplateId);
        }

        public List<usp_GetClientsResult> GetClients()
        {
            return context.usp_GetClients(null).ToList();
        }

        public List<usp_GetSubClientsResult> GetSubClients(Guid clientId)
        {
            return context.usp_GetSubClients(clientId).ToList();
        }

        public usp_GetClientsResult GetClient(Guid clientGuid)
        {
            return context.usp_GetClients(clientGuid).FirstOrDefault();
        }

        public List<usp_GetUsersResult> GetUsers(int? roleId, Guid? clientGUID)
        {
            return context.usp_GetUsers(roleId, clientGUID).ToList();
        }

        public usp_GetUserResult GetUser(Guid userId)
        {
            return context.usp_GetUser(userId.ToString()).FirstOrDefault();
        }

        public void DeleteClient(Guid clientGuid)
        {
            context.usp_DeleteClient(clientGuid);
        }

        public void DeleteUser(string userId)
        {
            context.usp_DeleteUser(userId);
        }
    }

    
}
