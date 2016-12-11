using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccessLayer;
using DoddleNow.API.Models;
using System.Threading.Tasks;
using DoddleNow.API.Utility;

namespace DoddleNow.API.Controllers
{
    ///<summary>
    ///Roles controller.  Used to get role related information across the whole system or an individual
    ///</summary>
    [RoutePrefix("api/v1/Insights")]
    public class InsightsController : ApiController
    {
        ///<summary>
        ///Get all market insights (global)
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}")]
        [Route("{availability:int}/{experience:int}/{scl:int}/{education:int}/{shift:int}")]
        public IHttpActionResult GetInsights(Guid clientId, int availability, int experience, int scl, int education, int shift)
        {
            return Ok(Insights.GetAllInsights(clientId, availability, experience, scl, education, shift));
        }

        ///<summary>
        ///Get market insights for specialty
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}/specialty/{specialtyId}")]
        [Route("{availability:int}/{experience:int}/{scl:int}/{education:int}/{shift:int}")]
        [Route("{perPage:int}/{page:int}/{orderBy:alpha?}")]
        public IHttpActionResult GetSpecialtyInsights(Guid clientId, int specialtyId, int availability, int experience, int scl, int education, int shift, int perPage = 1000, int page = 1, string orderBy = "", string sort = "asc", string filter = "")
        {
            Insight item = Insights.GetSpecialtyInsights(clientId, specialtyId, availability, experience, scl, education, shift);

             //only allow orderby on these
            if (orderBy.Length > 0 && !(orderBy.ToUpper().Contains("AVAILABLEON") || orderBy.ToUpper().Contains("EDUCATION") || orderBy.ToUpper().Contains("LOCATION") || orderBy.ToUpper().Contains("EXPERIENCE") || orderBy.ToUpper().Contains("SCLMATCH") ))
            {
                orderBy = string.Empty;
            }

            //count of items returned after filter and total pages
            var totalCount = item.PotentialCandidates.Count();
            var totalPages = Math.Ceiling((double)totalCount / perPage);

            if (QueryHelper.PropertyExists<HPJob>(orderBy))
            {
                ///var orderByExpression = QueryHelper.GetPropertyExpression<DataAccessLayer.DL>(orderBy);

                //need major refactor.  HPJobDL won't allow the orderByExpression so have to do a nasty if/else
                if (sort.ToUpper() == "ASC" || sort == string.Empty)
                {
                    if (orderBy.ToUpper() == "AVAILABLEON")
                        item.PotentialCandidates = item.PotentialCandidates.OrderBy(c => c.AvailableOn).ToList();
                    else if (orderBy.ToUpper() == "EDUCATION")
                        item.PotentialCandidates = item.PotentialCandidates.OrderBy(c => c.Education).ToList();
                    else if (orderBy.ToUpper() == "LOCATION")
                        item.PotentialCandidates = item.PotentialCandidates.OrderBy(c => c.Location).ToList();
                    else if (orderBy.ToUpper() == "EXPERIENCE")
                        item.PotentialCandidates = item.PotentialCandidates.OrderBy(c => c.Experience).ToList();
                    else if (orderBy.ToUpper() == "SCLMATCH")
                        item.PotentialCandidates = item.PotentialCandidates.OrderBy(c => c.SCLMatch).ToList();
                }
                else
                {
                    if (orderBy.ToUpper() == "AVAILABLEON")
                        item.PotentialCandidates = item.PotentialCandidates.OrderByDescending(c => c.AvailableOn).ToList();
                    else if (orderBy.ToUpper() == "EDUCATION")
                        item.PotentialCandidates = item.PotentialCandidates.OrderByDescending(c => c.Education).ToList();
                    else if (orderBy.ToUpper() == "LOCATION")
                        item.PotentialCandidates = item.PotentialCandidates.OrderByDescending(c => c.Location).ToList();
                    else if (orderBy.ToUpper() == "EXPERIENCE")
                        item.PotentialCandidates = item.PotentialCandidates.OrderByDescending(c => c.Experience).ToList();
                    else if (orderBy.ToUpper() == "SCLMATCH")
                        item.PotentialCandidates = item.PotentialCandidates.OrderByDescending(c => c.SCLMatch).ToList();
                }
            }
            else
            {
                item.PotentialCandidates = item.PotentialCandidates.OrderBy(c => c.AvailableOn).ToList();
            }

            var candidates = item.PotentialCandidates.Skip((page - 1) * perPage)
                                    .Take(perPage)
                                    .ToList();

            item.PotentialCandidates = candidates;

            var result = new
            {
                totalCount = totalCount,
                totalPages = totalPages,
                currentPage = page,
                specialtyId = item.SpecialtyID,
                name = item.Name,
                shortName = item.ShortName,
                matches = item.Matches,
                candidates = item.PotentialCandidates
            };

            return Ok(result);
        }


        ///<summary>
        ///Get client global settings for market insights
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}/Settings")]
        public IHttpActionResult GetClientGlobalSettings(Guid clientId)
        {
            return Ok(Insights.GetClientGlobalSettings(clientId));
        }

        ///<summary>
        ///Update global settings for market insights
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}/Settings")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateClientGlobalSettings(Guid clientId, Setting settings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            settings.ClientID = clientId;

            Insights.UpdateClientGlobalSettings(settings);

            return Ok();
            
        }

        ///<summary>
        ///Get client global settings for market insights
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}/Settings/{specialtyId}")]
        public IHttpActionResult GetClientSpecialtySettings(Guid clientId, int specialtyId)
        {
            return Ok(Insights.GetClientSpecialtySettings(clientId, specialtyId));
        }

        ///<summary>
        ///Update global settings for market insights
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}/Settings/{specialtyId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateClientSpecialtySettings(Guid clientId, int specialtyId, Setting settings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            settings.ClientID = clientId;
            settings.SpecialtyID = specialtyId;

            Insights.UpdateClientSpecialtySettings(settings);

            return Ok();

        }


    }


    #region Helpers

    ///<summary>
    /// 
    ///</summary>
    public class Insights
    {
        ///<summary>
        ///Get global insights for client
        ///</summary>
        public static List<Insight> GetAllInsights(Guid clientId, int availability, int experience, int scl, int education, int shift)
        {
            DataAccess da = new DataAccess();
            List<usp_GetMarketInsightsResult> insights = da.GetMarketInsights(clientId, availability, experience, scl, education, shift);
            List<Insight> list = new List<Models.Insight>();

            for (int i=0;i<insights.Count;++i)
            {
                List<Scale> scale = new List<Scale>();
                scale.Add(new Scale { Data = 0, Matches = insights[i]._0.Value });
                scale.Add(new Scale { Data = 20, Matches = insights[i]._20.Value });
                scale.Add(new Scale { Data = 40, Matches = insights[i]._40.Value });
                scale.Add(new Scale { Data = 60, Matches = insights[i]._60.Value });
                scale.Add(new Scale { Data = 80, Matches = insights[i]._80.Value });
                scale.Add(new Scale { Data = 100, Matches = insights[i]._100.Value });

                list.Add(new Insight { SpecialtyID=insights[i].SpecialtyID.Value, Name = insights[i].NAME, Matches = insights[i].matches.HasValue ? insights[i].matches.Value : 0, ShortName = insights[i].ShortName, Total = insights[i].total.HasValue ? insights[i].total.Value : 0, Scale = scale });
            }

            return list;
        }


        ///<summary>
        ///Get specialty insight
        ///</summary>
        public static Insight GetSpecialtyInsights(Guid clientId, int specialtyId, int availability, int experience, int scl, int education, int shift)
        {
            DataAccess da = new DataAccess();
            usp_GetMarketSpecialtyInsightsResult insight = da.GetMarketSpecialtyInsights(clientId, specialtyId, availability, experience, scl, education, shift);
            List<usp_GetSpecialtyUserMatchesResult> users = da.GetSpecialtyUserMatches(specialtyId);

            List<PotentialCandidate> matches = new List<Models.PotentialCandidate>();

            if(users != null)
            {
                for(int i=0; i<users.Count;++i)
                {
                    matches.Add(new PotentialCandidate { AvailableOn = users[i].AvailableOn, Education = users[i].Education.HasValue ? users[i].Education.Value : 0, Experience = users[i].Education.HasValue ? users[i].Education.Value : 0, Location = users[i].Location, SCLMatch = users[i].SCLMatch.HasValue ? users[i].SCLMatch.Value : 0, Shift = users[i].Shift.HasValue ? users[i].Shift.Value : 3, UserID = users[i].userId });
                }
            }
            

            Insight item = new Insight { SpecialtyID=insight.SpecialtyID, Name = insight.NAME, Matches = insight.matches.HasValue ? insight.matches.Value : 0, ShortName = insight.ShortName, PotentialCandidates=matches};
           

            return item;
        }

        ///<summary>
        ///Get roles related to a specific user by UserID
        ///</summary>
        public static Setting GetClientGlobalSettings(Guid clientId)
        {
            DataAccess da = new DataAccess();
            Setting s = null;
            usp_GetClientGlobalSettingsResult settings = da.GetClientGlobalSettings(clientId);
            if(settings != null)
            {
                s = new Models.Setting();
                s.Availability = settings.Availability;
                s.ClientID = clientId;
                s.Education = settings.Education;
                s.Experience = settings.Experience;
                s.SCLMatch = settings.SCLMatch;
                s.Shift = settings.Shift;
            }

            return s;
        }

        /// <summary>
        /// updates client global settings
        /// </summary>
        /// <param name="settings"></param>
        public static void UpdateClientGlobalSettings(Setting settings)
        {
            DataAccess da = new DataAccess();

            Setting orig = GetClientGlobalSettings(settings.ClientID);

            if(orig != null)
            {
                settings.Availability = settings.Availability == 0 ? orig.Availability : settings.Availability;
                settings.Experience = settings.Experience == 0 ? orig.Experience : settings.Experience;
                settings.Education = settings.Education == 0 ? orig.Education : settings.Education;
                settings.SCLMatch = settings.SCLMatch == 0 ? orig.SCLMatch : settings.SCLMatch;
                settings.Shift = settings.Shift == 0 ? orig.Shift : settings.Shift;
            }
            da.UpdateClientGlobalSettings(settings.ClientID, settings.Availability, settings.Experience, settings.SCLMatch, settings.Education, settings.Shift);
        }



        ///<summary>
        ///Get roles related to a specific user by UserID
        ///</summary>
        public static Setting GetClientSpecialtySettings(Guid clientId, int specialtyId)
        {
            DataAccess da = new DataAccess();
            Setting s = null;
            usp_GetClientSpecialtySettingsResult settings = da.GetClientSpecialtySettings(clientId, specialtyId);
            if (settings != null)
            {
                s = new Models.Setting();
                s.SpecialtyID = settings.SpecialtyID;
                s.Availability = settings.Availability;
                s.ClientID = settings.ClientID;
                s.Education = settings.Education;
                s.Experience = settings.Experience;
                s.SCLMatch = settings.SCLMatch;
                s.Shift = settings.Shift;
            }

            return s;
        }

        /// <summary>
        /// updates client global settings
        /// </summary>
        /// <param name="settings"></param>
        public static void UpdateClientSpecialtySettings(Setting settings)
        {
            DataAccess da = new DataAccess();

            Setting orig = GetClientGlobalSettings(settings.ClientID);

            if (orig != null)
            {
                settings.Availability = settings.Availability == 0 ? orig.Availability : settings.Availability;
                settings.Experience = settings.Experience == 0 ? orig.Experience : settings.Experience;
                settings.Education = settings.Education == 0 ? orig.Education : settings.Education;
                settings.SCLMatch = settings.SCLMatch == 0 ? orig.SCLMatch : settings.SCLMatch;
                settings.Shift = settings.Shift == 0 ? orig.Shift : settings.Shift;
            }
            da.UpdateClientSpecialtySettings(settings.ClientID, settings.SpecialtyID.Value, settings.Availability, settings.Experience, settings.SCLMatch, settings.Education, settings.Shift);
        }
    }
    #endregion
}
