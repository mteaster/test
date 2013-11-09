using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Helpers;
using System.Web.Security;
using test.Models;
using WebMatrix.WebData;
using test.Models.Band;
using test.Models.Dashboard;
using test.Models.Calendar;
using test.Models.FileCabinet;
using System.Web;
using System.IO;

namespace test.Stuff
{
    public class TestUtil
    {
        public static int MakeBand(string bandName, int creatorId, string password)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                BandProfile profile = new BandProfile(bandName, creatorId, Crypto.HashPassword(password));

                database.BandProfiles.Add(profile);

                BandMembership membership = new BandMembership(profile.BandId, creatorId);
                database.BandMemberships.Add(membership);

                database.SaveChanges();

                return profile.BandId;
            }
        }

        public static void PutInBand(int bandId, int userId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                if (database.BandProfiles.Find(bandId) == null)
                {
                    throw new BandNotFoundException();
                }

                BandMembership membership = new BandMembership(bandId, userId);
                database.BandMemberships.Add(membership);
                database.SaveChanges();
            }
        }
    }
}