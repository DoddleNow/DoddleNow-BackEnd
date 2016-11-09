using DataAccessLayer;
using DoddleNow.API.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Security.Claims;
using System.Web;
using System.Diagnostics;

namespace DoddleNow.API.Controllers
{
    ///<summary>
    ///All account related functions
    ///</summary>
    [RoutePrefix("api/v1/profile")]
    public class ProfileController : ApiController
    {
        private AuthRepository _repo = null;

        ///<summary>
        /// Account related functions
        ///</summary>
        public ProfileController()
        {
            _repo = new AuthRepository();
        }

        //[Authorize(Roles = "6")]
        //[Route("image")]
        //[HttpPost]
        //public Task<HttpResponseMessage> PostFormData()
        //{
        //    // Check if the request contains multipart/form-data.
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    string root = HttpContext.Current.Server.MapPath("~/App_Data");
        //    var provider = new MultipartFormDataStreamProvider(root);

        //    // Read the form data and return an async task.
        //    var task = Request.Content.ReadAsMultipartAsync(provider).
        //        ContinueWith<HttpResponseMessage>(t =>
        //        {
        //            if (t.IsFaulted || t.IsCanceled)
        //            {
        //                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
        //            }

        //            // This illustrates how to get the file names.
        //            foreach (MultipartFileData file in provider.FileData)
        //            {
        //                Trace.WriteLine(file.Headers.ContentDisposition.FileName);
        //                Trace.WriteLine("Server file path: " + file.LocalFileName);
        //            }
        //            return Request.CreateResponse(HttpStatusCode.OK);
        //        });

        //    return task;
        //}


            /// <summary>
            /// Posts image to server, saves to AWS, saves URL to profile
            /// </summary>
            /// <returns></returns>
        //[Authorize(Roles = "6")]
        [AllowAnonymous]
        [Route("image")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostUserImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please upload a file up to 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var filePath = HttpContext.Current.Server.MapPath("~/UserImages/" + postedFile.FileName + extension);
                            postedFile.SaveAs(filePath);

                            // off to AWS

                        //delete file
                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please upload an image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format(ex.ToString());
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }


        ///<summary>
        ///Get Profile based on signed in user's token
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetProfile()
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            return Ok(Profiles.GetProfile(Guid.Parse(userId)));
        }

        ///<summary>
        ///Get Profile based on userId passed in.  this is for a connected client or admin
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{userId}")]
        [HttpGet]
        public IHttpActionResult GetProfile(Guid userId)
        {
            return Ok(Profiles.GetProfile(userId));
        }

        ///<summary>
        ///Get Profile based on signed in user's token
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("overview")]
        [HttpGet]
        public IHttpActionResult GetOverview()
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            return Ok(Profiles.GetOverview(Guid.Parse(userId)));
        }

        ///<summary>
        ///Update user with id = id
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("overview")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateOverview(HPOverview user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.UserId = Guid.Parse(((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value);

            //add additional user info to database
            DataAccessLayer.DataAccess da = new DataAccessLayer.DataAccess();
            da.UpdateUser(user.UserId, 6, user.EMail, user.FirstName, user.LastName, user.Phone, user.Title, user.Department, user.ClientID);
            da.UpdateUserDetails(user.UserId, user.SecondaryEmail, user.CellPhone, user.Title, user.Department, false, user.ImageUrl, user.VideoUrl, user.AvailabilityInDays);

            return Ok();
        }

        #region Image

        ///<summary>
        ///Get Image URL 
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("image/{userId}")]
        [HttpGet]
        public IHttpActionResult GetImage(Guid userId)
        {
            return Ok(Profiles.GetProfile(userId));
        }

        ///<summary>
        ///Get Image URL for user
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("image")]
        [HttpGet]
        public IHttpActionResult GetImage()
        {
            Guid userId = Guid.Parse(((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value);
            return Ok(Profiles.GetProfile(userId));
        }

       
        ///<summary>
        ///Delete image
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("image")]
        [HttpDelete]
        public IHttpActionResult DeleteImage(Guid locationId)
        {
            Guid userId = Guid.Parse(((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value);
            //delete AWS file
            //remove imageUrl
            DataAccess da = new DataAccess();

            da.DeleteLocation(locationId);

            return Ok();
        }



        #endregion

        #region Locations
        ///<summary>
        ///Get locations based on signed in user's token
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("location")]
        [HttpGet]
        public IHttpActionResult GetLocations()
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            return Ok(Profiles.GetLocations(Guid.Parse(userId)));
        }

        ///<summary>
        ///Get locations based on signed in user's token
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("location/{locationId}")]
        [HttpGet]
        public IHttpActionResult GetLocations(Guid locationId)
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            return Ok(Profiles.GetLocations(Guid.Parse(userId)).Where(v => v.ID == locationId).FirstOrDefault());
        }

        ///<summary>
        ///Add location
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("location")]
        [HttpPost]
        public async Task<IHttpActionResult> AddLocation(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //add additional user info to database
            DataAccess da = new DataAccess();
            da.AddLocation(location.UserId, location.AddressTypeId, location.Address, location.Address_2, location.City, location.State, location.ZIP);

            return Ok();
        }

        ///<summary>
        ///Update locations with id = id
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("location/{locationId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateLocation(Guid locationId, Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            location.ID = locationId;
            //add additional user info to database
            DataAccess da = new DataAccess();
            da.UpdateLocation(location.ID, location.AddressTypeId, location.Address, location.Address_2, location.City, location.State, location.ZIP);

            return Ok();
        }

        ///<summary>
        ///Delete location with id = id
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("location/{locationId}")]
        [HttpDelete]
        public IHttpActionResult DeleteLocation(Guid locationId)
        {
            DataAccess da = new DataAccess();
            da.DeleteLocation(locationId);
            return Ok();
        }

        #endregion

        #region Education

        ///<summary>
        ///Get locations based on signed in user's token
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("education")]
        [HttpGet]
        public IHttpActionResult GetEducations()
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            return Ok(Profiles.GetEducations(Guid.Parse(userId)));
        }

        ///<summary>
        ///Get  based on signed in user's token
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("education/{educationId}")]
        [HttpGet]
        public IHttpActionResult GetEducation(Guid educationId)
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            return Ok(Profiles.GetEducations(Guid.Parse(userId)).Where(v => v.ID == educationId).FirstOrDefault());
        }

        ///<summary>
        ///Add 
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("education")]
        [HttpPost]
        public async Task<IHttpActionResult> AddEducation(Education education)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //add additional user info to database
            DataAccess da = new DataAccess();
            da.AddEducation(education.UserId, education.InstitutionName, education.Major, education.StartDate, education.EndDate, education.HighestDegreeEarnedID, education.OtherDegree, education.Graduated, education.GraduationDate);

            return Ok();
        }

        ///<summary>
        ///Update id = id
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("education/{educationId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateEducation(Guid educationId, Education education)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            education.ID = educationId;
            //add additional user info to database
            DataAccess da = new DataAccess();
            da.UpdateEducation(education.ID, education.InstitutionName, education.Major, education.StartDate, education.EndDate, education.HighestDegreeEarnedID, education.OtherDegree, education.Graduated, education.GraduationDate);

            return Ok();
        }

        ///<summary>
        ///Delete  id = id
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("education/{educationId}")]
        [HttpDelete]
        public IHttpActionResult DeleteEducation(Guid educationId)
        {
            DataAccess da = new DataAccess();
            da.DeleteEducation(educationId);
            return Ok();
        }
        #endregion

        #region Certifications

        ///<summary>
        ///Get certs based on signed in user's token
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("certifications")]
        [HttpGet]
        public IHttpActionResult GetCertifications()
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            return Ok(Profiles.GetCertifications(Guid.Parse(userId)));
        }

        ///<summary>
        ///Get  based on signed in user's token
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("certifications/{certificationId}")]
        [HttpGet]
        public IHttpActionResult GetCertification(Guid certificationId)
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            return Ok(Profiles.GetCertifications(Guid.Parse(userId)).Where(v => v.ID == certificationId).FirstOrDefault());
        }

        ///<summary>
        ///Add 
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("certifications")]
        [HttpPost]
        public async Task<IHttpActionResult> AddCertification(Certification cert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //add additional user info to database
            DataAccess da = new DataAccess();
            da.AddCertification(cert.UserId, cert.Name, cert.IssuingBody, cert.IssuanceDate, cert.ExpirationDate);

            return Ok();
        }

        ///<summary>
        ///Update id = id
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("certifications/{certificationId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateCertification(Guid certificationId, Certification cert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            cert.ID = certificationId;
            //add additional user info to database
            DataAccess da = new DataAccess();
            da.UpdateCertification(cert.ID, cert.Name, cert.IssuingBody, cert.IssuanceDate, cert.ExpirationDate);

            return Ok();
        }

        ///<summary>
        ///Delete  id = id
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("certifications/{certificationId}")]
        [HttpDelete]
        public IHttpActionResult DeleteCertification(Guid certificationId)
        {
            DataAccess da = new DataAccess();
            da.DeleteCertification(certificationId);
            return Ok();
        }
        #endregion

        #region References

        ///<summary>
        ///Get refs based on signed in user's token
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("references")]
        [HttpGet]
        public IHttpActionResult GetReferences()
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            return Ok(Profiles.GetReferences(Guid.Parse(userId)));
        }

        ///<summary>
        ///Get  based on signed in user's token
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("references/{referenceId}")]
        [HttpGet]
        public IHttpActionResult GetReference(Guid referenceId)
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            return Ok(Profiles.GetReferences(Guid.Parse(userId)).Where(v => v.ID == referenceId).FirstOrDefault());
        }

        ///<summary>
        ///Add 
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("references")]
        [HttpPost]
        public async Task<IHttpActionResult> AddReference(Reference reference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //add additional user info to database
            DataAccess da = new DataAccess();
            da.AddReference(reference.UserId, reference.Name, reference.Title, reference.DirectSupervisor, reference.ContactPhone);

            return Ok();
        }

        ///<summary>
        ///Update id = id
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("references/{referenceId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateReference(Guid referenceId, Reference reference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            reference.ID = referenceId;
            //add additional user info to database
            DataAccess da = new DataAccess();
            da.UpdateReference(reference.ID, reference.Name, reference.Title, reference.DirectSupervisor, reference.ContactPhone);

            return Ok();
        }

        ///<summary>
        ///Delete  id = id
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("references/{referenceId}")]
        [HttpDelete]
        public IHttpActionResult DeleteReference(Guid referenceId)
        {
            DataAccess da = new DataAccess();
            da.DeleteReference(referenceId);
            return Ok();
        }
        #endregion


        #region WorkHistory

        ///<summary>
        ///Get refs based on signed in user's token
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("workhistory")]
        [HttpGet]
        public IHttpActionResult GetWorkHistories()
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            return Ok(Profiles.GetWorkHistories(Guid.Parse(userId)));
        }

        ///<summary>
        ///Get  based on signed in user's token
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("workhistory/{workHistoryId}")]
        [HttpGet]
        public IHttpActionResult GetWorkHistory(Guid workHistoryId)
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            return Ok(Profiles.GetWorkHistories(Guid.Parse(userId)).Where(v => v.ID == workHistoryId).FirstOrDefault());
        }

        ///<summary>
        ///Add 
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("workhistory")]
        [HttpPost]
        public async Task<IHttpActionResult> AddWorkHistory(WorkHistory wh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //add additional user info to database
            DataAccess da = new DataAccess();
            da.AddWorkHistory(wh.UserId, wh.CompanyName, wh.CompanyCity, wh.CompanyState, wh.JobTitle, wh.JobResponsibilities, wh.StartDate, wh.EndDate);

            return Ok();
        }

        ///<summary>
        ///Update id = id
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("workhistory/{workHistoryId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateWorkHistory(Guid workHistoryId, WorkHistory wh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            wh.ID = workHistoryId;
            //add additional user info to database
            DataAccess da = new DataAccess();
            da.UpdateWorkHistory(wh.ID, wh.CompanyName, wh.CompanyCity, wh.CompanyState, wh.JobTitle, wh.JobResponsibilities, wh.StartDate, wh.EndDate);

            return Ok();
        }

        ///<summary>
        ///Delete  id = id
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("workhistory/{workHistoryId}")]
        [HttpDelete]
        public IHttpActionResult DeleteWorkHistory(Guid workHistoryId)
        {
            DataAccess da = new DataAccess();
            da.DeleteWorkHistory(workHistoryId);
            return Ok();
        }
        #endregion

        #region Languages

        ///<summary>
        ///Get refs based on signed in user's token
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("languages")]
        [HttpGet]
        public IHttpActionResult GetLanguages()
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            return Ok(Profiles.GetLanguages(Guid.Parse(userId)));
        }

        ///<summary>
        ///Get  based on signed in user's token
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("languages/{languageId}")]
        [HttpGet]
        public IHttpActionResult GetLanguage(Guid languageId)
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            return Ok(Profiles.GetLanguages(Guid.Parse(userId)).Where(v => v.ID == languageId).FirstOrDefault());
        }

        ///<summary>
        ///Add 
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("languages")]
        [HttpPost]
        public async Task<IHttpActionResult> AddLanguage(Language lang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //add additional user info to database
            DataAccess da = new DataAccess();
            da.AddUserLanguage(lang.UserId.ToString(), lang.Description);

            return Ok();
        }

       
        ///<summary>
        ///Delete  id = id
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("languages/{languageId}")]
        [HttpDelete]
        public IHttpActionResult DeleteLanguage(Guid languageId)
        {
            DataAccess da = new DataAccess();
            da.DeleteUserLanguage(languageId);
            return Ok();
        }
        #endregion



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }

    #region Helpers

    ///<summary>
    /// Users
    ///</summary>
    public class Profiles
    {
        ///<summary>
        ///Get all clients across DoddleNow
        ///</summary>
        public static List<usp_GetUsersResult> GetAllUsers(int? roleId, Guid? clientGUID)
        {
            DataAccess da = new DataAccess();
            return da.GetUsers(roleId, clientGUID).ToList();
        }

        /// <summary>
        /// Gets overview of profile only
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static HPOverview GetOverview(Guid userId)
        {
            DataAccess da = new DataAccess();
            usp_GetUserResult profile = da.GetUser(userId);

            HPOverview p = new HPOverview()
            {
                CellPhone = profile.CELL_PHONE,
                Department = profile.Department,
                EMail = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                ImageUrl = profile.ImageURL,
                PersonalInterests = profile.PERSONAL_INTERESTS,
                PersonalSummary = profile.PERSONAL_SUMMARY,
                Phone = profile.Phone,
                SecondaryEmail = profile.SECONDARY_EMAIL,
                Title = profile.Title,
                UserId = Guid.Parse(profile.Id),
                VideoUrl = profile.VideoUrl
            };

            return p;
        }

        /// <summary>
        /// Gets profile
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Profile GetProfile(Guid userId)
        {
            DataAccess da = new DataAccess();
            usp_GetUserResult profile = da.GetUser(userId);
            List<Location> locs = GetLocations(userId);
            List<Education> eds = GetEducations(userId);
            List<Certification> certs = GetCertifications(userId);
            List<WorkHistory> wh = GetWorkHistories(userId);
            List<Reference> refs = GetReferences(userId);
            List<Language> languages = GetLanguages(userId);

            HPOverview overview = new HPOverview
            {
                CellPhone = profile.CELL_PHONE,
                Department = profile.Department,
                EMail = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                ImageUrl = profile.ImageURL,
                PersonalInterests = profile.PERSONAL_INTERESTS,
                PersonalSummary = profile.PERSONAL_SUMMARY,
                Phone = profile.Phone,
                SecondaryEmail = profile.SECONDARY_EMAIL,
                Title = profile.Title,
                UserId = Guid.Parse(profile.Id),
                VideoUrl = profile.VideoUrl,
                AvailabilityInDays = profile.AvailabilityInDays.HasValue ? profile.AvailabilityInDays.Value : 0
            };


            Profile p = new Profile
            {
                Overview = overview,
                Locations = locs,
                Educations = eds,
                Certifications = certs,
                WorkHistories = wh,
                References = refs,
                Languages = languages
            };

            return p;
        }

        public static List<Location> GetLocations(Guid userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetLocationsResult> locations = da.GetLocations(userId);

            List<Location> locs = new List<Location>();
            for (int i = 0; i < locations.Count; ++i)
            {
                locs.Add(new Location { ID = locations[i].ID, Address = locations[i].ADDRESS_1, AddressType = locations[i].ADDRESS_TYPE, AddressTypeId = locations[i].ADDRESS_TYPE_ID, Address_2 = locations[i].ADDRESS_2, City = locations[i].CITY, State = locations[i].STATE, UserId = Guid.Parse(locations[i].UserID), ZIP = locations[i].ZIP});
            }

            return locs;
        }

        public static List<Education> GetEducations(Guid userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetEducationsResult> educations = da.GetEducations(userId);

            List<Education> eds = new List<Education>();
            for (int i = 0; i < educations.Count; ++i)
            {
                eds.Add(new Education { UserId = userId, EndDate = educations[i].EndDate, Graduated = educations[i].Graduated, GraduationDate = educations[i].GraduationDate, ID = educations[i].ID, HighestDegreeEarnedID = educations[i].HighestDegreeEarnedID.Value, InstitutionName = educations[i].InstitutionName, Major = educations[i].Major, OtherDegree = educations[i].OtherDegree, StartDate = educations[i].StartDate });
            }

            return eds;
        }

        public static List<Certification> GetCertifications(Guid userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetCertificationsResult> certifications = da.GetCertifications(userId);

            List<Certification> certs = new List<Certification>();
            for (int i = 0; i < certifications.Count; ++i)
            {
                certs.Add(new Certification { UserId = userId, ExpirationDate = certifications[i].ExpirationDate, ID = certifications[i].ID, Name=certifications[i].Name, IssuanceDate = certifications[i].IssuanceDate, IssuingBody = certifications[i].IssuingBody });
            }

            return certs;
        }

        public static List<WorkHistory> GetWorkHistories(Guid userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetWorkHistoriesResult> wh = da.GetWorkHistories(userId);

            List<WorkHistory> whs = new List<WorkHistory>();
            for (int i = 0; i < wh.Count; ++i)
            {
                whs.Add(new WorkHistory { UserId = userId, CompanyCity = wh[i].CompanyCity, CompanyName = wh[i].CompanyName, CompanyState = wh[i].CompanyState, EndDate = wh[i].EndDate, ID = wh[i].ID, JobResponsibilities = wh[i].JobResponsibilities, JobTitle = wh[i].JobTitle, StartDate = wh[i].StartDate });
            }

            return whs;
        }

        public static List<Reference> GetReferences(Guid userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetReferencesResult> r = da.GetReferences(userId);

            List<Reference> refs = new List<Reference>();
            for (int i = 0; i < r.Count; ++i)
            {
                refs.Add(new Reference { UserId = userId, ContactPhone = r[i].ContactPhone, DirectSupervisor = r[i].DirectSupervisor, ID = r[i].ID, Name = r[i].Name, Title = r[i].Title });
            }

            return refs;
        }

        public static List<Language> GetLanguages(Guid userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetUserLanguagesResult> r = da.GetUserLanguages(userId.ToString());

            List<Language> langs = new List<Language>();
            for (int i = 0; i < r.Count; ++i)
            {
                langs.Add(new Language { UserId = userId, Description = r[i].Description, ID = r[i].ID});
            }

            return langs;
        }


        ///<summary>
        ///Delete user
        ///</summary>
        public static void DeleteUser(string userId)
        {
            DataAccess da = new DataAccess();
            da.DeleteUser(userId);
        }
    }
    #endregion
}
