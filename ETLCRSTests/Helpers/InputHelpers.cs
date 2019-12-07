using System;
using System.Collections.Generic;
using System.IO;

namespace ETLCRSTests.Helpers
{
    public static class InputHelpers
    {
        public static List<string> Inputs = new List<string>
        {
            $@"# GCS_Voirol_Unifie_1960_Paris [Voirol Unifie 1960 (Paris)]{Environment.NewLine}# area: (lat: 31.99, 37.14) - (lon: -2.95, 9.09) [Algeria - north of 32~N]"+
                $@"{Environment.NewLine}# DEPRECATED: new code = 104025{Environment.NewLine}<4812> +proj=lonlat +a=6378249.145 +rf=293.465 +pm=paris +towgs84=-123.0,-206.0,219.0 +no_defs <>",
            
            $@"# S-JTSK_[JTSK03] [S-JTSK [JTSK03]]{Environment.NewLine}# area: (lat: 47.73, 49.61) - (lon: 16.84, 22.56) [Slovakia]{Environment.NewLine}"+
                $@"<8351> +proj=lonlat +ellps=bessel +towgs84=485.021,169.465,483.839,7.786342,4.397554,4.102655,0.0 +no_defs <>",

            $@"# Hong_Kong_Geodetic_CS [Hong Kong Geodetic CS]{Environment.NewLine}# area: (lat: 22.13, 22.58) - (lon: 113.76, 114.51) [China - Hong Kong]"+
                $@"{Environment.NewLine}<8427> +proj=lonlat +ellps=GRS80 +no_defs <>{Environment.NewLine}"
        }; 

        public static List<string> Outputs = new List<string>
        {
            "4812 GCS_Voirol_Unifie_1960_Paris Voirol Unifie 1960 (Paris) 31.99 37.14 -2.95 9.09 +proj=lonlat +a=6378249.145 +rf=293.465 +pm=paris +towgs84=-123.0,-206.0,219.0 +no_defs DEPRECATED",
            "8351 S-JTSK_[JTSK03] S-JTSK [JTSK03] 47.73 49.61 16.84 22.56 +proj=lonlat +ellps=bessel +towgs84=485.021,169.465,483.839,7.786342,4.397554,4.102655,0.0 +no_defs",
            "8427 Hong_Kong_Geodetic_CS Hong Kong Geodetic CS 22.13 22.58 113.76 114.51 +proj=lonlat +ellps=GRS80 +no_defs"
        };


        public static string Describable_A = "# GCS_Assumed_Geographic_1 [NAD27 for shapefiles w/o a PRJ]";
        public static string Area_A  = "# area: (lat: -90.0, 90.0) - (lon: -180.0, 180.0) [Not specified]";
        public static string Identifiable_A = "<104000> +proj=lonlat +datum=NAD27 +no_defs <>";
        public static string StatusSubType_A = "# DISCONTINUED";

        public static string Describable_B = "# GCS_Voirol_Unifie_1960 [Voirol Unifie 1960]";
        public static string Area_B  = "# area: (lat: 31.99, 37.14) - (lon: -2.95, 9.09) [Algeria - north of 32~N]";
        public static string Identifiable_B = "<4305> +proj=lonlat +a=6378249.145 +rf=293.465 +towgs84=-123.0,-206.0,219.0 +no_defs <>";
        public static string StatusSubType_B = "# DEPRECATED: new code = 104026";


        public static Stream GenerateStreamFromString(this string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}