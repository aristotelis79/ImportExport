using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ETLCRS.Models.Subtypes;

namespace ETLCRS.Models.ExportableItems
{
    public class Position : IExportableItem<List<string>>
    {
        #region Fields

        public Identifiable Identifiable { get; set; }

        public Describable Describable { get; set; }

        public Area Area { get; set; }

        public StatusType StatusType { get; set; }

        public static Dictionary<int, SubType> TypeOfLines { get; set; }

        /// <inheritdoc/>
        public int NumberOfSubtypes { get; set; }
        
        /// <inheritdoc/>
        public List<string> ResponseType { get; } = new List<string>();

        #endregion

        #region ctor

        public Position()
        {
            RegisterSubtypes();
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public bool IsExportableDivision(List<string> lines)
        {
            var numberOfSubtypes =  lines.Count(x => !string.IsNullOrWhiteSpace(x));

            return string.IsNullOrWhiteSpace(lines.LastOrDefault()) 
                   && numberOfSubtypes > 0 
                   && numberOfSubtypes <= NumberOfSubtypes;
        }

        /// <inheritdoc/>
        public List<string> ParseLines(List<string> lines)
        {
            var errorLines = new List<string>();

            if (!lines.Any()) return errorLines;

            //For all type of line object find the right line to get the values
            foreach (var typeOfLine in TypeOfLines)
            {
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    //Check if line is for this type of line else continue
                    if (!typeOfLine.Value.GetSubType(typeOfLine, line)) continue;

                    //fill values to the object and ger all line if something is wrong
                    var error = typeOfLine.Value.FeedValues(line);
                    if(!string.IsNullOrWhiteSpace(error)) errorLines.Add(error);

                    //Not need it the successful parsed line and it safe because of break
                    lines.Remove(line);

                    break;
                }
                //stop rest parsing
                if(errorLines.Any()) break;
            }

            //Add remain lines because has errors
             errorLines.AddRange(lines.Where(x => !string.IsNullOrWhiteSpace(x)));

             return errorLines;
        }

        /// <inheritdoc/>
        public void FeedResponseType(IExportableItem<List<string>> exportableItem)
        {
            ResponseType.Add(exportableItem.ToString());
        }

        ///<inheritdoc/>
        public void ClearValues()
        {
            Identifiable.ClearValues();
            Describable.ClearValues();
            Area.ClearValues();
            StatusType.ClearValues();
        }


        public override string ToString()
        {
            var representation = $@"{Identifiable.Identity.Value} {Describable.Name.Value} {Describable.Description.Value} "+
                                    $@"{Area.Lat.Value.Min.ToString(CultureInfo.CurrentCulture)} {Area.Lat.Value.Max.ToString(CultureInfo.CurrentCulture)} "+
                                    $@"{Area.Lon.Value.Min.ToString(CultureInfo.CurrentCulture)} {Area.Lon.Value.Max.ToString(CultureInfo.CurrentCulture)} "+
                                    $@"{Identifiable.ProjValue.Value}";
            
            return string.IsNullOrWhiteSpace(StatusType.Status.Value) 
                ? representation : 
                $"{representation} {StatusType.Status.Value}";
        }


        private void RegisterSubtypes()
        {
            Identifiable = new Identifiable();

            Describable = new Describable();

            Area = new Area();

            StatusType = new StatusType();

            TypeOfLines = new Dictionary<int, SubType>()
            {
                {1, Describable},
                {2, Area},
                {3, StatusType},
                {4, Identifiable}
            };

            NumberOfSubtypes = TypeOfLines.Count;
        }
                

        #endregion
    }
}