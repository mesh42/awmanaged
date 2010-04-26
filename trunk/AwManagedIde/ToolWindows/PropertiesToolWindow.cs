using System.Linq;
using System.Reflection;
using AwManaged.Configuration;
using AwManaged.Core.Commanding.Attributes;
using AwManaged.Core.ServicesManaging;
using AwManaged.Scene.ActionInterpreter.Attributes;

namespace AwManagedIde.ToolWindows
{
    public partial class PropertiesToolWindow : ToolWindow
    {
        public void SetPropertyGrid(object o)
        {
            propertyGrid1.SelectedObject = o;    
        }

        public PropertiesToolWindow()
        {
            InitializeComponent();
            _acSvc = new GenericInterpreterService<ACEnumTypeAttribute, ACEnumBindingAttribute, ACItemBindingAttribute>(Assembly.GetAssembly(typeof(ACEnumBindingAttribute)));
            _acSvc.Start();
            var ret = _acSvc.Interpret("create matfx type=2 coef=1 tex=self");
            //var ret = _acSvc.Interpret("activate media set radius=30.5 vol=100 osd=on name=foo");
            //propertyGrid1.SelectedObject = ret.ElementAt(0).Commands[0];
            //propertyGrid1.SelectedObject = new UniverseConnectionProperties();
        }

        private GenericInterpreterService<CCEnumTypeAttribute, CCEnumBindingAttribute, CCItemBindingAttribute> _ccSvc;
        private GenericInterpreterService<ACEnumTypeAttribute, ACEnumBindingAttribute, ACItemBindingAttribute> _acSvc;

        public void TearDown()
        {
            _ccSvc.Dispose();
            _acSvc.Dispose();
        }

        public void Setup()
        {
            //_ccSvc = new GenericInterpreterService<CCEnumTypeAttribute, CCEnumBindingAttribute, CCItemBindingAttribute>(Assembly.GetAssembly(typeof(ServerConsole)));
            //_ccSvc.Start();
        }

        public void LoadPluginTest()
        {
            _ccSvc.Interpret("load plugin gbot /persist.");

        }

        public void ActionResolveTest()
        {
            var ret = _acSvc.Interpret("activate media set vol=100 osd=on name=foo");


        }
    }
}


