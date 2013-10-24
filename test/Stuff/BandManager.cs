﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Models;

namespace test.Stuff
{
    public static class BandManager
    {
        private static DatabaseContext database = new DatabaseContext();

        public static bool UserInBand(int userId, int bandId)
        {
            return database.BandMemberships.Find(bandId, userId) != null;
        }
    }
}