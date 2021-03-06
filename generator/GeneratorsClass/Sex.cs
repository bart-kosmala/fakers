﻿using System;
using System.Collections.Generic;
using System.Text;
using ExternalAPI;

namespace GeneratorsClass
{
    public static class Sex
    {
        public static char getSex()
        {
            try
            {
                Logger.sexStart();
                if (RandomNumber.Draw(0, 1) == 0)
                    return 'm';
                else
                    return 'f';
            }
            catch (Exception e)
            {
                Logger.log(e);
                return getSex();
            }

        }

        public static char getSex(char[] pesel)
        {
            int sexNumber = (int)Char.GetNumericValue(pesel[9]);
            if (sexNumber % 2 == 1)
                return 'm';
            else
                return 'f';
        }
    }
}
