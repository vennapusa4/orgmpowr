using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPOWR.Model
{
  public class FileUpload
    {
        public int ID { get; set; }
        public int ApplicationID { get; set; }
        public string FileName { get; set; }
        public string FileNameForBlob { get; set; }
        public string ContainerName { get; set; }
        public string FilePath { get; set; }
        public string DataLevel { get; set; }
        public Nullable<int> RegionID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UploadedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string SelectedCriteria { get; set; }
        public string GeoIds { get; set; }
        public string CountryIds { get; set; }
        public string FinancialYearAndQuarter { get; set; }
    }

    public class Geos
    {
        public int GeoId { get; set; }
        public string Geo { get; set; }
        public bool? exist { get; set; }
    }
    public class Countries
    {
        public int CountryId { get; set; }
        public string Country { get; set; }
        public bool? exist { get; set; }
    }
}
