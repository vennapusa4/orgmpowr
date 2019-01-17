using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPOWR.Model
{
    public class GlossaryScreenModel
    {
        public string pageName { get; set; }
        public int ID { get; set; }
        public string RefinedFormula { get; set; }
        public string Description { get; set; }
        public bool IsChild { get; set; }
        public int? ParentScreenID { get; set; }
        public bool IsParent { get; set; }
        public int DisplayOrder { get; set; }

    }
    public class ParameterViewModel
    {
        public string RefinedParameter { get; set; }
        public string RefinedDescription { get; set; }
        public string Icon { get; set; }
        public int ID { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class GlossaryViewModel : GlossaryScreenModel
    {
        public List<ParameterViewModel> ParameterDetails { get; set; }

    }

    public class GlossaryEditModel : GlossaryScreenModel
    {
        public int? EditScreenID { get; set; }
        public string EditedFormula { get; set; }
        public string EditedDesc { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<ParameterEditModel> ParameterDetails { get; set; }
        public bool? IsApproved { get; set; }
        public int IsAltered { get; set; }
        public string Formula { get; set; }
    }



    public class ParameterEditModel : ParameterViewModel
    {
        public int? EditParamID { get; set; }
        public string EditedDesc { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsApproved { get; set; }
        public int IsAltered { get; set; }
        public string ParameterName { get; set; }
        public string Description { get; set; }

    }



    public class GlossaryReturnModel
    {
        public int ID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ErrorCode { get; set; }
    }


}
