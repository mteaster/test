using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Models;
using WebMatrix.WebData;
using System.Web.Helpers;

namespace test.Stuff
{
    public static class BandUtil
    {
        private static DatabaseContext database = new DatabaseContext();

        public static bool IsUserInBand(int userId, int bandId)
        {
            return database.BandMemberships.Find(bandId, userId) != null;
        }

        public static int Register(RegisterBandModel model)
        {
            BandProfile profile = new BandProfile(model.BandName, WebSecurity.CurrentUserId, Crypto.HashPassword(model.Password));
            database.BandProfiles.Add(profile);

            BandMembership membership = new BandMembership(profile.BandId, WebSecurity.CurrentUserId);
            database.BandMemberships.Add(membership);
            
            database.SaveChanges();

            return profile.BandId;
        }

        public static bool Join(int bandId)
        {
            //string hash = Crypto.HashPassword(model.Password);
            
            //if (true)hash == bandProfile.Password)
            //{
                BandMembership membership = new BandMembership(bandId, WebSecurity.CurrentUserId);
                database.BandMemberships.Add(membership);
                database.SaveChanges();

                return true;
            //}

            //return false;
        }

        public static bool IsBandNameTaken(string bandName)
        {
            return database.BandProfiles.Where(x => x.BandName == bandName).Count() > 0;
        }

        public static string MembersFor(int bandId)
        {
            var memberUsernames = from b in database.BandMemberships
                                  join u in database.UserProfiles
                                  on b.MemberId equals u.UserId
                                  where b.BandId == bandId
                                  select u.UserName;
            return string.Join(", ", memberUsernames.ToArray());
        }

        public static BandProfile BandProfileFor(int bandId)
        {
            return database.BandProfiles.Find(bandId);
        }

        public static List<BandModel> BandModels(bool membersFlag = false)
        {
            List<BandProfile> bandProfiles = database.BandProfiles.ToList();
            List<BandModel> bandModels = new List<BandModel>();

            foreach (BandProfile bandProfile in bandProfiles)
            {
                bandModels.Add(BandUtil.BandModelFor(bandProfile.BandId, true));
            }

            return bandModels;
        }

        public static List<BandModel> BandModelsFor(int userId, bool membersFlag = false)
        {
            List<BandModel> bandModels = new List<BandModel>();

            var bandIds = from m in database.BandMemberships
                          join p in database.BandProfiles
                          on m.BandId equals p.BandId
                          where m.MemberId == userId
                          select p.BandId;

            foreach (int bandId in bandIds)
            {
                bandModels.Add(BandModelFor(bandId, membersFlag));
            }

            return bandModels;
        }

        public static List<BandModel> CrazyBandModelsFor(int userId, bool membersFlag = false)
        {
            return (from m in database.BandMemberships
                    join p in database.BandProfiles
                    on m.BandId equals p.BandId
                    join u in database.UserProfiles
                    on p.CreatorId equals u.UserId
                    where m.MemberId == userId
                    select new BandModel(p.BandId, p.BandName, u.UserName, null)).ToList();
        }

        public static BandModel BandModelFor(BandProfile profile, bool membersFlag = false)
        {
            return new BandModel(profile.BandId,
                                        profile.BandName,
                                        database.UserProfiles.Find(profile.CreatorId).UserName,
                                        membersFlag ? MembersFor(profile.BandId) : null);
        }

        public static BandModel BandModelFor(int bandId, bool members = false)
        {
            return BandModelFor(database.BandProfiles.Find(bandId), members);
        }

        public static List<BandModel> SearchByName(String term)
        {
            return (from b in database.BandProfiles
                    join u in database.UserProfiles
                    on b.CreatorId equals u.UserId
                    where b.BandName.Contains(term)
                    select new BandModel(b.BandId, b.BandName, u.UserName, null)).ToList();
        }
    }
}