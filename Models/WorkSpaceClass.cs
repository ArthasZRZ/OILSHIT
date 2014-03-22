using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfRibbonApplication1
{
    public class WorkSpaceClass
    {
        public string NAME { set; get; }
        public string NLIST_FILENAME { set; get; }
        public string ELIST_FILENAME { set; get; }
        public string ROOT_DIR { set; get; }
        public string DBNAME { set; get; }
        public TowerModel TowerModelInstance = null;
        public List<Models.HeatDoubler> HeatDoublerInstances = null;
        public WorkSpaceCategory Category = null;

        public WorkSpaceClass()
        {
            NLIST_FILENAME = "";
            ELIST_FILENAME = "";
            TowerModelInstance = new TowerModel();
            HeatDoublerInstances = new List<Models.HeatDoubler>();
            ROOT_DIR = "";
            Category = new WorkSpaceCategory();
        }
        public WorkSpaceClass(string nfilename, string efilename, string rootdir,
            string dbname, TowerModel twmodel, List<Models.HeatDoubler> hdinst,
            WorkSpaceCategory Category)
        {
            NLIST_FILENAME = nfilename;
            ELIST_FILENAME = efilename;
            ROOT_DIR = rootdir;
            DBNAME  = dbname;
            TowerModelInstance = twmodel;
            HeatDoublerInstances = hdinst;

            this.Category = Category;
        }
    }
}
