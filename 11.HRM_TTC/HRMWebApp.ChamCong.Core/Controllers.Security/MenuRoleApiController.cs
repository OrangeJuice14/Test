using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using HRMWebApp.ChamCong.Core.DTO;
using HRMWeb_Business.Model;
using HRMWeb_Business.BusinessServiceFactory;
using System.Transactions;
//
namespace HRMWebApp.ChamCong.Core.Controllers
{
    public class MenuRoleApiController : ApiController
    {
        //WebMenu
        public WebMenuDTO GetObject(Guid id)
        {
            WebMenuDTO result = new WebMenuDTO();
            //
            WebMenu_Factory factory = new WebMenu_Factory();
            WebMenu webMenu = factory.GetByID(id);
            //
            result.Id = webMenu.Oid;
            result.Name = webMenu.Name;
            result.Index = webMenu.Global_idx;
            result.ParentId = webMenu.ParentId != null ? webMenu.ParentId : Guid.Empty;
            //
            return result;
        }
        public IEnumerable<WebMenuDTO> GetWebMenuTreeList()
        {
            List<WebMenuDTO> resultList = new List<WebMenuDTO>();
            //
            WebMenu_Factory factory = new WebMenu_Factory();
            List<WebMenu> list = factory.GetAll().ToList();
            foreach (WebMenu item in list)
            {
                WebMenuDTO menu = new WebMenuDTO();
                menu.Id = item.Oid;
                menu.Name = item.Name;
                menu.ParentId = item.ParentId;
                menu.Index = item.Global_idx;
                resultList.Add(menu);
            }
            //
            return resultList;
        }
        public IEnumerable<WebMenuDTO> GetListParentMenu()
        {
            List<WebMenuDTO> resultList = new List<WebMenuDTO>();
            //
            WebMenu_Factory factory = new WebMenu_Factory();
            List<WebMenu> list = factory.GetChildMenuList_ByMenuId(Guid.Empty).ToList();
            foreach (WebMenu item in list)
            {
                WebMenuDTO menu = new WebMenuDTO();
                menu.Id = item.Oid;
                menu.Name = item.Name;
                menu.ParentId = item.ParentId;
                menu.Index = item.Global_idx;
                resultList.Add(menu);
            }
            //
            return resultList;
        }
        public int Put(WebMenuDTO obj)
        {
            int result = 0;
            try
            {
                WebMenu_Factory factory = new WebMenu_Factory();
                WebMenu webMenu = factory.GetByID(obj.Id);
                webMenu.Name = obj.Name;
                webMenu.Global_idx = obj.Index;
                //
                factory.SaveChanges();
                //
                result = 1;
            }
            catch (Exception ex) { }
            //
            return result;
        }

        //WebGroup
        public WebGroupDTO GetWebGroup(Guid id)
        {
            WebGroupDTO result = new WebGroupDTO();
            //
            WebGroup_Factory factory = new WebGroup_Factory();
            WebGroup webGroup = factory.GetByID(id);
            result.Id = webGroup.Oid;
            result.Name = webGroup.Name;

            //Lấy danh sách menu
            List<WebMenu> menuList = factory.GetRoledWebMenuList_ByWebGroupID(id).ToList();
            //
            foreach (WebMenu item in menuList)
            {
                result.WebMenuIds.Add(item.Oid);
            }
            //
            return result;
        }
        public IEnumerable<WebMenuHierarchyDTO> GetWebMenuHierarchy(Guid webGroupId)
        {
            //
            List<WebMenuHierarchyDTO> resultList = new List<WebMenuHierarchyDTO>();

            //Lấy danh sách menu được phân quyền
            WebGroup_Factory factory = new WebGroup_Factory();
            List<WebMenu> roledMenuList = factory.GetRoledWebMenuList_ByWebGroupID(webGroupId).ToList();
            //Lấy danh sách menu cha
            WebMenu_Factory factoryMenu = new WebMenu_Factory();
            List<WebMenu> parentWebMenuList = factoryMenu.GetChildWebMenuList(Guid.Empty).ToList();
            //
            foreach (WebMenu item in parentWebMenuList)
            {
                WebMenuHierarchyDTO menu = new WebMenuHierarchyDTO();
                menu.Id = item.Oid;
                menu.Name = item.Name;
                menu.items = new List<WebMenuDTO>();
                //Lấy danh sách các con
                GetChildWebMenuList(ref menu, roledMenuList);
                //
                resultList.Add(menu);
            }
            //
            return resultList;
        }

        void GetChildWebMenuList(ref WebMenuHierarchyDTO menu, List<WebMenu> roledMenuList)
        {
            //
            WebMenu_Factory factoryMenu = new WebMenu_Factory();
            //Lấy danh sách menu con
            List<WebMenu> childWebMenuList = factoryMenu.GetChildWebMenuList(menu.Id).ToList();
            //
            foreach (WebMenu itemChild in childWebMenuList)
            {
                WebMenuDTO childMenu = new WebMenuDTO();
                childMenu.Id = itemChild.Oid;
                childMenu.Name = itemChild.Name;
                childMenu.ParentId = menu.Id;
                var exsist = (from x in roledMenuList
                              where x.Oid == itemChild.Oid
                              select true).SingleOrDefault();
                if (exsist) childMenu.@checked = true;

                //Chỉ có con 1 cấp nên không cần đệ qui
                menu.items.Add(childMenu);
            }
        }
        public IEnumerable<WebGroupDTO> GetListWebGroup()
        {
            List<WebGroupDTO> resultList = new List<WebGroupDTO>();
            //
            WebGroup_Factory factory = new WebGroup_Factory();
            List<WebGroup> list = factory.GetAll().ToList();
            //
            foreach (WebGroup item in list)
            {
                WebGroupDTO webGroup = new WebGroupDTO();
                webGroup.Id = item.Oid;
                webGroup.Name = item.Name;
                resultList.Add(webGroup);
                //
            }
            //
            return resultList;
        }
        public List<Guid> ChildrenMenuIds(Guid id)
        {
            List<Guid> resultList = new List<Guid>();
            //
            WebMenu_Factory factory = new WebMenu_Factory();
            List<WebMenu> list = factory.GetChildWebMenuList(id).ToList();
            //
            foreach (WebMenu item in list)
            {
                //
                resultList.Add(item.Oid);
                //
            }
            return resultList;
        }

       public int PutWebGroup(WebGroupDTO obj) // Ở đây không động được có thời gian xem lại
        {
            int result = 0;

            using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(360)))
            {
                WebGroup_Factory factory_webgroup = new WebGroup_Factory();
                WebMenu_Factory factory_webmenu = new WebMenu_Factory();
                WebMenu_Role_Factory factory_webmenu_role = new WebMenu_Role_Factory();
                WebGroup webGroup = factory_webgroup.GetByID(obj.Id);
                webGroup.WebMenu_Role.Clear();
                List<Guid> temp = new List<Guid>();
                //Check menu con lấy menu cha
                foreach (Guid t in obj.WebMenuIds)
                {
                    WebMenu menu = factory_webmenu.GetByID(t);
                    temp.Add(menu.Oid);
                    if (menu.ParentId != null)
                    {
                        temp.Add(menu.ParentId.Value);
                    }
                }
                obj.WebMenuIds = temp.Distinct().ToList();
                foreach (Guid w in obj.WebMenuIds)
                {
                    //Add menu con của hồ sơ, quá trình
                    if ((w.ToString() == "00000000-0000-0000-0000-000000000017") || (w.ToString() == "00000000-0000-0000-0000-000000000018"))
                    {
                        WebMenu_Role web = new WebMenu_Role() { WebMenuID = w };
                        webGroup.WebMenu_Role.Add(web);
                        List<WebMenu> childList = factory_webmenu.GetChildMenuList_ByMenuId(w).ToList();
                        foreach (WebMenu c in childList)
                        {
                            WebMenu_Role cw = new WebMenu_Role() { WebMenuID = c.Oid };
                            var exists = webGroup.WebMenu_Role.Where(q => q.WebMenuID == cw.WebMenuID).FirstOrDefault();
                            if (exists == null)
                                webGroup.WebMenu_Role.Add(cw);
                        }
                    }
                    else
                    {
                        WebMenu_Role web = new WebMenu_Role() { WebMenuID = w };
                        webGroup.WebMenu_Role.Add(web);
                    }
                }

                try
                {
                    factory_webgroup.SaveChangesWithoutTransactionScope();
                    transaction.Complete();
                    //
                    result = 1;
                }
                catch (Exception ex)
                {
                    return result = 0;
                }
            }
            //
            return result;
        }
    }
}
