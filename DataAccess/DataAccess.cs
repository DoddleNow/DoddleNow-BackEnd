using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;
using System.Runtime.Remoting.Contexts;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessLayer
{
    public class DataAccess
    {

        DataClasses1DataContext context = new DataClasses1DataContext(ConfigurationManager.ConnectionStrings["AuthContext"].ToString());

        public List<usp_GetJobShiftsResult> GetJobShifts(Guid jobId)
        {
            return context.usp_GetJobShifts(jobId).ToList();
        }

        public void AddJobShift(Guid jobId, int shiftId)
        {
            context.usp_AddJobShift(jobId, shiftId);
        }

        public List<usp_GetMarketInsightsResult> GetMarketInsights(Guid clientId, int availability, int experience, int sclMatch, int education, int shift)
        {
            return context.usp_GetMarketInsights(clientId, availability, experience, sclMatch, education, shift).ToList();
        }

        public usp_GetMarketSpecialtyInsightsResult GetMarketSpecialtyInsights(Guid clientId, int specialtyId, int availability, int experience, int sclMatch, int education, int shift)
        {
            return context.usp_GetMarketSpecialtyInsights(clientId, specialtyId, availability, experience, sclMatch, education, shift).FirstOrDefault();
        }

        public List<usp_GetSpecialtyUserMatchesResult> GetSpecialtyUserMatches(int specialtyId)
        {
            return context.usp_GetSpecialtyUserMatches(specialtyId).ToList();
        }

        public void UpdateClientGlobalSettings(Guid clientId, int availability, int experience, int sclMatch, int education, int shift)
        {
            context.usp_UpdateClientGlobalSettings(clientId, availability, experience, sclMatch, education, shift);
        }

        public usp_GetClientGlobalSettingsResult GetClientGlobalSettings(Guid clientId)
        {
            return context.usp_GetClientGlobalSettings(clientId).FirstOrDefault();
        }


        public void UpdateClientSpecialtySettings(Guid clientId, int specialtyId, int availability, int experience, int sclMatch, int education, int shift)
        {
            context.usp_UpdateClientSpecialtySettings(clientId, specialtyId, availability, experience, sclMatch, education, shift);
        }

        public usp_GetClientSpecialtySettingsResult GetClientSpecialtySettings(Guid clientId, int specialtyId)
        {
            return context.usp_GetClientSpecialtySettings(clientId, specialtyId).FirstOrDefault();
        }

        public List<usp_GetClientCandidatesResult> GetClientCandidates(Guid clientId)
        {
            return context.usp_GetClientCandidates(clientId).ToList();
        }

        public void DeleteJobShift(Guid jobId, int shiftId)
        {
            context.usp_DeleteJobShift(jobId, shiftId);
        }

        public void UpdateUserJob(string userId, Guid jobId, bool? starred = null, bool? applied = null, bool clientInterest = false)
        {
            context.usp_UpdateUserJob(userId, jobId, starred, applied, clientInterest);
        }

        public void AddHPSpecialty(string userId, int specialtyId)
        {
            context.usp_AddHPSpecialty(userId, specialtyId);
        }

        public void DeleteHPSpecialties(string userId)
        {
            context.usp_DeleteHPSpecialty(userId, 0);
        }

        public void DeleteHPSpecialty(string userId, int specialtyId)
        {
            context.usp_DeleteHPSpecialty(userId, specialtyId);
        }


        public List<usp_GetHPSkillsChecklistsResult> GetHPSkillsCheckists(string userId)
        {
            return context.usp_GetHPSkillsChecklists(userId).ToList();
        }

        public List<usp_GetHPSpecialtiesResult> GetHPSpecialties(string userId)
        {
            return context.usp_GetHPSpecialties(userId).ToList();
        }

        public void DeleteJobShifts(Guid jobId)
        {
            context.usp_DeleteJobShifts(jobId);
        }

        public List<usp_GetJobCandidatesResult> GetJobCandidates(Guid jobId)
        {
            return context.usp_GetJobCandidates(jobId, null).ToList();
        }

        public usp_GetJobCandidatesResult GetJobCandidate(Guid jobId, string candidateId)
        {
            return context.usp_GetJobCandidates(jobId, candidateId).FirstOrDefault();
        }

        public void UpdateJobCandidate(Guid jobId, string userId, bool clientInterest, bool clientStarred, bool coffeeConnect, bool applicantApplied, bool exclude)
        {
            context.usp_UpdateJobCandidate(jobId, userId, clientInterest, clientStarred, coffeeConnect, applicantApplied, exclude);
        }


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

        public List<usp_GetShiftsResult> GetShifts()
        {
            return context.usp_GetShifts().ToList();
        }

        public List<usp_GetHPJobsResult> GetHPJobs(string userId, Guid jobId)
        {
            return context.usp_GetHPJobs(userId, jobId).ToList();
        }

        public void DeleteUserLanguage(Guid id)
        {
            context.usp_DeleteUserLanguage(id);
        }

        public List<usp_GetWorkHistoryJobResponsibilitiesResult> GetWorkHistoryJobResponsibilities(Guid workHistoryId)
        {
            return context.usp_GetWorkHistoryJobResponsibilities(workHistoryId).ToList();
        }

        public void AddWorkHistoryJobResponsibility(Guid workHistoryId, string responsibility)
        {
            context.usp_AddWorkHistoryJobResponsibility(workHistoryId, responsibility);
        }

        public void DeleteWorkHistoryJobResponsibilities(Guid workHistoryId)
        {
            context.usp_DeleteWorkHistoryJobResponbilities(workHistoryId);
        }

        public void AddUserLanguage(string userId, string description, int level)
        {
            context.usp_AddUserLanguage(userId, description, level);
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

        public Guid? AddWorkHistory(Guid userId, string companyName, string companyCity, string companyState, string jobTitle, DateTime? startDate, DateTime? endDate)
        {
            var val = context.usp_AddWorkHistory(userId.ToString(), companyName, companyCity, companyState, jobTitle, startDate, endDate).FirstOrDefault().Column1;
            return val;
        }

        public void UpdateWorkHistory(Guid workHistoryId, string companyName, string companyCity, string companyState, string jobTitle, DateTime? startDate, DateTime? endDate)
        {
            context.usp_UpdateWorkHistory(workHistoryId, companyName, companyCity, companyState, jobTitle, startDate, endDate);
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

        public void UpdateUserDetails(Guid userId, string secondaryEmail, string cellPhone, string personalSummary, string personalInterests, bool disableNotifications, string imageUrl, string videoUrl, int availabilityInDays,
            string onNewMatches = "", bool contactViaPhone = false, bool contactViaEmail = false, bool contactViaSMS = false, int yearsOfExperience = 0, string maxEducation = "", string shiftPreference = "", DateTime? availableOn = null, int? willingnessToTravelMiles = 0)
        {
            context.usp_UpdateUserDetails(userId.ToString(), secondaryEmail, cellPhone, personalSummary, personalInterests, disableNotifications, availabilityInDays, imageUrl, videoUrl, onNewMatches, contactViaPhone,
                contactViaEmail, contactViaSMS, yearsOfExperience, maxEducation, shiftPreference, availableOn, willingnessToTravelMiles);
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


        public List<HPJobDL> GetJobsBySearchParam(string userId, string globalSearchParam, IEnumerable<long> ids, int distance = 0)
        {
            List<HPJobDL> list = new List<HPJobDL>();
            string connectionString = ConfigurationManager.ConnectionStrings["AuthContext"].ToString();
            DataSet ds = ExecuteJobSearchProcedure(connectionString, ids, userId, globalSearchParam);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
                {
                    var item = ds.Tables[0].Rows[i];
                    bool starred = bool.TryParse((item["STARRED"] ?? "0").ToString(), out starred);
                    bool clientInterest = bool.TryParse((item["CLIENT_INTEREST"] ?? "0").ToString(), out clientInterest);
                    bool applied = bool.TryParse((item["Applied"] ?? "0").ToString(), out applied);

                    DateTime? start = null;
                    DateTime? end = null;

                    DateTime s;
                    DateTime e;
                    if (DateTime.TryParse((item["StartDate"] ?? string.Empty).ToString(), out s))
                        start = s;
                    if (DateTime.TryParse((item["EndDate"] ?? string.Empty).ToString(), out e))
                        end = e;

                    list.Add(new HPJobDL
                    {
                        JobId = (Guid)item["JobID"],
                        ClientId = (Guid)item["ClientId"],
                        ClientName = (item["ClientName"] ?? string.Empty).ToString(),
                        ClientAddress = (item["ClientAddress"] ?? string.Empty).ToString(),
                        ClientAddress2 = (item["ClientAddress2"] ?? string.Empty).ToString(),
                        ClientCity = (item["ClientCity"] ?? string.Empty).ToString(),
                        ClientState = (item["ClientState"] ?? string.Empty).ToString(),
                        ClientZip = (item["ClientZip"] ?? string.Empty).ToString(),
                        JobName = (item["Name"] ?? string.Empty).ToString(),
                        JobDescription = (item["Description"] ?? string.Empty).ToString(),
                        EndDate = end,
                        StartDate = start,
                        Starred = starred,
                        ClientInterested = clientInterest,
                        Specialities = (item["Specialties"] ?? string.Empty).ToString(),
                        Applied = applied,
                        SCLMatch = int.Parse((item["SCLMatch"] ?? "0").ToString()),
                        Shifts = (item["Shifts"] ?? string.Empty).ToString()
                    }
                    );
                }
            }

            return list;
        }



        /// <summary>
        /// Need this to send table value param to SQL as unsupported by LinqToSql
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="ids"></param>
        private static DataSet ExecuteJobSearchProcedure(string connectionString, IEnumerable<long> ids, string userId, string globalParam)
        {
            DataSet ds = new DataSet();
            using (SqlCommand cmd = new SqlCommand("dbo.usp_GetJobsBySearchParam", new SqlConnection(connectionString)))
            {
                cmd.CommandText = "dbo.usp_GetJobsBySearchParam";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter;
                parameter = cmd.Parameters.AddWithValue("@SpecialtyIDs", CreateDataTable(ids));

                parameter.SqlDbType = SqlDbType.Structured;
                parameter.TypeName = "dbo.ttSpecialtyIDs";

                cmd.Parameters.AddWithValue("@USER_ID", userId);
                cmd.Parameters.AddWithValue("@GLOBAL_PARAM", globalParam);

                cmd.Connection.Open();
                DataTable table = new DataTable();
                table.Load(cmd.ExecuteReader());
                ds.Tables.Add(table);
            }
            return ds;
        }

        private static DataTable CreateDataTable(IEnumerable<long> ids)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(long));
            foreach (long id in ids)
            {
                table.Rows.Add(id);
            }
            return table;
        }

        private static IEnumerable<SqlDataRecord> CreateSqlDataRecords(IEnumerable<long> ids)
        {
            SqlMetaData[] metaData = new SqlMetaData[1];
            metaData[0] = new SqlMetaData("ID", SqlDbType.BigInt);
            SqlDataRecord record = new SqlDataRecord(metaData);
            foreach (long id in ids)
            {
                record.SetInt64(0, id);
                yield return record;
            }
        }
    }

   
    public class HPJobDL 
    {

        public Guid JobId { get; set; }

        public string JobName { get; set; }

        public string JobDescription { get; set; }

        public string ClientName { get; set; }

        public string ClientAddress { get; set; }

        public string ClientAddress2 { get; set; }

        public string ClientCity { get; set; }

        public string ClientState { get; set; }

        public string ClientZip { get; set; }

        public string Specialities { get; set; }

        public bool Starred { get; set; }

        public bool ClientInterested { get; set; }

        public Guid ClientId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool Applied { get; set; }

        public int SCLMatch { get; set; }

        public string Shifts { get; set; }

    }


}
