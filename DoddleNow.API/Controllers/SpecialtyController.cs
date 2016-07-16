using DataAccessLayer;
using System;
using DoddleNow.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

namespace DoddleNow.API.Controllers
{
    ///<summary>
    ///Client controller.  Used to get client related information across the whole system or an individual
    ///</summary>
    [RoutePrefix("api/v1/specialties")]
    public class SpecialtyController : ApiController
    {
        ///<summary>
        ///Get all specialties
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllSpecialties()
        {
            return Ok(Specialties.GetAllSpecialties());
        }

        ///<summary>
        ///Get specialty with id = id 
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{specialtyId}")]
        [HttpGet]
        public IHttpActionResult GetSpecialty(int specialtyId)
        {
            return Ok(Specialties.GetSpecialty(specialtyId));
        }

        ///<summary>
        ///Add Specialty
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddSpecialty(SpecialtyModel specialtyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Specialties.AddSpecialty(specialtyModel);
            return Ok();
        }

        ///<summary>
        ///Update Specialty with id = id
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{specialtyId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateSpecialty(SpecialtyModel specialtyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Specialties.UpdateSpecialty(specialtyModel);

            return Ok();
        }

        ///<summary>
        ///Delete Specialty
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{specialtyId}")]
        [HttpDelete]
        public IHttpActionResult DeleteSpecialty(int specialtyId)
        {
            Specialties.DeleteSpecialty(specialtyId);
            return Ok();
        }

    }

    #region Helpers

    ///<summary>
    /// Specialties
    ///</summary>
    public class Specialties
    {
        ///<summary>
        ///Get all specialties
        ///</summary>
        public static List<usp_GetSpecialtiesResult> GetAllSpecialties()
        {
            DataAccess da = new DataAccess();
            return da.GetSpecialties(null).ToList();
        }

        /// <summary>
        /// Get specific specialty
        /// </summary>
        /// <param name="specialtyId"></param>
        /// <returns></returns>
        public static usp_GetSpecialtiesResult GetSpecialty(int specialtyId)
        {
            DataAccess da = new DataAccess();
            return da.GetSpecialties(specialtyId).FirstOrDefault();
        }

        /// <summary>
        /// Adds Specialty
        /// </summary>
        /// <param name="specialtyModel"></param>
        /// <returns></returns>
        public static int AddSpecialty(SpecialtyModel specialtyModel)
        {
            DataAccess da = new DataAccess();
            return da.AddSpecialty(specialtyModel.Name, specialtyModel.Description);
        }

        /// <summary>
        /// Update Specialty
        /// </summary>
        /// <param name="specialtyModel"></param>
        /// <returns></returns>
        public static void UpdateSpecialty(SpecialtyModel specialtyModel)
        {
            DataAccess da = new DataAccess();
            da.UpdateSpecialty(specialtyModel.ID, specialtyModel.Name, specialtyModel.Description);
        }

        /// <summary>
        /// Delete Specialty
        /// </summary>
        /// <param name="specialtyId"></param>
        /// <returns></returns>
        public static void DeleteSpecialty(int specialtyId)
        {
            DataAccess da = new DataAccess();
            da.DeleteSpecialty(specialtyId);
        }
    }
    #endregion
}
