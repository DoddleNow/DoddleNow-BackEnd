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
        public enum  TemplateType { 
            Evaluation = 1,
            Skillschecklist = 2,
            Resume = 3
        }
        DataClasses1DataContext context = new DataClasses1DataContext(ConfigurationManager.ConnectionStrings["DoddleNowConnectionString"].ToString());

        public void DeleteUserDocument(int userDocumentId)
        {
            context.usp_DeleteUserDocument(userDocumentId);
        }

        
        public List<usp_GetClinicalGroupUsersResult> GetClinicalGroupUsers(int clinicalGroupId)
        {
            return context.usp_GetClinicalGroupUsers(clinicalGroupId).ToList();
        }

        public usp_GetSchoolGroupDataResult GetSchoolGroupData(int schoolId)
        {
            return context.usp_GetSchoolGroupData(schoolId).FirstOrDefault();
        }

        public List<usp_GetCurrentShiftHoursRequiredAndCompleteResult> GetCurrentShiftHoursRequiredAndComplete(int schoolId)
        {
            return context.usp_GetCurrentShiftHoursRequiredAndComplete(schoolId).ToList();
        }

        public List<usp_GetSurveySignatureAndNotesResult> GetSurveySignatureAndNotes(int surveyId, int clinicalGroupId, int userId, int evaluatedId)
        {
            return context.usp_GetSurveySignatureAndNotes(surveyId, clinicalGroupId, userId, evaluatedId).ToList();
        }

        public List<rpt_AdHocClinicalGroupsResult> GetAdhocClinicalGroups(int schoolId)
        {
            return context.rpt_AdHocClinicalGroups(schoolId).ToList();
        }

        public usp_GetSchoolClinicalAggregateResult GetSchoolClinicalAggregate(int schoolId)
        {
            return context.usp_GetSchoolClinicalAggregate(schoolId).FirstOrDefault();
        }

        public void AddSurveySignature(int surveyId, int clinicalGroupId, int userId, int evaluatedId, string notes, string signature, DateTime signatureDate, string ipAddress)
        {
            context.usp_AddSurveySignature(surveyId, clinicalGroupId, userId, evaluatedId, notes, signature, signatureDate, ipAddress);
        }

        public void DeleteUserDocumentImage(int userDocumentImageId)
        {
            context.usp_DeleteUserDocumentImage(userDocumentImageId);
        }

        public List<usp_GetFacilityAdminsResult> GetFacilityAdmins(int facilityId)
        {
            return context.usp_GetFacilityAdmins(facilityId).ToList();
        }

        public List<usp_GetUserDocumentImagesResult> GetUserDocumentImages(int userDocumentId)
        {
            return context.usp_GetUserDocumentImages(userDocumentId).ToList();
        }

        public List<usp_GetStudentsDocumentsByTypeResult> GetStudentsDocumentsByType(int clinicalGroupId, int documentTypeId)
        {
            return context.usp_GetStudentsDocumentsByType(clinicalGroupId, documentTypeId).ToList();
        }

        public List<usp_GetStudentDocumentsByClinicalGroupResult> GetStudentDocumentsByClinicalGroup(int studentId, int clinicalGroupId)
        {
            return context.usp_GetStudentDocumentsByClinicalGroup(studentId, clinicalGroupId).ToList();
        }

        public int GetAdminFacility(int adminUserId)
        {
            return context.usp_GetAdminFacility(adminUserId).FirstOrDefault().FACILITY_ID;
        }

        public void DeleteFacilityAdmin(int facilityId, int adminId)
        {
            context.usp_DeleteFacilityAdmin(facilityId, adminId);
        }

        public int AddUserDocument(int userId, int documentTypeId, string iP_ADDRESS, bool permission)
        {
            return context.usp_AddUserDocument(userId, documentTypeId, iP_ADDRESS, permission);
        }

        public void AddUserDocumentValue(int userDocumentId, int documentMetadataTypeId, string value)
        {
            context.usp_AddUserDocumentValue(userDocumentId, documentMetadataTypeId, value);
        }

        public List<usp_GetCompaniesResult> GetCompanies(int? companyId)
        {
            return context.usp_GetCompanies(companyId).ToList();
        }

        public List<usp_GetStudentDocumentsResult> GetStudentDocuments(int studentId)
        {
            return context.usp_GetStudentDocuments(studentId).ToList();
        }

        public int AddUserDocumentImage(int userDocumentId, string AWSKey)
        {
            int returnValue = context.usp_AddUserDocumentImage(userDocumentId, AWSKey).FirstOrDefault().Column1.HasValue ? context.usp_AddUserDocumentImage(userDocumentId, AWSKey).FirstOrDefault().Column1.Value : 0;
            return returnValue;
        }

        public void UpdateUserDocumentImage(int userDocumentImageId, string AWSKey)
        {
            context.usp_UpdateUserDocumentImage(userDocumentImageId, AWSKey);
        }

        public List<usp_GetUserDocumentMetadataWithValuesResult> GetUserDocumentMetaDataWithValues(int userDocumentId)
        {
            return context.usp_GetUserDocumentMetadataWithValues(userDocumentId).ToList();
        }

        public List<usp_GetMissingRequirementsByStudentIDResult> GetMissingRequirementsByStudentID(int studentId)
        {
            return context.usp_GetMissingRequirementsByStudentID(studentId).ToList();
        }

        public void DeleteClinicalGroupRequirement(int requirementId)
        {
            context.usp_DeleteClinicalGroupRequirement(requirementId);
        }

        public List<usp_GetClinicalGroupRequirementsResult> GetClinicalGroupRequirements(int clinicalGroupId)
        {
            return context.usp_GetClinicalGroupRequirements(clinicalGroupId).ToList();
        }

        public void AddClinicalGroupRequirement(int clinicalGroupId, int requirementTypeId, int documentTypeId, int createdByUserId, string roles)
        {
            context.usp_AddClinicalGroupRequirement(clinicalGroupId, requirementTypeId, documentTypeId, createdByUserId, roles);
        }

        public List<usp_GetDocumentFieldTypesResult> GetDocumentFieldTypes()
        {
            return context.usp_GetDocumentFieldTypes().ToList();
        }

        public List<usp_GetDocumentMetadataTypesResult> GetDocumentMetadataTypes(int documentTypeId)
        {
            return context.usp_GetDocumentMetadataTypes(documentTypeId).ToList();
        }

        public void DeleteDocumentMetadataType(int documentMetadataTypeId)
        {
            context.usp_DeleteDocumentMetadataType(documentMetadataTypeId);
        }

        public void UpdateDocumentMetadataType(int documentMetadataTypeId, int documentTypeId, string name, string description, int documentFieldTypeId)
        {
            context.usp_UpdateDocumentMetadataType(documentMetadataTypeId, documentTypeId, name, description, documentFieldTypeId);
        }

        public void AddDocumentMetadataType(int documentTypeId, string name, string description, int documentFieldTypeId)
        {
            context.usp_AddDocumentMetadataType(documentTypeId, name, description, documentFieldTypeId);
        }

        public List<usp_GetDocumentTypesResult> GetDocumentTypes(int schoolId)
        {
            return context.usp_GetDocumentTypes(schoolId).ToList();
        }

        public int AddDocumentType(int schoolId, string name, string description, string awsKey, bool global)
        {
            return context.usp_AddDocumentType(schoolId, name, description, awsKey, global, string.Empty).FirstOrDefault().Column1.Value;
        }

        public int AddDocumentType(int schoolId, string name, string description, string awsKey, bool global, string roles)
        {
            return context.usp_AddDocumentType(schoolId, name, description, awsKey, global, roles).FirstOrDefault().Column1.Value;
        }

        public void DeleteDocumentType(int documentTypeId)
        {
            context.usp_DeleteDocumentType(documentTypeId);
        }

        public void UpdateDocumentType(int documentTypeId, string name, string description, string awsKey, bool global, string roles)
        {
            context.usp_UpdateDocumentType(documentTypeId, name, description, awsKey, global, roles);
        }
        
        public void AddAnswer(int surveyId, int userId, int evaluatedId, string surveyQuestionGuid, string value)
        {
            context.usp_AddAnswer(surveyId, userId, evaluatedId, new Guid(surveyQuestionGuid), value);
        }

        public List<usp_GetQuestionsWithAnswersResult> GetQuestionsWithAnswers(string surveyGuid, int evaluatedId)
        {
            return context.usp_GetQuestionsWithAnswers(new Guid(surveyGuid), evaluatedId).ToList();
        }

        public List<usp_GetClinicalFormativesByStudentResult> GetClinicalFormativesByStudent(int clinicalGroupId, int studentId)
        {
            return context.usp_GetClinicalFormativesByStudent(clinicalGroupId, studentId).ToList();
        }

        public List<usp_GetSummativeSummaryResult> GetSummativeSummary(int? clinicalGroupId, int? instructorId)
        {
            return context.usp_GetSummativeSummary(clinicalGroupId, instructorId).ToList();
        }

        public List<usp_GetInstructorsBySchoolResult> GetInstructorsBySchool(int schoolId)
        {
            return context.usp_GetInstructorsBySchool(schoolId).ToList();
        }

        public usp_GetShiftInstructorResult GetShiftInstructor(int shiftId)
        {
            return context.usp_GetShiftInstructor(shiftId).FirstOrDefault();
        }

        public List<usp_GetClinicalSummativesResult> GetClinicalSummatives(int clinicalGroupId, int? studentId)
        {
            return context.usp_GetClinicalSummatives(clinicalGroupId, studentId).ToList();
        }

        public void DeleteClinicalSummative(int clinicalSummativeId)
        {
            context.usp_DeleteClinicalSummative(clinicalSummativeId);
        }

        public void UpdateClinicalSummative(int? clinicalSummativeId, int clinicalGroupId, int studentId, int createdBy, int status, string statusSummary, bool studentNotified)
        {
            context.usp_UpdateClinicalSummative(clinicalSummativeId, clinicalGroupId, studentId, createdBy,  status, statusSummary, studentNotified);  
        }

        public void AddUserRoleWithSchool(int userId, int roleId, int schoolId)
        {
            AspNetUserRoles2 ur = new AspNetUserRoles2();
            ur.RoleId = roleId;
            ur.UserId = userId;
            ur.SCHOOL_ID = schoolId;


            context.AspNetUserRoles2s.InsertOnSubmit(ur);
            context.SubmitChanges();
        }

        public List<usp_GetEvaluationAssignmentsResult> GetEvaluationAssignments(int surveyId)
        {
            return context.usp_GetEvaluationAssignments(surveyId).ToList();
        }

        public void AddLoginActivity(int userId, string IPAddress)
        {
            context.usp_AddLoginActivity(userId, IPAddress);
        }

        public List<usp_GetClinicalGroupPreceptorsResult> GetClinicalGroupPreceptors(int clinicalGroupId)
        {
            return context.usp_GetClinicalGroupPreceptors(clinicalGroupId).ToList();
        }

        public List<usp_GetFacilityPreceptorsResult> GetFacilityPreceptors(int facilityId)
        {
            return context.usp_GetFacilityPreceptors(facilityId).ToList();
        }

        public List<usp_GetFacilityAdminsBySchoolResult> GetFacilityAdminsBySchool(int schoolId)
        {
            return context.usp_GetFacilityAdminsBySchool(schoolId).ToList();
        }

        public void AddFacilityAdmin(int facilityId, int adminId)
        {
            context.usp_AddFacilityAdmin(facilityId, adminId);
        }

        public void AddFacilityPreceptor(int facilityId, int preceptorId)
        {
            context.usp_AddFacilityPreceptor(facilityId, preceptorId);
        }

        public List<usp_GetPreceptorsBySchoolResult> GetPreceptorsBySchool(int schoolId)
        {
            return context.usp_GetPreceptorsBySchool(schoolId).ToList();
        }

        public void DeleteFacilityPreceptor(int facilityId, int preceptorId)
        {
            context.usp_DeleteFacilityPreceptor(facilityId, preceptorId);
        }

        public void AddSurveyFromTemplate(int surveyTemplateID, int schoolID, bool template)
        {
            context.usp_AddSurveyFromTemplate(surveyTemplateID, schoolID, template);
        }

        public void CopyQuestion(Guid surveyQuestionGuid, int surveyId)
        {
            context.usp_CopyQuestion(surveyQuestionGuid, surveyId);
        }

        public List<usp_GetTemplatesResult> GetTemplates(int surveyTypeID)
        {
            return context.usp_GetTemplates(surveyTypeID).ToList();
        }

        public void UpdateSurveyQuestionOrder(int surveyID, Guid questionGUID, int position)
        {
            context.usp_UpdateSurveyQuestionOrder(surveyID, questionGUID, position);
        }

        public void DeleteQuestionDetails(int questionID)
        {
            context.usp_DeleteQuestionDetails(questionID);
        }

        public int UpdateQuestion(Guid surveyGUID, int questionID, string text, string questionType, bool required, int position)
        {
            return context.usp_UpdateQuestion(surveyGUID, questionID, text, questionType, required, position).FirstOrDefault().Column1.Value;
        }

        public usp_GetSurveyResult GetSurvey(Guid surveyGUID)
        {
            return context.usp_GetSurvey(surveyGUID).FirstOrDefault();
        }

        public List<usp_GetQuestionsResult> GetQuestions(Guid surveyGUID)
        {
            return context.usp_GetQuestions(surveyGUID).ToList();
        }

        public List<usp_GetQuestionDetailsResult> GetQuestionDetails(int questionId)
        {
            return context.usp_GetQuestionDetails(questionId).ToList();
        }

        public usp_AddQuestionResult AddQuestion(Guid surveyGUID, string text, string questionType, bool required)
        {
            return context.usp_AddQuestion(surveyGUID, text, questionType, required).FirstOrDefault();
        }

        public void AddQuestionDetail(int questionId, string text)
        {
            context.usp_AddQuestionDetail(questionId, text);
        }

        public void DeleteQuestion(Guid surveyGUID, Guid surveyQuestionGUID)
        {
            context.usp_DeleteQuestion(surveyGUID, surveyQuestionGUID);
        }

        public void DeleteQuestionDetail(int questionDetailId, Guid surveyGUID)
        {
            context.usp_DeleteQuestionDetail(questionDetailId, surveyGUID);
        }

        public List<usp_GetEvaluationTemplatesResult> GetEvaluationTemplates(int schoolID)
        {
            return context.usp_GetEvaluationTemplates(schoolID).ToList();
        }

        public List<usp_GetEvaluationsResult> GetEvaluations(int? surveyID, int? schoolID, int? clinicalGroupID, bool template)
        {
            return context.usp_GetEvaluations(surveyID, schoolID, clinicalGroupID, template).ToList();
        }

        public void AddFacility(int schoolID, string Name, string Description, DateTime EFFDT, int AddressTypeID, string Address1, string Address2, string City, string State, string Zip)
        {
            context.usp_AddFacility(schoolID, Name, Description, EFFDT, AddressTypeID, Address1, Address2, City, State, Zip);
        }

        public void UpdateFacility(int facilityId, int schoolID, string Name, string Description, DateTime EFFDT, int AddressTypeID, string Address1, string Address2, string City, string State, string Zip)
        {
            context.usp_UpdateFacility(facilityId, schoolID, Name, Description, EFFDT, AddressTypeID, Address1, Address2, City, State, Zip);
        }

        public void AddSurveyAssignment(int surveyId, int userId)
        {
            context.usp_AddSurveyAssignment(userId, surveyId);
        }

        public void DeleteFacility(int facilityId)
        {
            context.usp_DeleteFacility(facilityId);
        }

        public int AddEvaluation(int schoolID, int? surveyID, int? clinicalGroupID, bool? active, string title, string description, int? templateId, bool template)
        {
            return context.usp_AddEvaluation(schoolID, surveyID, clinicalGroupID, active, title, description, template, 1, templateId);
        }

        public void UpdateEvaluation(int surveyID, int? clinicalGroupID, bool? active, string title, string description, bool? notified)
        {
            context.usp_UpdateEvaluation(surveyID, clinicalGroupID, active, title, description, notified);
        }

        public void DeleteEvaluation(int surveyID)
        {
            context.usp_DeleteEvaluation(surveyID);
        }

        public void UpdateTask(int taskId, DateTime? archiveDate, DateTime? readDate)
        {
            context.usp_UpdateTask(taskId, archiveDate, readDate);
        }

        public List<usp_GetUserTasksResult> GetUserTasks(int userId)
        {
            return context.usp_GetUserTasks(userId).ToList();
        }

        public void AddUserTask(int taskTemplateId, int userId)
        {
            context.usp_AddUserTask(taskTemplateId, userId);
        }

        public List<usp_GetShiftsByStudentIDResult> GetShiftsByStudentID(int studentId)
        {
            return context.usp_GetShiftsByStudentID(studentId).ToList();
        }

        public List<usp_GetShiftsbyClinicalGroupTypeResult> GetShiftsByClinicalGroupType(int? clinicalGroupTypeId, int schoolId)
        {
            return context.usp_GetShiftsbyClinicalGroupType(clinicalGroupTypeId, schoolId).ToList();
        }

        public List<usp_GetShiftsbyStudentClinicalGroupTypeResult> GetShiftsByStudentClinicalGroupType(int studentId, int? clinicalGroupTypeId)
        {
            return context.usp_GetShiftsbyStudentClinicalGroupType(studentId, clinicalGroupTypeId).ToList();
        }

        public List<usp_GetClinicalGroupsResult> GetClinicalGroups(int schoolId, int? instructorId)
        {
            return context.usp_GetClinicalGroups(schoolId, instructorId).ToList();
        }

        public void UpdateTerm(int termId, string Description)
        {
            context.usp_UpdateTerm(termId, Description);
        }

        public void DeleteTerm(int termId)
        {
            context.usp_DeleteTerm(termId);
        }

        public List<usp_GetTermsResult> GetTerms(int schoolId)
        {
            return context.usp_GetTerms(schoolId).ToList();
        }

        public void AddTerm(int schoolId, string Description)
        {
            context.usp_AddTerm(schoolId, Description);
        }


        public List<usp_GetClinicalGroupsEvaluationsResult> GetClinicalGroupsEvaluations(int schoolId)
        {
            return context.usp_GetClinicalGroupsEvaluations(schoolId).ToList();
        }

        public List<usp_GetClinicalGroupTypesResult> GetClinicalGroupTypes(int schoolId)
        {
            return context.usp_GetClinicalGroupTypes(schoolId).ToList();
        }

        public List<usp_GetClinicalGroupTypesbyStudentResult> GetClinicalGroupTypesbyStudent(int studentId)
        {
            return context.usp_GetClinicalGroupTypesbyStudent(studentId).ToList();
        }

        public List<usp_GetShiftStudentsResult> GetShiftStudents(int shiftId)
        {
            return context.usp_GetShiftStudents(shiftId).ToList();
        }

        public List<usp_GetInstructorClinicalGroupsResult> GetInstructorClinicalGroups(int instructorId)
        {
            return context.usp_GetInstructorClinicalGroups(instructorId).ToList();
        }

        public List<usp_GetPreceptorClinicalGroupsResult> GetPreceptorClinicalGroups(int preceptorId)
        {
            return context.usp_GetPreceptorClinicalGroups(preceptorId).ToList();
        }

        public List<usp_GetPreceptorSummativeSummaryResult> GetPreceptorSummativeSummary(int? clinicalGroupId, int? preceptorId)
        {
            return context.usp_GetPreceptorSummativeSummary(clinicalGroupId, preceptorId).ToList();
        }

        public List<usp_GetStudentClinicalGroupsResult> GetStudentClinicalGroups(int studentId)
        {
            return context.usp_GetStudentClinicalGroups(studentId).ToList();
        }

        public List<usp_FindStudentsResult> FindStudents(string firstName, string lastName, int schoolId, int clinicalGroupId)
        {
            List<usp_FindStudentsResult> items = context.usp_FindStudents(firstName, lastName, schoolId, clinicalGroupId).ToList();
            return items;
        }

        public List<usp_GetFacilitiesResult> GetFacilities(int schoolId)
        {
            return context.usp_GetFacilities(schoolId).ToList();
        }

        public usp_GetSchoolResult GetSchool(int schoolId)
        {
            return context.usp_GetSchool(schoolId).FirstOrDefault();
        }

        public List<usp_GetSchoolsResult> GetSchools()
        {
            return context.usp_GetSchools().ToList();
        }

        public void UpdateSchool(int SchoolID, string Name, string Description, string Website, DateTime EFFDT, string CampusLocation, string Timezone)
        {
            context.usp_UpdateSchool(SchoolID, Name, Description, Website, EFFDT, CampusLocation, Timezone);
        }

        public int? AddSchool(string Name, string Description, string Website, DateTime EFFDT, string CampusLocation, string Timezone)
        {
            return context.usp_AddSchool(Name, Description, Website, EFFDT, CampusLocation, Timezone).FirstOrDefault().SCHOOL_ID;
        }

        public void DeleteSchool(int SchoolID)
        {
            context.usp_DeleteSchool(SchoolID);
        }

        public void AddClinicalGroup(int clinicalGroupTypeID, int facilityID, string specialInstructions, DateTime? startDate, DateTime? endDate, DateTime eFFDT, int unitID, decimal hoursRequired, int termId, bool precepted)
        {
            context.usp_AddClinicalGroup(clinicalGroupTypeID, facilityID, specialInstructions, startDate, endDate, eFFDT, unitID, hoursRequired, termId, precepted);
        }

        public void AddClinicalGroupStudents(int clinicalGroupID, int studentID, int StudentStatusID, int OutcomeID)
        {
            context.usp_AddClinicalGroupStudents(clinicalGroupID, studentID, StudentStatusID, OutcomeID);
        }

        public void AddClinicalGroupInstructors(int clinicalGroupID, int clinicalGroupInstructorID)
        {
            context.usp_AddClinicalGroupInstructors(clinicalGroupID, clinicalGroupInstructorID);
        }

        public void AddClinicalGroupPreceptors(int clinicalGroupID, int clinicalGroupPreceptorID)
        {
            context.usp_AddClinicalGroupPreceptors(clinicalGroupID, clinicalGroupPreceptorID);
        }

        public void UpdateClinicalGroup(int clinicalGroupID, int clinicalGroupTypeID, int facilityID, string specialInstructions, DateTime startDate, DateTime endDate, DateTime eFFDT, int unitID, decimal hoursRequired, int termId, bool precepted)
        {
            context.usp_UpdateClinicalGroup(clinicalGroupID, clinicalGroupTypeID, facilityID, specialInstructions, startDate, endDate, eFFDT, unitID, hoursRequired, termId, precepted);
        }

        public List<usp_GetUnitsResult> GetUnits(int facilityId)
        {
            return context.usp_GetUnits(facilityId).ToList();
        }

        public usp_GetStudentHoursResult GetStudentHours(int studentId)
        {
            return context.usp_GetStudentHours(studentId).FirstOrDefault();
        }


        public usp_GetStudentRequiredHoursResult GetStudentRequiredHours(int studentId)
        {
            return context.usp_GetStudentRequiredHours(studentId).FirstOrDefault();
        }

        public usp_GetStudentScheduledHoursResult GetStudentScheduledHours(int studentId)
        {
            return context.usp_GetStudentScheduledHours(studentId).FirstOrDefault();
        }

        public usp_GetStudentCompletedHoursResult GetStudentCompletedHours(int studentId)
        {
            return context.usp_GetStudentCompletedHours(studentId).FirstOrDefault();
        }

        public usp_GetClinicalGroupResult GetClinicalGroup(int clinicalGroupId)
        {
            return context.usp_GetClinicalGroup(clinicalGroupId).FirstOrDefault();
        }

        public List<usp_GetClinicalGroupStudentsResult> GetClinicalGroupStudents(int clinicalGroupId)
        {
            return context.usp_GetClinicalGroupStudents(clinicalGroupId).ToList();
        }

        public List<usp_GetClinicalGroupInstructorsResult> GetClinicalGroupInstructors(int clinicalGroupId)
        {
            return context.usp_GetClinicalGroupInstructors(clinicalGroupId).ToList();
        }

        public List<usp_GetStudentsBySchoolResult> GetStudentsBySchool(int SchoolId)
        {
            return context.usp_GetStudentsBySchool(SchoolId).ToList();
        }

        

        public int GetUserSchool(int userId)
        {
            usp_GetUserSchoolResult res = context.usp_GetUserSchool(userId).FirstOrDefault();
            return res.SCHOOL_ID;
        }

        public usp_GetShiftResult GetShift(int shiftId)
        {
            return context.usp_GetShift(shiftId).FirstOrDefault();
        }

        public List<usp_GetShiftsResult> GetShifts(int clinicalGroupId)
        {
            return context.usp_GetShifts(clinicalGroupId).ToList();
        }

        public void UpdateShift(int shiftId, int clinicalGroupId, DateTime startDate, DateTime endDate, int creditHours, string Notes)
        {
            context.usp_UpdateShift(shiftId, clinicalGroupId, startDate, endDate, creditHours, Notes);
        }

        public void AddShift(int clinicalGroupId, int facilityId, int? unitId, DateTime startDate, DateTime endDate, int creditHours, string Notes)
        {
            context.usp_AddShift(clinicalGroupId, facilityId, unitId, startDate, endDate, creditHours, Notes);
        }

        public void DeleteShift(int shiftId)
        {
            context.usp_DeleteShift(shiftId);
        }

        public void DeleteShiftStudents(int ShiftId, int studentId)
        {
            context.usp_DeleteShiftStudents(ShiftId, studentId);
        }

        public void UpdateShiftStudents(int shiftId, int studentId, int Hours, string Notes, bool alternative, string roomNo, string diagnosis, string diagnosisComments, bool studentNotified, bool studentConfirmed)
        {
            context.usp_UpdateShiftStudents(shiftId, studentId, Hours, Notes, alternative, roomNo, diagnosis, diagnosisComments, studentNotified, studentConfirmed);
        }

        public void DeleteClinicalGroup(int clinicalGroupId)
        {
            context.usp_DeleteClinicalGroup(clinicalGroupId);
        }

        public void DeleteClinicalGroupInstructor(int clinicalGroupId, int clinicalGroupInstructorID)
        {
            context.usp_DeleteClinicalGroupInstructor(clinicalGroupId, clinicalGroupInstructorID);
        }

        public void DeleteClinicalGroupPreceptor(int clinicalGroupId, int clinicalGroupPreceptorID)
        {
            context.usp_DeleteClinicalGroupPreceptor(clinicalGroupId, clinicalGroupPreceptorID);
        }

        public void DeleteClinicalGroupStudent(int clinicalGroupId, int studentId)
        {
            context.usp_DeleteClinicalGroupStudent(clinicalGroupId, studentId);

        }

        public void UpdateUserDetails(int userId, bool disableNotifications)
        {
            var details = (from p in context.USER_DETAILs where p.USER_ID == userId select p).SingleOrDefault();

            if (details == null)
            { 
                //need to add the record
                USER_DETAIL ud = new USER_DETAIL();
                ud.USER_ID = userId;
                ud.DISABLE_NOTIFICATIONS = disableNotifications;
                context.USER_DETAILs.InsertOnSubmit(ud);
            }
            if (details != default(USER_DETAIL))
            {
                //update
                details.DISABLE_NOTIFICATIONS = disableNotifications;
            }
            context.SubmitChanges();
        }

        public USER_DETAIL GetUserDetail(int userId)
        {
            var detail = (from p in context.USER_DETAILs where p.USER_ID == userId select p).SingleOrDefault();
            return detail;
        }

        public void UpdateUserPhoto(int userId, Byte[] img)
        {
            var photo = (from p in context.USER_DETAILs where p.USER_ID == userId select p).SingleOrDefault();

            if (photo != default(USER_DETAIL))
            {
                //update
                photo.PROFILE_IMAGE = img;
            }
            else
            {
                //insert
                USER_DETAIL ud = new USER_DETAIL();
                ud.USER_ID = userId;
                ud.PROFILE_IMAGE = img;
                context.USER_DETAILs.InsertOnSubmit(ud);
            }
            context.SubmitChanges();
        }


    }


}
